using System.Globalization;
using Renee.Application.Helpers;
using Renee.Domain.Enums;

namespace CommandHandlerTests.AccompanyingFileTests.ImportExcelData;

public class ImportExcelDataHelperTests : TestContext
{
	// Boolean
	[Fact]
	public void TryParseValue_ShouldParseBoolean()
	{
		var input = "Oui";
		var expectedOutput = true;
		var expectedResult = true;

		var result = ImportFileDataHelper.TryParseValue<bool>(input, out var output);
		result.Should().Be(expectedResult);
		output.Should().Be(expectedOutput);
	}

	// Datetime
	[Fact]
	public void TryParseValue_ShouldParseDatetime()
	{
		var input = "01/01/2022";
		var expectedOutput = DateTime.Parse("01/01/2022", CultureInfo.InvariantCulture);
		var expectedResult = true;

		var result = ImportFileDataHelper.TryParseValue<DateTime>(input, out var output);
		result.Should().Be(expectedResult);
		output.Should().Be(expectedOutput);
	}

	// Double
	[Fact]
	public void TryParseValue_ShouldParseDouble()
	{
		var input = "450000,200";
		var expectedOutput = 450000.200;
		var expectedResult = true;

		var originalCulture = CultureInfo.CurrentCulture;

		CultureInfo.CurrentCulture = new CultureInfo("fr-FR");

		var result = ImportFileDataHelper.TryParseValue<double>(input, out var output);

		result.Should().Be(expectedResult);
		output.Should().Be(expectedOutput);
	}

	// Enum
	[Fact]
	public void TryParseValue_ShouldParseEnum()
	{
		var input = "Résidentiel collectif";
		var expectedOutput = HousingType.ResidentialCollective;
		var expectedResult = true;

		var result = ImportFileDataHelper.TryParseValue<HousingType>(input, out var output);
		result.Should().Be(expectedResult);
		output.Should().Be(expectedOutput);
	}

	// Integer
	[Fact]
	public void TryParseValue_ShouldParseInteger()
	{
		var input = "5";
		var expectedOutput = 5;
		var expectedResult = true;

		var result = ImportFileDataHelper.TryParseValue<int>(input, out var output);
		result.Should().Be(expectedResult);
		output.Should().Be(expectedOutput);
	}

	[Fact]
	public void TryParseValue_WhenBooleanIsNotNullableWithNullableValue_ShouldNotParseBoolean()
	{
		var input = "";
		var expectedResult = false;
		var result = ImportFileDataHelper.TryParseValue<bool>(input, out _);
		result.Should().Be(expectedResult);
	}

	[Fact]
	public void TryParseValue_WhenBooleanIsNullableWithNullableValue_ShouldParseNullableBoolean()
	{
		var input = "";
		bool? expectedOutput = null;
		var expectedResult = true;

		var result = ImportFileDataHelper.TryParseValue<bool?>(input, out var output);
		result.Should().Be(expectedResult);
		output.Should().Be(expectedOutput);
	}

	[Fact]
	public void TryParseValue_WhenDatetimeIsNotNullableWithNullableValue_ShouldNotParseDatetime()
	{
		var input = "";
		var expectedResult = false;
		var result = ImportFileDataHelper.TryParseValue<DateTime>(input, out _);
		result.Should().Be(expectedResult);
	}

	[Fact]
	public void TryParseValue_WhenDatetimeIsNullableWithNullableValue_ShouldParseNullableDatetime()
	{
		var input = "";
		DateTime? expectedOutput = null;
		var expectedResult = true;

		var result = ImportFileDataHelper.TryParseValue<DateTime?>(input, out var output);
		result.Should().Be(expectedResult);
		output.Should().Be(expectedOutput);
	}

	[Fact]
	public void TryParseValue_WhenDoubleIsNotNullableWithNullableValue_ShouldNotParseDouble()
	{
		var input = "";
		double expectedOutput = default;
		var expectedResult = false;

		var result = ImportFileDataHelper.TryParseValue<double>(input, out var output);
		result.Should().Be(expectedResult);
		output.Should().Be(expectedOutput);
	}

	[Fact]
	public void TryParseValue_WhenDoubleIsNullableWithNullableValue_ShouldParseNullableDouble()
	{
		var input = "";
		double? expectedOutput = null;
		var expectedResult = true;

		var result = ImportFileDataHelper.TryParseValue<double?>(input, out var output);
		result.Should().Be(expectedResult);
		output.Should().Be(expectedOutput);
	}

	[Fact]
	public void TryParseValue_WhenEnumIsNotNullableWithNullableValue_ShouldNotParseEnum()
	{
		var input = "";
		var expectedResult = false;
		var result = ImportFileDataHelper.TryParseValue<GeographicalHousingAreaTypology>(input, out _);
		result.Should().Be(expectedResult);
	}

	[Fact]
	public void TryParseValue_WhenEnumIsNullableWithNullableValue_ShouldParseNullableEnum()
	{
		var input = "";
		HouseholdTypology? expectedOutput = null;
		var expectedResult = true;

		var result = ImportFileDataHelper.TryParseValue<HouseholdTypology?>(input, out var output);
		result.Should().Be(expectedResult);
		output.Should().Be(expectedOutput);
	}

	[Fact]
	public void TryParseValue_WhenIntegerIsNotNullableWithNullableValue_ShouldNotParseInteger()
	{
		var input = "";
		int expectedOutput = default;
		var expectedResult = false;

		var result = ImportFileDataHelper.TryParseValue<int>(input, out var output);
		result.Should().Be(expectedResult);
		output.Should().Be(expectedOutput);
	}

	[Fact]
	public void TryParseValue_WhenIntegerIsNullableWithNullableValue_ShouldParseNullableInteger()
	{
		var input = "";
		int? expectedOutput = null;
		var expectedResult = true;

		var result = ImportFileDataHelper.TryParseValue<int?>(input, out var output);
		result.Should().Be(expectedResult);
		output.Should().Be(expectedOutput);
	}

	[Fact]
	public void TryParseValue_WhenIsIntegerAndNullValue_ShouldReturnFalse()
	{
		var input = "test";
		var expectedResult = false;

		var result = ImportFileDataHelper.TryParseValue<int>(input, out var output);
		result.Should().Be(expectedResult);
		output.Should().Be(default);
	}

	[Fact]
	public void TryParseValue_WhenIsNullableStringAndNotNullValue_ShouldReturnTrue()
	{
		var input = "test";
		var expectedOutput = "test";
		var expectedResult = true;

		var result = ImportFileDataHelper.TryParseValue<string?>(input, out var output);
		result.Should().Be(expectedResult);
		output.Should().Be(expectedOutput);
	}

	// String
	[Fact]
	public void TryParseValue_WhenIsNullableStringAndNullValue_ShouldReturnTrueAndDefaultValue()
	{
		var input = "";
		string? expectedOutput = default;
		var expectedResult = true;

		var result = ImportFileDataHelper.TryParseValue<string?>(input, out var output);
		result.Should().Be(expectedResult);
		output.Should().Be(expectedOutput);
	}
}