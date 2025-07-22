using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    // Only users with "Admin" role can access this endpoint
    [Authorize(Roles = "Admin")]
    [HttpGet("admin-area")]
    public IActionResult AdminArea()
    {
        return Ok("Welcome, Admin! You are in the admin area.");
    }

    // Both "Admin" and "Manager" roles can access this endpoint
    [Authorize(Roles = "Admin,Manager")]
    [HttpGet("manager-area")]
    public IActionResult ManagerArea()
    {
        return Ok("Welcome, Manager or Admin! You are in the manager area.");
    }

    // Any authenticated user (any role)
    [Authorize]
    [HttpGet("user-area")]
    public IActionResult UserArea()
    {
        return Ok("Welcome, authenticated user! You are in the user area.");
    }

    // Allow anonymous access (no authentication required)
    [AllowAnonymous]
    [HttpGet("public-area")]
    public IActionResult PublicArea()
    {
        return Ok("This is a public area. No authentication required.");
    }
}
