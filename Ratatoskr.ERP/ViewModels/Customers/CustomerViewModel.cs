using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ratatoskr.Core.Models;
using Ratatoskr.Infrastructure.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Ratatoskr.App.ViewModels.Customers;

public partial class CustomersViewModel : ObservableObject
{
    private readonly CustomerService _service;

    public CustomersViewModel(CustomerService service)
    {
        _service = service;
        _ = LoadCustomersAsync();
    }

    private List<Customer> _allCustomers = new();

    [ObservableProperty]
    private ObservableCollection<Customer> customers = new();

    [ObservableProperty]
    private Customer? selectedCustomer;

    [ObservableProperty]
    private string? nameFilter;

    [ObservableProperty]
    private string? companyFilter;

    [ObservableProperty]
    private bool isNewCustomerFormVisible = false;

    [RelayCommand]
    private async Task LoadCustomersAsync()
    {
        _allCustomers = (await _service.GetAllAsync()).Where(c => c.IsActive).ToList();
        ApplyFilter();
    }

    [RelayCommand]
    private void ApplyFilter()
    {
        var query = _allCustomers.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(NameFilter))
            query = query.Where(c => c.FullName.Contains(NameFilter, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(CompanyFilter))
            query = query.Where(c => (c.CompanyName ?? string.Empty).Contains(CompanyFilter, StringComparison.OrdinalIgnoreCase));

        Customers = new ObservableCollection<Customer>(query);
    }

    [RelayCommand]
    private void ResetFilters()
    {
        NameFilter = null;
        CompanyFilter = null;
        ApplyFilter();
    }

    [RelayCommand]
    private void ShowNewCustomerForm()
    {
        SelectedCustomer = null;
        IsNewCustomerFormVisible = true;
    }

    [RelayCommand(CanExecute = nameof(CanEditCustomer))]
    private void EditCustomer()
    {
        if (SelectedCustomer is null) return;
        IsNewCustomerFormVisible = true;
    }

    [RelayCommand(CanExecute = nameof(CanEditCustomer))]
    private async Task DeleteCustomerAsync()
    {
        if (SelectedCustomer is null) return;

        SelectedCustomer.IsActive = false;
        await _service.UpdateAsync(SelectedCustomer);
        await LoadCustomersAsync();
    }

    public async Task SaveCustomerAsync(Customer customer, bool isEdit)
    {
        if (isEdit)
            await _service.UpdateAsync(customer);
        else
            await _service.AddAsync(customer);

        IsNewCustomerFormVisible = false;
        await LoadCustomersAsync();
    }

    private bool CanEditCustomer() => SelectedCustomer is not null;
}
