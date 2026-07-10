using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using FitnessApi.Models;

namespace FitnessApi.Services;

public class AuthService(IOptions<JwtSettings> jwtSettings) : IAuthService
{
    private readonly JwtSettings _jwt = jwtSettings.Value;

    // Temporary in-memory users – replace with DB in next commit
    private static readonly List<UserCredentials> _users =
    [
        new("admin@fitness.com", "Admin123!", "Admin"),
        new("member@fitness.com", "Member123!", "Member")
    ];

    public async Task<string> AuthenticateAsync(string email, string password)
    {
        var user = _users.FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
            u.Password == password);

        if (user is null)
            return null!;

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.ExpiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class JwtSettings
{
    public required string Key { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public int ExpiryMinutes { get; init; }
}