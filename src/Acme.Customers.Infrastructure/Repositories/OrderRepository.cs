using Acme.Customers.Domain;
using Acme.Customers.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Acme.Customers.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly CustomerContext _context;

    public OrderRepository(CustomerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StockItem>> GetAllStockItemsAsync(CancellationToken cancellationToken)
    {
        return await _context
            .StockItems
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<Order> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Orders
            .Where(x => x.Id == id)
            .Include(x => x.Items)
            .ThenInclude(x => x.StockItem)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetAllForCustomerAsync(int customerId, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Include(x => x.Items)
            .ThenInclude(x => x.StockItem)
            .Where(x => x.CustomerId == customerId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
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