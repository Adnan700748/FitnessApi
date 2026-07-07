namespace FitnessApi.Entities;

public class Member
{
    public int Id { get; set; }
    public required string MembershipNumber { get; set; }
    public required string Name { get; set; }
    public decimal MembershipLevel { get; set; } // e.g., 3.8 = Gold
    public bool IsActive { get; set; } = true;

    public ICollection<Booking> Bookings { get; set; } = [];
}