using AutoService.Dto.ReportDtos;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace AutoService.Web.Services;

public sealed class ReportExportService : IReportExportService
{
    private static readonly CultureInfo Tr = CultureInfo.GetCultureInfo("tr-TR");

    public byte[] CreateExcel(ReportDashboardDto report)
    {
        using var workbook = new XLWorkbook();
        CreateSummarySheet(workbook, report);
        CreateMonthlySheet(workbook, report);
        CreateMechanicSheet(workbook, report);
        CreatePartsSheet(workbook, report);
        CreateStockSheet(workbook, report);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] CreatePdf(ReportDashboardDto report)
    {
        var period = GetPeriod(report);
        var partRevenue = report.TotalRevenue - report.TotalLaborCost;
        var deliveryRate = report.TotalServiceCount == 0
            ? 0
            : Math.Round((decimal)report.DeliveredVehicleCount / report.TotalServiceCount * 100, 1);

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(28);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Header().Column(column =>
                {
                    column.Item().Text("AUTOSERVICE ERP")
                        .FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                    column.Item().Text("Servis Performans ve Gelir Raporu")
                        .FontSize(13).SemiBold();
                    column.Item().Text($"Rapor dönemi: {period} | Oluşturulma: {DateTime.Now:dd.MM.yyyy HH:mm}")
                        .FontSize(8).FontColor(Colors.Grey.Darken1);
                    column.Item().PaddingTop(8).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                });

                page.Content().PaddingVertical(14).Column(column =>
                {
                    column.Spacing(12);
                    column.Item().Row(row =>
                    {
                        AddMetric(row, "Toplam Servis", report.TotalServiceCount.ToString("N0", Tr));
                        AddMetric(row, "Toplam Gelir", report.TotalRevenue.ToString("C2", Tr));
                        AddMetric(row, "İşçilik Geliri", report.TotalLaborCost.ToString("C2", Tr));
                        AddMetric(row, "Parça Geliri", partRevenue.ToString("C2", Tr));
                    });

                    column.Item().Row(row =>
                    {
                        AddMetric(row, "Teslim Edilen", report.DeliveredVehicleCount.ToString("N0", Tr));
                        AddMetric(row, "Aktif Servis", report.ActiveServiceCount.ToString("N0", Tr));
                        AddMetric(row, "Teslim Oranı", $"%{deliveryRate:N1}");
                        AddMetric(row, "Kritik Stok", report.LowStockPartCount.ToString("N0", Tr));
                    });

                    AddSectionTitle(column, "Aylık Gelir Analizi");
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(1.6f); c.RelativeColumn(); c.RelativeColumn(1.3f);
                            c.RelativeColumn(1.3f); c.RelativeColumn(1.3f);
                        });
                        AddHeader(table, "Dönem", "Servis", "Toplam", "İşçilik", "Parça");
                        foreach (var item in report.MonthlyRevenues)
                            AddRow(table, $"{item.MonthName} {item.Year}", item.ServiceCount.ToString(),
                                item.TotalRevenue.ToString("C2", Tr), item.LaborRevenue.ToString("C2", Tr),
                                item.PartRevenue.ToString("C2", Tr));
                    });

                    AddSectionTitle(column, "Teknisyen Performansı");
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(1.6f); c.RelativeColumn(1.2f); c.RelativeColumn();
                            c.RelativeColumn(); c.RelativeColumn(1.3f);
                        });
                        AddHeader(table, "Teknisyen", "Uzmanlık", "Servis", "Teslim", "Gelir");
                        foreach (var item in report.MechanicPerformances.Take(10))
                            AddRow(table, item.MechanicName, item.Specialty, item.TotalServiceCount.ToString(),
                                item.DeliveredServiceCount.ToString(), item.TotalRevenue.ToString("C2", Tr));
                    });

                    AddSectionTitle(column, "En Çok Kullanılan Parçalar");
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(1.8f); c.RelativeColumn(); c.RelativeColumn();
                            c.RelativeColumn(); c.RelativeColumn(1.3f);
                        });
                        AddHeader(table, "Parça", "Kod", "Adet", "Kullanım", "Gelir");
                        foreach (var item in report.MostUsedParts)
                            AddRow(table, item.PartName, item.PartCode, item.TotalQuantity.ToString(),
                                item.UsageCount.ToString(), item.TotalRevenue.ToString("C2", Tr));
                    });
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("AutoService ERP • ");
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
        }).GeneratePdf();
    }

    private static void CreateSummarySheet(XLWorkbook workbook, ReportDashboardDto report)
    {
        var ws = workbook.Worksheets.Add("Özet");
        ws.Cell("A1").Value = "AUTOSERVICE ERP - SERVİS RAPORU";
        ws.Range("A1:D1").Merge().Style.Font.SetBold().Font.SetFontSize(18);
        ws.Cell("A3").Value = "Rapor Dönemi";
        ws.Cell("B3").Value = GetPeriod(report);
        ws.Cell("A5").Value = "Toplam Servis"; ws.Cell("B5").Value = report.TotalServiceCount;
        ws.Cell("A6").Value = "Toplam Gelir"; ws.Cell("B6").Value = report.TotalRevenue;
        ws.Cell("A7").Value = "İşçilik Geliri"; ws.Cell("B7").Value = report.TotalLaborCost;
        ws.Cell("A8").Value = "Parça Geliri"; ws.Cell("B8").Value = report.TotalRevenue - report.TotalLaborCost;
        ws.Cell("A9").Value = "Teslim Edilen"; ws.Cell("B9").Value = report.DeliveredVehicleCount;
        ws.Cell("A10").Value = "Aktif Servis"; ws.Cell("B10").Value = report.ActiveServiceCount;
        ws.Cell("A11").Value = "Kullanılan Parça Adedi"; ws.Cell("B11").Value = report.TotalUsedPartQuantity;
        ws.Cell("A12").Value = "Kritik Stok Sayısı"; ws.Cell("B12").Value = report.LowStockPartCount;
        ws.Range("A5:A12").Style.Font.Bold = true;
        ws.Range("B6:B8").Style.NumberFormat.Format = "₺ #,##0.00";
        StyleWorksheet(ws);
    }

    private static void CreateMonthlySheet(XLWorkbook workbook, ReportDashboardDto report)
    {
        var ws = workbook.Worksheets.Add("Aylık Gelir");
        WriteHeader(ws, "Dönem", "Servis Sayısı", "Toplam Gelir", "İşçilik", "Parça Geliri");
        var row = 2;
        foreach (var item in report.MonthlyRevenues)
        {
            ws.Cell(row, 1).Value = $"{item.MonthName} {item.Year}";
            ws.Cell(row, 2).Value = item.ServiceCount;
            ws.Cell(row, 3).Value = item.TotalRevenue;
            ws.Cell(row, 4).Value = item.LaborRevenue;
            ws.Cell(row, 5).Value = item.PartRevenue;
            row++;
        }
        ws.Columns(3, 5).Style.NumberFormat.Format = "₺ #,##0.00";
        StyleWorksheet(ws);
    }

    private static void CreateMechanicSheet(XLWorkbook workbook, ReportDashboardDto report)
    {
        var ws = workbook.Worksheets.Add("Teknisyenler");
        WriteHeader(ws, "Teknisyen", "Uzmanlık", "Toplam Servis", "Teslim", "Aktif", "Toplam Gelir", "İşçilik");
        var row = 2;
        foreach (var item in report.MechanicPerformances)
        {
            ws.Cell(row, 1).Value = item.MechanicName; ws.Cell(row, 2).Value = item.Specialty;
            ws.Cell(row, 3).Value = item.TotalServiceCount; ws.Cell(row, 4).Value = item.DeliveredServiceCount;
            ws.Cell(row, 5).Value = item.ActiveServiceCount; ws.Cell(row, 6).Value = item.TotalRevenue;
            ws.Cell(row, 7).Value = item.TotalLaborCost; row++;
        }
        ws.Columns(6, 7).Style.NumberFormat.Format = "₺ #,##0.00";
        StyleWorksheet(ws);
    }

    private static void CreatePartsSheet(XLWorkbook workbook, ReportDashboardDto report)
    {
        var ws = workbook.Worksheets.Add("Parça Kullanımı");
        WriteHeader(ws, "Parça", "Kod", "Toplam Adet", "Kullanım Sayısı", "Toplam Gelir");
        var row = 2;
        foreach (var item in report.MostUsedParts)
        {
            ws.Cell(row, 1).Value = item.PartName; ws.Cell(row, 2).Value = item.PartCode;
            ws.Cell(row, 3).Value = item.TotalQuantity; ws.Cell(row, 4).Value = item.UsageCount;
            ws.Cell(row, 5).Value = item.TotalRevenue; row++;
        }
        ws.Column(5).Style.NumberFormat.Format = "₺ #,##0.00";
        StyleWorksheet(ws);
    }

    private static void CreateStockSheet(XLWorkbook workbook, ReportDashboardDto report)
    {
        var ws = workbook.Worksheets.Add("Kritik Stok");
        WriteHeader(ws, "Parça", "Kod", "Stok", "Birim Fiyat", "Durum");
        var row = 2;
        foreach (var item in report.LowStockParts)
        {
            ws.Cell(row, 1).Value = item.PartName; ws.Cell(row, 2).Value = item.PartCode;
            ws.Cell(row, 3).Value = item.StockQuantity; ws.Cell(row, 4).Value = item.UnitPrice;
            ws.Cell(row, 5).Value = item.StockStatus; row++;
        }
        ws.Column(4).Style.NumberFormat.Format = "₺ #,##0.00";
        StyleWorksheet(ws);
    }

    private static void WriteHeader(IXLWorksheet ws, params string[] headers)
    {
        for (var i = 0; i < headers.Length; i++) ws.Cell(1, i + 1).Value = headers[i];
        var range = ws.Range(1, 1, 1, headers.Length);
        range.Style.Font.Bold = true;
        range.Style.Fill.BackgroundColor = XLColor.FromHtml("#1E3A8A");
        range.Style.Font.FontColor = XLColor.White;
    }

    private static void StyleWorksheet(IXLWorksheet ws)
    {
        ws.SheetView.FreezeRows(1);
        ws.Columns().AdjustToContents(12, 42);
        ws.RangeUsed()?.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            .Border.SetInsideBorder(XLBorderStyleValues.Hair);
        ws.RangeUsed()?.SetAutoFilter();
    }

    private static string GetPeriod(ReportDashboardDto report) =>
        report.StartDate.HasValue || report.EndDate.HasValue
            ? $"{report.StartDate?.ToString("dd.MM.yyyy") ?? "İlk kayıt"} - {report.EndDate?.ToString("dd.MM.yyyy") ?? "Bugün"}"
            : "Tüm zamanlar";

    private static void AddMetric(RowDescriptor row, string title, string value)
    {
        row.RelativeItem().Padding(4).Border(1).BorderColor(Colors.Grey.Lighten2)
            .Background(Colors.Grey.Lighten4).Padding(8).Column(c =>
            {
                c.Item().Text(title).FontSize(7).FontColor(Colors.Grey.Darken1);
                c.Item().Text(value).FontSize(11).SemiBold().FontColor(Colors.Blue.Darken2);
            });
    }

    private static void AddSectionTitle(ColumnDescriptor column, string title) =>
        column.Item().PaddingTop(4).Text(title).FontSize(12).SemiBold().FontColor(Colors.Blue.Darken2);

    private static void AddHeader(TableDescriptor table, params string[] values)
    {
        foreach (var value in values)
            table.Cell().Background(Colors.Blue.Darken2).Padding(5)
                .Text(value).FontColor(Colors.White).SemiBold().FontSize(8);
    }

    private static void AddRow(TableDescriptor table, params string[] values)
    {
        foreach (var value in values)
            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(value).FontSize(7.5f);
    }
}
