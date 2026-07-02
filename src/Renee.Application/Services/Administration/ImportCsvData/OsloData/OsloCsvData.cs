using System.ComponentModel;
using Renee.Domain;

namespace Renee.Application.Services.Administration.ImportCsvData.OsloData;

public class OsloCsvData
{
    [Description(CsvDataLabel.OsloCsvColumns.ProjetStatus)]
    public string? ProjectStatus { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ProjetId)]
    public string? ProjectId { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ProjectVisitDate)]
    public string? ProjectVisitDate { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ApplicantContactDate)]
    public string? ApplicantContactDate { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ControlVisitToScheduleDate)]
    public string? ControlVisitToScheduleDate { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.SettlementDate)]
    public string? SettlementDate { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ProjectNationalPartners)]
    public string? ProjectNationalPartners { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ProjectCity)]
    public string? ProjectCity { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.RfrAmounts)]
    public string? RfrAmounts { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.RfrYears)]
    public string? RfrYears { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ApplicantBirthDate)]
    public string? ApplicantBirthDate { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ApplicantProfession)]
    public string? ApplicantProfession { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ApplicantPhones)]
    public string? ApplicantPhones { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ApplicantEmail)]
    public string? ApplicantEmail { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ApplicantFirstName)]
    public string? ApplicantFirstName { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ApplicantLastName)]
    public string? ApplicantLastName { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ApplicantAdultCount)]
    public string? ApplicantAdultCount { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ApplicantMinorCount)]
    public string? ApplicantMinorCount { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.HousingType)]
    public string? HousingType { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.HousingSurface)]
    public string? HousingSurface { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.HousingPurchaseYear)]
    public string? HousingPurchaseYear { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ProjectAddress)]
    public string? ProjectAddress { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.HousingConstructionYear)]
    public string? HousingConstructionYear { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ProjectZipCity)]
    public string? ProjectZipCity { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.ApplicantCadastralReference)]
    public string? ApplicantCadastralReference { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.HousingInitialEnergyClass)]
    public string? HousingInitialEnergyClass { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.HousingInitialGesClass)]
    public string? HousingInitialGesClass { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.HousingInitialEnergyConsumption)]
    public string? HousingInitialEnergyConsumption { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.HousingInitialGesEmission)]
    public string? HousingInitialGesEmission { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.HousingTargetEnergyClass)]
    public string? HousingTargetEnergyClass { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.HousingTargetEnergyConsumption)]
    public string? HousingTargetEnergyConsumption { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.HousingTargetGesEmission)]
    public string? HousingTargetGesEmission { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.HousingTargetGesClass)]
    public string? HousingTargetGesClass { get; set; }

    [Description(CsvDataLabel.OsloCsvColumns.Referents)]
    public string? Referents { get; set; }
}
