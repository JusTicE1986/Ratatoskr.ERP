using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ratatoskr.App.ViewModels.Customers;
using Ratatoskr.App.ViewModels.Invoices;
using Ratatoskr.App.ViewModels.Services;
using Ratatoskr.App.Views.Customers;
using Ratatoskr.App.Views.Invoices;
using Ratatoskr.App.Views.Services;
using Ratatoskr.Infrastructure.Database;
using Ratatoskr.Infrastructure.Services;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace Ratatoskr.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(AppContext.BaseDirectory);
                config.AddJsonFile("appsettings.json", optional: true);
            })
            .ConfigureServices((context, services) =>
            {
                var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Ratatoskr");
                Directory.CreateDirectory(appDataPath);
                var dbPath = Path.Combine(appDataPath, "ratatoskr.db");

                services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

                services.AddTransient<CustomerService>();
                services.AddTransient<CustomerViewModel>();
                services.AddTransient<CustomerView>();

                services.AddTransient<ServiceManagementView>();
                services.AddTransient<ServiceManagementViewModel>();

                services.AddTransient<NewInvoiceView>();
                services.AddTransient<NewInvoiceViewModel>();

                services.AddTransient<InvoicesView>();
                services.AddTransient<InvoicesViewModel>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost.StartAsync();

        using (var scope = AppHost.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        }

        var mainWindow = new MainWindow();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost.StopAsync();
        base.OnExit(e);
    }
}
