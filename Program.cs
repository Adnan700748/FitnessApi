using FitnessApi.Middleware;
using FitnessApi.Services;
using FitnessApi.Options;
using FitnessApi.Exceptions;
using FitnessApi.Data;
using FitnessApi.Dtos;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// ===== Services =====
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

// JWT Authentication (replaces dummy Fitness auth)
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
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

// DI services
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IFitnessClassService, FitnessClassService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// BookingWorker – intentionally singleton to demonstrate captive dependency (fixed with IServiceScopeFactory)
builder.Services.AddSingleton<BookingWorker>();

// EF Core
builder.Services.AddDbContext<FitnessDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("FitnessDatabase"))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging());

// Options pattern with startup validation
builder.Services.AddOptions<PaymentOptions>()
    .BindConfiguration("Payments")
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Captive dependency detection
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

var app = builder.Build();

// ===== Middleware Pipeline (order matters) =====
app.UseMiddleware<RequestLoggingMiddleware>();   // Must be first

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ===== Development-only endpoints =====
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// ===== Seed data (Development only) =====
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<FitnessDbContext>();
    await DataSeeder.SeedAsync(context);
}

// ===== Test/Diagnostic endpoints =====
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

// ===== Protected endpoints =====
app.MapGet("/api/assessments/results", () =>
    Results.Ok(new { classCode = "YOG-101", memberId = "M-001", score = "A" }))
    .RequireAuthorization("Admin");

app.MapGet("/api/member/profile", () => Results.Ok(new { message = "Welcome member!" }))
    .RequireAuthorization("Member");

// ===== Controllers =====
app.MapControllers();

app.Run();