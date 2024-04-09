using Acme.Customers.API.Authentication;
using Acme.Customers.API.Dtos;
using Acme.Customers.API.Mediatr.Commands;
using Acme.Customers.Domain;
using Acme.Customers.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Customers.API.Controllers;

/// <summary>
/// Orders controller
/// </summary>
[ApiController]
[ServiceFilter(typeof(ApiKeyAuthFilter))]
[Produces("application/json")]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IOrderRepository _repository;

    public OrdersController(IMediator mediator, IOrderRepository repository)
    {
        _mediator = mediator;
        _repository = repository;
    }
    
    /// <summary>
    /// Returns a list of Orders for customer specified by Id 
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(IEnumerable<OrderResponse>), 200)]
    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetOrdersForCustomer([FromRoute] int customerId,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllForCustomerAsync(customerId, cancellationToken);
        return Ok(result.Select(x => new OrderResponse(x)));
    }

    /// <summary>
    /// Returns order specified by Id
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(OrderResponse), 200)]
    [ProducesResponseType(typeof(OrderResponse), 404)]
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder([FromRoute] int orderId, CancellationToken cancellationToken)
    {
        var result = await _repository.FindByIdAsync(orderId, cancellationToken);
        return result is null ? NotFound() : Ok(new OrderResponse(result));
    }

    /// <summary>
    /// Creates new Order
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(OrderResponse), 201)]
    [HttpPut]
    public async Task<IActionResult> CreateOrder(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        var routeValues = new { OrderId = result.Id };
        return CreatedAtRoute(routeValues, new OrderResponse(result));
    }

    /// <summary>
    /// Cancels Order
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(200)]
    [HttpPost]
    public async Task<IActionResult> CancelOrder(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}