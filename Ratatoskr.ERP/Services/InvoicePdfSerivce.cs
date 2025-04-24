using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Ratatoskr.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratatoskr.App.Services;
public class InvoicePdfSerivce
{
    public void GeneratePdf(Invoice invoice, bool isCopy = false)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var filename = $"Rechnung_{invoice.InvoiceNumber}" + (isCopy ? "Kopie" : "") + ".pdf";
        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var filePath = Path.Combine(desktop, filename);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header().Text($"Rechnung {invoice.InvoiceNumber}" + (isCopy ? " (Kopie)" : "")).FontSize(20).Bold();
                page.Content().Element(ComposeContent(invoice));
                page.Footer().AlignCenter().Text($"Erstellt am {DateTime.Today:dd.MM.yyyy}");
            });
        });

        document.GeneratePdf(filePath);
        OpenFile(filePath);
    }

    private Action<IContainer> ComposeContent(Invoice invoice) => container =>
    {
        container.Column(col =>
        {
            col.Spacing(10);
            //Kundenname
            col.Item().Text($"Kunde: {invoice.Customer.DisplayName}");

            //Rechnungsdatum
            col.Item().Text($"Datum: {invoice.InvoiceDate:dd.MM.yyyy}");

            //Tabelle der Positionen
            col.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(40); //Pos.
                    columns.RelativeColumn(); //Beschreibung
                    columns.ConstantColumn(50); // Menge
                    columns.ConstantColumn(70); // Einzelpreis
                    columns.ConstantColumn(70); // Netto
                    columns.ConstantColumn(70); // MwSt
                    columns.ConstantColumn(70); // Brutto
                });

                table.Header(header =>
                {
                    header.Cell().Text("Pos.").Bold();
                    header.Cell().Text("Beschreibung").Bold();
                    header.Cell().Text("Menge").Bold();
                    header.Cell().Text("Einzelpreis").Bold();
                    header.Cell().Text("Netto").Bold();
                    header.Cell().Text("MwSt.").Bold();
                    header.Cell().Text("Brutto").Bold();
                });

                foreach (var item in invoice.Items.OrderBy(i => i.PositionNumber))
                {
                    table.Cell().Text(item.PositionNumber.ToString());
                    table.Cell().Text(item.Description);
                    table.Cell().Text(item.Quantity.ToString());
                    table.Cell().Text(item.UnitPrice.ToString("C", CultureInfo.CurrentCulture));
                    table.Cell().Text(item.NetTotal.ToString("C", CultureInfo.CurrentCulture));
                    table.Cell().Text(item.VatAmount.ToString("C", CultureInfo.CurrentCulture));
                    table.Cell().Text(item.GrossTotal.ToString("C", CultureInfo.CurrentCulture));
                }
            });

            //Summenbereich
            col.Item().AlignRight().Column(sums =>
            {
                sums.Spacing(2);
                sums.Item().Text($"Zwischensumme: {invoice.Items.Sum(i => i.NetTotal):C}");
                sums.Item().Text($"MwSt.: {invoice.Items.Sum(i => i.VatAmount):C}");
                sums.Item().Text($"Gesamtbetrag: {invoice.Items.Sum(i => i.GrossTotal):C}").Bold();
            });
        });
    };

    private void OpenFile(string path)
    {
        var psi = new System.Diagnostics.ProcessStartInfo
        {
            FileName = path,
            UseShellExecute = true
        };

        System.Diagnostics.Process.Start(psi);
    }

}
