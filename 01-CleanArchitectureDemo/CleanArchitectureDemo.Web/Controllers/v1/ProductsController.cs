using CleanArchitectureDemo.Application.Features.Products.Commands.Create;
using CleanArchitectureDemo.Application.Features.Products.Commands.Delete;
using CleanArchitectureDemo.Application.Features.Products.Commands.Restore;
using CleanArchitectureDemo.Application.Features.Products.Commands.Update;
using CleanArchitectureDemo.Application.Features.Products.Queries.Export;
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

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteProductCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}/softDelete")]
    public async Task<IActionResult> SoftDelete(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SoftDeleteProductCommand(id), cancellationToken);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("{id}/restore")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new RestoreProductCommand(id), cancellationToken);
        return result ? NoContent() : NotFound();
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

    [HttpPost("export-to-excel")]
    public async Task<IActionResult> ExportToExcel([FromBody] ExportProductToExcelQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var fileContent = await Mediator.Send(query, cancellationToken);

            if (fileContent == null || fileContent.Length == 0)
                return BadRequest(new { error = "Exported file is empty." });

            var fileName = $"Products_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(fileContent, contentType, fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
        }
    }
}
