using System.ComponentModel.DataAnnotations;
using Renee.Domain;

namespace Renee.UI.Components.Features.ProfileManagement.Modal.ViewModel;

public class UpdateProfileManagementViewModel
{
	[Required(ErrorMessage = Labels.Errors.InvalidEmail)]
	public string? Email { get; set; }

	private string? _lastName;

    [Required(ErrorMessage = Labels.Errors.RequiredLastnameInput)]
	public string? LastName { 
		get => _lastName; 
		set => _lastName = value?.Trim() ?? string.Empty; 
	}


    private string? _firstName;

    [Required(ErrorMessage = Labels.Errors.RequiredFirstnameInput)]
	public string? FirstName { 
		get => _firstName; 
		set => _firstName = value?.Trim(); 
	}

	[Phone]
	[Required(ErrorMessage = Labels.Errors.RequiredPhoneNumberInput)]
	public string? PhoneNumber { get; set; }

	public Guid? TerritoryId { get; set; }

	[RegularExpression("^[0-9]{14}$", ErrorMessage = Labels.Errors.IncorrectFormatOfSiretNumber)]
	[Required(ErrorMessage = Labels.Errors.RequiredSiretNumber)]
	public string? SiretNumber { get; set; }
}