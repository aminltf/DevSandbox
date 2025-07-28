using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserManagementDemo.Application.Common.Interfaces.Services;
using UserManagementDemo.Application.Common.Models;
using UserManagementDemo.Domain.Entities;

namespace UserManagementDemo.Infrastructure.Identity.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public int TokenLifetimeSeconds => _jwtSettings.TokenLifetimeSeconds;

    public string GenerateJwtToken(ApplicationUser user)
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };
        if (!string.IsNullOrEmpty(user.Email))
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(_jwtSettings.TokenLifetimeSeconds),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    public RefreshToken GenerateRefreshToken()
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeDays)
        };
    }
}
