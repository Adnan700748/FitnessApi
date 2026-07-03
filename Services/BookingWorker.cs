namespace FitnessApi.Services;

public class BookingWorker(IBookingService bookingService)
{
    public void ProcessBatch()
    {
        // Simulate background work – this will cause a captive dependency problem
        // because BookingWorker is Singleton, but IBookingService is Scoped.
        var all = bookingService.GetAllAsync().GetAwaiter().GetResult();
        Console.WriteLine($"Batch processed {all.Count} bookings.");
    }
}