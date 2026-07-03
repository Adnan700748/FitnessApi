using FitnessApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

// Order matters: RequestLoggingMiddleware must be FIRST
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();

app.Run();