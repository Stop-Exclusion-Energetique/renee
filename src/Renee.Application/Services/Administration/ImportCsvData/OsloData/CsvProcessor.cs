using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Renee.Application.Helpers;
using Renee.Domain;

namespace Renee.Application.Services.Administration.ImportCsvData.OsloData;

public class CsvProcessor(Encoding? encoding = null)
{
    private readonly Encoding _encoding = encoding ?? new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
    
    static CsvProcessor()
    {
        // Ensure code pages (e.g. Windows-1252) are available when running on Linux.
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public List<CsvData> ReadAndTransformCsv(Stream fileStream, out List<LineErrorReport> lineErrorsReporting)
    {
        // UTF-8 strict (erreur si octets invalides)
        var utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

        // Windows-1252 strict (erreur si caractère non représentable)
        var win1252 = Encoding.GetEncoding(1252,EncoderFallback.ReplacementFallback,DecoderFallback.ExceptionFallback);

        lineErrorsReporting = [];
        var validatedLines = new List<CsvData>();

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            Encoding = win1252
        };

        using var reader = new StreamReader(fileStream, utf8, detectEncodingFromByteOrderMarks: true);
        using var csv = new CsvReader(reader, config);

        csv.Read();
        csv.ReadHeader();

        var expectedColumnCount = csv.HeaderRecord?.Length ?? 0;

        csv.Context.RegisterClassMap<MappedOsloCsvData>();

        while (csv.Read())
        {
            var actualColumnCount = csv.Parser.Count;
            var lineNumber = csv.Parser.RawRow;
            var lineErrors = LineErrorReport.Create(lineNumber,[]);

            if (expectedColumnCount != actualColumnCount)
            {
                CsvHelperExtensions.ReportErrors(lineErrorsReporting, lineErrors, string.Format(CsvDataLabel.Errors.IncorrectNumberOfFieldsOnLine, actualColumnCount, expectedColumnCount));
                continue;
            }

            try
            {
                var osloRecord = csv.GetRecord<OsloCsvData>();

                int? parsedId = null;
                if (osloRecord.ProjectId != null && int.TryParse(osloRecord.ProjectId, out var id))
                {
                    parsedId = id;
                }
                lineErrors.LineId = parsedId;

                var transformedRecord = TransformToCsvData(osloRecord, out List<string> errorsByTransform);

                CsvHelperExtensions.ReportErrors(lineErrorsReporting, lineErrors, errorsByTransform);

                var missingRequiredFieldsErrors = CsvHelperExtensions.ValidateRequiredFields(transformedRecord);

                if (missingRequiredFieldsErrors.Count > 0)
                {
                    CsvHelperExtensions.ReportErrors(lineErrorsReporting, lineErrors, missingRequiredFieldsErrors);
                    continue;
                }

                validatedLines.Add(transformedRecord);
            }
            catch (Exception)
            {
                CsvHelperExtensions.ReportErrors(lineErrorsReporting, lineErrors, CsvDataLabel.Errors.UnknownError);
            }
        }

        return validatedLines;
    }

    public MemoryStream WriteCsvToStream(IEnumerable<CsvData> records)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            Encoding = _encoding
        };

        var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, _encoding, leaveOpen: true))
        using (var csvWriter = new CsvWriter(writer, config))
        {
            csvWriter.Context.RegisterClassMap<MappedCsvData>();
            csvWriter.WriteHeader<CsvData>();
            csvWriter.NextRecord();
            csvWriter.WriteRecords(records);
        }

        stream.Position = 0; 
        return stream;
    }


    private static CsvData TransformToCsvData(OsloCsvData oslo, out List<string> errors)
    {
        errors = new List<string>();
        
        int minorOccupants = 0;
        int majorOccupants = 0;

        if (ImportFileDataHelper.TryParseValue<int>(oslo.ApplicantMinorCount, out var minorOccupantsOutpout))
            minorOccupants = minorOccupantsOutpout;
        
        if (ImportFileDataHelper.TryParseValue<int>(oslo.ApplicantAdultCount, out var majorOccupantsOutput))
            majorOccupants = majorOccupantsOutput;

        var numberOfOccupants = minorOccupants + majorOccupants;
        
        var (postalCode, city, territory, department, region, locationError) = OsloCsvMapper.TransformLocation(oslo.ProjectZipCity, oslo.ProjectCity);
        if(locationError != null)
            errors.Add(locationError);

        if (numberOfOccupants == 0) numberOfOccupants = 1;
  
        return new CsvData
        {
            Reference = ReturnTrimmedStringValue(oslo.ProjectId),
            FirstEncounterDate = ReturnTrimmedStringValue(oslo.ProjectVisitDate),
            EndOfAccompanyingDate = ReturnTrimmedStringValue(oslo.ControlVisitToScheduleDate),
            EndOfEncounterDate = ReturnTrimmedStringValue(oslo.SettlementDate),
            Label = ReturnTrimmedStringValue(oslo.ProjectAddress),
            PostalCode = postalCode,
            City = city,
            AccompanyingFileTerritory = territory,
            Department = department,
            Region = region,
            Birthdate = ReturnTrimmedStringValue(oslo.ApplicantBirthDate),
            PhoneNumber = ReturnTrimmedStringValue(oslo.ApplicantPhones),
            Email = ReturnTrimmedStringValue(oslo.ApplicantEmail),
            FirstName = ReturnTrimmedStringValue(oslo.ApplicantFirstName),
            LastName = ReturnTrimmedStringValue(oslo.ApplicantLastName),
            LivingSpace = ReturnTrimmedStringValue(oslo.HousingSurface),
            YearOfAcquisitionOrEntry = ReturnTrimmedStringValue(oslo.HousingPurchaseYear),
            Dpe = ReturnTrimmedStringValue(oslo.HousingInitialEnergyClass),
            Ges = ReturnTrimmedStringValue(oslo.HousingInitialGesClass),
            AnnualEnergyConsumption = ReturnTrimmedStringValue(oslo.HousingInitialEnergyConsumption),
            AnnualGesEmission = ReturnTrimmedStringValue(oslo.HousingInitialGesEmission),
            EstimatedDpeAfterWork = ReturnTrimmedStringValue(oslo.HousingTargetEnergyClass),
            EstimatedAnnualEnergyConsumptionAfterWork = ReturnTrimmedStringValue(oslo.HousingTargetEnergyConsumption),
            EstimatedAnnualGesEmissionsAfterWork = ReturnTrimmedStringValue(oslo.HousingTargetGesEmission),
            EstimatedGesAfterWork = ReturnTrimmedStringValue(oslo.HousingTargetGesClass),
            SolidarBuilder = OsloCsvMapper.GetSolidarBuilderEmail(oslo.Referents),
            CadastralReference = ReturnTrimmedStringValue(oslo.ApplicantCadastralReference),
            HousingType = OsloCsvMapper.MapHousingType(oslo.HousingType, errors),
            SocioProfessionalCategory = OsloCsvMapper.MapProfession(oslo.ApplicantProfession, errors),
            ConstructionYear = ReturnTrimmedStringValue(oslo.HousingConstructionYear),
            ZeroEnergyExclusionTerritoriesProgram = CsvDataLabel.YesValue,
            MarkerNature = MarkerNatureLabels.Other,
            GeographicAreaTypology = GeographicalAreaTypologyLabel.Urban,
            IsDeleted = CsvDataLabel.NoValue,
            NumberOfOccupants = numberOfOccupants.ToString(),
            HouseholdTypology = OsloCsvMapper.MapHouseholdTypology(majorOccupants, minorOccupants, errors),
            ReferenceIncomeTax = OsloCsvMapper.ParseAndSumIncome(oslo.RfrAmounts, errors),
            AccompanyingType = AccompanyingTypeLabel.Targeted
        };
    }

    static string? ReturnTrimmedStringValue(string? value) => string.IsNullOrEmpty(value) ? string.Empty : value.Trim();

}
