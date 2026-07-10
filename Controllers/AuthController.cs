using Microsoft.AspNetCore.Mvc;
using FitnessApi.Services;

namespace FitnessApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await authService.AuthenticateAsync(request.Email, request.Password);
        if (string.IsNullOrEmpty(token))
            return Unauthorized(new { message = "Invalid email or password" });

        return Ok(new { Token = token });
    }
}

public record LoginRequest(string Email, string Password);