using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FitnessApi.Entities;

namespace FitnessApi.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.MembershipNumber)
            .IsRequired()
            .HasMaxLength(20);
        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(m => m.MembershipLevel)
            .HasPrecision(3, 2); // e.g. 3.80
        builder.HasIndex(m => m.MembershipNumber).IsUnique();
        builder.HasMany(m => m.Bookings)
    .WithOne(b => b.Member)
    .HasForeignKey(b => b.MemberId)
    .OnDelete(DeleteBehavior.Restrict);
    }
}