using Ratatoskr.App.ViewModels.Customers;
using Ratatoskr.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ratatoskr.App.View.Customers;

/// <summary>
/// Interaktionslogik für NewCustomerView.xaml
/// </summary>
public partial class NewCustomerView : Window
{
    private readonly bool _isEditMode;
    private readonly bool _hasLockedFields;
    
    public NewCustomerViewModel ViewModel { get; } = new();
    public Customer? CreatedCustomer { get; private set; }



    public NewCustomerView(Customer? customer = null, bool isEditMode = false, bool hasLockedFields = false)
    {
        InitializeComponent();
        DataContext = ViewModel;
        _isEditMode = isEditMode;
        _hasLockedFields = hasLockedFields;

        if (customer is not null)
        {
            ViewModel.Id = customer.Id;
            ViewModel.Prename = customer.Prename;
            ViewModel.Surname = customer.Surname;
            ViewModel.Email = customer.Email;
            ViewModel.Phone = customer.Phone;
            ViewModel.Address = customer.Address;
            ViewModel.ZipCode = customer.ZipCode;
            ViewModel.City = customer.City;
            ViewModel.Country = customer.Country;
        }

        ViewModel.HasLockedFields = hasLockedFields;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (!ViewModel.CanSave)
        {
            MessageBox.Show("Bitte fülle alle Pflichtfelder korrekt aus.", "Eingabefehler");
            return;
        }

        CreatedCustomer = new Customer
        {
            Id = _isEditMode ? ViewModel.Id : 0,
            Prename = ViewModel.Prename,
            Surname = ViewModel.Surname,
            Email = ViewModel.Email,
            Phone = ViewModel.Phone,
            Address = ViewModel.Address,
            ZipCode = ViewModel.ZipCode,
            City = ViewModel.City,
            Country = ViewModel.Country
        };

        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
