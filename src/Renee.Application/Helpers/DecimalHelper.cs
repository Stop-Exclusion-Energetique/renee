namespace Renee.Application.Helpers;

public static class DecimalHelper
{
	public static bool AlmostEquals(double a, double b)
	{
		return Math.Abs(a - b) <= 0.001;
	}
}