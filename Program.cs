using FitnessApi.Middleware;
using FitnessApi.Auth;
using Microsoft.AspNetCore.Authentication;
using FitnessApi.Services;
using FitnessApi.Options;
using FitnessApi.Exceptions;
using Scalar.AspNetCore;
using FitnessApi.Data;
using Microsoft.EntityFrameworkCore;
using FitnessApi.Dtos;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FitnessApi.Services;


var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();

// ... after builder.Services.AddControllers() ...

// 🆕 JWT Settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// 🆕 Authentication (JWT)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// 🆕 Register services
builder.Services.AddScoped<IAuthService, AuthService>();

// ... remove the old "Fitness" auth and TrainingAuthHandler references

builder.Services.AddProblemDetails();

builder.Services.AddDbContext<FitnessDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("FitnessDatabase"))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging());

builder.Services.AddOpenApi();
builder.Services
    .AddAuthentication("Fitness")
    .AddScheme<AuthenticationSchemeOptions, FitnessAuthHandler>("Fitness", null);
builder.Services.AddAuthorization();
// Register services (scoped = per request)
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IFitnessClassService, FitnessClassService>();
// This is the BUG: BookingWorker is Singleton, but it depends on IBookingService (Scoped)
builder.Services.AddSingleton<BookingWorker>();

// Enable captive dependency detection
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});
// Options pattern with startup validation
builder.Services.AddOptions<PaymentOptions>()
    .BindConfiguration("Payments")
    .ValidateDataAnnotations()
    .ValidateOnStart();
var app = builder.Build();

// Middleware pipeline (Order: Logging → Auth → Routing → Endpoints)
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();
app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Interactive API explorer at /scalar/v1
}

app.MapGet("/api/test/classes", async (
    [AsParameters] PagedRequest request,
    IFitnessClassService service,
    CancellationToken ct) =>
{
    var result = await service.GetClassesPagedAsync(
        request.Page, request.PageSize, request.Search, ct);
    return Results.Ok(result);
});

app.MapGet("/api/test/top5", async (
    IFitnessClassService service,
    CancellationToken ct) =>
{
    var result = await service.GetTop5ClassesByBookingsAsync(ct);
    return Results.Ok(result);
});

app.MapGet("/api/error", () =>
{
    throw new FitnessDatabaseException("Simulated database failure for ProblemDetails testing");
});

// Protected route (same assessment endpoint as lab)
app.MapGet("/api/assessments/results", () =>
    Results.Ok(new { classCode = "YOG-101", memberId = "M-001", score = "A" }))
    .RequireAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<FitnessDbContext>();
    await DataSeeder.SeedAsync(context);
}

app.Run();