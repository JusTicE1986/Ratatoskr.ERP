using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Ratatoskr.App.Messages;
using Ratatoskr.App.Services;
using Ratatoskr.Core.Enums;
using Ratatoskr.Core.Models;
using Ratatoskr.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ratatoskr.App.ViewModels.Invoices;

public partial class InvoicesViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    private readonly InvoicePdfSerivce _pdfService;

    public InvoicesViewModel(AppDbContext db, InvoicePdfSerivce pdfSerivce)
    {
        _db = db;
        _pdfService = pdfSerivce;
        _ = LoadInvoicesAsync();

        WeakReferenceMessenger.Default.Register<InvoiceSavedMessage>(this, async (r, m) =>
        {
            await LoadInvoicesAsync();
        });
    }

    public List<Invoice> _allInvoices = new();

    [ObservableProperty]
    private ObservableCollection<Invoice> invoices = new();
    [ObservableProperty]
    private Invoice? selectedInvoice;
    [ObservableProperty]
    private InvoiceStatus? selectedStatusFilter;
    [ObservableProperty]
    private string? invoiceNumberFilter;
    [ObservableProperty]
    private string? nameFilter;


    partial void OnSelectedStatusFilterChanged(InvoiceStatus? oldValue , InvoiceStatus? newValue)
    {
        ApplyFilter();
    }
    partial void OnInvoiceNumberFilterChanged(string? oldValie, string? newValue)
    {
        ApplyFilter();
    }
    partial void OnNameFilterChanged(string? oldValue, string? newValue)
    {
        ApplyFilter();
    }
    private void ApplyFilter()
    {
        var query = _allInvoices.AsEnumerable();

        if (SelectedStatusFilter is not null)
            query = query.Where(i => i.Status == SelectedStatusFilter);

        if (!string.IsNullOrWhiteSpace(InvoiceNumberFilter))
            query = query.Where(i => i.InvoiceNumber.Contains(InvoiceNumberFilter, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(NameFilter))
            query = query.Where(i =>
                $"{i.Customer.Prename} {i.Customer.Surname}".Contains(NameFilter, StringComparison.OrdinalIgnoreCase));

        Invoices = new ObservableCollection<Invoice>(query);
    }

    private bool IsOverdue(Invoice invoice) => invoice.Status == InvoiceStatus.Open && invoice.DueDate < DateTime.Today;

    //[RelayCommand(CanExecute = nameof(CanBookInvoice))]
    //private async Task BookInvoiceAsync()
    //{
    //    if (SelectedInvoice is null || SelectedInvoice.Status != Core.Enums.InvoiceStatus.Open) return;

    //    SelectedInvoice.Status = Core.Enums.InvoiceStatus.Paid;
    //    _db.Update(SelectedInvoice);
    //    await _db.SaveChangesAsync();

    //    Invoices = new ObservableCollection<Invoice>(await _db.Invoices
    //        .Include(i => i.Customer)
    //        .ToListAsync());
    //}
    [RelayCommand]
    private async Task MarkAsPaidAsync()
    {
        if (SelectedInvoice is null || SelectedInvoice.Status != InvoiceStatus.Open) return;

        SelectedInvoice.Status = InvoiceStatus.Paid;
        _db.Update(SelectedInvoice);
        await _db.SaveChangesAsync();

        await LoadInvoicesAsync();
    }

    [RelayCommand]
    private async Task CancelInvoiceAsync()
    {
        if (SelectedInvoice is null || SelectedInvoice.Status == InvoiceStatus.Paid) return;
        SelectedInvoice.Status = InvoiceStatus.Canceled;
        _db.Update(SelectedInvoice);
        await _db.SaveChangesAsync();

        await LoadInvoicesAsync();
    }

    [RelayCommand]
    private async Task PrintInvoiceAsync()
    {
        if (SelectedInvoice is null) return;
        bool isCopy = SelectedInvoice.Status != InvoiceStatus.Draft;

        var invoice = await _db.Invoices
            .Include(i => i.Customer)
            .Include(i => i.Items)
            .FirstAsync(i => i.Id == SelectedInvoice.Id);

        _pdfService.GeneratePdf(invoice, isCopy);

        if (!isCopy)
        {
            
            SelectedInvoice.Status = InvoiceStatus.Open;
            _db.Update(SelectedInvoice);
            await _db.SaveChangesAsync();
            await LoadInvoicesAsync();
        }
            
    }

    [RelayCommand]
    private async Task PrintCopy()
    {
        if (SelectedInvoice is null) return;
        var invoice = await _db.Invoices
            .Include(i => i.Customer)
            .Include(i => i.Items)
            .FirstAsync(i => i.Id == SelectedInvoice.Id);
        _pdfService.GeneratePdf(invoice, isCopy:true);
    }

    [RelayCommand]
    private void ResetFilters()
    {
        SelectedStatusFilter = null;
        InvoiceNumberFilter = null;
        NameFilter = null;

        ApplyFilter();
    }

    private bool CanBookInvoice() => SelectedInvoice is not null && SelectedInvoice.Status == Core.Enums.InvoiceStatus.Open;

    private async Task LoadInvoicesAsync()
    {
        var result = await _db.Invoices
            .Include(i => i.Customer)
            .ToListAsync();
        Invoices = new ObservableCollection<Invoice>(result);
        _allInvoices = result;
        ApplyFilter();
    }
}
