using Renee.Application.Abstraction.Query;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Task;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;
using Renee.Application.Helpers;

namespace Renee.Application.Handlers.QueryHandlers.Task;

public class GetAllAccompanyingFileTaskAndSupportTeamQueryHandler(
	ITaskRepository taskRepository,
	IAccompanyingFileRepository accompanyingFileRepository,
	IUserRepository userRepository,
	ICopropertyProfileRepository copropertyProfileRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetAllAccompanyingFileTaskAndSupportTeamQuery,
		ReneeOperationResult<GetAllAccompanyingFileTasksAndSupportTeamQueryObjectResult>>
{
	public override async Task<ReneeOperationResult<GetAllAccompanyingFileTasksAndSupportTeamQueryObjectResult>> HandleQuery(
		GetAllAccompanyingFileTaskAndSupportTeamQuery request)
	{
		try
		{
			var connectedUser = await userRepository.GetUserById(request.UserId);

			var supportTeamMembers = new List<SupportTeamMemberObjectResult>();

			if (!request.IsCoproperty)
			{
				var tasks = await taskRepository.GetAccompanyingFileTasks(request.AssociatedResourceId);
				var accompanyingFile = await accompanyingFileRepository.GetAccompanyingFileSupportTeam(request.AssociatedResourceId);
				var targetInformation = accompanyingFile is not null
					? BuildAccompanyingFileTargetInformationObjectResult(accompanyingFile)
					: null;

				if (accompanyingFile is not null && connectedUser is not null)
					supportTeamMembers = BuildSupportTeamObjectResultList(accompanyingFile.AccompanyingFileSupportTeamNavigation, connectedUser);

				return ReneeOperationResult<GetAllAccompanyingFileTasksAndSupportTeamQueryObjectResult>.Success(new GetAllAccompanyingFileTasksAndSupportTeamQueryObjectResult
				{
					AccompanyingFileReference = accompanyingFile?.AccompanyingFileReference ?? string.Empty,
					Tasks = tasks.Select(
						t => new TaskObjectResult
						{
							TaskId = t.Id,
							TaskName = t.Title,
							Priority = (TaskPriority)t.Priority,
							DueDate = t.DueDate,
							StartDate = t.StartDate,
							AssignedUserName = $"{t.AssignedUser.FirstName} {t.AssignedUser.LastName}",
							ProgressTask = (ProgressTask?)t.Progress
						}).ToList(),
					SupportTeamMembers = supportTeamMembers,
					IsUserInSupportTeam = accompanyingFile?.AccompanyingFileSupportTeamNavigation.SolidarBuilder == request.UserId ||
						accompanyingFile?.AccompanyingFileSupportTeamNavigation.TerritorialBuilder == request.UserId ||
						accompanyingFile?.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilder == request.UserId ||
						accompanyingFile?.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilder == request.UserId ||
						accompanyingFile?.AccompanyingFileSupportTeamNavigation.DiffuseCoordinator == request.UserId ||
						accompanyingFile?.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilder == request.UserId ||
						accompanyingFile?.AccompanyingFileSupportTeamNavigation.TargetCoordinator == request.UserId,
					TargetInformation = targetInformation,
					BillingLog = GetBillingLogObjectResult(accompanyingFile?.AccompanyingFileBillingLog)
				});
			}

			var copropertyProfile = await copropertyProfileRepository.GetCopropertyProfileSupportTeam(request.AssociatedResourceId);
			if (copropertyProfile is not null && connectedUser is not null) supportTeamMembers = BuildSupportTeamObjectResultList(copropertyProfile.CopropertySupportTeamNavigation, connectedUser);

			return ReneeOperationResult<GetAllAccompanyingFileTasksAndSupportTeamQueryObjectResult>.Success(new GetAllAccompanyingFileTasksAndSupportTeamQueryObjectResult
			{
				Tasks = null,
				SupportTeamMembers = supportTeamMembers,
				IsUserInSupportTeam = copropertyProfile?.CopropertySupportTeamNavigation.SolidarBuilder == request.UserId ||
				copropertyProfile?.CopropertySupportTeamNavigation.TerritorialBuilder == request.UserId ||
				copropertyProfile?.CopropertySupportTeamNavigation.SecondSolidarBuilder == request.UserId ||
				copropertyProfile?.CopropertySupportTeamNavigation.ThirdSolidarBuilder == request.UserId ||
				copropertyProfile?.CopropertySupportTeamNavigation.DiffuseCoordinator == request.UserId ||
				copropertyProfile?.CopropertySupportTeamNavigation.SecondTerritorialBuilder == request.UserId ||
				copropertyProfile?.CopropertySupportTeamNavigation.TargetCoordinator == request.UserId,
				TargetInformation = null
			});
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<GetAllAccompanyingFileTasksAndSupportTeamQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}

	private static AccompanyingFileTargetInformationObjectResult BuildAccompanyingFileTargetInformationObjectResult(Domain.Entity.AccompanyingFile accompanyingFile)
	{
		return new AccompanyingFileTargetInformationObjectResult
		{
			GeographicalHousingAreaTypology = (GeographicalHousingAreaTypology?)accompanyingFile.AccompanyingFileHousingNavigation.GeographicAreaTypology,
			AccompanyingType = (AccompanyingType?)accompanyingFile.AccompanyingType,
			AccompanyingTerritory = accompanyingFile.AccompanyingFileTerritory,
			MarValue = AccompanyingFileHelper.GetMar(
				(DegradationIndex?)accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.DegradationIndex,
				(UnsanitaryCoefficient?)accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.UnsanitaryCoefficient),
			IsInTzeeProgram = accompanyingFile.ZeroEnergyExclusionTerritoriesProgram!.Value
		};
	}
	
	private static BillingLogObjectResult? GetBillingLogObjectResult(Domain.Entity.AccompanyingFileBillingLog? billingLog)
	{
		if (billingLog == null)
			return null;

		return new BillingLogObjectResult
		{
			BilledJalon1 = billingLog.BilledJalon1,
			AmountBilledFirstStage = billingLog.AmountBilledFirstStage,
			FundraisingLauchDateForFirstStage = billingLog.FundraisingLauchDateForFirstStage,
			BillingCallNumberFirstStage = billingLog.BillingCallNumberFirstStage,
			InvoiceNumberFirstStage = billingLog.InvoiceNumberFirstStage,
			BillingDateFirstStage = billingLog.BillingDateFirstStage,
			BilledJalon2 = billingLog.BilledJalon2,
			AmountBilledSecondStage = billingLog.AmountBilledSecondStage,
			FundraisingLauchDateForSecondStage = billingLog.FundraisingLauchDateForSecondStage,
			BillingCallNumberSecondStage = billingLog.BillingCallNumberSecondStage,
			InvoiceNumberSecondStage = billingLog.InvoiceNumberSecondStage,
			BillingDateSecondStage =  billingLog.BillingDateSecondStage,
			BilledJalon3 = billingLog.BilledJalon3,
			AmountBilledThirdStage = billingLog.AmountBilledThirdStage,
			FundraisingLauchDateForThirdStage = billingLog.FundraisingLauchDateForThirdStage,
			BillingCallNumberThirdStage = billingLog.BillingCallNumberThirdStage,
			InvoiceNumberThirdStage = billingLog.InvoiceNumberThirdStage,
			BillingDateThirdStage = billingLog.BillingDateThirdStage
		};
	}

	private static List<SupportTeamMemberObjectResult> BuildSupportTeamObjectResultList(
		SupportTeam supportTeam,
		Domain.Entity.User connectedUser)
	{
		var supportTeamMembers = new List<SupportTeamMemberObjectResult>();
		var supportTeamNavigation = supportTeam;

        supportTeamMembers.Add(MapUserToSupportTeamObjectResult(
			supportTeamNavigation.SolidarBuilderNavigation,
			Labels.ReferentSolidarBuilder,
			connectedUser));

		supportTeamMembers.Add(MapUserToSupportTeamObjectResult(
			supportTeamNavigation.SecondSolidarBuilderNavigation,
			Labels.SecondReferentSolidarBuilder,
			connectedUser));

		supportTeamMembers.Add(MapUserToSupportTeamObjectResult(
			supportTeamNavigation.ThirdSolidarBuilderNavigation,
			Labels.ThirdReferentSolidarBuilder,
			connectedUser));

		supportTeamMembers.Add(new SupportTeamMemberObjectResult
		{
			FullName = supportTeamNavigation.TrustedTierFirstName is not null && supportTeamNavigation.TrustedTierLastName is not null
				? $"{supportTeamNavigation.TrustedTierFirstName} {supportTeamNavigation.TrustedTierLastName}"
				: null,
			Email = supportTeamNavigation.TrustedTierEmail,
			PhoneNumber = supportTeamNavigation.TrustedTierPhoneNumber,
			Role = Labels.TrustedTier,
			IsEditable = connectedUser.Role.Name != Constants.StructuralReferentRole
		});

		AddSupportTeamMember(supportTeamNavigation.TerritorialBuilderNavigation, Labels.ReferentEt);
        AddSupportTeamMember(supportTeamNavigation.SecondTerritorialBuilderNavigation, Labels.SecondReferentEt);
        AddSupportTeamMember(supportTeamNavigation.DiffuseCoordinatorNavigation, Labels.ReferentDiffuseCoordinator);
		AddSupportTeamMember(supportTeamNavigation.TargetCoordinatorNavigation, Labels.ReferentTargetedCoordinator);

		return supportTeamMembers;

		static SupportTeamMemberObjectResult MapUserToSupportTeamObjectResult(
			Domain.Entity.User? supportTeamMember,
			string supportTeamMemberRole,
			Domain.Entity.User connectedUser)
		{
			return new SupportTeamMemberObjectResult
			{
				FullName = supportTeamMember is not null && !supportTeamMember.IsDeleted
					? $"{supportTeamMember.FirstName} {supportTeamMember.LastName}"
					: null,
				Email = supportTeamMember is not null && !supportTeamMember.IsDeleted 
					? supportTeamMember.Email 
					: null,
				PhoneNumber = supportTeamMember is not null && !supportTeamMember.IsDeleted 
					? supportTeamMember.PhoneNumber
					: null,
				Role = supportTeamMemberRole,
				IsEditable = CanUserEditSupportTeamMember(
					supportTeamMemberRole,
					connectedUser)
			};
		}

		void AddSupportTeamMember(Domain.Entity.User? userNavigation, string roleLabel)
		{
			if (userNavigation is not null || CanUserEditSupportTeamMember(roleLabel, connectedUser))
			{
                supportTeamMembers.Add(MapUserToSupportTeamObjectResult(userNavigation, roleLabel, connectedUser));
			}
		}

		static bool CanUserEditSupportTeamMember(
			string supportTeamMemberRole,
			Domain.Entity.User connectedUser)
		{
			if (connectedUser.Role.Name == Constants.StructuralReferentRole)
				return false;

			if (supportTeamMemberRole.Contains(Labels.ReferentSolidarBuilder) ||
				connectedUser.Role.Name == Constants.AdminRole) 
				return true;

			var connectedUserRoleFormated = $"{connectedUser.Role.LongName} Référent·e";

			var roles = new[]
			{
				Constants.TerritorialBuilderRole,
				Constants.DiffuseCoordinatorRole,
				Constants.TargetedCoordinatorRole
			};

			if (supportTeamMemberRole == Labels.SecondReferentEt && 
				roles.Contains(connectedUser.Role.Name) && connectedUser.Role.Name!= Constants.DiffuseCoordinatorRole)
				return true;

			// Les coordinateurs (Diffuse et Targeted) ne peuvent PAS s'éditer eux-mêmes
			// Seuls les TerritorialBuilder peuvent s'éditer eux-mêmes
			if (connectedUser.Role.Name == Constants.DiffuseCoordinatorRole || 
				connectedUser.Role.Name == Constants.TargetedCoordinatorRole)
			{
				// Le TargetedCoordinator peut éditer le TerritorialBuilder mais pas lui-même
				return connectedUser.Role.Name == Constants.TargetedCoordinatorRole && 
					supportTeamMemberRole == Labels.ReferentEt;
			}

			return roles.Contains(connectedUser.Role.Name) &&
				supportTeamMemberRole == connectedUserRoleFormated;
		}
	}
}