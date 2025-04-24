using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
    [NotifyCanExecuteChangedFor(nameof(EditCustomerCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteCustomerCommand))]
    [NotifyCanExecuteChangedFor(nameof(ShowDetailsCommand))]
    [NotifyCanExecuteChangedFor(nameof(ToggleDetailsCommand))]
    private Customer? selectedCustomer;

    partial void OnSelectedCustomerChanged(Customer? oldValue, Customer? newValue)
    {
        if (IsDetailVisible && newValue is not null)
        {
            CurrentCustomerForm = new Customer
            {
                Id = newValue.Id,
                Prename = newValue.Prename,
                Surname = newValue.Surname,
                CompanyName = newValue.CompanyName,
                IsCompany = newValue.IsCompany,
                Address = newValue.Address,
                ZipCode = newValue.ZipCode,
                City = newValue.City,
                Country = newValue.Country,
                Email = newValue.Email,
                Phone = newValue.Phone,
                IsActive = newValue.IsActive
            };

            IsEditMode = false;
            IsInCreateMode = false;

            OnPropertyChanged(nameof(FormTitle));
            OnPropertyChanged(nameof(IsInputDisabled));
        }
    }

    [ObservableProperty]
    private string? displayNameFilter;

    #region Properties für Anzeige/Bearbeitung/Erstellung
    [ObservableProperty]
    private Customer? currentCustomerForm;
    [ObservableProperty]
    private bool isDetailVisible;
    [ObservableProperty]
    private bool isEditMode;

    partial void OnIsEditModeChanged(bool oldValue, bool newValue)
    {
        OnPropertyChanged(nameof(IsInputDisabled));
        OnPropertyChanged(nameof(FormTitle));
    }

    [ObservableProperty]
    private bool isInCreateMode;

    partial void OnIsInCreateModeChanged(bool oldValue, bool newValue)
    {
        OnPropertyChanged(nameof(IsInputDisabled));
        OnPropertyChanged(nameof(FormTitle));
    }

    public string FormTitle
    {
        get
        {
            if (IsEditMode) return "Kunde bearbeiten";
            if (IsInCreateMode) return "Neuer Kunde";
            return "Kundendetails";
        }
    }
    public bool IsInputDisabled => !(IsEditMode || IsInCreateMode);

    #endregion

    [RelayCommand]
    private async Task LoadCustomersAsync()
    {
        _allCustomers = (await _service.GetAllAsync()).Where(c => c.IsActive).ToList();
        ApplyFilter();
    }

    #region Filter
    [RelayCommand]
    private void ApplyFilter()
    {
        var query = _allCustomers.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(DisplayNameFilter))
            query = query.Where(c => c.DisplayName.Contains(DisplayNameFilter, StringComparison.OrdinalIgnoreCase));

        Customers = new ObservableCollection<Customer>(query);
    }

    [RelayCommand]
    private void ResetFilters()
    {
        DisplayNameFilter = null;
        ApplyFilter();
    }
    #endregion


    [RelayCommand]
    private void CreateNewCustomer()
    {
        CurrentCustomerForm = new Customer();
        IsEditMode = false;
        IsInCreateMode = true;
        IsDetailVisible = true;
        OnPropertyChanged(nameof(FormTitle));
        OnPropertyChanged(nameof(IsInputDisabled));
    }

    [RelayCommand(CanExecute = nameof(CanEditCustomer))]
    private void EditCustomer()
    {
        if (SelectedCustomer is null) return;

        CurrentCustomerForm = new Customer
        {
            Id = SelectedCustomer.Id,
            Prename = SelectedCustomer.Prename,
            Surname = SelectedCustomer.Surname,
            CompanyName = SelectedCustomer.CompanyName,
            IsCompany = SelectedCustomer.IsCompany,
            Address = SelectedCustomer.Address,
            ZipCode = SelectedCustomer.ZipCode,
            City = SelectedCustomer.City,
            Country = SelectedCustomer.Country,
            Email = SelectedCustomer.Email,
            Phone = SelectedCustomer.Phone,
            IsActive = SelectedCustomer.IsActive
        };

        IsEditMode = true;
        IsInCreateMode = false;
        IsDetailVisible = true;
        OnPropertyChanged(nameof(FormTitle));
        OnPropertyChanged(nameof(IsInputDisabled));
    }

    [RelayCommand(CanExecute = nameof(CanEditCustomer))]
    private async Task DeleteCustomerAsync()
    {
        if (SelectedCustomer is null) return;

        SelectedCustomer.IsActive = false;
        await _service.UpdateAsync(SelectedCustomer);
        await LoadCustomersAsync();

    }

    #region DetailView anzeigen
    [RelayCommand(CanExecute =nameof(CanShowDetails))]
    private void ShowDetails()
    {
        if (SelectedCustomer is null) return;

        CurrentCustomerForm = new Customer
        {
            Id = SelectedCustomer.Id,
            Prename = SelectedCustomer.Prename,
            Surname = SelectedCustomer.Surname,
            CompanyName = SelectedCustomer.CompanyName,
            IsCompany = SelectedCustomer.IsCompany,
            Address = SelectedCustomer.Address,
            ZipCode = SelectedCustomer.ZipCode,
            City = SelectedCustomer.City,
            Country = SelectedCustomer.Country,
            Email = SelectedCustomer.Email,
            Phone = SelectedCustomer.Phone,
            IsActive = SelectedCustomer.IsActive
        };

        IsEditMode = false;
        IsInCreateMode = false;
        IsDetailVisible = true;
        OnPropertyChanged(nameof(FormTitle));
        OnPropertyChanged(nameof(IsInputDisabled));
    }

    [RelayCommand]
    private void HideDetails()
    {
        IsDetailVisible = false;
    }

    [RelayCommand(CanExecute = nameof(CanToggleDetails))]
    private void ToggleDetails()
    {
        if (!IsDetailVisible)
        {
            if (SelectedCustomer is null) return;

            CurrentCustomerForm = new Customer
            {
                Id = SelectedCustomer.Id,
                Prename = SelectedCustomer.Prename,
                Surname = SelectedCustomer.Surname,
                CompanyName = SelectedCustomer.CompanyName,
                IsCompany = SelectedCustomer.IsCompany,
                Address = SelectedCustomer.Address,
                ZipCode = SelectedCustomer.ZipCode,
                City = SelectedCustomer.City,
                Country = SelectedCustomer.Country,
                Email = SelectedCustomer.Email,
                Phone = SelectedCustomer.Phone,
                IsActive = SelectedCustomer.IsActive
            };

            IsEditMode = false;
            IsInCreateMode = false;
        }
        else
        {
            CurrentCustomerForm = null;
        }

        IsDetailVisible = !IsDetailVisible;
        OnPropertyChanged(nameof(FormTitle));
        OnPropertyChanged(nameof(IsInputDisabled));
    }
    #endregion


    partial void OnDisplayNameFilterChanged(string? oldValue, string? newValue)
    {
        ApplyFilter();
    }

    [RelayCommand]
    public async Task SaveCustomerAsync()
    {
        if (CurrentCustomerForm is null) return;

        if (IsInCreateMode)
        {
            await _service.AddAsync(CurrentCustomerForm);
        }
        else if (IsEditMode)
        {
            await _service.UpdateAsync(CurrentCustomerForm);
        }

        await LoadCustomersAsync();
        ResetForm();
    }

    [RelayCommand]
    private void Cancel()
    {
        ResetForm();
    }

    private bool CanEditCustomer() => SelectedCustomer is not null;
    private bool CanShowDetails() => SelectedCustomer is not null;
    private bool CanToggleDetails() => SelectedCustomer is not null;

    private void ResetForm()
    {
        IsDetailVisible = false;
        IsEditMode = false;
        IsInCreateMode = false;
        CurrentCustomerForm = null;
        OnPropertyChanged(nameof(FormTitle));
        OnPropertyChanged(nameof(IsInputDisabled));
    }
}
