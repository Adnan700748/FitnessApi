using Microsoft.EntityFrameworkCore;
using FitnessApi.Entities;

namespace FitnessApi.Data;

public class FitnessDbContext(DbContextOptions<FitnessDbContext> options) : DbContext(options)
{
    public DbSet<Member> Members => Set<Member>();
    public DbSet<FitnessClass> FitnessClasses => Set<FitnessClass>();
    public DbSet<Booking> Bookings => Set<Booking>();
}