using Acme.Customers.Domain.Model;

namespace Acme.Customers.Domain;

public interface IOrderRepository : IUnitOfWork
{
    Task<IEnumerable<StockItem>> GetAllStockItemsAsync(CancellationToken cancellationToken);
    Task<Order> FindByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Order>> GetAllForCustomerAsync(int customerId, CancellationToken cancellationToken);
}