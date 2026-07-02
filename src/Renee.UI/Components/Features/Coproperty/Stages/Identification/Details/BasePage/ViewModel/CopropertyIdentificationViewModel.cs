using System.ComponentModel.DataAnnotations;
using Renee.Application.DTOs.CopropertyProfile;
using Renee.UI.Components.Features.Coproperty.Stages.Identification.Details.DiagnosticsPerformance.ViewModel;
using Renee.UI.Components.Features.Coproperty.Stages.Identification.Details.GeneralInfo.ViewModel;
using Renee.UI.Components.Features.Coproperty.Stages.Identification.Details.GovernanceContacts.ViewModel;

namespace Renee.UI.Components.Features.Coproperty.Stages.Identification.Details.BasePage.ViewModel;

public class CopropertyIdentificationViewModel(
    GeneralInfoViewModel generalInfoViewModel,
    GovernanceContactsViewModel governanceContactsViewModel,
    DiagnosticsPerformanceViewModel diagnosticsPerformanceViewModel)
{
    [ValidateComplexType] public GeneralInfoViewModel GeneralInfoViewModel { get; set; } = generalInfoViewModel;

    [ValidateComplexType] public GovernanceContactsViewModel GovernanceContactsViewModel { get; set; } = governanceContactsViewModel;

    [ValidateComplexType] public DiagnosticsPerformanceViewModel DiagnosticsPerformanceViewModel { get; set; } = diagnosticsPerformanceViewModel;

    public static CopropertyIdentificationViewModel CreateViewModelFromDto(CopropertyProfileIdentificationDto dto)
    {
        var generalInfoViewModel = new GeneralInfoViewModel()
        {
            StreetNumberName = dto.Address,
            AdditionalAddress = dto.AdditionalAddress,
            Typology = dto.Typology,
            HousingTypology = dto.HousingType,
            NumberOfLots = dto.NumberOfLots,
            NumberOfFloor = dto.NumberOfFloor,
            HeatingType = dto.HeatingType,
            PerilTypeLabel = dto.PerilType
        };

        var governanceContactsViewModel = new GovernanceContactsViewModel()
        {
            NatureOfSyndic = dto.NatureOfSyndicType,
            NameOfSyndic = dto.NameOfSyndic,
            PhoneOfSyndic = dto.PhoneOfSyndic,
            MailOfSyndic = dto.MailOfSyndic,
            NameOfAmo = dto.NameOfAmo,
            ContactOfAmo = dto.ContactOfAmo,
            NumberOfContacts = dto.NumberOfContacts
        };

        var diagnosticsPerformanceViewModel = new DiagnosticsPerformanceViewModel()
        {
            BuildingDpeLabel = dto.BuildingDpeLabel,
            BuildingDpeEnergy = dto.BuildingDpeEnergy,
            ApartmentDpeLabel = dto.ApartmentDpeLabel,
            ApartmentDpeEnergy = dto.ApartmentDpeEnergy
        };

        return new CopropertyIdentificationViewModel(generalInfoViewModel, governanceContactsViewModel, diagnosticsPerformanceViewModel);
    }
}
