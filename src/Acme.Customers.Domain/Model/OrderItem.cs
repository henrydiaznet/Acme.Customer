using System.Text.Json.Serialization;

namespace Acme.Customers.Domain.Model;

public class OrderItem
{
    public int OrderId { get; set; }
    [JsonIgnore] 
    public Order Order { get; set; }
    public int ItemId { get; set; }
    public StockItem StockItem { get; set; }
    public int Quantity { get; set; }
}