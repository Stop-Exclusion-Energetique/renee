using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.DTOs.ReportingStructure;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.UI.Components.Features.NewUser;
using Renee.UI.Components.FormComponents;
using Renee.Application.DTOs.User;
using Renee.Application.Queries.User;
using Renee.Domain.ReneeError;
using Renee.Application.DTOs.Territory;

namespace UITests.Components.Features.NewUser;

public class UserCreationFormTests
{
	[Fact]
	public void InUserCreationForm_WhenOtherValueIsSelectedForReportingStructureField_OtherReportingStructureFieldShouldAppear()
	{
		//Arrange
		var ctx = SetupContext();
		var cut = ctx.Render<UserCreationForm>();

		//Act
		cut.Instance.UserViewModel.SelectedReportingStructureId = cut.Instance.ReportingStructures?.Where(r => r.Label == Labels.Other).Select(r => r.Value).FirstOrDefault();
		cut.Instance.OnNewReportingStructureSelected();
		cut.Render();

		var otherReportingStructureField = cut.FindComponents<OutlinedText>()
			.FirstOrDefault(c => c.Instance.Label == Labels.OtherReportingStructure);

		//Assert
		otherReportingStructureField.Should().NotBeNull();
	}

	[Fact]
	public void InUserCreationForm_WhenOtherValueIsNotSelectedForReportingStructureField_OtherReportingStructureFieldShouldNotAppear()
	{
		//Arrange
		var ctx = SetupContext();
		var mockReportingStructureService = A.Fake<IReportingStructureService>();
        var reportingStructureId = Guid.NewGuid();

		A.CallTo(() => mockReportingStructureService.GetAllReportingStructures()).Returns(
		ReneeOperationResult<IEnumerable<ReportingStructureDto>>.Success(
		[
			new ReportingStructureDto(reportingStructureId, Labels.Other, null)
		]));

		ctx.Services.AddSingleton(mockReportingStructureService);

		var cut = ctx.Render<UserCreationForm>();

		//Act
		cut.Instance.UserViewModel.SelectedReportingStructureId = Guid.NewGuid();
		cut.Instance.OnNewReportingStructureSelected();
		cut.Render();

		var otherReportingStructureField = cut.FindComponents<OutlinedText>()
			.FirstOrDefault(c => c.Instance.Label == Labels.OtherReportingStructure);

		//Assert
		otherReportingStructureField.Should().BeNull();
	}

	[Fact]
	public void InUserCreationForm_WhenOtherValueIsSelectedForReportingStructureFieldAndNoValueForOtherReportingStructure_ErrorMessageShouldAppear()
	{
		//Arrange
		var ctx = SetupContext();
		var cut = ctx.Render<UserCreationForm>();

		//Act
		cut.Instance.UserViewModel.SelectedReportingStructureId = cut.Instance.ReportingStructures?.Where(r => r.Label == Labels.Other).Select(r => r.Value).FirstOrDefault();
		cut.Instance.OnNewReportingStructureSelected();
		cut.Render();

		var otherReportingStructureField = cut.FindComponents<OutlinedText>()
			.FirstOrDefault(c => c.Instance.Label == Labels.OtherReportingStructure);

		var input = otherReportingStructureField?.Find("input");
		input?.Blur();

		var validationMessage = cut.FindAll(".validation-message")
			.FirstOrDefault(e => e.TextContent == Labels.Errors.RequiredOtherReportingStructure);

		//Assert
		validationMessage.Should().NotBeNull();
	}

	[Fact]
	public void InUserCreationForm_WhenOtherValueIsSelectedForReportingStructureFieldAndValueIsSetForOtherReportingStructure_ErrorMessageShouldNotAppear()
	{
		//Arrange
		var ctx = SetupContext();
		var mockReportingStructureService = A.Fake<IReportingStructureService>();
        var reportingStructureId = Guid.NewGuid();

		A.CallTo(() => mockReportingStructureService.GetAllReportingStructures()).Returns(
		ReneeOperationResult<IEnumerable<ReportingStructureDto>>.Success(
		[
			new ReportingStructureDto(reportingStructureId, Labels.Other, null)
		]));

		ctx.Services.AddSingleton(mockReportingStructureService);

		var cut = ctx.Render<UserCreationForm>();

		//Act
		cut.Instance.UserViewModel.SelectedReportingStructureId = reportingStructureId;
		cut.Instance.OnNewReportingStructureSelected();
		cut.Render();

		var otherReportingStructureField = cut.FindComponents<OutlinedText>()
			.FirstOrDefault(c => c.Instance.Label == Labels.OtherReportingStructure);

		var input = otherReportingStructureField?.Find("input");
		input?.Change("Test");
		input?.Blur();

		var validationMessage = cut.FindAll(".validation-message")
			.FirstOrDefault(e => e.TextContent == Labels.Errors.RequiredOtherReportingStructure);

		//Assert
		validationMessage.Should().BeNull();
	}


    [Fact]
    public void InUserCreationForm_WhenLastNameIsEmpty_ShouldShowValidationMessage()
    {
        // Arrange
        var ctx = SetupContext();
        var cut = ctx.Render<UserCreationForm>();

		// Act
        var outlinedText = cut.FindComponents<OutlinedText>().FirstOrDefault(c => c.Instance.Label == Labels.Name);
		var input = outlinedText?.Find("input");

		input?.Change(" ");
		input?.Blur();

        // Assert
        var validationMessageLastName = cut.FindAll(".validation-message")
            .FirstOrDefault(e => e.TextContent == Labels.Errors.RequiredLastnameInput);

        validationMessageLastName.Should().NotBeNull();
    }

    [Fact]
    public void InUserCreationForm_WhenFirstNameIsEmpty_ShouldShowValidationMessage()
    {
        // Arrange
        var ctx = SetupContext();
        var cut = ctx.Render<UserCreationForm>();

        // Act
		var outlinedText = cut.FindComponents<OutlinedText>().FirstOrDefault(c => c.Instance.Label == Labels.FirstName);
		var input = outlinedText?.Find("input");

		input?.Change(" ");
		input?.Blur();

		// Assert
		var validationMessages = cut.FindAll(".validation-message");
        var validationMessageFirstName = validationMessages
                .FirstOrDefault(e => e.TextContent == Labels.Errors.RequiredFirstnameInput);

        validationMessageFirstName.Should().NotBeNull();
    }

    [Fact]
    public void InUserCreationForm_WhenLastNameHasSpaces_ShouldBeTrimmedOnSubmit()
    {
        // Arrange
        var ctx = SetupContext();
        var cut = ctx.Render<UserCreationForm>();

		// Act
		var outlinedText = cut.FindComponents<OutlinedText>().FirstOrDefault(c => c.Instance.Label == Labels.Name);
		var input = outlinedText?.Find("input");

		input?.Change("  Dupont  ");
		input?.Blur();

		// Assert
		cut.Instance.UserViewModel.LastName.Should().Be("Dupont");
    }

    [Fact]
    public void InUserCreationForm_WhenFirstNameHasSpaces_ShouldBeTrimmedOnSubmit()
    {
        // Arrange
        var ctx = SetupContext();
        var cut = ctx.Render<UserCreationForm>();

		// Act
		var outlinedText = cut.FindComponents<OutlinedText>().FirstOrDefault(c => c.Instance.Label == Labels.FirstName);
		var input = outlinedText?.Find("input");

		input?.Change("  Tom");
		input?.Blur();

        // Assert
        cut.Instance.UserViewModel.FirstName.Should().Be("Tom");
    }

    private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();
		ctx.Services.AddSingleton(A.Fake<IUserValidationService>());
		ctx.Services.AddSingleton(A.Fake<IUserService>());
		ctx.Services.AddSingleton(A.Fake<IModalService>());
		ctx.Services.AddSingleton(A.Fake<IFileViewerService>());
		ctx.Services.AddSingleton(A.Fake<ICguVersionService>());
		ctx.Services.AddSingleton(A.Fake<NotificationService>());

		var sendEventQueryMock = A.Fake<ISendEventQuery>();
		A.CallTo(() => sendEventQueryMock.Send(A<GetInitialisationDataForUserCreationFormQuery>._)).Returns(
			ReneeOperationResult<GetInitialisationDataForUserCreationFormQueryResult>
			.Success(new GetInitialisationDataForUserCreationFormQueryResult
			{
				Roles = new List<RoleDto>
				{
					new RoleDto{Id = Guid.NewGuid(), LongName = "Admin"},
					new RoleDto{Id = Guid.NewGuid(), LongName = "Ensemblier"}
				},
				ReportingStructures = new List<ReportingStructureDto>
				{
					new ReportingStructureDto(Guid.NewGuid(), "Main Office", null),
					new ReportingStructureDto(Guid.NewGuid(), Labels.Other, null)
				},
				Territories = new List<TerritoryQueryObjectResult>
				{
					new TerritoryQueryObjectResult("North", Guid.NewGuid()),
					new TerritoryQueryObjectResult("South", Guid.NewGuid())
				}
			}));
		ctx.Services.AddSingleton(sendEventQueryMock);

		var authContext = ctx.AddAuthorization();
		authContext.SetNotAuthorized();

		ctx.JSInterop.Mode = JSRuntimeMode.Loose;
		return ctx;
	}
}