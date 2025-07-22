﻿namespace AuthDemo.Application.DTOs;

public class RegisterDto
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
}
