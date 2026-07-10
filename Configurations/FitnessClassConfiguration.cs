using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FitnessApi.Entities;

namespace FitnessApi.Configurations;

public class FitnessClassConfiguration : IEntityTypeConfiguration<FitnessClass>
{
    public void Configure(EntityTypeBuilder<FitnessClass> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Code)
            .IsRequired()
            .HasMaxLength(20);
        builder.Property(f => f.Title)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(f => f.MaxCapacity)
            .IsRequired();
        builder.HasIndex(f => f.Code).IsUnique();
        builder.HasMany(f => f.Bookings)
    .WithOne(b => b.FitnessClass)
    .HasForeignKey(b => b.FitnessClassId)
    .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a class with bookings
    builder.HasOne(f => f.Instructor)
    .WithMany(i => i.Classes)
    .HasForeignKey(f => f.InstructorId)
    .OnDelete(DeleteBehavior.Restrict);
    builder.Property(f => f.ScheduledAt)
    .IsRequired();

builder.Property(f => f.DurationMinutes)
    .IsRequired()
    .HasDefaultValue(60);
    }
}