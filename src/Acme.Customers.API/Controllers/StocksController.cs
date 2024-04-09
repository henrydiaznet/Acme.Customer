using Acme.Customers.Domain;
using Acme.Customers.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Customers.API.Controllers;

[ApiController]
[Produces("application/json")]
[Route("[controller]")]
public class StocksController : ControllerBase
{
    private readonly IOrderRepository _repository;

    public StocksController(IOrderRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Lists available stock
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(IEnumerable<StockItem>), 200)]
    [HttpGet]
    public async Task<IActionResult> GetAllItems(CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllStockItemsAsync(cancellationToken);
        return Ok(result);
    }
}