using System.Text.Json.Serialization;
using Acme.Customers.Domain.Model;

namespace Acme.Customers.API.Dtos;

public class OrderResponse
{
    public int Id { get; init; }
    public int CustomerId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; set; }
    public IEnumerable<OrderItem> Items { get; set; }

    public OrderResponse()
    { }

    public OrderResponse(Order order)
    {
        Id = order.Id;
        CustomerId = order.CustomerId;
        Status = order.Status;
        Items = order.Items;
    }
}