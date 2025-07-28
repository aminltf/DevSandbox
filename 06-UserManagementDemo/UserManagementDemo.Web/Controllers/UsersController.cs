using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using UserManagementDemo.Application.Features.Users.Commands;
using UserManagementDemo.Application.Features.Users.Dtos;
using UserManagementDemo.Application.Features.Users.Queries;

namespace UserManagementDemo.Web.Controllers;


//[ApiController]
//[Route("api/[controller]")]
//[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var command = new CreateUserCommand(dto);
        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result.UserId }, result);
    }

    [HttpGet("{id:guid}/get-by-id")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetUserByIdQuery(id);
        var user = await _mediator.Send(query);

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost("login")]
    //[AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var command = new LoginCommand(dto);
        var result = await _mediator.Send(command);

        if (result.Token is null && !result.IsPasswordChangeRequired)
            return Unauthorized(new { result.Message });

        if (result.IsPasswordChangeRequired)
            return Ok(result);

        return Ok(result);
    }

    [HttpPost("change-password")]
    //[AllowAnonymous]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var command = new ChangePasswordCommand(dto);
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(new { result.Message });

        return Ok(result);
    }

}
