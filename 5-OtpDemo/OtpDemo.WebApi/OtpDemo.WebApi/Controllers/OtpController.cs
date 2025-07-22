using OtpDemo.Core.Dtos;
using OtpDemo.Core.Entities;
using OtpDemo.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace OtpDemo.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OtpController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOtpService _otpService;
    private readonly IJwtTokenService _jwtTokenService;

    public OtpController(
        UserManager<ApplicationUser> userManager,
        IOtpService otpService,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _otpService = otpService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("request")]
    public async Task<IActionResult> RequestOtp([FromBody] OtpRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            return BadRequest("Phone number is required.");

        await _otpService.GenerateAndSendOtpAsync(dto.PhoneNumber);
        return Ok("OTP sent.");
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyDto dto)
    {
        var valid = await _otpService.ValidateOtpAsync(dto.PhoneNumber, dto.Code);
        if (!valid) return BadRequest("Invalid or expired OTP.");

        // اگر کاربر وجود ندارد، ایجاد کن
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);
        if (user == null)
        {
            user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = dto.PhoneNumber,
                PhoneNumber = dto.PhoneNumber,
                PhoneNumberConfirmed = true,
                LastLoginAt = DateTime.UtcNow
            };
            await _userManager.CreateAsync(user);
        }
        else
        {
            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        var token = _jwtTokenService.GenerateToken(user);
        return Ok(new { token });
    }
}
