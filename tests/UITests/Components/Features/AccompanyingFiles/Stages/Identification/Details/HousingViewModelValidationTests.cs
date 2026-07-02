using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.Housing.ViewModel;

namespace UITests.Components.Features.AccompanyingFiles.Stages.Identification.Details;

public class HousingViewModelValidationTests
{
	[Fact]
	public void CopropertyProfileId_ShouldBeRequired_WhenAskedPropertyTypeIsResidentialCollective()
	{
		// Arrange
		var viewModel = new HousingViewModel
		{
			AskedPropertyType = HousingType.ResidentialCollective,
			CopropertyProfileId = null
		};
		var validationResults = new List<ValidationResult>();

		// Act
		var isValid = Validator.TryValidateObject(
			viewModel,
			new ValidationContext(viewModel),
			validationResults,
			validateAllProperties: true);

		// Assert
		isValid.Should().BeFalse();
		validationResults.Should().Contain(result =>
			result.MemberNames.Contains(nameof(HousingViewModel.CopropertyProfileId)) &&
			result.ErrorMessage == Labels.Errors.RequiredCopropertyProfileInput);
	}

	[Fact]
	public void CopropertyProfileId_ShouldNotBeRequired_WhenAskedPropertyTypeIsNotResidentialCollective()
	{
		// Arrange
		var viewModel = new HousingViewModel
		{
			AskedPropertyType = HousingType.IndividualHouse,
			CopropertyProfileId = null
		};
		var validationResults = new List<ValidationResult>();

		// Act
		var isValid = Validator.TryValidateObject(
			viewModel,
			new ValidationContext(viewModel),
			validationResults,
			validateAllProperties: true);

		// Assert
		validationResults.Should().NotContain(result =>
			result.MemberNames.Contains(nameof(HousingViewModel.CopropertyProfileId)) &&
			result.ErrorMessage == Labels.Errors.RequiredCopropertyProfileInput);
	}
}