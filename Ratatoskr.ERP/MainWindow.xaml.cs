using AvalonDock.Layout;
using Ratatoskr.App.Helpers;
using Ratatoskr.App.Views.Customers;
using Ratatoskr.App.Views.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            //MainContent.Content = ViewLoader.LoadView<CustomerView>();
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
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}