using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Renee.Application.DTOs.Occupant;
using Renee.Domain;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.HouseholdIdentity.ViewModels;

public class MainOccupantViewModel
{
	public Guid? Id { get; private init; }

	private string? _firstName; 
	[Required(ErrorMessage = Labels.Errors.FirstNameRequiredInQuickAdd)]
	public string? FirstName
	{
		get => _firstName;
		set
		{
			if (_firstName != value)
			{
				_firstName = value;
				UpdateTrigram();
			}
		}
	}

	private string? _lastName;
	[Required(ErrorMessage = Labels.Errors.LastNameRequiredInQuickAdd)]
	public string? LastName
	{
		get => _lastName;
		set
		{
			if (_lastName != value)
			{
				_lastName = value;
				UpdateTrigram();
			}
		}
	}

	[Required(ErrorMessage = Labels.Errors.RequiredTrigramInput)]
	public string? Trigram { get; set; }

	[EmailAddress(ErrorMessage = Labels.Errors.EmailAddressFormatInput)]
	public string? Email
	{
		get => _email;
		set => _email = string.IsNullOrWhiteSpace(value) ? null : value;
	}

	public string? PhoneNumber { get; set; } = string.Empty;

	[Required(ErrorMessage = Labels.Errors.RequiredBirthdayInput)]
	public DateTime? Birthday { get; set; }

	[Required(ErrorMessage = Labels.Errors.RequiredAgeMainOccupant)]
	public int? Age { get; set; }

	[RequiredIfAgeOver(
		nameof(AskedCsp),
		nameof(Age),
		ErrorMessage = Labels.Errors.RequiredAskedSocioProfessionalCategoryInput)]
	public SocioProfessionalCategory? AskedCsp { get; set; }

	public string? Profession { get; set; }

	public SocialProtectionFund? AskedSocialProtectionFund { get; set; }

	[RequiredIfOther(
		nameof(AskedSocialProtectionFund),
		(int)SocialProtectionFund.Other,
		ErrorMessage = Labels.Errors.RequiredSocialProtectionFreeInput)]
	public string? SocialProtectionFundFreeInput { get; set; }

	public PensionFund? AskedPensionFund { get; set; }

	[RequiredIfOther(
		nameof(AskedPensionFund),
		(int)PensionFund.Other,
		ErrorMessage = Labels.Errors.RequiredPensionFreeInput)]
	public string? PensionFundFreeInput { get; set; }

	public AdditionalFund? AskedAdditionalFund { get; set; }

	[RequiredIfOther(
		nameof(AskedAdditionalFund),
		(int)AdditionalFund.Other,
		ErrorMessage = Labels.Errors.RequiredAdditionalFundFreeInput)]
	public string? AdditionalFundFreeInput { get; set; }
	private string? _email;

	public static MainOccupantViewModel DtoToOccupantViewModel(MainOccupantDto dto)
	{
		return new MainOccupantViewModel
		{
			Trigram = dto.Trigram,
			AdditionalFundFreeInput = dto.OtherComplementaryFund,
			AskedAdditionalFund = dto.ComplementaryFund,
			Age = dto.Age,
			Birthday = dto.Birthdate,
			Email = dto.Email,
			Id = dto.Id,
			AskedPensionFund = dto.PensionFund,
			PensionFundFreeInput = dto.OtherPensionFund,
			PhoneNumber = dto.PhoneNumber,
			Profession = dto.Job,
			AskedSocialProtectionFund = dto.SocialWelfareFund,
			SocialProtectionFundFreeInput = dto.OtherSocialWelfareFund,
			AskedCsp = dto.SocioProfessionalCategoryId,
			FirstName = dto.FirstName,
			LastName = dto.LastName
		};
	}

	private void UpdateTrigram()
	{
		string firstNameInitial = "X";
		if (!string.IsNullOrWhiteSpace(_firstName))
		{
			firstNameInitial = _firstName[0].ToString().ToUpper();
		}

		string lastNameInitials = "YY";
		if (!string.IsNullOrWhiteSpace(_lastName))
		{
			string[] parts = _lastName
				.Split([' ', '-'], StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length > 0)
			{
				string firstPart = parts[0].ToUpper();
				if (firstPart.Length >= 2)
					lastNameInitials = firstPart[..2];
				else
					lastNameInitials = firstPart[0].ToString() + firstPart[0];
			}
		}

		Trigram = firstNameInitial + lastNameInitials;
	}
}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredIfOtherAttribute(string otherPropertyName, int propertyValue) : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext? validationContext)
	{
		if (validationContext == null)
			return new ValidationResult(ErrorMessage, new[] { Labels.Errors.UnknownErrorFormField });

		var otherPropertyInfo = validationContext.ObjectType.GetProperty(otherPropertyName);

		var otherPropertyValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance);

		if (otherPropertyValue is null || (int)otherPropertyValue != propertyValue) return ValidationResult.Success;

		if ((int)otherPropertyValue == propertyValue && string.IsNullOrEmpty(value?.ToString()))
			return new ValidationResult(
				ErrorMessage,
				new[] { validationContext.MemberName ?? Labels.Errors.UnknownErrorFormField });

		return ValidationResult.Success;
	}
}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredIfAgeOverAttribute(string propertyName, string otherPropertyName) : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext? validationContext)
	{
		if (validationContext == null)
			return new ValidationResult(ErrorMessage, new[] { Labels.Errors.UnknownErrorFormField });

		var otherPropertyInfo = validationContext.ObjectType.GetProperty(otherPropertyName);

		var otherPropertyValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance)?.ToString();

		if (!int.TryParse(otherPropertyValue, out var age) || age < 18) return ValidationResult.Success;

		if (propertyName == "PhoneNumber" && !IsValidPhoneNumber(value?.ToString()!))
			return new ValidationResult(
				Labels.Errors.PhoneNumberFormatInput,
				new[] { validationContext.MemberName ?? Labels.Errors.UnknownErrorFormField });

		if (string.IsNullOrEmpty(value?.ToString()))
			return new ValidationResult(
				ErrorMessage,
				new[] { validationContext.MemberName ?? Labels.Errors.UnknownErrorFormField });

		return ValidationResult.Success;
	}

	private static bool IsValidPhoneNumber(string phoneNumber)
	{
		const string phoneNumberPattern = @"^(\+|00)[1-9][0-9 \-\(\)\.]{7,32}$";

		return Regex.IsMatch(
			phoneNumber,
			phoneNumberPattern,
			RegexOptions.NonBacktracking,
			TimeSpan.FromMilliseconds(1000));
	}
}