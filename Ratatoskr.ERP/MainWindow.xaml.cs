using AvalonDock.Layout;
using Microsoft.EntityFrameworkCore;
using Ratatoskr.App.Helpers;
using Ratatoskr.App.ViewModels.Invoices;
using Ratatoskr.App.Views.Customers;
using Ratatoskr.App.Views.Invoices;
using Ratatoskr.App.Views.Services;
using Ratatoskr.Infrastructure.Database;
using System.Windows;

namespace Ratatoskr.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
        }
        private void OpenCustomerView_Click(object sender, RoutedEventArgs e)
        {
            // 🔍 Prüfen, ob bereits offen
            var existingDoc = MainDocumentPane.Children
                .OfType<LayoutDocument>()
                .FirstOrDefault(d => d.Title == "Kunden");

            if (existingDoc is not null)
            {
                existingDoc.IsActive = true;
                return;
            }

            // 📄 Neue View laden
            var customerView = ViewLoader.LoadView<CustomerView>();

            var document = new LayoutDocument
            {
                Title = "Kunden",
                Content = customerView
            };

            MainDocumentPane.Children.Add(document);
            document.IsActive = true;
        }
        private void OpenServiceView_Click(object sender, RoutedEventArgs e)
        {
            // 🔍 Bereits geöffnet?
            var existingDoc = MainDocumentPane.Children
                .OfType<LayoutDocument>()
                .FirstOrDefault(d => d.Title == "Leistungen");

            if (existingDoc is not null)
            {
                existingDoc.IsActive = true;
                return;
            }

            // 📄 Neue View laden
            var serviceView = ViewLoader.LoadView<ServiceManagementView>();

            var document = new LayoutDocument
            {
                Title = "Leistungen",
                Content = serviceView
            };

            MainDocumentPane.Children.Add(document);
            document.IsActive = true;
        }
        private void OpenInvoiceView_Click(object sender, RoutedEventArgs e)
        {
            // 🔍 Bereits geöffnet?
            var existingDoc = MainDocumentPane.Children
                .OfType<LayoutDocument>()
                .FirstOrDefault(d => d.Title == "Neue Rechnung");

            if (existingDoc is not null)
            {
                existingDoc.IsActive = true;
                return;
            }

            // 📄 Neue View laden
            var invoiceView = ViewLoader.LoadView<NewInvoiceView>();
            var document = new LayoutDocument
            {
                Title = "Neue Rechnung",
                Content = invoiceView
            };

            MainDocumentPane.Children.Add(document);
            document.IsActive = true;
        }
        private void OpenAllInvoicesView_Click(object sender, RoutedEventArgs e)
        {
            // 🔍 Bereits geöffnet?
            var existingDoc = MainDocumentPane.Children
                .OfType<LayoutDocument>()
                .FirstOrDefault(d => d.Title == "Rechnungen");

            if (existingDoc is not null)
            {
                existingDoc.IsActive = true;
                return;
            }

            // 📄 Neue View laden
            var invoiceView = ViewLoader.LoadView<InvoicesView>();
            var document = new LayoutDocument
            {
                Title = "Rechnungen anzeigen",
                Content = invoiceView
            };

            MainDocumentPane.Children.Add(document);
            document.IsActive = true;
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}