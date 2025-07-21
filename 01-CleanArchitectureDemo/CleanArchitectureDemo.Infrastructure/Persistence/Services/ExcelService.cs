using ClosedXML.Excel;
using DevSandbox.Shared.Kernel.Interfaces.Services;
using DevSandbox.Shared.Kernel.Results;
using Microsoft.Extensions.Localization;
using System.Data;

namespace CleanArchitectureDemo.Infrastructure.Persistence.Services;

public class ExcelService : IExcelService
{
    private readonly IStringLocalizer<ExcelService> _localizer;

    public ExcelService(IStringLocalizer<ExcelService> localizer)
    {
        _localizer = localizer;
    }

    private void ApplyHeaderStyle(IXLCell cell)
    {
        var style = cell.Style;
        style.Fill.PatternType = XLFillPatternValues.Solid;
        style.Fill.BackgroundColor = XLColor.LightBlue;
        style.Border.BottomBorder = XLBorderStyleValues.Thin;
    }

    private static byte[] SaveWorkbookToByteArray(XLWorkbook workbook)
    {
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return stream.ToArray();
    }

    public Task<byte[]> CreateTemplateAsync(IEnumerable<string> fields, string sheetName = "Sheet1")
    {
        using var workbook = new XLWorkbook();
        workbook.Properties.Author = string.Empty;
        var ws = workbook.Worksheets.Add(sheetName);
        int rowIndex = 1, colIndex = 1;
        foreach (var header in fields)
        {
            var cell = ws.Cell(rowIndex, colIndex++);
            ApplyHeaderStyle(cell);
            cell.Value = header;
        }
        return Task.FromResult(SaveWorkbookToByteArray(workbook));
    }

    public Task<byte[]> ExportAsync<T>(IEnumerable<T> data, Dictionary<string, Func<T, object?>> mappers, string sheetName = "Sheet1")
    {
        using var workbook = new XLWorkbook();
        workbook.Properties.Author = string.Empty;
        var ws = workbook.Worksheets.Add(sheetName);

        int rowIndex = 1, colIndex = 1;
        var headers = mappers.Keys.ToList();

        // Header Row
        foreach (var header in headers)
        {
            var cell = ws.Cell(rowIndex, colIndex++);
            ApplyHeaderStyle(cell);
            cell.Value = header;
        }

        // Data Rows
        foreach (var item in data)
        {
            colIndex = 1; rowIndex++;
            foreach (var header in headers)
            {
                var value = mappers[header](item);
                ws.Cell(rowIndex, colIndex++).Value = value?.ToString() ?? string.Empty;
            }
        }

        return Task.FromResult(SaveWorkbookToByteArray(workbook));
    }

    public async Task<IResult<IEnumerable<T>>> ImportAsync<T>(
        byte[] fileBytes,
        Dictionary<string, Func<DataRow, T, object?>> mappers,
        string sheetName = "Sheet1")
    {
        using var workbook = new XLWorkbook(new MemoryStream(fileBytes));
        if (!workbook.Worksheets.TryGetWorksheet(sheetName, out var ws))
            return await Result<IEnumerable<T>>.FailureAsync($"Sheet '{sheetName}' does not exist.");

        int lastCell = ws.LastCellUsed()?.Address.ColumnNumber ?? 0;
        if (lastCell == 0)
            return await Result<IEnumerable<T>>.FailureAsync($"Sheet '{sheetName}' is empty.");

        var dt = new DataTable();
        foreach (var cell in ws.Range(1, 1, 1, lastCell).Cells())
            dt.Columns.Add(cell.Value.ToString());

        int startRow = 2;
        var headers = mappers.Keys.ToList();
        var errors = new List<string>();

        // Validate headers
        foreach (var header in headers)
            if (!dt.Columns.Contains(header))
                errors.Add($"Header '{header}' not found.");
        if (errors.Any())
            return await Result<IEnumerable<T>>.FailureAsync(errors.ToArray());

        int lastRow = ws.LastRowUsed()?.RowNumber() ?? 0;
        var list = new List<T>();

        for (int rowIndex = startRow; rowIndex <= lastRow; rowIndex++)
        {
            var row = ws.Row(rowIndex);
            try
            {
                var dataRow = dt.NewRow();
                foreach (var cell in row.Cells(1, dt.Columns.Count))
                {
                    int colIdx = cell.Address.ColumnNumber - 1;
                    dataRow[colIdx] = cell.DataType == XLDataType.DateTime
                        ? cell.GetDateTime().ToString("yyyy-MM-dd HH:mm:ss")
                        : cell.Value.ToString();
                }
                dt.Rows.Add(dataRow);

                var item = Activator.CreateInstance<T>();
                foreach (var header in headers)
                    mappers[header](dataRow, item);
                list.Add(item);
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<T>>.FailureAsync($"Row {rowIndex}: {ex.Message}");
            }
        }

        return await Result<IEnumerable<T>>.SuccessAsync(list);
    }
}
