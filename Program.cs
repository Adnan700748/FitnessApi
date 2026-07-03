using FitnessApi.Middleware;
using FitnessApi.Auth;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();

builder.Services
    .AddAuthentication("Fitness")
    .AddScheme<AuthenticationSchemeOptions, FitnessAuthHandler>("Fitness", null);
builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware pipeline (Order: Logging → Auth → Routing → Endpoints)
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

// Protected route (same assessment endpoint as lab)
app.MapGet("/api/assessments/results", () =>
    Results.Ok(new { classCode = "YOG-101", memberId = "M-001", score = "A" }))
    .RequireAuthorization();

app.MapControllers();

app.Run();