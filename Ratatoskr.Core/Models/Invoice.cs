using Ratatoskr.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratatoskr.Core.Models;

public class Invoice
{
    public int Id { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; } = DateTime.Today;
    public DateTime DueDate { get; set; } = DateTime.Today.AddDays(14);

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public List<InvoiceItem> Items { get; set; } = new();

    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

    public decimal TotalNet { get; set; }
    public decimal TotalVat { get; set; }
    public decimal TotalGross { get; set; }

    public string Notes { get; set; } = string.Empty;
}
