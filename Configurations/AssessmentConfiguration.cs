using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FitnessApi.Entities;

namespace FitnessApi.Configurations;

public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
{
    public void Configure(EntityTypeBuilder<Assessment> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(a => a.MaxScore)
            .HasPrecision(6, 2);
        builder.Property(a => a.Weight)
            .HasPrecision(4, 2);
            builder.HasOne(a => a.FitnessClass)
    .WithMany() // or WithMany(f => f.Assessments) if you add the collection
    .HasForeignKey(a => a.FitnessClassId)
    .OnDelete(DeleteBehavior.Restrict);
    }
}