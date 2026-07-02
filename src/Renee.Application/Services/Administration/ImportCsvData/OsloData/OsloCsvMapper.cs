using System.Globalization;
using Renee.Domain;

namespace Renee.Application.Services.Administration.ImportCsvData.OsloData;

public static class OsloCsvMapper
{

    private static readonly Dictionary<string, string> _mappingProfession = new(StringComparer.OrdinalIgnoreCase)
    {
        {  CsvDataLabel.OsloCsvDataColumn.Employee, SocioProfessionalCategoryLabel.Employee },
        { CsvDataLabel.OsloCsvDataColumn.RetiredOther, SocioProfessionalCategoryLabel.Retired },
        { CsvDataLabel.OsloCsvDataColumn.IndependentWorker, SocioProfessionalCategoryLabel.Artisan },
        { CsvDataLabel.OsloCsvDataColumn.PublicServant, SocioProfessionalCategoryLabel.IntermediateProfession },
        { CsvDataLabel.OsloCsvDataColumn.Unemployed, SocioProfessionalCategoryLabel.SearchingJob },
        { CsvDataLabel.OsloCsvDataColumn.Student, SocioProfessionalCategoryLabel.Unemployed },
        { CsvDataLabel.OsloCsvDataColumn.Disabled, SocioProfessionalCategoryLabel.Unemployed }
    };

    public static string MapProfession(string? osloProfession, List<string> errors)
    {

        if (string.IsNullOrWhiteSpace(osloProfession))
        {
            errors.Add(CsvDataLabel.OsloCsvErrors.NullProfession);
            return string.Empty;
        }

        if(_mappingProfession.TryGetValue(osloProfession.Trim(), out var mapped))
            return mapped;

        errors.Add(string.Format(CsvDataLabel.OsloCsvErrors.UnknownProfession, osloProfession.Trim()));
        return string.Empty; 
    }

    private static readonly Dictionary<string, string> _mappingHousingType = new(StringComparer.OrdinalIgnoreCase)
    {
        { CsvDataLabel.OsloCsvDataColumn.HousingTypeHome, HousingTypeLabel.IndividualHouse },
        { CsvDataLabel.OsloCsvDataColumn.HousingTypeApartment, HousingTypeLabel.ResidentialCollective },
        { CsvDataLabel.OsloCsvDataColumn.HousingTypebuilding, HousingTypeLabel.ResidentialCollective },
        { CsvDataLabel.OsloCsvDataColumn.HousingTypeOther, HousingTypeLabel.ResidentialCollective },
        { CsvDataLabel.OsloCsvDataColumn.HousingTypeLocalCommercial, HousingTypeLabel.ResidentialCollective }
    };

    public static string MapHousingType(string? osloType, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(osloType))
        {
            errors.Add(CsvDataLabel.OsloCsvErrors.NullHousingType);
            return string.Empty;
        }

        if (_mappingHousingType.TryGetValue(osloType.Trim(), out var mapped))
            return mapped;

        errors.Add(string.Format(CsvDataLabel.OsloCsvErrors.UnknownHousingType, osloType.Trim()));
        return string.Empty; 
    }


    public static string MapHouseholdTypology(int nbAdults, int nbMinors, List<string> errors)
    {

        var result = (nbAdults, nbMinors) switch
        {
            (2, > 0) => HouseholdTypologyLabel.CoupleWithChildren,
            (2, 0) => HouseholdTypologyLabel.CoupleWithoutChildren,
            (1, > 0) => HouseholdTypologyLabel.SingleParentFamily,
            (1, 0) => HouseholdTypologyLabel.SinglePerson,
            (>2, _) => HouseholdTypologyLabel.CoupleWithAdultStaying,
            _ => string.Empty 
        };

        if (string.IsNullOrEmpty(result))
        {
            errors.Add(string.Format(CsvDataLabel.OsloCsvErrors.UnknownHouseholdTypology, nbAdults, nbMinors));
        }
        return result;
    }

    public static string? ParseAndSumIncome(string? rawValue, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            errors.Add(CsvDataLabel.OsloCsvErrors.NullIncomingTax);
            return string.Empty;
        }

        var parts = rawValue.Split(';', StringSplitOptions.RemoveEmptyEntries);
        double total = 0;

        foreach (var part in parts)
        {
            var cleaned = part.Trim().Replace(",", ".");
            if (double.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            {
                total += value;
            }
            else
            {
                errors.Add(string.Format(CsvDataLabel.OsloCsvErrors.InvalidIncomingTax, part));
                return string.Empty; 
            }
        }

        return total.ToString("0.##", CultureInfo.InvariantCulture).Replace(".", ",");
    }

    public static string? ExtractPostalCode(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var digits = new string(value.TakeWhile(char.IsDigit).ToArray());
        return digits.Length >= 5 ? digits.Substring(0, 5) : null;
    }

    public static string? ExtractTerritoryCode(string? postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode) || postalCode.Length < 2)
            return null;

        return postalCode.Substring(0, 2);
    }

    public static readonly Dictionary<string, (string Territory, string Department, string Region)> TerritoryMapping =
    new()
    {
        { CsvDataLabel.OsloDepartments.D22Code, (CsvDataLabel.OsloDepartments.D22Name, CsvDataLabel.OsloDepartments.D22Display, CsvDataLabel.OsloDepartments.D22Region) },
        { CsvDataLabel.OsloDepartments.D67Code, (CsvDataLabel.OsloDepartments.D67Name , CsvDataLabel.OsloDepartments.D67Display, CsvDataLabel.OsloDepartments.D67Region) },
        { CsvDataLabel.OsloDepartments.D75Code, (CsvDataLabel.OsloDepartments.D75Name , CsvDataLabel.OsloDepartments.D75Display , CsvDataLabel.OsloDepartments.D75Region) },
        { CsvDataLabel.OsloDepartments.D93Code, (CsvDataLabel.OsloDepartments.D93Name , CsvDataLabel.OsloDepartments.D93Display , CsvDataLabel.OsloDepartments.D93Region) }
    };

    public static (string? PostalCode, string? City, string? Territory, string? Department, string? Region, string? Error)
    TransformLocation(string? rawPostalCodeCity, string? rawCity)
    {
        var postalCode = ExtractPostalCode(rawPostalCodeCity);
        var city = rawCity?.Trim();

        if (postalCode == null)
            return (null, city, null, null, null, CsvDataLabel.OsloCsvErrors.UnknownPostalCode);

        var territoryCode = ExtractTerritoryCode(postalCode) ?? string.Empty;

        if (!TerritoryMapping.TryGetValue(territoryCode, out var mapping))
            return (postalCode, city, null, null, null, string.Format(CsvDataLabel.OsloCsvErrors.UnknownTerritory, territoryCode));

        return (postalCode, city, mapping.Territory, mapping.Department, mapping.Region, null);
    }

    public static string GetSolidarBuilderEmail(string? rawEmails)
    {
        if (string.IsNullOrWhiteSpace(rawEmails))
            return string.Empty;

        var separators = new[] { ';', ',' };
        var emails = rawEmails.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        return emails.Length > 0 ? emails[0].Trim() : string.Empty;
    }


}
