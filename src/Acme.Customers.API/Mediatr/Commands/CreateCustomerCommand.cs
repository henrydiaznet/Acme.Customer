using System.ComponentModel.DataAnnotations;
using Acme.Customers.Domain.Model;
using MediatR;

namespace Acme.Customers.API.Mediatr.Commands;

public class CreateCustomerCommand : IRequest<Customer>
{
    [Required]
    public string FullName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public string Address { get; set; }
}