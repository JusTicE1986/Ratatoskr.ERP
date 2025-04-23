using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ratatoskr.Core.Models;

namespace Ratatoskr.Infrastructure.Database;

public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ServiceType> ServiceTypes => Set<ServiceType>();
    public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options ) 
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ServiceType>().HasData(
        new ServiceType { Id = 1, Code = "DL", Name = "Dienstleistung" },
        new ServiceType { Id = 2, Code = "PR", Name = "Produkt" },
        new ServiceType { Id = 3, Code = "MI", Name = "Miete" }
    );

        modelBuilder.Entity<ServiceCategory>().HasData(
            new ServiceCategory { Id = 1, Code = "BE", Name = "Beratung" },
            new ServiceCategory { Id = 2, Code = "WT", Name = "Wartung" },
            new ServiceCategory { Id = 3, Code = "SC", Name = "Schulung" }
        );

        modelBuilder.Entity<InvoiceItem>()
            .HasOne(ii => ii.Invoice)
            .WithMany(i => i.Items)
            .HasForeignKey(ii => ii.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
