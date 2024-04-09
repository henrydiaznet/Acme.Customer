using Acme.Customers.API.Mediatr.Commands;
using Acme.Customers.Domain;
using MediatR;

namespace Acme.Customers.API.Mediatr.Handlers;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
{
    private readonly ICustomerRepository _repository;

    public CancelOrderCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await _repository.FindByIdAsync(request.CustomerId, cancellationToken);
        customer.CancelOrder(request.OrderId);
        _repository.Update(customer);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}