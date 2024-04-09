using System.Text.Json.Serialization;

namespace Acme.Customers.Domain.Model;

public class Order
{
    private readonly List<OrderItem> _items = new();

    public Order() { }

    public Order(IEnumerable<OrderItem> items)
    {
        _items = items.ToList();
    }

    public int Id { get; init; }
    public int CustomerId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; private set; } = OrderStatus.Processing;
    public IEnumerable<OrderItem> Items => _items.AsReadOnly();

    public void CancelOrder()
    {
        Status = OrderStatus.Cancelled;
    }

    public void FulfillOrder()
    {
        Status = OrderStatus.Fulfilled;
    }
}