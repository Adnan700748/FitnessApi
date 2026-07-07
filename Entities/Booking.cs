namespace FitnessApi.Entities;

public class Booking
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int FitnessClassId { get; set; }
    public decimal? Grade { get; set; }
    public DateTime BookedAt { get; set; } = DateTime.UtcNow;

    public Member Member { get; set; } = null!;
    public FitnessClass FitnessClass { get; set; } = null!;
}