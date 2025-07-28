namespace UserManagementDemo.Application.Features.RefreshTokens.Dtos;

public class RefreshTokenResponseDto
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public int ExpiresIn { get; set; }
    public string? Message { get; set; }
}
