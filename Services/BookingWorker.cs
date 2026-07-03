namespace FitnessApi.Services;

public class BookingWorker(IServiceScopeFactory scopeFactory)
{
    public void ProcessBatch()
    {
        // Create a short-lived scope for each batch
        using var scope = scopeFactory.CreateScope();
        var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();

        var all = bookingService.GetAllAsync().GetAwaiter().GetResult();
        Console.WriteLine($"Batch processed {all.Count} bookings.");
    }
}