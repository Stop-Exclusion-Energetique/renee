using Radzen.Blazor;
using Renee.Domain;

namespace Renee.UI.Components.DisplayComponents;

public class ZeeDataGrid<TItem> : RadzenDataGrid<TItem>
{
	public ZeeDataGrid() : base()
	{
		base.AndOperatorText = RadzenGridFilterLabel.AndOperatorText;
		base.OrOperatorText = RadzenGridFilterLabel.OrOperatorText;
		base.EqualsText = RadzenGridFilterLabel.EqualsText;
		base.NotEqualsText = RadzenGridFilterLabel.NotEqualsText;
		base.LessThanText = RadzenGridFilterLabel.LessThanText;
		base.LessThanOrEqualsText = RadzenGridFilterLabel.LessThanOrEqualsText;
		base.GreaterThanText = RadzenGridFilterLabel.GreaterThanText;
		base.GreaterThanOrEqualsText = RadzenGridFilterLabel.GreaterThanOrEqualsText;
		base.IsNullText = RadzenGridFilterLabel.IsNullText;
		base.IsNotNullText = RadzenGridFilterLabel.IsNotNullText;
		base.IsEmptyText = RadzenGridFilterLabel.IsEmptyText;
		base.IsNotEmptyText = RadzenGridFilterLabel.IsNotEmptyText;
		base.ContainsText = RadzenGridFilterLabel.ContainsText;
		base.DoesNotContainText = RadzenGridFilterLabel.DoesNotContainText;
		base.StartsWithText = RadzenGridFilterLabel.StartsWithText;
		base.EndsWithText = RadzenGridFilterLabel.EndsWithText;
		base.ClearFilterText = RadzenGridFilterLabel.ClearFilterText;
		base.ApplyFilterText = RadzenGridFilterLabel.ApplyFilterText;
		base.FilterText = RadzenGridFilterLabel.FilterText;
	}
}