namespace Acme.Customers.Domain.Model;

public class ContactInformation
{
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public int CustomerId { get; set; }
}