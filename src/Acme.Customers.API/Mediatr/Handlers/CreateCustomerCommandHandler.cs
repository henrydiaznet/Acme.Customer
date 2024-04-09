using Acme.Customers.API.Mediatr.Commands;
using Acme.Customers.Domain;
using Acme.Customers.Domain.Model;
using MediatR;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Acme.Customers.API.Mediatr.Handlers;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Customer>
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer(request.FullName, new ContactInformation
        {
            Address = request.Address,
            Email = request.Email,
            Phone = request.Phone
        }, Array.Empty<Order>());

        _repository.Add(customer);
        await _repository.SaveChangesAsync(cancellationToken);

        return customer;
    }
}