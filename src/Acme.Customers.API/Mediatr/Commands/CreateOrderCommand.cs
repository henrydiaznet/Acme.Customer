using System.ComponentModel.DataAnnotations;
using Acme.Customers.Domain.Model;
using MediatR;

namespace Acme.Customers.API.Mediatr.Commands;

public class CreateOrderCommand : IRequest<Order>
{
    public int CustomerId { get; set; }
    [Required]
    public IEnumerable<BasketItem> Items { get; set; }
}

public record BasketItem(int ItemId, int Quantity);