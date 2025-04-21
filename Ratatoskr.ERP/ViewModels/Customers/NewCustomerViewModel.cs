using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ratatoskr.App.ViewModels.Customers;

public partial class NewCustomerViewModel : ObservableObject, INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string>> _errors = new();

    public bool HasErrors => _errors.Any();
    public bool CanSave => !HasErrors;

    public int Id { get; set; }
    public bool HasLockedFields { get; set; }

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName)) return Enumerable.Empty<string>();
        return _errors.TryGetValue(propertyName, out var list) ? list : Enumerable.Empty<string>();
    }

    private void AddError(string propertyName, string error)
    {
        if (!_errors.ContainsKey(propertyName))
            _errors[propertyName] = new List<string>();

        if (!_errors[propertyName].Contains(error))
        {
            _errors[propertyName].Add(error);
            OnErrorsChanged(propertyName);
        }
    }

    private void ClearErrors(string propertyName)
    {
        if (_errors.Remove(propertyName))
            OnErrorsChanged(propertyName);
    }

    private void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(nameof(HasErrors));
        OnPropertyChanged(nameof(CanSave));
    }

    // Felder mit Validierung

    [ObservableProperty] 
    private string prename = string.Empty;

    partial void OnPrenameChanged(string value)
    {
        System.Diagnostics.Debug.WriteLine($"[DEBUG] Prename changed to: '{value}'");
        ClearErrors(nameof(Prename));
        if (string.IsNullOrWhiteSpace(value))
            AddError(nameof(Prename), "Vorname darf nicht leer sein.");
    }

    [ObservableProperty] 
    private string surname = string.Empty;

    partial void OnSurnameChanged(string value)
    {
        ClearErrors(nameof(Surname));
        if (string.IsNullOrWhiteSpace(value))
            AddError(nameof(Surname), "Nachname darf nicht leer sein.");
    }

    [ObservableProperty] 
    private string email = string.Empty;

    partial void OnEmailChanged(string value)
    {
        ClearErrors(nameof(Email));
        if (string.IsNullOrWhiteSpace(value))
            AddError(nameof(Email), "E-Mail darf nicht leer sein.");
        else if (!value.Contains("@"))
            AddError(nameof(Email), "Ungültige E-Mail-Adresse.");
    }

    [ObservableProperty] private string phone = string.Empty;
    [ObservableProperty] private string address = string.Empty;
    [ObservableProperty] private string zipCode = string.Empty;
    [ObservableProperty] private string city = string.Empty;
    [ObservableProperty] private string country = string.Empty;

    public NewCustomerViewModel()
    {
        // Initiale Validierung aufrufen
        OnPrenameChanged(Prename);
        OnSurnameChanged(Surname);
        OnEmailChanged(Email);
    }

}
