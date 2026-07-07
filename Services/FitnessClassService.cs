using Microsoft.EntityFrameworkCore;
using FitnessApi.Data;
using FitnessApi.Dtos;

namespace FitnessApi.Services;

public class FitnessClassService(
    FitnessDbContext context,
    ILogger<FitnessClassService> logger) : IFitnessClassService
{
    public async Task<PagedResponse<ClassResponseDto>> GetClassesPagedAsync(
        int page, int pageSize, string? search, CancellationToken ct)
    {
        const int maxPageSize = 50;
        pageSize = pageSize < 1 ? 20 : (pageSize > maxPageSize ? maxPageSize : pageSize);
        page = page < 1 ? 1 : page;

        var query = context.FitnessClasses.AsNoTracking();

        // Search (case-insensitive)
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c =>
                EF.Functions.ILike(c.Title, $"%{search}%") ||
                EF.Functions.ILike(c.Code, $"%{search}%"));
        }

        // Count BEFORE pagination
        var totalCount = await query.CountAsync(ct);

        // Sort, Skip, Take, Project
        var items = await query
            .OrderBy(c => c.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ClassResponseDto(
                c.Id,
                c.Code,
                c.Title,
                c.MaxCapacity,
                c.Bookings.Count))
            .ToListAsync(ct);

        return new PagedResponse<ClassResponseDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<List<TopClassDto>> GetTop5ClassesByBookingsAsync(CancellationToken ct)
    {
        return await context.FitnessClasses
            .AsNoTracking()
            .Select(c => new TopClassDto(
                c.Title,
                c.Bookings.Count))
            .OrderByDescending(x => x.BookingCount)
            .Take(5)
            .ToListAsync(ct);
    }
}