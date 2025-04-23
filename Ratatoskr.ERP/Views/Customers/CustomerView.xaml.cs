using Microsoft.Extensions.DependencyInjection;
using Ratatoskr.App.ViewModels.Customers;
using System.Windows.Controls;

namespace Ratatoskr.App.Views.Customers
{
    /// <summary>
    /// Interaktionslogik für CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
        public CustomerView()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetRequiredService<CustomersViewModel>();
        }
    }
}
