using FluentAssertions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.
	WorkPackage;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.
	WorkPackage.ViewModel;
using Renee.UI.Components.FormComponents;

namespace UITests.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details;

public class WorkPackageTests
{
	[Fact]
	public void TotalPrice_WhenAWorkTypeIsSelectedWithAPriceOf20_ShouldReturn20()
	{
		// Arrange
		var viewModel = new WorkPackageViewModel
		{
			WorkTypes = [new WorkPackageViewModel.WorkType(Guid.NewGuid(), "WorkType1", false) { Price = 20 }]
		};

		// Act
		var totalPrice = viewModel.TotalPrice;

		// Assert
		totalPrice.Should().Be(20);
	}

	[Fact]
	public void WhenWorktypeIsSelected_TwoFieldsRelatedToThisWorktypeShouldBeDisplayed()
	{
		// Arrange
		var ctx = new BunitContext();
		ctx.Services.AddSingleton(A.Fake<NotificationService>());
		ctx.JSInterop.Mode = JSRuntimeMode.Loose;

		var id = Guid.NewGuid();
		var viewModel = new WorkPackageViewModel { WorkTypes = [new WorkPackageViewModel.WorkType(id, "WorkType1", false)] };

		var cut = ctx.Render<WorkPackage>(
			parameters => parameters.Add(p => p.ViewModel, viewModel)
				.Add(p => p.WorkTypes, [new ZeeSelectItem<Guid>("WorkType1", id)]).Add(p => p.Index, 1)
				.Add(p => p.EditContext, new EditContext(viewModel)));
		// Act
		var priceFieldDisplayed = cut.FindComponents<OutlinedDouble>()
			.FirstOrDefault(p => p.Instance.Label!.Equals("WorkType1 (€ TTC)"));
		var descriptionFieldDisplayed = cut.FindComponents<OutlinedText>()
			.FirstOrDefault(p => p.Instance.Label!.Equals("Précisions sur WorkType1"));

		// Assert
		priceFieldDisplayed.Should().NotBeNull();
		descriptionFieldDisplayed.Should().NotBeNull();
	}
}