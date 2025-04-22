using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratatoskr.Core.Models;

public class Service
{
    public int Id { get; set; }

    public string ArticleNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string Unit { get; set; } = "Stück";
    public decimal PriceNet { get; set; }
    public int TaxRate { get; set; }

    public bool IsActive { get; set; } = true;

    public int ServiceTypeId { get; set; }
    public ServiceType? ServiceType { get; set; }

    public int ServiceCategoryId { get; set; }
    public ServiceCategory? ServiceCategory { get; set; }
}
