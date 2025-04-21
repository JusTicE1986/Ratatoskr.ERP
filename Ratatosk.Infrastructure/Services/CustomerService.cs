using Microsoft.EntityFrameworkCore;
using Ratatoskr.Core.Models;
using Ratatoskr.Infrastructure.Database;

namespace Ratatoskr.Infrastructure.Services;

public class CustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .OrderBy(c => c.Surname)
            .ToListAsync();
    }

    public async Task AddAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Customer customer)
    {
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        // Prüfen, ob bereits eine Instanz mit derselben Id im Kontext getrackt wird
        var trackedEntity = _context.ChangeTracker
            .Entries<Customer>()
            .FirstOrDefault(e => e.Entity.Id == customer.Id);

        if (trackedEntity is not null)
            trackedEntity.State = EntityState.Detached;

        // Nun sicher aktualisieren
        _context.Entry(customer).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

}
