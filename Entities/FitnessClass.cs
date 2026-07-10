namespace FitnessApi.Entities;

public class FitnessClass
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string Title { get; set; }
    public int MaxCapacity { get; set; }

    // 🆕 Instructor relationship
    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; } = null!;

    public ICollection<Booking> Bookings { get; set; } = [];
}