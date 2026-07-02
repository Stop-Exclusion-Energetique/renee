using System.Drawing;

namespace Renee.UI.Components.Features.Index.GraphicsObjects;

public abstract class GraphicalObjects
{
	public required string Name { get; set; }
	public Color? Color { get; set; }
}

public class VerticalChart : GraphicalObjects
{
	public required int VerticalAxisValue { get; set; }
}

public class PieChart : GraphicalObjects
{
	public required double PieAxisValue { get; set; }
}

public class HorizontalChart : GraphicalObjects
{
	public required double HorizontalAxisValue { get; set; }
}