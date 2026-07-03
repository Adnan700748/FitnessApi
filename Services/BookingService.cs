using FitnessApi.Services;

namespace FitnessApi.Services;

public class BookingService(ILogger<BookingService> logger) : IBookingService
{
    private readonly Dictionary<string, BookingRecord> _store = new();

    public Task<BookingRecord> BookAsync(string memberId, string classCode)
    {
        // Duplicate check (same member + same class)
        var existing = _store.Values
            .FirstOrDefault(b => b.MemberId == memberId && b.ClassCode == classCode);

        if (existing is not null)
        {
            logger.LogWarning(
                "Duplicate booking attempt {MemberId} already in {ClassCode} (record {BookingId})",
                memberId, classCode, existing.Id);
            return Task.FromResult(existing);
        }

        var id = Guid.NewGuid().ToString("N")[..8];
        var record = new BookingRecord(id, memberId, classCode, DateTime.UtcNow);
        _store[id] = record;

        logger.LogInformation(
            "Booked {MemberId} in {ClassCode} (booking {BookingId})",
            memberId, classCode, id);

        return Task.FromResult(record);
    }

    public Task<BookingRecord?> GetByIdAsync(string id)
    {
        _store.TryGetValue(id, out var record);
        if (record is null)
        {
            logger.LogWarning("Booking {BookingId} not found", id);
        }
        return Task.FromResult(record);
    }

    public Task<IReadOnlyList<BookingRecord>> GetAllAsync()
    {
        return Task.FromResult<IReadOnlyList<BookingRecord>>(_store.Values.ToList());
    }

    public Task<bool> DeleteAsync(string id)
    {
        var removed = _store.Remove(id);
        if (removed)
            logger.LogInformation("Deleted booking {BookingId}", id);
        else
            logger.LogWarning("Delete failed: booking {BookingId} not found", id);
        return Task.FromResult(removed);
    }
}