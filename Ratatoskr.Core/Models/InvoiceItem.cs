using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ratatoskr.Core.Models;

public partial class InvoiceItem : ObservableObject
{
    public int Id { get; set; }

    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;

    [ObservableProperty]
    public int? serviceId;
    public Service? Service { get; set; }

    [ObservableProperty]
    private int positionNumber;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private int quantity;

    [ObservableProperty]
    private decimal unitPrice;

    [ObservableProperty]
    private decimal vatRate = 0.19m;

    [NotMapped]
    public decimal NetTotal => Math.Round(Quantity * UnitPrice, 2);

    [NotMapped]
    public decimal VatAmount => Math.Round(NetTotal * VatRate, 2);

    [NotMapped]
    public decimal GrossTotal => NetTotal + VatAmount;

    /// <summary>
    /// Wird automatisch aufgerufen, wenn sich die ServiceId ändert (durch ComboBox-Auswahl).
    /// Holt sich die Werte aus der zugehörigen Navigation, falls vorhanden.
    /// </summary>
    partial void OnServiceIdChanged(int? oldValue, int? newValue)
    {
        if (newValue is not int id || Service is null)
            return;

        UnitPrice = Service.PriceNet;
        VatRate = Service.TaxRate;

        // Optional: Nur setzen, wenn Description leer ist
        if (string.IsNullOrWhiteSpace(Description))
            Description = Service.Name;
    }
}
