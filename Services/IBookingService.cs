namespace FitnessApi.Services;

public interface IBookingService
{
    Task<BookingRecord> BookAsync(string memberId, string classCode);
    Task<BookingRecord?> GetByIdAsync(string id);
    Task<IReadOnlyList<BookingRecord>> GetAllAsync();
    Task<bool> DeleteAsync(string id);
}

public record BookingRecord(
    string Id,
    string MemberId,
    string ClassCode,
    DateTime BookedAt);