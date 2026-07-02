using FakeItEasy;
using FluentAssertions;
using Renee.Application.Handlers.QueryHandlers.Task;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Task;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;

namespace QueryHandlerTests.AccompanyingFileMenu;

public class GetAllAccompanyingFileTaskQueryHandlerTests
{
	private readonly ITaskRepository _taskRepositoryMock;
	private readonly IUserRepository _userRepositoryMock;
	private readonly IAccompanyingFileRepository _accompanyingFileRepositoryMock;
	private readonly ICopropertyProfileRepository _copropertyProfileRepositoryMock;
	private readonly ITelemetryService _telemetryService;

	public GetAllAccompanyingFileTaskQueryHandlerTests()
	{
		_taskRepositoryMock = A.Fake<ITaskRepository>();
		_userRepositoryMock = A.Fake<IUserRepository>();
		_accompanyingFileRepositoryMock = A.Fake<IAccompanyingFileRepository>();
		_copropertyProfileRepositoryMock = A.Fake<ICopropertyProfileRepository>();
		_telemetryService = A.Fake<ITelemetryService>();
	}

	[Fact]
	public async Task Handler_WhenUserIsNotFound_SupportTeamMembersResultShouldBeEmpty_()
	{
		// Arrange
		A.CallTo(() => _userRepositoryMock.GetUserById(
			A<Guid>._)).Returns<User?>(null);

		var housing = CreateHousingWithInitialState();
		var accompanyingFile = new AccompanyingFile
		{
			Id = Guid.NewGuid(),
			AccompanyingFileHousing = housing.Id,
			AccompanyingFileHousingNavigation = housing,
			AccompanyingFileTerritory = Guid.NewGuid(),
			AccompanyingFileSupportTeam = Guid.NewGuid(),
			ZeroEnergyExclusionTerritoriesProgram = true,
			AccompanyingFileSupportTeamNavigation = new SupportTeam
			{
				SolidarBuilder = Guid.NewGuid(),
				SolidarBuilderNavigation = new User
				{
					FirstName = "Alice",
					LastName = "Martin",
					PhoneNumber = "+33 6.45.78.12.34",
					Email = "alice.martin@example.com"
				},
				SecondSolidarBuilder = Guid.NewGuid(),
				SecondSolidarBuilderNavigation = new User
				{
					FirstName = "Benjamin",
					LastName = "Dupont",
					PhoneNumber = "+33 6.89.23.45.67",
					Email = "benjamin.dupont@example.com"
				},
				ThirdSolidarBuilder = Guid.NewGuid(),
				ThirdSolidarBuilderNavigation = new User
				{
					FirstName = "Charlotte",
					LastName = "Lemoine",
					PhoneNumber = "+33 6.33.67.89.90",
					Email = "charlotte.lemoine@example.com"
				},
				TrustedTierFirstName = "David",
				TrustedTierLastName = "Bertrand",
				TrustedTierEmail = "david.bertrand@test.com",
				TrustedTierPhoneNumber = "+33 6.68.87.12.45",
				TerritorialBuilder = Guid.NewGuid(),
				TerritorialBuilderNavigation = new User
				{
					FirstName = "Elodie",
					LastName = "Girard",
					PhoneNumber = "+33 6.12.34.56.78",
					Email = "elodie.girard@example.com"
				},
				SecondTerritorialBuilder = Guid.NewGuid(),
				SecondTerritorialBuilderNavigation = new User
				{
					FirstName = "Andreas",
					LastName = "Lila",
					PhoneNumber = "+33 6.12.34.56.78",
					Email = "lila.andreas@example.com"
				},
				DiffuseCoordinator = null,
				DiffuseCoordinatorNavigation = null,
				TargetCoordinator = Guid.NewGuid(),
				TargetCoordinatorNavigation = new User
				{
					FirstName = "Gabrielle",
					LastName = "Renaud",
					PhoneNumber = "+33 6.98.76.54.32",
					Email = "gabrielle.renaud@example.com"
				}
			}
		};
		
		A.CallTo(() => _accompanyingFileRepositoryMock.GetAccompanyingFileSupportTeam(
			A<Guid>._)).Returns(accompanyingFile);

		// Act
		var request = new GetAllAccompanyingFileTaskAndSupportTeamQuery 
		{ 
			AssociatedResourceId = Guid.NewGuid(),
			UserId = Guid.NewGuid() 
		};
		var handler = new GetAllAccompanyingFileTaskAndSupportTeamQueryHandler(
			_taskRepositoryMock,
			_accompanyingFileRepositoryMock,
			_userRepositoryMock,
            _copropertyProfileRepositoryMock,
			_telemetryService);
		var result = await handler.HandleQuery(request);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value!.SupportTeamMembers.Count.Should().Be(0);
	}

	[Fact]
	public async Task Handler_WhenAccompanyingFileIsNotFound_SupportTeamMembersResultShouldBeEmpty_()
	{
		// Arrange
		A.CallTo(() => _accompanyingFileRepositoryMock.GetAccompanyingFileSupportTeam(
			A<Guid>._)).Returns<AccompanyingFile?>(null);

		// Act
		var request = new GetAllAccompanyingFileTaskAndSupportTeamQuery
		{
			AssociatedResourceId = Guid.NewGuid(),
			UserId = Guid.NewGuid()
		};
		var handler = new GetAllAccompanyingFileTaskAndSupportTeamQueryHandler(
			_taskRepositoryMock,
			_accompanyingFileRepositoryMock,
			_userRepositoryMock,
            _copropertyProfileRepositoryMock,
			_telemetryService);
		var result = await handler.HandleQuery(request);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value!.SupportTeamMembers.Count.Should().Be(0);
	}

	[Fact]
	public async Task Handler_WhenUserIsSolidarBuilder_SupportTeamMembersResultShouldContainTheCorrectMembers()
	{
		// Arrange
		var user = new User
		{
			Id = Guid.NewGuid(),
			Role = new Role
			{
				Name = Constants.SolidarBuilderRole,
				LongName = "Ensemblier·ère Solidaire"
            },
			
		};
		var housing = CreateHousingWithInitialState();
		var accompanyingFile = new AccompanyingFile
		{
			Id = Guid.NewGuid(),
			AccompanyingFileHousing = housing.Id,
			AccompanyingFileHousingNavigation = housing,
			AccompanyingFileTerritory = Guid.NewGuid(),
			AccompanyingFileSupportTeam = Guid.NewGuid(),
			ZeroEnergyExclusionTerritoriesProgram = true,
			AccompanyingFileSupportTeamNavigation = new SupportTeam
			{
				SolidarBuilder = Guid.NewGuid(),
				SolidarBuilderNavigation = new User
				{
					FirstName = "Alice",
					LastName = "Martin",
					PhoneNumber = "+33 6.45.78.12.34",
					Email = "alice.martin@example.com"
				},
				SecondSolidarBuilder = Guid.NewGuid(),
				SecondSolidarBuilderNavigation = new User
				{
					FirstName = "Benjamin",
					LastName = "Dupont",
					PhoneNumber = "+33 6.89.23.45.67",
					Email = "benjamin.dupont@example.com"
				},
				ThirdSolidarBuilder = Guid.NewGuid(),
				ThirdSolidarBuilderNavigation = new User
				{
					FirstName = "Charlotte",
					LastName = "Lemoine",
					PhoneNumber = "+33 6.33.67.89.90",
					Email = "charlotte.lemoine@example.com"
				},
				TrustedTierFirstName = "David",
				TrustedTierLastName = "Bertrand",
				TrustedTierEmail = "david.bertrand@test.com",
				TrustedTierPhoneNumber = "+33 6.68.87.12.45",
				TerritorialBuilder = Guid.NewGuid(),
				TerritorialBuilderNavigation = new User
				{
					FirstName = "Elodie",
					LastName = "Girard",
					PhoneNumber = "+33 6.12.34.56.78",
					Email = "elodie.girard@example.com"
				},
                SecondTerritorialBuilder = Guid.NewGuid(),
                SecondTerritorialBuilderNavigation = new User
                {
                    FirstName = "Andreas",
                    LastName = "Lila",
                    PhoneNumber = "+33 6.12.34.56.78",
                    Email = "lila.andreas@example.com"
                },
                DiffuseCoordinator = null,
				DiffuseCoordinatorNavigation = null,
				TargetCoordinator = Guid.NewGuid(),
				TargetCoordinatorNavigation = new User
				{
					FirstName = "Gabrielle",
					LastName = "Renaud",
					PhoneNumber = "+33 6.98.76.54.32",
					Email = "gabrielle.renaud@example.com"
				}
			}
		};

		A.CallTo(() => _userRepositoryMock.GetUserById(
			A<Guid>._)).Returns(user);
		A.CallTo(() => _accompanyingFileRepositoryMock.GetAccompanyingFileSupportTeam(
			A<Guid>._)).Returns(accompanyingFile);

		var supportTeamNavigation = accompanyingFile.AccompanyingFileSupportTeamNavigation;
		var expectedResult = new List<SupportTeamMemberObjectResult>
		{
			new()
			{
				FullName = 
					$"{supportTeamNavigation.SolidarBuilderNavigation.FirstName} {supportTeamNavigation.SolidarBuilderNavigation.LastName}",
				Email = supportTeamNavigation.SolidarBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.SolidarBuilderNavigation.PhoneNumber,
				Role = Labels.ReferentSolidarBuilder,
				IsEditable = true
			},
			new()
			{
				FullName =
					$"{supportTeamNavigation.SecondSolidarBuilderNavigation.FirstName} {supportTeamNavigation.SecondSolidarBuilderNavigation.LastName}",
				Email = supportTeamNavigation.SecondSolidarBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.SecondSolidarBuilderNavigation.PhoneNumber,
				Role = $"Second {Labels.ReferentSolidarBuilder}",
				IsEditable = true
			},
			new()
			{
				FullName =
					$"{supportTeamNavigation.ThirdSolidarBuilderNavigation.FirstName} {supportTeamNavigation.ThirdSolidarBuilderNavigation.LastName}",
				Email = supportTeamNavigation.ThirdSolidarBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.ThirdSolidarBuilderNavigation.PhoneNumber,
				Role = $"Troisième {Labels.ReferentSolidarBuilder}",
				IsEditable = true
			},
			new()
			{
				FullName = $"{supportTeamNavigation.TrustedTierFirstName} {supportTeamNavigation.TrustedTierLastName}",
				Email = supportTeamNavigation.TrustedTierEmail,
				PhoneNumber = supportTeamNavigation.TrustedTierPhoneNumber,
				Role = Labels.TrustedTier,
				IsEditable = true
			},
			new()
			{
				FullName = $"{supportTeamNavigation.TerritorialBuilderNavigation.FirstName} {supportTeamNavigation.TerritorialBuilderNavigation.LastName}",
				Email = supportTeamNavigation.TerritorialBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.TerritorialBuilderNavigation.PhoneNumber,
				Role = Labels.ReferentEt,
				IsEditable = false
			},
            new()
            {
                FullName = $"{supportTeamNavigation.SecondTerritorialBuilderNavigation.FirstName} {supportTeamNavigation.SecondTerritorialBuilderNavigation.LastName}",
                Email = supportTeamNavigation.SecondTerritorialBuilderNavigation.Email,
                PhoneNumber = supportTeamNavigation.SecondTerritorialBuilderNavigation.PhoneNumber,
                Role = Labels.SecondReferentEt,
                IsEditable = false
            },
            new()
			{
				FullName = $"{supportTeamNavigation.TargetCoordinatorNavigation.FirstName} {supportTeamNavigation.TargetCoordinatorNavigation.LastName}",
				Email = supportTeamNavigation.TargetCoordinatorNavigation.Email,
				PhoneNumber = supportTeamNavigation.TargetCoordinatorNavigation.PhoneNumber,
				Role = Labels.ReferentTargetedCoordinator,
				IsEditable = false
			}
		};

		// Act
		var request = new GetAllAccompanyingFileTaskAndSupportTeamQuery 
		{ 
			AssociatedResourceId = Guid.NewGuid(), 
			UserId = Guid.NewGuid() 
		};
		var handler = new GetAllAccompanyingFileTaskAndSupportTeamQueryHandler(
			_taskRepositoryMock,
			_accompanyingFileRepositoryMock,
			_userRepositoryMock, 
			_copropertyProfileRepositoryMock,
			_telemetryService);
		var result = await handler.HandleQuery(request);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value!.SupportTeamMembers.Should().BeEquivalentTo(expectedResult);
	}

	[Fact]
	public async Task Handler_WhenUserIsTerritorialBuilder_SupportTeamMembersResultShouldContainTheCorrectMembers()
	{
		// Arrange
		var user = new User
		{
			Id = Guid.NewGuid(),
			Role = new Role
			{
				Name = Constants.TerritorialBuilderRole,
				LongName = "Ensemblier·ère Territorial·e"
            },

		};
		var housing = CreateHousingWithInitialState();
		var accompanyingFile = new AccompanyingFile
		{
			Id = Guid.NewGuid(),
			AccompanyingFileHousing = housing.Id,
			AccompanyingFileHousingNavigation = housing,
			AccompanyingFileTerritory = Guid.NewGuid(),
			AccompanyingFileSupportTeam = Guid.NewGuid(),
			ZeroEnergyExclusionTerritoriesProgram = true,
			AccompanyingFileSupportTeamNavigation = new SupportTeam
			{
				SolidarBuilder = Guid.NewGuid(),
				SolidarBuilderNavigation = new User
				{
					FirstName = "Alice",
					LastName = "Martin",
					PhoneNumber = "+33 6.45.78.12.34",
					Email = "alice.martin@example.com"
				},
				SecondSolidarBuilder = Guid.NewGuid(),
				SecondSolidarBuilderNavigation = new User
				{
					FirstName = "Benjamin",
					LastName = "Dupont",
					PhoneNumber = "+33 6.89.23.45.67",
					Email = "benjamin.dupont@example.com"
				},
				ThirdSolidarBuilder = Guid.NewGuid(),
				ThirdSolidarBuilderNavigation = new User
				{
					FirstName = "Charlotte",
					LastName = "Lemoine",
					PhoneNumber = "+33 6.33.67.89.90",
					Email = "charlotte.lemoine@example.com"
				},
				TrustedTierFirstName = "David",
				TrustedTierLastName = "Bertrand",
				TrustedTierEmail = "david.bertrand@test.com",
				TrustedTierPhoneNumber = "+33 6.68.87.12.45",
				TerritorialBuilder = Guid.NewGuid(),
				TerritorialBuilderNavigation = new User
				{
					FirstName = "Elodie",
					LastName = "Girard",
					PhoneNumber = "+33 6.12.34.56.78",
					Email = "elodie.girard@example.com"
				},
                SecondTerritorialBuilder = Guid.NewGuid(),
                SecondTerritorialBuilderNavigation = new User
                {
                    FirstName = "Andreas",
                    LastName = "Lila",
                    PhoneNumber = "+33 6.12.34.56.78",
                    Email = "lila.andreas@example.com"
                },
				DiffuseCoordinator = null,
				DiffuseCoordinatorNavigation = null,
				TargetCoordinator = Guid.NewGuid(),
				TargetCoordinatorNavigation = new User
				{
					FirstName = "Gabrielle",
					LastName = "Renaud",
					PhoneNumber = "+33 6.98.76.54.32",
					Email = "gabrielle.renaud@example.com"
				}
			}
		};

		A.CallTo(() => _userRepositoryMock.GetUserById(
			A<Guid>._)).Returns(user);
		A.CallTo(() => _accompanyingFileRepositoryMock.GetAccompanyingFileSupportTeam(
			A<Guid>._)).Returns(accompanyingFile);

		var supportTeamNavigation = accompanyingFile.AccompanyingFileSupportTeamNavigation;
		var expectedResult = new List<SupportTeamMemberObjectResult>
		{
			new()
			{
				FullName =
					$"{supportTeamNavigation.SolidarBuilderNavigation.FirstName} {supportTeamNavigation.SolidarBuilderNavigation.LastName}",
				Email = supportTeamNavigation.SolidarBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.SolidarBuilderNavigation.PhoneNumber,
				Role = Labels.ReferentSolidarBuilder,
				IsEditable = true
			},
			new()
			{
				FullName =
					$"{supportTeamNavigation.SecondSolidarBuilderNavigation.FirstName} {supportTeamNavigation.SecondSolidarBuilderNavigation.LastName}",
				Email = supportTeamNavigation.SecondSolidarBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.SecondSolidarBuilderNavigation.PhoneNumber,
				Role = $"Second {Labels.ReferentSolidarBuilder}",
				IsEditable = true
			},
			new()
			{
				FullName =
					$"{supportTeamNavigation.ThirdSolidarBuilderNavigation.FirstName} {supportTeamNavigation.ThirdSolidarBuilderNavigation.LastName}",
				Email = supportTeamNavigation.ThirdSolidarBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.ThirdSolidarBuilderNavigation.PhoneNumber,
				Role = $"Troisième {Labels.ReferentSolidarBuilder}",
				IsEditable = true
			},
			new()
			{
				FullName = $"{supportTeamNavigation.TrustedTierFirstName} {supportTeamNavigation.TrustedTierLastName}",
				Email = supportTeamNavigation.TrustedTierEmail,
				PhoneNumber = supportTeamNavigation.TrustedTierPhoneNumber,
				Role = Labels.TrustedTier,
				IsEditable = true
			},
			new()
			{
				FullName = $"{supportTeamNavigation.TerritorialBuilderNavigation.FirstName} {supportTeamNavigation.TerritorialBuilderNavigation.LastName}",
				Email = supportTeamNavigation.TerritorialBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.TerritorialBuilderNavigation.PhoneNumber,
				Role = Labels.ReferentEt,
				IsEditable = true
			},
            new()
            {
                FullName = $"{supportTeamNavigation.SecondTerritorialBuilderNavigation.FirstName} {supportTeamNavigation.SecondTerritorialBuilderNavigation.LastName}",
                Email = supportTeamNavigation.SecondTerritorialBuilderNavigation.Email,
                PhoneNumber = supportTeamNavigation.SecondTerritorialBuilderNavigation.PhoneNumber,
                Role = Labels.SecondReferentEt,
                IsEditable = true
            },
            new()
			{
				FullName = $"{supportTeamNavigation.TargetCoordinatorNavigation.FirstName} {supportTeamNavigation.TargetCoordinatorNavigation.LastName}",
				Email = supportTeamNavigation.TargetCoordinatorNavigation.Email,
				PhoneNumber = supportTeamNavigation.TargetCoordinatorNavigation.PhoneNumber,
				Role = Labels.ReferentTargetedCoordinator,
				IsEditable = false
			}
		};

		// Act
		var request = new GetAllAccompanyingFileTaskAndSupportTeamQuery
		{
			AssociatedResourceId = Guid.NewGuid(),
			UserId = Guid.NewGuid()
		};
		var handler = new GetAllAccompanyingFileTaskAndSupportTeamQueryHandler(
			_taskRepositoryMock,
			_accompanyingFileRepositoryMock,
			_userRepositoryMock, 
			_copropertyProfileRepositoryMock,
			_telemetryService);
		var result = await handler.HandleQuery(request);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value!.SupportTeamMembers.Should().BeEquivalentTo(expectedResult);
	}

	[Fact]
	public async Task Handler_WhenUserIsTargetedCoordinator_SupportTeamMembersResultShouldContainTheCorrectMembers()
	{
		// Arrange
		var userId = Guid.NewGuid();
		var user = new User
		{
			Id = userId,
			Role = new Role
			{
				Name = Constants.TargetedCoordinatorRole,
				LongName = "Coordinateur·rice du Ciblé·e"
            }
		};
		var housing = CreateHousingWithInitialState();
		var accompanyingFile = new AccompanyingFile
		{
			Id = Guid.NewGuid(),
			AccompanyingFileHousing = housing.Id,
			AccompanyingFileHousingNavigation = housing,
			AccompanyingFileTerritory = Guid.NewGuid(),
			AccompanyingFileSupportTeam = Guid.NewGuid(),
			ZeroEnergyExclusionTerritoriesProgram = true,
			AccompanyingFileSupportTeamNavigation = new SupportTeam
			{
				SolidarBuilder = Guid.NewGuid(),
				SolidarBuilderNavigation = new User
				{
					FirstName = "Alice",
					LastName = "Martin",
					PhoneNumber = "+33 6.45.78.12.34",
					Email = "alice.martin@example.com"
				},
				SecondSolidarBuilder = Guid.NewGuid(),
				SecondSolidarBuilderNavigation = new User
				{
					FirstName = "Benjamin",
					LastName = "Dupont",
					PhoneNumber = "+33 6.89.23.45.67",
					Email = "benjamin.dupont@example.com"
				},
				ThirdSolidarBuilder = Guid.NewGuid(),
				ThirdSolidarBuilderNavigation = new User
				{
					FirstName = "Charlotte",
					LastName = "Lemoine",
					PhoneNumber = "+33 6.33.67.89.90",
					Email = "charlotte.lemoine@example.com"
				},
				TrustedTierFirstName = "David",
				TrustedTierLastName = "Bertrand",
				TrustedTierEmail = "david.bertrand@test.com",
				TrustedTierPhoneNumber = "+33 6.68.87.12.45",
				TerritorialBuilder = Guid.NewGuid(),
				TerritorialBuilderNavigation = new User
				{
					FirstName = "Elodie",
					LastName = "Girard",
					PhoneNumber = "+33 6.12.34.56.78",
					Email = "elodie.girard@example.com"
				},
                SecondTerritorialBuilder = Guid.NewGuid(),
                SecondTerritorialBuilderNavigation = new User
                {
                    FirstName = "Andreas",
                    LastName = "Lila",
                    PhoneNumber = "+33 6.12.34.56.78",
                    Email = "lila.andreas@example.com"
                },
                DiffuseCoordinator = null,
				DiffuseCoordinatorNavigation = null,
				TargetCoordinator = userId,
				TargetCoordinatorNavigation = new User
				{
					Id = userId,
					FirstName = "Gabrielle",
					LastName = "Renaud",
					PhoneNumber = "+33 6.98.76.54.32",
					Email = "gabrielle.renaud@example.com",
					Role = new Role
					{
						Name = Constants.TargetedCoordinatorRole,
						LongName = "Coordinateur·rice du Ciblé·e"
					}
				}
			}
		};

		A.CallTo(() => _userRepositoryMock.GetUserById(
			A<Guid>._)).Returns(user);
		A.CallTo(() => _accompanyingFileRepositoryMock.GetAccompanyingFileSupportTeam(
			A<Guid>._)).Returns(accompanyingFile);

		var supportTeamNavigation = accompanyingFile.AccompanyingFileSupportTeamNavigation;
		var expectedResult = new List<SupportTeamMemberObjectResult>
		{
			new()
			{
				FullName =
					$"{supportTeamNavigation.SolidarBuilderNavigation.FirstName} {supportTeamNavigation.SolidarBuilderNavigation.LastName}",
				Email = supportTeamNavigation.SolidarBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.SolidarBuilderNavigation.PhoneNumber,
				Role = Labels.ReferentSolidarBuilder,
				IsEditable = true
			},
			new()
			{
				FullName =
					$"{supportTeamNavigation.SecondSolidarBuilderNavigation.FirstName} {supportTeamNavigation.SecondSolidarBuilderNavigation.LastName}",
				Email = supportTeamNavigation.SecondSolidarBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.SecondSolidarBuilderNavigation.PhoneNumber,
				Role = $"Second {Labels.ReferentSolidarBuilder}",
				IsEditable = true
			},
			new()
			{
				FullName =
					$"{supportTeamNavigation.ThirdSolidarBuilderNavigation.FirstName} {supportTeamNavigation.ThirdSolidarBuilderNavigation.LastName}",
				Email = supportTeamNavigation.ThirdSolidarBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.ThirdSolidarBuilderNavigation.PhoneNumber,
				Role = $"Troisième {Labels.ReferentSolidarBuilder}",
				IsEditable = true
			},
			new()
			{
				FullName = $"{supportTeamNavigation.TrustedTierFirstName} {supportTeamNavigation.TrustedTierLastName}",
				Email = supportTeamNavigation.TrustedTierEmail,
				PhoneNumber = supportTeamNavigation.TrustedTierPhoneNumber,
				Role = Labels.TrustedTier,
				IsEditable = true
			},
			new()
			{
				FullName = $"{supportTeamNavigation.TerritorialBuilderNavigation.FirstName} {supportTeamNavigation.TerritorialBuilderNavigation.LastName}",
				Email = supportTeamNavigation.TerritorialBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.TerritorialBuilderNavigation.PhoneNumber,
				Role = Labels.ReferentEt,
				IsEditable = true
			},
            new()
            {
                FullName = $"{supportTeamNavigation.SecondTerritorialBuilderNavigation.FirstName} {supportTeamNavigation.SecondTerritorialBuilderNavigation.LastName}",
                Email = supportTeamNavigation.SecondTerritorialBuilderNavigation.Email,
                PhoneNumber = supportTeamNavigation.SecondTerritorialBuilderNavigation.PhoneNumber,
                Role = Labels.SecondReferentEt,
                IsEditable = true
            },
            new()
			{
				FullName = $"{supportTeamNavigation.TargetCoordinatorNavigation.FirstName} {supportTeamNavigation.TargetCoordinatorNavigation.LastName}",
				Email = supportTeamNavigation.TargetCoordinatorNavigation.Email,
				PhoneNumber = supportTeamNavigation.TargetCoordinatorNavigation.PhoneNumber,
				Role = Labels.ReferentTargetedCoordinator,
				IsEditable = false
			}
		};

		// Act
		var request = new GetAllAccompanyingFileTaskAndSupportTeamQuery
		{
			AssociatedResourceId = Guid.NewGuid(),
			UserId = userId
		};
		var handler = new GetAllAccompanyingFileTaskAndSupportTeamQueryHandler(
			_taskRepositoryMock,
			_accompanyingFileRepositoryMock,
			_userRepositoryMock, 
			_copropertyProfileRepositoryMock,
			_telemetryService);
		var result = await handler.HandleQuery(request);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value!.SupportTeamMembers.Should().BeEquivalentTo(expectedResult);
	}

	[Fact]
	public async Task Handler_WhenUsersAccountAreDeleted_SupportTeamMemberInformationsShouldBeNull()
	{
		// Arrange
		var user = new User
		{
			Id = Guid.NewGuid(),
			Role = new Role
			{
				Name = Constants.SolidarBuilderRole,
				LongName = Labels.ReferentSolidarBuilder,
			}
		};
		var housing = CreateHousingWithInitialState();
		var accompanyingFile = new AccompanyingFile
		{
			Id = Guid.NewGuid(),
			AccompanyingFileHousing = housing.Id,
			AccompanyingFileHousingNavigation = housing,
			AccompanyingFileTerritory = Guid.NewGuid(),
			AccompanyingFileSupportTeam = Guid.NewGuid(),
			ZeroEnergyExclusionTerritoriesProgram = true,
			AccompanyingFileSupportTeamNavigation = new SupportTeam
			{
				SolidarBuilder = Guid.NewGuid(),
				SolidarBuilderNavigation = new User
				{
					FirstName = "Alice",
					LastName = "Martin",
					PhoneNumber = "+33 6.45.78.12.34",
					Email = "alice.martin@example.com",
					IsDeleted = true
				},
				SecondSolidarBuilder = Guid.NewGuid(),
				SecondSolidarBuilderNavigation = new User
				{
					FirstName = "Benjamin",
					LastName = "Dupont",
					PhoneNumber = "+33 6.89.23.45.67",
					Email = "benjamin.dupont@example.com"
				},
				ThirdSolidarBuilder = Guid.NewGuid(),
				ThirdSolidarBuilderNavigation = new User
				{
					FirstName = "Charlotte",
					LastName = "Lemoine",
					PhoneNumber = "+33 6.33.67.89.90",
					Email = "charlotte.lemoine@example.com",
					IsDeleted = true
				},
				TrustedTierFirstName = "David",
				TrustedTierLastName = "Bertrand",
				TrustedTierEmail = "david.bertrand@test.com",
				TrustedTierPhoneNumber = "+33 6.68.87.12.45",
				TerritorialBuilder = Guid.NewGuid(),
				TerritorialBuilderNavigation = new User
				{
					FirstName = "Elodie",
					LastName = "Girard",
					PhoneNumber = "+33 6.12.34.56.78",
					Email = "elodie.girard@example.com"
				},
                SecondTerritorialBuilder = Guid.NewGuid(),
                SecondTerritorialBuilderNavigation = new User
                {
                    FirstName = "Andreas",
                    LastName = "Lila",
                    PhoneNumber = "+33 6.12.34.56.78",
                    Email = "lila.andreas@example.com"
                },
                DiffuseCoordinator = null,
				DiffuseCoordinatorNavigation = null,
				TargetCoordinator = Guid.NewGuid(),
				TargetCoordinatorNavigation = new User
				{
					FirstName = "Gabrielle",
					LastName = "Renaud",
					PhoneNumber = "+33 6.98.76.54.32",
					Email = "gabrielle.renaud@example.com"
				},
			}
		};

		A.CallTo(() => _userRepositoryMock.GetUserById(
			A<Guid>._)).Returns(user);
		A.CallTo(() => _accompanyingFileRepositoryMock.GetAccompanyingFileSupportTeam(
			A<Guid>._)).Returns(accompanyingFile);

		var supportTeamNavigation = accompanyingFile.AccompanyingFileSupportTeamNavigation;
		var expectedResult = new List<SupportTeamMemberObjectResult>
		{
			new()
			{
				FullName = null,
				Email = null,
				PhoneNumber = null,
				Role = Labels.ReferentSolidarBuilder,
				IsEditable = true
			},
			new()
			{
				FullName =
					$"{supportTeamNavigation.SecondSolidarBuilderNavigation.FirstName} {supportTeamNavigation.SecondSolidarBuilderNavigation.LastName}",
				Email = supportTeamNavigation.SecondSolidarBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.SecondSolidarBuilderNavigation.PhoneNumber,
				Role = $"Second {Labels.ReferentSolidarBuilder}",
				IsEditable = true
			},
			new()
			{
				FullName = null,
				Email = null,
				PhoneNumber = null,
				Role = $"Troisième {Labels.ReferentSolidarBuilder}",
				IsEditable = true
			},
			new()
			{
				FullName = $"{supportTeamNavigation.TrustedTierFirstName} {supportTeamNavigation.TrustedTierLastName}",
				Email = supportTeamNavigation.TrustedTierEmail,
				PhoneNumber = supportTeamNavigation.TrustedTierPhoneNumber,
				Role = Labels.TrustedTier,
				IsEditable = true
			},
			new()
			{
				FullName = $"{supportTeamNavigation.TerritorialBuilderNavigation.FirstName} {supportTeamNavigation.TerritorialBuilderNavigation.LastName}",
				Email = supportTeamNavigation.TerritorialBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.TerritorialBuilderNavigation.PhoneNumber,
				Role = Labels.ReferentEt,
				IsEditable = false
			},
            new()
            {
                FullName = $"{supportTeamNavigation.SecondTerritorialBuilderNavigation.FirstName} {supportTeamNavigation.SecondTerritorialBuilderNavigation.LastName}",
                Email = supportTeamNavigation.SecondTerritorialBuilderNavigation.Email,
                PhoneNumber = supportTeamNavigation.SecondTerritorialBuilderNavigation.PhoneNumber,
                Role = Labels.SecondReferentEt,
                IsEditable = false
            },
            new()
			{
				FullName = $"{supportTeamNavigation.TargetCoordinatorNavigation.FirstName} {supportTeamNavigation.TargetCoordinatorNavigation.LastName}",
				Email = supportTeamNavigation.TargetCoordinatorNavigation.Email,
				PhoneNumber = supportTeamNavigation.TargetCoordinatorNavigation.PhoneNumber,
				Role = Labels.ReferentTargetedCoordinator,
				IsEditable = false
			}
		};

		// Act
		var request = new GetAllAccompanyingFileTaskAndSupportTeamQuery
		{
			AssociatedResourceId = Guid.NewGuid(),
			UserId = Guid.NewGuid()
		};
		var handler = new GetAllAccompanyingFileTaskAndSupportTeamQueryHandler(
			_taskRepositoryMock,
			_accompanyingFileRepositoryMock,
			_userRepositoryMock, 
			_copropertyProfileRepositoryMock,
			_telemetryService);
		var result = await handler.HandleQuery(request);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value!.SupportTeamMembers.Should().BeEquivalentTo(expectedResult);
	}

	[Fact]
	public async Task Handler_WhenUserIsAdmin_SupportTeamMembersResultShouldContainTheCorrectMembers()
	{
		// Arrange
		var user = new User
		{
			Id = Guid.NewGuid(),
			Role = new Role
			{
				Name = Constants.AdminRole,
			}
		};
		var housing = CreateHousingWithInitialState();
		var accompanyingFile = new AccompanyingFile
		{
			Id = Guid.NewGuid(),
			AccompanyingFileHousing = housing.Id,
			AccompanyingFileHousingNavigation = housing,
			AccompanyingFileTerritory = Guid.NewGuid(),
			AccompanyingFileSupportTeam = Guid.NewGuid(),
			ZeroEnergyExclusionTerritoriesProgram = true,
			AccompanyingFileSupportTeamNavigation = new SupportTeam
			{
				SolidarBuilder = Guid.NewGuid(),
				SolidarBuilderNavigation = new User
				{
					FirstName = "Alice",
					LastName = "Martin",
					PhoneNumber = "+33 6.45.78.12.34",
					Email = "alice.martin@example.com"
				},
				SecondSolidarBuilder = Guid.NewGuid(),
				SecondSolidarBuilderNavigation = new User
				{
					FirstName = "Benjamin",
					LastName = "Dupont",
					PhoneNumber = "+33 6.89.23.45.67",
					Email = "benjamin.dupont@example.com"
				},
				ThirdSolidarBuilder = Guid.NewGuid(),
				ThirdSolidarBuilderNavigation = new User
				{
					FirstName = "Charlotte",
					LastName = "Lemoine",
					PhoneNumber = "+33 6.33.67.89.90",
					Email = "charlotte.lemoine@example.com"
				},
				TrustedTierFirstName = "David",
				TrustedTierLastName = "Bertrand",
				TrustedTierEmail = "david.bertrand@test.com",
				TrustedTierPhoneNumber = "+33 6.68.87.12.45",
				TerritorialBuilder = Guid.NewGuid(),
				TerritorialBuilderNavigation = new User
				{
					FirstName = "Andreas",
					LastName = "Lila",
					PhoneNumber = "+33 6.12.34.56.78",
					Email = "lila.andreas@example.com"
				},
                SecondTerritorialBuilder = Guid.NewGuid(),
                SecondTerritorialBuilderNavigation = new User
                {
                    FirstName = "Elodie",
                    LastName = "Girard",
                    PhoneNumber = "+33 6.12.34.56.78",
                    Email = "elodie.girard@example.com"
                },
                DiffuseCoordinator = null,
				DiffuseCoordinatorNavigation = null,
				TargetCoordinator = Guid.NewGuid(),
				TargetCoordinatorNavigation = new User
				{
					FirstName = "Gabrielle",
					LastName = "Renaud",
					PhoneNumber = "+33 6.98.76.54.32",
					Email = "gabrielle.renaud@example.com"
				}
			}
		};

		A.CallTo(() => _userRepositoryMock.GetUserById(
			A<Guid>._)).Returns(user);
		A.CallTo(() => _accompanyingFileRepositoryMock.GetAccompanyingFileSupportTeam(
			A<Guid>._)).Returns(accompanyingFile);

		var supportTeamNavigation = accompanyingFile.AccompanyingFileSupportTeamNavigation;
		var expectedResult = new List<SupportTeamMemberObjectResult>
		{
			new()
			{
				FullName =
					$"{supportTeamNavigation.SolidarBuilderNavigation.FirstName} {supportTeamNavigation.SolidarBuilderNavigation.LastName}",
				Email = supportTeamNavigation.SolidarBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.SolidarBuilderNavigation.PhoneNumber,
				Role = Labels.ReferentSolidarBuilder,
				IsEditable = true
			},
			new()
			{
				FullName =
					$"{supportTeamNavigation.SecondSolidarBuilderNavigation.FirstName} {supportTeamNavigation.SecondSolidarBuilderNavigation.LastName}",
				Email = supportTeamNavigation.SecondSolidarBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.SecondSolidarBuilderNavigation.PhoneNumber,
				Role = $"Second {Labels.ReferentSolidarBuilder}",
				IsEditable = true
			},
			new()
			{
				FullName =
					$"{supportTeamNavigation.ThirdSolidarBuilderNavigation.FirstName} {supportTeamNavigation.ThirdSolidarBuilderNavigation.LastName}",
				Email = supportTeamNavigation.ThirdSolidarBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.ThirdSolidarBuilderNavigation.PhoneNumber,
				Role = $"Troisième {Labels.ReferentSolidarBuilder}",
				IsEditable = true
			},
			new()
			{
				FullName = $"{supportTeamNavigation.TrustedTierFirstName} {supportTeamNavigation.TrustedTierLastName}",
				Email = supportTeamNavigation.TrustedTierEmail,
				PhoneNumber = supportTeamNavigation.TrustedTierPhoneNumber,
				Role = Labels.TrustedTier,
				IsEditable = true
			},
			new()
			{
				FullName = $"{supportTeamNavigation.TerritorialBuilderNavigation.FirstName} {supportTeamNavigation.TerritorialBuilderNavigation.LastName}",
				Email = supportTeamNavigation.TerritorialBuilderNavigation.Email,
				PhoneNumber = supportTeamNavigation.TerritorialBuilderNavigation.PhoneNumber,
				Role = Labels.ReferentEt,
				IsEditable = true
			},
            new()
            {
                FullName = $"{supportTeamNavigation.SecondTerritorialBuilderNavigation.FirstName} {supportTeamNavigation.SecondTerritorialBuilderNavigation.LastName}",
                Email = supportTeamNavigation.SecondTerritorialBuilderNavigation.Email,
                PhoneNumber = supportTeamNavigation.SecondTerritorialBuilderNavigation.PhoneNumber,
                Role = Labels.SecondReferentEt,
                IsEditable = true
            },
			new()
			{
				FullName = null,
				Email = null,
				PhoneNumber = null,
				Role = Labels.ReferentDiffuseCoordinator,
				IsEditable = true
			},
			new()
			{
				FullName = $"{supportTeamNavigation.TargetCoordinatorNavigation.FirstName} {supportTeamNavigation.TargetCoordinatorNavigation.LastName}",
				Email = supportTeamNavigation.TargetCoordinatorNavigation.Email,
				PhoneNumber = supportTeamNavigation.TargetCoordinatorNavigation.PhoneNumber,
				Role = Labels.ReferentTargetedCoordinator,
				IsEditable = true
			}
		};

		// Act
		var request = new GetAllAccompanyingFileTaskAndSupportTeamQuery
		{
			AssociatedResourceId = Guid.NewGuid(),
			UserId = Guid.NewGuid()
		};
		var handler = new GetAllAccompanyingFileTaskAndSupportTeamQueryHandler(
			_taskRepositoryMock,
			_accompanyingFileRepositoryMock,
			_userRepositoryMock, 
			_copropertyProfileRepositoryMock,
			_telemetryService);
		var result = await handler.HandleQuery(request);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Value!.SupportTeamMembers.Should().BeEquivalentTo(expectedResult);
	}

	private static Housing CreateHousingWithInitialState()
	{
		var housingInitialState = new HousingInitialState
		{
			Id = Guid.NewGuid(),
			DegradationIndex = (int)DegradationIndex.Medium,
			UnsanitaryCoefficient = (int)UnsanitaryCoefficient.Low
		};

		return new Housing
		{
			Id = Guid.NewGuid(),
			HousingInitialState = housingInitialState.Id,
			GeographicAreaTypology = (int)GeographicalHousingAreaTypology.Rural,
			HousingInitialStateNavigation = housingInitialState
		};
	}
}
