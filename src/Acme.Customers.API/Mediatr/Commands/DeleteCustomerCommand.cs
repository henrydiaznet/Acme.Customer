using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Customers.API.Mediatr.Commands;

public class DeleteCustomerCommand : IRequest
{
    [FromRoute(Name = "customerId")] 
    public int CustomerId { get; set; }
}