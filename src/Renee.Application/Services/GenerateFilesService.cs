using ClosedXML.Excel;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.AcroForms;
using PdfSharp.Pdf.IO;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.DTOs.WorkPackage;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.ReneeError;
using System.Globalization;

namespace Renee.Application.Services;

public class GenerateFilesService(
	ISendEventQuery QuerySender,
	IWorkTypesLabelsService workTypesLabelsService,
	ITelemetryService telemetryService)
	: IGenerateFilesService
{
	public async Task<ReneeOperationResult<string>> GenerateAnahSynthesis(string path, string accompanyingFileReference, Guid connectedUserId)
	{
		try
		{
			if (connectedUserId == Guid.Empty)
				return ReneeOperationResult<string>.Failure(Labels.Errors.UserNotFound);

			var queryResult = await QuerySender.Send(
				new GetAccompanyingFileForAnahSynthesisPdfQuery(accompanyingFileReference, connectedUserId));

			if (!queryResult.IsSuccess || queryResult.Value is null)
				return ReneeOperationResult<string>.Failure(queryResult.Message ?? string.Empty);

			using var ms = new MemoryStream();
			var pdf = PdfReader.Open(path, PdfDocumentOpenMode.Modify);
			var forms = pdf.AcroForm;
			forms.Elements.Add("/NeedAppearances", new PdfBoolean(true));
			var queryResultValue = queryResult.Value;

			UpdateFormField(forms, GenerateFilesLabels.AnahSynthesisLabel.AnahFileAddressNumber, queryResultValue.StreetNumber);
			UpdateFormField(forms, GenerateFilesLabels.AnahSynthesisLabel.AnahFileAddressStreet, queryResultValue.StreetName);
			UpdateFormField(forms, GenerateFilesLabels.AnahSynthesisLabel.AnahFileAddressPostalCode, queryResultValue.PostalCode);
			UpdateFormField(forms, GenerateFilesLabels.AnahSynthesisLabel.AnahFileAddressCity, queryResultValue.City);
			UpdateFormField(forms, GenerateFilesLabels.AnahSynthesisLabel.AnahFileSiretNumber, queryResultValue.SiretNumber);
			UpdateFormField(
				forms,
				GenerateFilesLabels.AnahSynthesisLabel.AnahFileAccompanyingName,
				queryResultValue.SolidarBuilderFullName);

			var firstVisitDate = queryResultValue.FirstVisitDate.HasValue
				? DateOnly.FromDateTime(queryResultValue.FirstVisitDate.Value).ToString("dd/MM/yyyy", new CultureInfo("fr-FR"))
				: null;
			UpdateFormField(
				forms,
				GenerateFilesLabels.AnahSynthesisLabel.AnahFileVisitDate,
				firstVisitDate);

			UpdateCheckBoxField(forms, GenerateFilesLabels.AnahSynthesisLabel.AnahFileInsalubrity, queryResultValue.HasUnsanitaryExit);
			UpdateCheckBoxField(
				forms,
				GenerateFilesLabels.AnahSynthesisLabel.AnahFileAdapatation,
				queryResultValue.HasHousingAdaptationWork);
			UpdateCheckBoxField(forms, GenerateFilesLabels.AnahSynthesisLabel.Degradation, queryResultValue.HasProjectTypeDegradation);
			UpdateCheckBoxField(
				forms,
				GenerateFilesLabels.AnahSynthesisLabel.AnahFileIsolation,
				queryResultValue.HasProjectTypeIsolation);
			UpdateCheckBoxField(
				forms,
				GenerateFilesLabels.AnahSynthesisLabel.HeatingSystem,
				queryResultValue.HasProjectTypeChangeOfHeatingSystem);
			UpdateCheckBoxField(
				forms,
				GenerateFilesLabels.AnahSynthesisLabel.AnahFileReparation,
				queryResultValue.HasProjectTypeReparation);

			AddHeaderMentionForPdfFiles(pdf);

			pdf.Save(ms, false);
			pdf.Close();

			var output = ms.ToArray();
			ms.Close();

			return ReneeOperationResult<string>.Success(Convert.ToBase64String(output));
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<string>.Failure(GenerateFilesLabels.Errors.ErrorWhileGeneratingDocument);
		}
	}

	public async Task<ReneeOperationResult<MemoryStream>> ExportAccompanyingFileForExcel(Guid userId, string userRole)
	{
		try
		{
			var result = await QuerySender.Send(new GetCompleteAccompanyingFileForExcelExportQuery { UserId = userId, UserRole = userRole });

			if (!result.IsSuccess)
			{
				return ReneeOperationResult<MemoryStream>.Failure(result.Message ?? string.Empty);
			}

			var exportFile = result.Value;
			var workbook = new XLWorkbook();

			var accompanyingFileworksheet = workbook.AddWorksheet("Dossier d'accompagnement");

			AddHeaderMentionForExcelFiles(accompanyingFileworksheet);

			accompanyingFileworksheet.Cell("A3").InsertTable(exportFile?.AccompanyingFileExcelExportDto);
			accompanyingFileworksheet.Columns(); //.AdjustToContents();

			var stream = new MemoryStream();
			workbook.SaveAs(stream);
			stream.Position = 0;

			return ReneeOperationResult<MemoryStream>.Success(stream);
		}
		catch (Exception ex) 
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<MemoryStream>.Failure(ExcelDataLabel.Errors.ErrorWhileGeneratingExcelFile);
		}
	}

	public async Task<ReneeOperationResult<string>> GenerateWorkCertificate(string path, string accompanyingFileReference, Guid connectedUserId)
	{
		try
		{
			if (connectedUserId == Guid.Empty)
				return ReneeOperationResult<string>.Failure(Labels.Errors.UserNotFound);

			var queryResult = await QuerySender.Send(
				new GetAccompanyingFileForWorkCertificatePdfQuery(accompanyingFileReference, connectedUserId));

			if (!queryResult.IsSuccess || queryResult.Value is null)
				return ReneeOperationResult<string>.Failure(queryResult.Message ?? string.Empty);

			using var ms = new MemoryStream();
			var pdf = PdfReader.Open(path, PdfDocumentOpenMode.Modify);
			var forms = pdf.AcroForm;
			var queryResultValue = queryResult.Value;

			forms.Elements.Add("/NeedAppearances", new PdfBoolean(true));

			UpdateFormField(forms, GenerateFilesLabels.WorkCertificateLabel.RequesterName, queryResultValue.OccupantFullName);
			UpdateFormField(forms, GenerateFilesLabels.WorkCertificateLabel.StreetNumber, queryResultValue.StreetNumber);
			UpdateFormField(forms, GenerateFilesLabels.AnahSynthesisLabel.AnahFileAddressStreet, queryResultValue.StreetName);
			UpdateFormField(forms, GenerateFilesLabels.AnahSynthesisLabel.AnahFileAddressPostalCode, queryResultValue.PostalCode);
			UpdateFormField(forms, GenerateFilesLabels.AnahSynthesisLabel.AnahFileAddressCity, queryResultValue.City);
			UpdateFormField(
				forms,
				GenerateFilesLabels.WorkCertificateLabel.WorkPackagesTotalCostExcludingTax,
				ConvertIncludingAllTaxesAmountToExcludingTax(queryResultValue.WorkPackagesTotalCostIncludingAllTaxes).ToString());
			UpdateFormField(
				forms,
				GenerateFilesLabels.WorkCertificateLabel.WorkPackagesTotalCostIncludingAllTaxes,
				queryResultValue.WorkPackagesTotalCostIncludingAllTaxes.ToString());
			UpdateFormField(forms, GenerateFilesLabels.WorkCertificateLabel.PrimaryEnergyBeforeWork, queryResultValue.AnnualEnergyConsumptionBeforeWork.ToString());
			UpdateFormField(forms, GenerateFilesLabels.WorkCertificateLabel.AnnualGesEmissionsBeforeWork, queryResultValue.EstimatedAnnualGesEmissionsBeforeWork.ToString());
			UpdateFormField(forms, GenerateFilesLabels.WorkCertificateLabel.EnergyClassBeforeWork, queryResultValue.EstimatedEnergyDpeBeforeWork);
			UpdateFormField(forms, GenerateFilesLabels.WorkCertificateLabel.SurfaceBeforeWork, queryResultValue.Surface.ToString());

			UpdateFormField(forms, GenerateFilesLabels.WorkCertificateLabel.PrimaryEnergyAfterWork, queryResultValue.AnnualEnergyConsumptionAfterWork.ToString());
			UpdateFormField(forms, GenerateFilesLabels.WorkCertificateLabel.AnnualGesEmissionsAfterWork, queryResultValue.EstimatedAnnualGesEmissionsAfterWork.ToString());
			UpdateFormField(forms, GenerateFilesLabels.WorkCertificateLabel.EnergyClassAfterWork, queryResultValue.EstimatedEnergyDpeAfterWork);
			UpdateFormField(forms, GenerateFilesLabels.WorkCertificateLabel.SurfaceAfterWork, queryResultValue.Surface.ToString());

			if (queryResultValue.EstimatedDpeClassJump >= 2)
			{
				string fieldName = queryResultValue.EstimatedDpeClassJump switch
				{
					2 => GenerateFilesLabels.WorkCertificateLabel.TwoClassesEarned,
					3 => GenerateFilesLabels.WorkCertificateLabel.ThreeClassesEarned,
					_ => GenerateFilesLabels.WorkCertificateLabel.FourClassesOrMoreEarned
				};
				UpdateCheckBoxField(forms, fieldName, true);
			}

			var workPackages = queryResultValue.WorkPackages;
			await FillWorkPackagesForWorkCertificate(workPackages, forms);

			UpdateFormField(forms, GenerateFilesLabels.WorkCertificateLabel.RequesterNameBis, queryResultValue.OccupantFullName);
			UpdateFormField(forms, GenerateFilesLabels.WorkCertificateLabel.AccompanyingPersonName, queryResultValue.SolidarBuilderFullName);
			UpdateFormField(forms, GenerateFilesLabels.WorkCertificateLabel.SocialContext, queryResultValue.SocialContext);
			UpdateFormField(forms, GenerateFilesLabels.WorkCertificateLabel.SiretNumber, queryResultValue.SiretNumber);

			AddHeaderMentionForPdfFiles(pdf);

			pdf.Save(ms, false);
			pdf.Close();

			var output = ms.ToArray();
			ms.Close();

			return ReneeOperationResult<string>.Success(Convert.ToBase64String(output));
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<string>.Failure(GenerateFilesLabels.Errors.ErrorWhileGeneratingDocument);
		}
	}

	public async Task<ReneeOperationResult<MemoryStream>> ExportAccompanyingFileBillingInformationAndAdministrativeData(Guid userId, string userRole)
    {
		try
		{
			var result = await QuerySender.Send(new GetCompleteAccompanyingFileForBillingLogAndAdministrationExcelExportQuery { UserId = userId, UserRole = userRole });

			if (!result.IsSuccess)
			{
				return ReneeOperationResult<MemoryStream>.Failure(result.Message ?? string.Empty);
			}

			var accompanyingFiles = result.Value;
			var workbook = new XLWorkbook();

			var accompanyingFileworksheet = workbook.AddWorksheet("Dossier d'accompagnement");

			AddHeaderMentionForExcelFiles(accompanyingFileworksheet);

			accompanyingFileworksheet.Cell("A3").InsertTable(accompanyingFiles);
			accompanyingFileworksheet.Columns().AdjustToContents();

			var stream = new MemoryStream();
			workbook.SaveAs(stream);
			stream.Position = 0;
			return ReneeOperationResult<MemoryStream>.Success(stream);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<MemoryStream>.Failure(ExcelDataLabel.Errors.ErrorWhileGeneratingExcelFile);
		}
    }

	private static void UpdateCheckBoxField(PdfAcroForm forms, string fieldName, bool isChecked)
	{
		if (forms.Fields[fieldName] is PdfCheckBoxField checkField) checkField.Checked = isChecked;
	}

	private static void UpdateFormField(PdfAcroForm forms, string fieldName, string? value)
	{
		if (value == null)
			return;

		var field = forms.Fields[fieldName];
		if (field != null) field.Value = new PdfString(value);
	}

	private static double ConvertIncludingAllTaxesAmountToExcludingTax(double? includingAllTaxesAmount) =>
		includingAllTaxesAmount.HasValue ? Math.Round(includingAllTaxesAmount.Value / (1 + Constants.TvaValue), 2) : 0;

	private async Task FillWorkPackagesForWorkCertificate(List<WorkPackageDto>? workPackageDtos, PdfAcroForm forms)
	{
		if (workPackageDtos is null || workPackageDtos.Count == 0)
			return;

		var culture = CultureInfo.CreateSpecificCulture("fr-FR");
		var workTypesLabelsResult = await workTypesLabelsService.GetAllWorkTypesLabels();
		var workTypesLabelsDictionary = workTypesLabelsResult is { IsSuccess: true, Value: not null }
			? workTypesLabelsResult.Value.ToDictionary(wt => wt.Id, wt => wt.Label)
			: new Dictionary<Guid, string>();

		foreach (var (workPackage, i) in workPackageDtos.Select((wp, i) => (wp, i)))
		{
			var parts = (workPackage.TypeCostDtos ?? [])
				.Where(wtc => workTypesLabelsDictionary.TryGetValue(wtc.Id, out var label)
							  && !string.IsNullOrWhiteSpace(label))
				.Select(wtc => $"{workTypesLabelsDictionary[wtc.Id]} :\n{wtc.Description}".Trim())
				.ToList();

			if (!string.IsNullOrWhiteSpace(workPackage.EnergeticsEffectOfWorks))
				parts.Add(workPackage.EnergeticsEffectOfWorks.Trim());

			UpdateFormField(
				forms,
				string.Format(GenerateFilesLabels.WorkCertificateLabel.WorkTypesDescription, i + 2),
				string.Join("\n\n", parts)
			);
		}

		var costFieldNames = new[] { "Row1", "Row 2", "Row 3", "55", "56", "Row 6", "Row 7" };

		foreach (var (fieldName, wp) in costFieldNames.Zip(workPackageDtos, (f, w) => (f, w)))
		{
			var workPackagesTotalCost = wp.TypeCostDtos?.Sum(wt => wt.Cost) ?? 0;

			UpdateFormField(
				forms,
				string.Format(GenerateFilesLabels.WorkCertificateLabel.WorkPackagesTotalCost, fieldName),
				string.Format(
					GenerateFilesLabels.WorkCertificateLabel.TotalCostFormat,
					ConvertIncludingAllTaxesAmountToExcludingTax(workPackagesTotalCost).ToString(),
					workPackagesTotalCost.ToString("C", culture)));
		}
	}

	private static void AddHeaderMentionForExcelFiles(IXLWorksheet? worksheet)
	{
		if (worksheet is null)
			return;

		worksheet.Cell("A1").Value = ExcelDataLabel.RgpdMention;

		var headerRange = worksheet.Range("A1:B1");
		headerRange.Style.Font.Bold = true;
		headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

		worksheet.Row(1).Height = 30;
	}

	private static void AddHeaderMentionForPdfFiles(PdfDocument? pdf)
	{
		if (pdf is null)
			return;

		var font = new XFont("Arial", 8, XFontStyleEx.Italic);
		var brush = XBrushes.Gray;

		foreach (var page in pdf.Pages)
		{
			using var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);

			var rect = new XRect(
				0,
				10,
				page.Width.Point,
				20
			);

			gfx.DrawString(
				ExcelDataLabel.RgpdMention,
				font,
				brush,
				rect,
				XStringFormats.TopCenter
			);
		}
	}
}