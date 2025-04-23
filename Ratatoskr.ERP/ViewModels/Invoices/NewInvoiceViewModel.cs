using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Ratatoskr.App.Messages;
using Ratatoskr.Core.Models;
using Ratatoskr.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ratatoskr.App.ViewModels.Invoices;

public partial class NewInvoiceViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    public NewInvoiceViewModel(AppDbContext db)
    {
        _db = db;
        InvoiceDate = DateTime.Today;

        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        Customers = new ObservableCollection<Customer>(
            await _db.Customers
            .Where(c => c.IsActive)
            .ToListAsync());
        Services = new ObservableCollection<Service>(
            await _db.Services.ToListAsync());

        Items = new ObservableCollection<InvoiceItem>();
        Items.CollectionChanged += (s, e) =>
        {
            if (e.NewItems != null)
                foreach (InvoiceItem item in e.NewItems)
                {
                    item.PropertyChanged += InvoiceItemChanged;
                }
            OnPropertyChanged(nameof(TotalNet));
            OnPropertyChanged(nameof(TotalVat));
            OnPropertyChanged(nameof(TotalGross));
        };

        InvoiceNumber = await GenerateInvoiceNumberAsync();
    }

    private void InvoiceItemChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(InvoiceItem.Quantity) or nameof(InvoiceItem.UnitPrice) or nameof(InvoiceItem.VatRate))
        {
            OnPropertyChanged(nameof(TotalNet));
            OnPropertyChanged(nameof(TotalVat));
            OnPropertyChanged(nameof(TotalGross));
        }
    }

    [ObservableProperty]
    private ObservableCollection<Customer> customers = new();
    [ObservableProperty]
    private ObservableCollection<Service> services = new();
    [ObservableProperty]
    private Customer? selectedCustomer;
    [ObservableProperty]
    private ObservableCollection<InvoiceItem> items;
    [ObservableProperty]
    private string invoiceNumber = string.Empty;
    [ObservableProperty]
    private DateTime invoiceDate;
    [ObservableProperty]
    private Service? selectedServiceToAdd;
    [ObservableProperty]
    private int quantityToAdd = 1;
    [ObservableProperty]
    private InvoiceItem? selectedItem;

    public decimal TotalNet => Items?.Sum(i => i.NetTotal) ?? 0m;
    public decimal TotalVat => Items?.Sum(i => i.VatAmount) ?? 0m;
    public decimal TotalGross => TotalNet + TotalVat;

    private async Task<string> GenerateInvoiceNumberAsync()
    {
        var count = await _db.Invoices.CountAsync() + 1;
        return $"RE-{DateTime.Today:yyyy}-{count:D4}";
    }

    partial void OnSelectedItemChanged(InvoiceItem? value)
    {
        DeleteSelectedItemCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private void AddItem()
    {
        // Falls keine Leistungen geladen wurden, nichts tun
        if (Services.Count == 0) return;

        // Nimm den ersten Service als Standardauswahl
        var selectedService = Services.First();

        var nextPosition = Items.Count + 1;
        var item = new InvoiceItem
        {
            PositionNumber = nextPosition,
            ServiceId = selectedService.Id,
            Service = selectedService, // notwendig für OnServiceIdChanged
            Description = selectedService.Name,
            UnitPrice = selectedService.PriceNet,
            VatRate = selectedService.TaxRate,
            Quantity = 1
        };

        Items.Add(item);
    }
    [RelayCommand]
    private async Task SaveInvoiceAsync()
    {
        if (SelectedCustomer is null || Items.Count == 0) return;
        var invoice = new Invoice
        {
            InvoiceNumber = InvoiceNumber,
            InvoiceDate = InvoiceDate,
            DueDate = InvoiceDate.AddDays(14),
            Status = Core.Enums.InvoiceStatus.Draft,
            CustomerId = SelectedCustomer.Id,
            Items = Items.ToList(),

            TotalNet = Items.Sum(i => i.NetTotal),
            TotalVat = Items.Sum(i => i.VatAmount),
            TotalGross = Items.Sum(i => i.GrossTotal)
        };

        _db.Invoices.Add(invoice);
        await _db.SaveChangesAsync();

        WeakReferenceMessenger.Default.Send(new InvoiceSavedMessage(invoice));

    }
    [RelayCommand]
    private void IncreaseQuantity(InvoiceItem item)
    {
        item.Quantity++;
    }
    [RelayCommand]
    private void DecreaseQuantity(InvoiceItem item)
    {
        if (item.Quantity > 0)
            item.Quantity--;
    }
    [RelayCommand]
    private void AddSelectedServiceToInvoice()
    {
        if (SelectedServiceToAdd is null || QuantityToAdd <= 0)
            return;

        var item = new InvoiceItem
        {
            ServiceId = SelectedServiceToAdd.Id,
            Service = SelectedServiceToAdd,
            Description = SelectedServiceToAdd.Name,
            UnitPrice = SelectedServiceToAdd.PriceNet,
            VatRate = SelectedServiceToAdd.TaxRate,
            Quantity = QuantityToAdd
        };

        Items.Add(item);
    }
    [RelayCommand(CanExecute = nameof(CanDeleteItem))]
    private void DeleteSelectedItem()
    {
        if (SelectedItem is not null)
            Items.Remove(SelectedItem);
    }

    private bool CanDeleteItem() => SelectedItem is not null;


}
