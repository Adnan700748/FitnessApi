namespace FitnessApi.Dtos;

public record ClassResponseDto(
    int Id,
    string Code,
    string Title,
    int MaxCapacity,
    int BookingCount);