using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
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
        LoadData();
    }

    public ObservableCollection<Service> Services { get; set; } = new();
    public ObservableCollection<ServiceType> ServiceTypes { get; set; } = new();
    public ObservableCollection<ServiceCategory> ServiceCategories { get; set; } = new();
    public ObservableCollection<Service> FilteredServices { get; set; } = new();

    private Service? _selectedService;
    public Service? SelectedService
    {
        get => _selectedService;
        set
        {
            SetProperty(ref _selectedService, value);
            EditCommand.NotifyCanExecuteChanged(); // <- manuell ergänzen
        }
    }

    [ObservableProperty]
    private bool isInEditMode = false;
    [ObservableProperty]
    private string? searchText;
    [ObservableProperty]
    private int? selectedTypeId;
    [ObservableProperty]
    private int? selectedCategoryId;

    [RelayCommand]
    private void New()
    {
        SelectedService = new Service();
        IsInEditMode = false;
    }
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedService is null) return;

        var duplicate = await _db.Services.AnyAsync(s =>
        s.Name.ToLower() == SelectedService.Name.ToLower() &&
        s.ServiceTypeId == SelectedService.ServiceTypeId &&
        s.ServiceCategoryId == SelectedService.ServiceCategoryId &&
        (!IsInEditMode || s.Id != SelectedService.Id));

        if (duplicate)
        {
            var type = ServiceTypes.FirstOrDefault(t => t.Id == SelectedService.ServiceTypeId)?.Name ?? "Unbekannt";
            var category = ServiceCategories.FirstOrDefault(c => c.Id == SelectedService.ServiceCategoryId)?.Name ?? "Unbekannt";

            string msg = $"Die Leistung \"{SelectedService.Name}\" existiert bereits " +
                $"in der Kombination: \n\n" +
                $"Typ: {type}\n" +
                $"Kategorie: {category}";

            MessageBox.Show(msg, "Doppelter Eintrag", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!IsInEditMode)
        {
            SelectedService.ArticleNumber = await GenerateArticleNumberAsync(
                SelectedService.ServiceTypeId,
                SelectedService.ServiceCategoryId
                );
            await _db.Services.AddAsync(SelectedService);
        }
        else
        {
            _db.Services.Update(SelectedService);
        }

        await _db.SaveChangesAsync();
        await ReloadServicesAsync();
        SelectedService = new();
        IsInEditMode = false;
    }
    [RelayCommand]
    private async Task DeleteAsync(Service? service)
    {
        if (service is null) return;

        _db.Services.Remove(service);
        await _db.SaveChangesAsync();
        await ReloadServicesAsync();
    }
    [RelayCommand]
    private void Edit(Service? service)
    {
        if (service == null) return;
        SelectedService = new Service
        {
            Id = service.Id,
            ArticleNumber = service.ArticleNumber,
            Name = service.Name,
            Description = service.Description,
            Unit = service.Unit,
            PriceNet = service.PriceNet,
            TaxRate = service.TaxRate,
            ServiceTypeId = service.ServiceTypeId,
            ServiceCategoryId = service.ServiceCategoryId,
            IsActive = service.IsActive
        };

        IsInEditMode = true;
    }

    private bool CanEdit(Service? service)
    {
        return service != null;
    }

    private async void LoadData()
    {
        await ReloadServicesAsync();
        await ReloadTypesAndCategoriesAsync();
    }

    private async Task ReloadServicesAsync()
    {
        Services.Clear();
        var list = await _db.Services.Include(s => s.ServiceType).Include(s => s.ServiceCategory).ToListAsync();
        foreach (var item in list)
        {
            Services.Add(item);
        }
        ApplyFilter();
    }

    private async Task ReloadTypesAndCategoriesAsync()
    {
        ServiceTypes.Clear();
        ServiceCategories.Clear();

        var types = await _db.ServiceTypes.ToListAsync();
        foreach (var type in types)
        {
            ServiceTypes.Add(type);
        }

        var categories = await _db.ServiceCategories.ToListAsync();
        foreach (var category in categories)
        {
            ServiceCategories.Add(category);
        }
    }

    private async Task<string> GenerateArticleNumberAsync(int serviceTypeId, int serviceCategoryId)
    {
        var type = await _db.ServiceTypes.FindAsync(serviceTypeId);
        var category = await _db.ServiceCategories.FindAsync(serviceCategoryId);

        if (type is null || category is null) return "INVALID-REF";

        var existing = await _db.Services
            .Where(s => s.ServiceTypeId == serviceTypeId && s.ServiceCategoryId == serviceCategoryId)
            .Select(s => s.ArticleNumber)
            .ToListAsync();

        int index = 1;
        string generated;

        do
        {
            generated = $"{type.Code}-{category.Code}-{index:000}";
            index++;
        }
        while (existing.Contains(generated));

        return generated;
    }

    partial void OnSearchTextChanged(string? value) => ApplyFilter();
    partial void OnSelectedTypeIdChanged(int? value) => ApplyFilter();
    partial void OnSelectedCategoryIdChanged(int? value) => ApplyFilter();

    private void ApplyFilter()
    {
        var filtered = Services.Where(s =>
        (string.IsNullOrWhiteSpace(SearchText) ||
        s.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
        s.Description?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
        s.ArticleNumber?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true)
        &&
        (!SelectedTypeId.HasValue || s.ServiceTypeId == SelectedTypeId)
        &&
        (!SelectedCategoryId.HasValue || s.ServiceCategoryId == SelectedCategoryId)
        );

        FilteredServices.Clear();
        foreach (var s in filtered)
        {
            FilteredServices.Add(s);
        }
    }
}


