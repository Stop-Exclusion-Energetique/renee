namespace Renee.Application.Services.Administration.ImportCsvData.OsloData;

using CsvHelper.Configuration;

public sealed class MappedOsloCsvData : ClassMap<OsloCsvData>
{
    public MappedOsloCsvData()
    {
        Map(c => c.ProjectStatus).Index(0);
        Map(c => c.ProjectId).Index(1);
        Map(c => c.ProjectVisitDate).Index(2);
        Map(c => c.ApplicantContactDate).Index(3);
        Map(c => c.ControlVisitToScheduleDate).Index(4);
        Map(c => c.SettlementDate).Index(5);
        Map(c => c.ProjectNationalPartners).Index(6);
        Map(c => c.ProjectCity).Index(7);
        Map(c => c.RfrAmounts).Index(8);
        Map(c => c.RfrYears).Index(9);
        Map(c => c.ApplicantBirthDate).Index(10);
        Map(c => c.ApplicantProfession).Index(11);
        Map(c => c.ApplicantPhones).Index(12);
        Map(c => c.ApplicantEmail).Index(13);
        Map(c => c.ApplicantFirstName).Index(14);
        Map(c => c.ApplicantLastName).Index(15);
        Map(c => c.ApplicantAdultCount).Index(16);
        Map(c => c.ApplicantMinorCount).Index(17);
        Map(c => c.HousingType).Index(18);
        Map(c => c.HousingSurface).Index(19);
        Map(c => c.HousingPurchaseYear).Index(20);
        Map(c => c.ProjectAddress).Index(21);
        Map(c => c.HousingConstructionYear).Index(22);
        Map(c => c.ProjectZipCity).Index(23);
        Map(c => c.ApplicantCadastralReference).Index(24);
        Map(c => c.HousingInitialEnergyClass).Index(25);
        Map(c => c.HousingInitialGesClass).Index(26);
        Map(c => c.HousingInitialEnergyConsumption).Index(27);
        Map(c => c.HousingInitialGesEmission).Index(28);
        Map(c => c.HousingTargetEnergyClass).Index(29);
        Map(c => c.HousingTargetEnergyConsumption).Index(30);
        Map(c => c.HousingTargetGesEmission).Index(31);
        Map(c => c.HousingTargetGesClass).Index(32);
        Map(c => c.Referents).Index(33);
    }
}

