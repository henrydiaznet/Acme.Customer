using Acme.Customers.Domain.Model;

namespace Acme.Customers.API.Dtos;

public class CustomerFullResponse
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public IEnumerable<Order> Orders { get; set; }

    public CustomerFullResponse() { }

    public CustomerFullResponse(Customer customer)
    {
        Id = customer.Id;
        FullName = customer.FullName;
        Email = customer.ContactInformation?.Email;
        Phone = customer.ContactInformation?.Phone;
        Address = customer.ContactInformation?.Address;
        Orders = customer.Orders;
    }    
}