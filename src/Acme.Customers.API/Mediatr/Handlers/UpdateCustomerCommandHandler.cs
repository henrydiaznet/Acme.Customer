using Acme.Customers.API.Mediatr.Commands;
using Acme.Customers.Domain;
using Acme.Customers.Domain.Model;
using MediatR;

namespace Acme.Customers.API.Mediatr.Handlers;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Customer>
{
    private readonly ICustomerRepository _repository;

    public UpdateCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Customer> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _repository.FindByIdAsync(request.CustomerId, cancellationToken);
        customer.FullName = request.FullName;
        customer.ContactInformation ??= new ContactInformation();
        customer.ContactInformation.Address = request.Address;
        customer.ContactInformation.Phone = request.Phone;

        _repository.Update(customer);
        await _repository.SaveChangesAsync(cancellationToken);
        return customer;
    }
}