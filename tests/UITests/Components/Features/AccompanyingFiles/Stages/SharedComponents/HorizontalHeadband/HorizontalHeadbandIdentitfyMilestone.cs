using System.Security.Claims;
using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI.Components.Layout.StageLayout;

namespace UITests.Components.Features.AccompanyingFiles.Stages.SharedComponents.HorizontalHeadband;

public class HorizontalHeadbandIdentitfyMilestone
{
	private static BunitContext SetupContext()
	{
		var ctx = new BunitContext();
		ctx.Services.AddSingleton(A.Fake<IMainOccupantService>());
		ctx.Services.AddSingleton(A.Fake<IFinancialAidService>());
		ctx.Services.AddSingleton(A.Fake<IDifficultyFacedByFamilyService>());
		ctx.Services.AddSingleton(A.Fake<IHouseholdResourcesTypologyService>());
		ctx.Services.AddSingleton(A.Fake<IAddressService>());
		ctx.Services.AddSingleton(A.Fake<IModalService>());

		ctx.Services.AddSingleton(A.Fake<NotificationService>());
		ctx.Services.AddScoped<NavigationManager, BunitNavigationManager>();

		var mockAccompanyingFileService = A.Fake<IAccompanyingFileService>();
		A.CallTo(() => mockAccompanyingFileService.GetAccompanyingFileById(A<Guid>._, A<Guid>._, A<string>._)).Returns(
			ReneeOperationResult<AccompanyingFileDto?>.Success(
				new AccompanyingFileDto
				{
					Id = Guid.Parse("ac67eed2-eb14-4015-b483-8cefefc21f95"),
					Reference = "Michael SCOTT-NDU-75018-25/04/2024",
					CreationDatetimeUtc = DateTime.Now,
					LastUpdateDatetimeUtc = DateTime.Now,
					ClosedDatetimeUtc = null,
					Housing = new HousingDto
					{
						Id = Guid.NewGuid(),
						Address =
							new AddressDto
							{
								Id = Guid.NewGuid(),
								AdditionalAddress = "",
								City = "Rouen",
								Department = "Seine-Maritime",
								Label = "1 rue du gros horloge",
								Name = "test",
								PostalCode = "76000",
								Region = "Haute-Normandie",
								Type = "housenumber"
							},
						GeographicalTypology = GeographicalHousingAreaTypology.Urban
					},
					MainOccupant =
						new MainOccupantDto
						{
							Id = Guid.NewGuid(),
							Email = "duplicateaddress@sqli.com",
							PhoneNumber = "+33 7.77.77.77.77",
							Gender = "Mr",
							LastName = "test"
						},
					EnsemblierSolidaireUserId = Guid.Parse("c933830b-debf-4cb2-b7db-c40e603ca3bc"),
					EnsemblierTerritorialUserId = Guid.Parse("547de4d0-8ffc-4e1f-9b3d-a37a8b9fc1f9"),
					AnahCategory = "",
					Stage = AccompanyingFileStage.Identify,
					RiskType = AccompanyingFileRiskType.Low
				}
			)
		);
		ctx.Services.AddSingleton(mockAccompanyingFileService);

		var mockSendEventQuery = A.Fake<ISendEventQuery>();
		A.CallTo(() => mockSendEventQuery.Send(A<GetAccompanyingFileForHeadBandQuery>._)).Returns(
			ReneeOperationResult<GetAccompanyingFileForHeadBandQueryObjectResult>.Success(
			new GetAccompanyingFileForHeadBandQueryObjectResult
			{
				AccompanyingFileReference = "Michael SCOTT-NDU-75018-25/04/2024",
				AccompanyingFileStage = AccompanyingFileStage.Identify
			}));

		ctx.Services.AddSingleton(mockSendEventQuery);

		var stageStateService = A.Fake<IStageNavigationStateService>();

		A.CallTo(() => stageStateService.GetAccompanyingFileStageNavigation()).Returns(
			new List<StageNavigationModel>
			{
				new() { Url = Endpoints.NewOccupant, Stage = AccompanyingFileStage.Identify, IsCurrentPage = true }
			});

		A.CallTo(() => stageStateService.AssociatedResourceReference).Returns("Michael SCOTT-NDU-75018-25/04/2024");

		ctx.Services.AddSingleton(stageStateService);

		var authContext = ctx.AddAuthorization();
		var claims = new Claim[]
		{
			new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
		};

		authContext.SetRoles(Constants.SolidarBuilderRole);
		authContext.SetClaims(claims);
		authContext.SetAuthorized("testuser@sqli.com", AuthorizationState.Authorized);

		ctx.JSInterop.Mode = JSRuntimeMode.Loose;
		return ctx;
	}

	[Fact]
	public void OnRender_Milestone_ShouldBeCorrect()
	{
		// Arrange
		var ctx = SetupContext();
		var navManager = ctx.Services.GetRequiredService<NavigationManager>();

		navManager.NavigateTo($"{navManager.BaseUri}newoccupantdisplay/ac67eed2-eb14-4015-b483-8cefefc21f95");

		var cut = ctx.Render<StageLayout>();

		// Act
		var milestone = cut.Find("span.rz-dropdown-label.rz-inputtext");

		// Assert
		milestone.TextContent.Contains(AccompanyingFileStage.Identify.GetDescription()).Should().BeTrue();
	}

	[Fact]
	public void OnRender_Reference_ShouldBeCorrect()
	{
		// Arrange
		var ctx = SetupContext();
		var navManager = ctx.Services.GetRequiredService<NavigationManager>();

		navManager.NavigateTo($"{navManager.BaseUri}newoccupantdisplay/ac67eed2-eb14-4015-b483-8cefefc21f95");

		var cut = ctx.Render<StageLayout>();

		// Act
		var reference = cut.Find("h3.font-weight-bold");

		// Assert
		reference.TextContent.Equals("Michael SCOTT-NDU-75018-25/04/2024").Should().BeTrue();
	}
}