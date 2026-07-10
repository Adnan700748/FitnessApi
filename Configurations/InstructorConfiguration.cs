using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FitnessApi.Entities;

namespace FitnessApi.Configurations;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(i => i.LastName).IsRequired().HasMaxLength(50);
        builder.Property(i => i.Email).IsRequired().HasMaxLength(100);
        builder.HasIndex(i => i.Email).IsUnique();
        builder.Property(i => i.Phone).HasMaxLength(20);
        builder.Property(i => i.Bio).HasMaxLength(500);
    }
}