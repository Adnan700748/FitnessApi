using FitnessApi.Middleware;
using FitnessApi.Auth;
using Microsoft.AspNetCore.Authentication;
using FitnessApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();

builder.Services
    .AddAuthentication("Fitness")
    .AddScheme<AuthenticationSchemeOptions, FitnessAuthHandler>("Fitness", null);
builder.Services.AddAuthorization();
// Register services (scoped = per request)
builder.Services.AddScoped<IBookingService, BookingService>();

// This is the BUG: BookingWorker is Singleton, but it depends on IBookingService (Scoped)
builder.Services.AddSingleton<BookingWorker>();

// Enable captive dependency detection
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});
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