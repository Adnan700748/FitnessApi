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
    }
}