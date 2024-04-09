using System.ComponentModel.DataAnnotations;
using Acme.Customers.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Customers.API.Mediatr.Commands;

public class UpdateCustomerCommand : IRequest<Customer>
{
    [FromRoute] 
    public int CustomerId { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public string Address { get; set; }
}