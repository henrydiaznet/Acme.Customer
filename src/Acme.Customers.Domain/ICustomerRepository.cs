using Acme.Customers.Domain.Model;

namespace Acme.Customers.Domain;

public interface ICustomerRepository : IUnitOfWork
{
    Task<Customer> FindByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken);
    void Update(Customer update);
    void Add(Customer add);
    Task DeleteAsync(int customerId, CancellationToken cancellationToken);
}