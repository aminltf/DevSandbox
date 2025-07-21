using DevSandbox.Shared.Kernel.Interfaces.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CleanArchitectureDemo.Infrastructure.Persistence.Services;

public class PDFService : IPDFService
{
    private const int MarginPTs = 56;
    private const string FontFamilyName = Fonts.Arial;
    private const float FontSize = 10F;
    private const int MaxCharsPerCell = 80;
    private const int MinCharsPerCell = 10;

    public async Task<byte[]> ExportAsync<TData>(
        IEnumerable<TData> data,
        Dictionary<string, Func<TData, object?>> mappers,
        string title,
        bool landscape)
    {
        var stream = new MemoryStream();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                // Page configuration
                page.Size(landscape ? PageSizes.A4.Landscape() : PageSizes.A4);
                page.Margin(MarginPTs);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(FontSize).FontFamily(FontFamilyName));

                // Header
                page.Header()
                    .Text(title)
                    .SemiBold().FontSize(16).FontColor(Colors.Black);

                // Table with row numbers and centered
                page.Content()
                    .PaddingVertical(5, Unit.Millimetre)
                    .AlignCenter()
                    .Table(table =>
                    {
                        // Add "Row Number" as first column
                        var headers = new List<string> { "ردیف" };
                        headers.AddRange(mappers.Keys);
                        var dataList = data.ToList();

                        // Estimate columns width
                        var tableWidth = landscape
                            ? (int)(PageSizes.A4.Landscape().Width - MarginPTs * 2)
                            : (int)(PageSizes.A4.Width - MarginPTs * 2);
                        var columnsWidth = new int[headers.Count];

                        for (int c = 0; c < headers.Count; c++)
                        {
                            var cellWidth = Math.Max(MinCharsPerCell,
                                Math.Min($"{headers[c]}".Length, MaxCharsPerCell));
                            columnsWidth[c] = cellWidth;
                        }

                        int rowNumber = 1;
                        foreach (var item in dataList)
                        {
                            // First column width (row number)
                            var cellWidth = Math.Max(MinCharsPerCell,
                                Math.Min(rowNumber.ToString().Length, MaxCharsPerCell));
                            if (columnsWidth[0] < cellWidth)
                                columnsWidth[0] = cellWidth;

                            var result = mappers.Keys.Select(header => mappers[header](item));
                            int c = 1;
                            foreach (var value in result)
                            {
                                cellWidth = Math.Max(MinCharsPerCell,
                                    Math.Min($"{value}".Length, MaxCharsPerCell));
                                if (columnsWidth[c] < cellWidth)
                                    columnsWidth[c] = cellWidth;
                                c += 1;
                            }
                            rowNumber++;
                        }

                        var sumWidth = columnsWidth.Sum();
                        var ratio = sumWidth == 0 ? 1 : (float)tableWidth / sumWidth;
                        for (var i = 0; i < columnsWidth.Length; i++)
                            columnsWidth[i] = (int)(columnsWidth[i] * ratio);

                        // Table columns definition
                        table.ColumnsDefinition(columns =>
                        {
                            for (int c = 0; c < headers.Count; c++)
                                columns.ConstantColumn(columnsWidth[c]);
                        });

                        // Header Row (Row = 1)
                        for (int c = 0; c < headers.Count; c++)
                            table.Cell().Row((uint)1).Column((uint)(c + 1)).Element(BlockHeader).Text(headers[c]);

                        // Data Rows (Row >= 2)
                        uint rowIndex = 2;
                        rowNumber = 1;
                        foreach (var item in dataList)
                        {
                            // First column: Row number
                            table.Cell().Row(rowIndex).Column(1).Element(BlockCell).Text(rowNumber.ToString());

                            // Next columns: Data
                            var values = mappers.Keys.Select(h => mappers[h](item)).ToList();
                            for (int c = 0; c < values.Count; c++)
                            {
                                var value = values[c];
                                table.Cell().Row(rowIndex).Column((uint)(c + 2)).Element(BlockCell).Text($"{value}");
                            }
                            rowIndex++;
                            rowNumber++;
                        }
                    });

                // Footer with page number
                page.Footer()
                    .AlignRight()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });
        })
        .GeneratePdf(stream);

        return await Task.FromResult(stream.ToArray());
    }

    private static IContainer BlockCell(IContainer container)
    {
        return container
            .Border(1)
            .Background(Colors.White)
            .Padding(1, Unit.Millimetre)
            .ShowOnce()
            .AlignCenter()
            .AlignMiddle();
    }

    private static IContainer BlockHeader(IContainer container)
    {
        return container
            .Border(1)
            .Background(Colors.Grey.Lighten3)
            .Padding(1, Unit.Millimetre)
            .AlignCenter()
            .AlignMiddle();
    }
}

