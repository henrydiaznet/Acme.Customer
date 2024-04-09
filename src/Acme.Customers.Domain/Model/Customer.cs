using Acme.Customers.Domain.Exceptions;

namespace Acme.Customers.Domain.Model;

public class Customer
{
    private readonly List<Order> _orders = new();

    public Customer() { }

    public Customer(string fullName, ContactInformation contactInformation, IEnumerable<Order> orders)
    {
        FullName = fullName;
        ContactInformation = contactInformation;
        _orders = orders.ToList();
    }

    public int Id { get; set; }
    public string FullName { get; set; }
    public ContactInformation? ContactInformation { get; set; }
    public IEnumerable<Order> Orders => _orders.AsReadOnly();

    public void AddOrder(Order order)
    {
        if (_orders.Any(x => x.Id == order.Id))
        {
            throw new OrderException("Order already exists");
        }

        if (_orders.Any(x => x.Status == OrderStatus.Processing))
        {
            throw new OrderException("Can't create an order while another order is processing");
        }

        _orders.Add(order);
    }

    public void CancelOrder(int orderId)
    {
        if (_orders.All(x => x.Id != orderId))
        {
            throw new OrderException("Trying to cancel order that doesn't exist");
        }

        var toCancel = _orders.First(x => x.Id == orderId);

        if (toCancel.Status != OrderStatus.Processing)
        {
            throw new OrderException("Trying to cancel order that is either cancelled or fulfilled");
        }

        toCancel.CancelOrder();
    }

    public void FulfillOrder(int orderId)
    {
        if (_orders.All(x => x.Id != orderId))
        {
            throw new OrderException("Trying to fulfill order that doesn't exist");
        }
        
        var toFulfill = _orders.First(x => x.Id == orderId);
        if (toFulfill.Status != OrderStatus.Processing)
        {
            throw new OrderException("Trying to cancel order that is either cancelled or fulfilled");
        }

        toFulfill.FulfillOrder();
    }
}