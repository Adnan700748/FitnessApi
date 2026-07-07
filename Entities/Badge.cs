namespace FitnessApi.Entities;

public class Badge
{
    public int Id { get; set; }
    public required string SerialNumber { get; set; }
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    public int MemberId { get; set; }
    public int FitnessClassId { get; set; }

    public Member Member { get; set; } = null!;
    public FitnessClass FitnessClass { get; set; } = null!;
}