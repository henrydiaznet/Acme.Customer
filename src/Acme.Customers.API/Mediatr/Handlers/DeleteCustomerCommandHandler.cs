using Acme.Customers.API.Mediatr.Commands;
using Acme.Customers.Domain;
using MediatR;

namespace Acme.Customers.API.Mediatr.Handlers;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
{
    private readonly ICustomerRepository _repository;

    public DeleteCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.CustomerId, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}