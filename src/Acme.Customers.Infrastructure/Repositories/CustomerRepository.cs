using Acme.Customers.Domain;
using Acme.Customers.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Acme.Customers.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerContext _context;

    public CustomerRepository(CustomerContext context)
    {
        _context = context;
    }

    public Task<Customer> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Customers
            .Include(x => x.ContactInformation)
            .Include(x => x.Orders)
            .ThenInclude(x => x.Items)
            .ThenInclude(x => x.StockItem)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Customers
            .Include(x => x.ContactInformation)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public void Update(Customer update)
    {
        _context.Entry(update).State = EntityState.Modified;
    }

    public void Add(Customer add)
    {
        _context.Customers.Add(add);
    }

    public async Task DeleteAsync(int customerId, CancellationToken cancellationToken)
    {
        var customer = await FindByIdAsync(customerId, cancellationToken);
        if (customer is null) return;

        _context.Customers.Remove(customer);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}