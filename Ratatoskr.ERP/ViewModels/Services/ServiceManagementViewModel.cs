using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
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

namespace Ratatoskr.App.ViewModels.Services;

public partial class ServiceManagementViewModel : ObservableObject
{
    private readonly AppDbContext _db;

    public ServiceManagementViewModel(AppDbContext db)
    {
        _db = db;
        LoadDataAsync();
    }

    private ObservableCollection<Service> _allServices = new();

    [ObservableProperty]
    private ObservableCollection<Service> services = new();

    [ObservableProperty]
    private Service? selectedService;

    [ObservableProperty]
    private string? searchText;

    [ObservableProperty]
    private ObservableCollection<ServiceType> serviceTypes = new(Enum.GetValues(typeof(ServiceType)).Cast<ServiceType>());
    [ObservableProperty]
    private ObservableCollection<TaxRate> taxRates = new(Enum.GetValues(typeof(TaxRate)).Cast<TaxRate>());

    #region Formular & Detailbereich

    [ObservableProperty]
    private Service? currentServiceForm;
    [ObservableProperty]
    private bool isDetailVisible;
    [ObservableProperty]
    private bool isInEditMode;

    partial void OnIsInEditModeChanged(bool oldValue, bool newValue)
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

    public string FormTitle => IsInEditMode ? "Leistung bearbeiten" : IsInCreateMode ? "Neue Leistung" : "Details anzeigen";
    public bool IsInputDisabled => !(IsInEditMode || IsInCreateMode);
    #endregion

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        var list = await _db.Services.ToListAsync();
        _allServices = new ObservableCollection<Service>(list);
        ApplyFilter();
    }
    partial void OnSearchTextChanged(string? oldValue, string? newValue) => ApplyFilter();

    [RelayCommand]
    private void ApplyFilter()
    {
        var filtered = _allServices.Where(s =>
        string.IsNullOrWhiteSpace(SearchText) ||
        s.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
        s.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
        s.ArticleNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        Services = new ObservableCollection<Service>(filtered);
    }
    [RelayCommand]
    private void ResetFilters()
    {
        SearchText = null;
        ApplyFilter();
    }

    [RelayCommand]
    private void CreateNewService()
    {
        CurrentServiceForm = new Service();
        IsInCreateMode = true;
        IsInEditMode = false;
        IsDetailVisible = true;
    }

    [RelayCommand]
    private void EditService()
    {
        if (SelectedService == null) return;

        CurrentServiceForm = new Service
        {
            Id = SelectedService.Id,
            ArticleNumber = SelectedService.ArticleNumber,
            Name = SelectedService.Name,
            Description = SelectedService.Description,
            Unit = SelectedService.Unit,
            PriceNet = SelectedService.PriceNet,
            TaxRateEnum = SelectedService.TaxRateEnum,
            ServiceTypeId = SelectedService.ServiceTypeId,
            IsActive = SelectedService.IsActive
        };

        IsInCreateMode = false;
        IsInEditMode = true;
        IsDetailVisible = true;
        
    }

    [RelayCommand]
    private async Task DeleteServiceAsync()
    {
        if (SelectedService is null) return;
        _db.Services.Remove(SelectedService);
        await _db.SaveChangesAsync();
        await LoadDataAsync();
    }

    [RelayCommand]
    private void ToggleDetails()
    {
        if (SelectedService == null) return;
        CurrentServiceForm = SelectedService;
        IsDetailVisible = !IsDetailVisible;
        IsInCreateMode = false;
        IsInEditMode = false;
    }

    [RelayCommand]
    private async Task SaveServiceAsync()
    {
        if (CurrentServiceForm is null) return;
        if (IsInCreateMode)
        {
            CurrentServiceForm.ArticleNumber = await GenerateArticleNumberAsync(CurrentServiceForm.ServiceTypeId);
            await _db.Services.AddAsync(CurrentServiceForm);
        }
        else if (IsInEditMode)
        {
            var local = _db.Services.Local.FirstOrDefault(s => s.Id == CurrentServiceForm.Id);
            if (local is not null) _db.Entry(local).State = EntityState.Detached;
            _db.Entry(CurrentServiceForm).State = EntityState.Modified;
        }

        await _db.SaveChangesAsync();
        await LoadDataAsync();
        ResetForm();
    }

    [RelayCommand]
    private void Cancel()
    {
        ResetForm();
    }

    private void ResetForm()
    {
        IsInEditMode = false;
        IsInCreateMode = false;
        IsDetailVisible = false;
        CurrentServiceForm = null;
        OnPropertyChanged(nameof(IsInputDisabled));
        OnPropertyChanged(nameof(FormTitle));
    }

    private async Task<string> GenerateArticleNumberAsync(int typeId)
    {
        var typePrefix = typeId switch
        {
            0 => "PRD",
            1 => "DL",
            2 => "SON",
            _ => "UNK"
        };

        var existingNumbers = await _db.Services
            .Where(s => s.ServiceTypeId == typeId)
            .Select(s => s.ArticleNumber)
            .ToListAsync();

        int index = 1;
        string generated;

        do
        {
            generated = $"{typePrefix}-{index:000}";
            index++;
        }
        while (existingNumbers.Contains(generated));

        return generated;
    }
}


