using Renee.Application.Helpers;

namespace CommandHandlerTests.Helper;

public class StatisticHelperTest
{
	[Fact]
	public void PrivationRateAverage_WhenNoAccompanyingFileWithTotalPrivationRate_ShouldReturnZero()
	{
		// Arrange
		var accompanyingFiles = new List<AccompanyingFile>
		{
			new()
			{
				AccompanyingFileHousingNavigation = new Housing
				{
					HousingInitialStateNavigation = new HousingInitialState { EnergyDepravation = 1 }
				}
			},
			new()
			{
				AccompanyingFileHousingNavigation = new Housing
				{
					HousingInitialStateNavigation = new HousingInitialState { EnergyDepravation = 0 }
				}
			}
		};

		// Act
		var result = StatisticsHelper.GetEnergyPrivationRate(accompanyingFiles);

		// Assert
		result.Should().Be(0);
	}

	[Fact]
	public void PrivationRateAverage_WhenOneAccompanyingFileWithTotalPrivationRateInAListOfTwo_ShouldReturnZeroAndHalf()
	{
		// Arrange
		var accompanyingFiles = new List<AccompanyingFile>
		{
			new()
			{
				AccompanyingFileHousingNavigation = new Housing
				{
					HousingInitialStateNavigation = new HousingInitialState { EnergyDepravation = 2 }
				}
			},
			new()
			{
				AccompanyingFileHousingNavigation = new Housing
				{
					HousingInitialStateNavigation = new HousingInitialState { EnergyDepravation = 0 }
				}
			}
		};

		// Act
		var result = StatisticsHelper.GetEnergyPrivationRate(accompanyingFiles);

		// Assert
		result.Should().Be(0.5);
	}
}