using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ratatoskr.App.View.Customers;
using Ratatoskr.Core.Models;
using Ratatoskr.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ratatoskr.App.ViewModels.Customers;

public partial class CustomerViewModel : ObservableObject
{
    private readonly CustomerService _service;

    private bool CanDeleteCustomer() => SelectedCustomer is not null;
    private bool CanEditCustomer() => SelectedCustomer is not null;

    [ObservableProperty]
    private ObservableCollection<Customer> customers = new();
    [ObservableProperty]
    private Customer newCustomer = new();
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditCustomerCommand))]
    private Customer? selectedCustomer;

    public CustomerViewModel(CustomerService service)
    {
        _service = service;
        LoadCustomers();
    }

    [RelayCommand]
    private async Task AddCustomer()
    {
        var dialog = new NewCustomerView();
        if (dialog.ShowDialog() == true && dialog.CreatedCustomer is not null)
        {
            await _service.AddAsync(dialog.CreatedCustomer);
            await LoadCustomers(); // oder deine Methode zum Reload
        }

    }

    [RelayCommand]
    private async Task LoadCustomers()
    {
        var list = await _service.GetAllAsync();
        Customers = new ObservableCollection<Customer>(list.Where(c => c.IsActive));
    }

    [RelayCommand]
    private async Task DeleteCustomer(Customer customer)
    {
        if (SelectedCustomer is null)
        {
            MessageBox.Show("Kein Kunde ausgewählt."); 
            return;
        }
        var result = MessageBox.Show($"Möchtest du wirklich den Kunden '{SelectedCustomer.Prename} {SelectedCustomer.Surname}' deaktivieren?\n\n" +
            "Der Kunde kann nicht mehr verwendet werden.", "Löschvermerk setzen", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;
        SelectedCustomer.IsActive = false;
        await _service.UpdateAsync(SelectedCustomer);
        await LoadCustomers();
    }

    [RelayCommand(CanExecute = nameof(CanEditCustomer))]
    private async Task EditCustomer()
    {
        if (SelectedCustomer is null) return;

        //bool hasPastInvoices = await _invoiceService.HastPastInvoicesAsync(SelectedCustomer.Id);
        bool hasLockedFields = false;

        var customerToEdit = new Customer
        {
            Id = SelectedCustomer.Id,
            Prename = SelectedCustomer.Prename,
            Surname = SelectedCustomer.Surname,
            Email = SelectedCustomer.Email,
            Phone = SelectedCustomer.Phone,
            Address = SelectedCustomer.Address,
            ZipCode = SelectedCustomer.ZipCode,
            City = SelectedCustomer.City,
            Country = SelectedCustomer.Country,
            IsActive = SelectedCustomer.IsActive
        };

        var dialog = new NewCustomerView(customerToEdit, isEditMode: true, hasLockedFields: hasLockedFields);
        if (dialog.ShowDialog() == true && dialog.CreatedCustomer is not null)
        {
            await _service.UpdateAsync(dialog.CreatedCustomer);
            await LoadCustomers();
        }
    }
    
}
