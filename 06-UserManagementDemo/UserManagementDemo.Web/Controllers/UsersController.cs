using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using UserManagementDemo.Application.Features.Users.Commands;
using UserManagementDemo.Application.Features.Users.Dtos;
using UserManagementDemo.Application.Features.Users.Queries;
using UserManagementDemo.Application.Features.RefreshTokens.Commands;
using UserManagementDemo.Application.Features.RefreshTokens.Dtos;
using UserManagementDemo.Application.Features.PasswordResetRequests.Commands;
using UserManagementDemo.Application.Features.PasswordResetRequests.Dtos;

namespace UserManagementDemo.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(dto);
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = result.UserId }, result);
    }

    [HttpPut("{id:guid}/update")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
    {
        dto.Id = id;
        var result = await _mediator.Send(new UpdateUserCommand(dto));
        if (!result.Success)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> ActivateUser(Guid id)
    {
        var result = await _mediator.Send(new ActivateUserCommand(id));
        if (!result.Success)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateUser(Guid id)
    {
        var result = await _mediator.Send(new DeactivateUserCommand(id));
        if (!result.Success)
            return BadRequest(result);
        return Ok(result);
    }


    [HttpGet("{id:guid}/get-by-id")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var user = await _mediator.Send(query, cancellationToken);

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet("get-users")]
    //[Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("login")]
    //[AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(dto);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.Token is null && !result.IsPasswordChangeRequired)
            return Unauthorized(new { result.Message });

        if (result.IsPasswordChangeRequired)
            return Ok(result);

        return Ok(result);
    }

    [HttpPost("change-password")]
    //[AllowAnonymous]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto, CancellationToken cancellationToken)
    {
        var command = new ChangePasswordCommand(dto);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.Success)
            return BadRequest(new { result.Message });

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    //[AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto, CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(dto);
        var result = await _mediator.Send(command, cancellationToken);

        if (result.Token == null)
            return Unauthorized(new { message = result.Message ?? "Invalid refresh token." });

        return Ok(result);
    }

    [HttpPost("{userId:guid}/revoke-all-refresh-tokens")]
    //[Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> RevokeAllRefreshTokens(Guid userId, CancellationToken cancellationToken)
    {
        var command = new RevokeAllRefreshTokensCommand(userId);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
            return BadRequest(new { message = "Failed to revoke refresh tokens." });

        return Ok(new { message = "All refresh tokens revoked successfully." });
    }

    [HttpPost("logout")]
    //[AllowAnonymous]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto dto, CancellationToken cancellationToken)
    {
        var command = new LogoutCommand(dto.RefreshToken);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result)
            return BadRequest(new { message = "Logout failed or token is already revoked." });

        return Ok(new { message = "Logout successful." });
    }

    [HttpPost("forgot-password")]
    //[AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        var result = await _mediator.Send(new ForgotPasswordCommand(dto));
        // Always return 200 OK (never reveal if user exists)
        return Ok(result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var result = await _mediator.Send(new ResetPasswordCommand(dto));
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
