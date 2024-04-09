using Acme.Customers.API.Mediatr.Commands;
using Acme.Customers.Domain;
using Acme.Customers.Domain.Exceptions;
using Acme.Customers.Domain.Model;
using MediatR;

namespace Acme.Customers.API.Mediatr.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateOrderCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var items = request.Items.Select(x => new OrderItem { ItemId = x.ItemId, Quantity = x.Quantity });
        var order = new Order(items);
        var customer = await _customerRepository.FindByIdAsync(request.CustomerId, cancellationToken);
        if (customer is null)
        {
            throw new OrderException($"Customer with id: {request.CustomerId} is not found");
        }
        
        customer.AddOrder(order);
        _customerRepository.Update(customer);
        await _customerRepository.SaveChangesAsync(cancellationToken);
        return order;
    }
}