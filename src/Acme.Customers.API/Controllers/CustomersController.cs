using Acme.Customers.API.Authentication;
using Acme.Customers.API.Dtos;
using Acme.Customers.API.Mediatr.Commands;
using Acme.Customers.Domain;
using Acme.Customers.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Customers.API.Controllers;

/// <summary>
///     Customers controller
/// </summary>
[ApiController]
[ServiceFilter(typeof(ApiKeyAuthFilter))]
[Produces("application/json")]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICustomerRepository _repository;

    public CustomersController(IMediator mediator, ICustomerRepository repository)
    {
        _mediator = mediator;
        _repository = repository;
    }

    /// <summary>
    /// Returns a list of all customers
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(IEnumerable<Customer>), 200)]
    [HttpGet]
    public async Task<IActionResult> GetAllCustomers(CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync(cancellationToken);
        return Ok(result.Select(x => new CustomerShortResponse { Id = x.Id, FullName = x.FullName }));
    }

    /// <summary>
    /// Returns a specific customer by id
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(Customer), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{customerId}")]
    public async Task<IActionResult> Find([FromRoute] int customerId, CancellationToken cancellationToken)
    {
        var result = await _repository.FindByIdAsync(customerId, cancellationToken);
        return result is null ? NotFound() : Ok(new CustomerFullResponse(result));
    }

    /// <summary>
    /// Create customer
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(Customer), 201)]
    [HttpPut]
    public async Task<IActionResult> Create(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        var routeValues = new { CustomerId = result.Id };
        return CreatedAtRoute(routeValues, result);
    }

    /// <summary>
    /// Update customer
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(Customer), 200)]
    [HttpPut("{customerId}")]
    public async Task<IActionResult> Update([FromRoute] int customerId, UpdateCustomerCommand command,
        CancellationToken cancellationToken)
    {
        command.CustomerId = customerId;
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Delete customer
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(200)]
    [HttpDelete("{customerId}")]
    public async Task<IActionResult> Delete([FromRoute] DeleteCustomerCommand command,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}