using CleanArchitectureDemo.Application.Features.Products.Commands.Create;
using CleanArchitectureDemo.Application.Features.Products.Queries.GetAll;
using CleanArchitectureDemo.Application.Features.Products.Queries.GetById;
using CleanArchitectureDemo.Web.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureDemo.Web.Controllers.v1;

[ApiVersion("1.0")]
public class ProductsController : BaseController<ProductsController>
{
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var id = await Mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpGet("{id}/getById")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(id);
        var result = await Mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll([FromQuery] GetProductsQuery query, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
