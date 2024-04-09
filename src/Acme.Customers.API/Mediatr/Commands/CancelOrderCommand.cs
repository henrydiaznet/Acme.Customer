using MediatR;

namespace Acme.Customers.API.Mediatr.Commands;

public class CancelOrderCommand : IRequest
{
    public int CustomerId { get; set; }
    public int OrderId { get; set; }
}