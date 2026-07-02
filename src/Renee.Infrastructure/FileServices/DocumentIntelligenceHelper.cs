using System.Text;
using Azure.AI.DocumentIntelligence;

namespace Renee.Infrastructure.FileServices;

public static class DocumentIntelligenceHelper
{
    public static string ProcessContent(string raw) =>
        raw.Replace(":selected:", "☑")
           .Replace(":unselected:", "☐")
           .Replace("\f", "\n---\n");

    public static string RenderTables(AnalyzeResult result)
    {
        if (result.Tables is null || result.Tables.Count == 0)
            return string.Empty;

        var sb = new StringBuilder();
        foreach (var table in result.Tables)
        {
            var grid = new string[table.RowCount, table.ColumnCount];
            foreach (var cell in table.Cells)
                grid[cell.RowIndex, cell.ColumnIndex] = cell.Content.Replace("|", "\\|").Replace("\n", " ");

            for (int r = 0; r < table.RowCount; r++)
            {
                sb.Append("| ");
                for (int c = 0; c < table.ColumnCount; c++)
                    sb.Append($"{grid[r, c] ?? string.Empty} | ");
                sb.AppendLine();
                if (r == 0)
                {
                    sb.Append("| ");
                    for (int c = 0; c < table.ColumnCount; c++)
                        sb.Append("--- | ");
                    sb.AppendLine();
                }
            }
            sb.AppendLine();
        }
        return sb.ToString().Trim();
    }
}
