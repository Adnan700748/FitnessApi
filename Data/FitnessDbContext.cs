using Microsoft.EntityFrameworkCore;
using FitnessApi.Entities;
using FitnessApi.Configurations; // optional, but not needed if assembly scanning

namespace FitnessApi.Data;

public class FitnessDbContext(DbContextOptions<FitnessDbContext> options) : DbContext(options)
{
    public DbSet<Member> Members => Set<Member>();
    public DbSet<FitnessClass> FitnessClasses => Set<FitnessClass>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Assessment> Assessments => Set<Assessment>();
    public DbSet<Badge> Badges => Set<Badge>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Scan the assembly for all IEntityTypeConfiguration classes
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FitnessDbContext).Assembly);
    }
}