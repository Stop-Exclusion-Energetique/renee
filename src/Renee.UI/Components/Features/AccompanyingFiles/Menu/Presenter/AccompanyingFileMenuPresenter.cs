using Renee.Application.Helpers;
using Renee.Application.Queries.Task;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.Billing.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.SupportTeam.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.TargetInformations.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.Tasks.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Menu.Presenter;

public class AccompanyingFileMenuPresenter
{
	public string AccompanyingFileReference { get; set; } = string.Empty;
	private List<TaskViewModel> Tasks { get; set; } = new();
	private List<TaskViewModel> DoneTasks { get; set; } = new();

	private List<TaskViewModel> ToBeCompletedTask { get; set; } = new();
	private List<TaskViewModel> UpcomingTasks { get; set; } = new();
	private List<SupportTeamMemberViewModel> SupportTeamMembers { get; set; } = new();
	private bool IsUserInSupportTeam { get; set; }

	private AccompanyingFileTargetDetailsViewModel? TargetInformation { get; set; } 
	private BillingLogViewModel BillingLog { get; set; } = new();

	public AccompanyingFileMenuPresenter FromQuery(
		GetAllAccompanyingFileTasksAndSupportTeamQueryObjectResult queryResult)
	{
		AccompanyingFileReference = queryResult.AccompanyingFileReference;

		var rawTasks = queryResult.Tasks ?? [];

		static TaskViewModel MapToViewModel(TaskObjectResult qr) => new()
		{
			TaskId = qr.TaskId,
			Priority = qr.Priority,
			Title = qr.TaskName,
			DueDate = qr.DueDate,
			StartDate = qr.StartDate,
			AssignedUserFullName = qr.AssignedUserName,
			ProgressTask = qr.ProgressTask
		};

		UpcomingTasks     = rawTasks.UpcomingTasks().Select(MapToViewModel).ToList();
		Tasks             = rawTasks.CurrentTasks().Select(MapToViewModel).ToList();
		ToBeCompletedTask = rawTasks.ToBeCompletedTasks().Select(MapToViewModel).ToList();
		DoneTasks         = rawTasks.DoneTasks().Select(MapToViewModel).ToList();

		SupportTeamMembers = queryResult.SupportTeamMembers.Select(
			qr => new SupportTeamMemberViewModel
			{
				FullName = qr.FullName, Email = qr.Email, PhoneNumber = qr.PhoneNumber, Role = qr.Role, IsEditable = qr.IsEditable
			}).ToList();

		IsUserInSupportTeam = queryResult.IsUserInSupportTeam;

		if (queryResult.TargetInformation is not null)
		{
			TargetInformation = new AccompanyingFileTargetDetailsViewModel
			{
				GeographicalHousingAreaTypology = queryResult.TargetInformation.GeographicalHousingAreaTypology,
				AccompanyingType = queryResult.TargetInformation.AccompanyingType,
				AccompanyingTerritory = queryResult.TargetInformation.AccompanyingTerritory,
				MarValue = queryResult.TargetInformation.MarValue,
				IsInTzeeProgram = queryResult.TargetInformation.IsInTzeeProgram
			};
		}

		if (queryResult.BillingLog is not null)
		{
			BillingLog.BilledJalon1 = queryResult.BillingLog.BilledJalon1;
			BillingLog.AmountBilledFirstStage = queryResult.BillingLog.AmountBilledFirstStage;
			BillingLog.FundraisingLauchDateForFirstStage = queryResult.BillingLog.FundraisingLauchDateForFirstStage;
			BillingLog.BillingCallNumberFirstStage = queryResult.BillingLog.BillingCallNumberFirstStage;
			BillingLog.InvoiceNumberFirstStage = queryResult.BillingLog.InvoiceNumberFirstStage;
			BillingLog.BillingDateFirstStage = queryResult.BillingLog.BillingDateFirstStage;

			BillingLog.BilledJalon2 = queryResult.BillingLog.BilledJalon2;
			BillingLog.AmountBilledSecondStage = queryResult.BillingLog.AmountBilledSecondStage;
			BillingLog.FundraisingLauchDateForSecondStage = queryResult.BillingLog.FundraisingLauchDateForSecondStage;
			BillingLog.BillingCallNumberSecondStage = queryResult.BillingLog.BillingCallNumberSecondStage;
			BillingLog.InvoiceNumberSecondStage = queryResult.BillingLog.InvoiceNumberSecondStage;
			BillingLog.BillingDateSecondStage = queryResult.BillingLog.BillingDateSecondStage;
			
			BillingLog.BilledJalon3 = queryResult.BillingLog.BilledJalon3;
			BillingLog.AmountBilledThirdStage = queryResult.BillingLog.AmountBilledThirdStage;
			BillingLog.FundraisingLauchDateForThirdStage = queryResult.BillingLog.FundraisingLauchDateForThirdStage;
			BillingLog.BillingCallNumberThirdStage = queryResult.BillingLog.BillingCallNumberThirdStage;
			BillingLog.InvoiceNumberThirdStage = queryResult.BillingLog.InvoiceNumberThirdStage;
			BillingLog.BillingDateThirdStage = queryResult.BillingLog.BillingDateThirdStage;			
		}

		return this;
	}

	public AccompanyingFileMenuViewModel Present()
	{
		return new AccompanyingFileMenuViewModel
		{
			AccompanyingFileReference = AccompanyingFileReference,
			Tasks = Tasks,
			DoneTasks = DoneTasks,
			ToBeCompletedTask = ToBeCompletedTask,
			UpcomingTasks = UpcomingTasks,
			SupportTeamMembers = SupportTeamMembers,
			ShouldDisplayTask = IsUserInSupportTeam,
			TargetInformation = TargetInformation,
			BillingLog = BillingLog
		};
	}
}