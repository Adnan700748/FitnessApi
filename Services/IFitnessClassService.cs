using FitnessApi.Dtos;

namespace FitnessApi.Services;

public interface IFitnessClassService
{
    Task<PagedResponse<ClassResponseDto>> GetClassesPagedAsync(
        int page, int pageSize, string? search, CancellationToken ct);
    Task<List<TopClassDto>> GetTop5ClassesByBookingsAsync(CancellationToken ct);
}

public record ClassResponseDto(
    int Id,
    string Code,
    string Title,
    int MaxCapacity,
    int BookingCount);

public record TopClassDto(
    string Title,
    int BookingCount);