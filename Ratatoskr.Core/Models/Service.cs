using CommunityToolkit.Mvvm.ComponentModel;
using Ratatoskr.Core.Enums;
using Ratatoskr.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratatoskr.Core.Models;

public enum ServiceType
{
    Produkt,
    Dienstleistung,
    Sonstiges
}

public class Service
{
    public int Id { get; set; }

    public string ArticleNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public Unit Unit { get; set; }
    public decimal PriceNet { get; set; }
    public TaxRate TaxRateEnum { get; set; }

    public bool IsActive { get; set; } = true;

    public ServiceType? ServiceType { get; set; }
    public int ServiceTypeId { get; set; }

    [NotMapped]
    public decimal TaxRate => TaxRateEnum.ToDecimal();
}
