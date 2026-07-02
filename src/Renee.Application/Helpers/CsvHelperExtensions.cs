using Renee.Application.Services.Administration.ImportCsvData;
using Renee.Domain;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Renee.Application.Helpers;

public static class CsvHelperExtensions
{
    public static string CsvName<TProp>(Expression<Func<CsvData, TProp>> expr)
    {
        if (expr.Body is MemberExpression memberExpr &&
            memberExpr.Member is PropertyInfo propertyInfo)
        {
            var description = propertyInfo.GetCustomAttribute<DescriptionAttribute>();
            return description?.Description ?? propertyInfo.Name;
        }
        return expr.Body.ToString();
    }

    public static List<string> ValidateRequiredFields(CsvData csvData)
    {
        var errors = new List<string>();
        var requiredFields = new Dictionary<string, string>
        {
            { nameof(csvData.FirstName), CsvDataLabel.CsvColumnsNames.FirstName },
            { nameof(csvData.LastName), CsvDataLabel.CsvColumnsNames.LastName },
            { nameof(csvData.ZeroEnergyExclusionTerritoriesProgram), CsvDataLabel.CsvColumnsNames.ZeroEnergyExclusionTerritoriesProgram },
            { nameof(csvData.GeographicAreaTypology), CsvDataLabel.CsvColumnsNames.GeographicAreaTypology },
            { nameof(csvData.Label), CsvDataLabel.CsvColumnsNames.Label },
            { nameof(csvData.PostalCode), CsvDataLabel.CsvColumnsNames.PostalCode },
            { nameof(csvData.City), CsvDataLabel.CsvColumnsNames.City },
            { nameof(csvData.Department), CsvDataLabel.CsvColumnsNames.Department },
            { nameof(csvData.Region), CsvDataLabel.CsvColumnsNames.Region },
            { nameof(csvData.SolidarBuilder), CsvDataLabel.CsvColumnsNames.SolidarBuilder },
            { nameof(csvData.MarkerNature), CsvDataLabel.CsvColumnsNames.MarkerNature },
            { nameof(csvData.IsDeleted), CsvDataLabel.CsvColumnsNames.IsDeleted },
            { nameof(csvData.Reference), CsvDataLabel.CsvColumnsNames.Reference }
        };

        foreach (var field in requiredFields)
        {
            var value = typeof(CsvData).GetProperty(field.Key)?.GetValue(csvData) as string;
            if (string.IsNullOrEmpty(value))
            {
                errors.Add(string.Format(CsvDataLabel.Errors.RequiredFieldToCreateAccompanyingFile, field.Value));
            }
        }

        return errors;
    }

    public static void ReportErrors(
            List<LineErrorReport> lineErrorReports,
            LineErrorReport lineErrors,
            params List<string> errors)
    {
        lineErrors.AddErrors([.. errors]);
        lineErrorReports.Add(lineErrors);
    }
}