namespace FitnessApi.Entities;

public class FitnessClass
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string Title { get; set; }
    public int MaxCapacity { get; set; }

    // 🆕 Schedule
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; } = 60;


    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; } = null!;

    public ICollection<Booking> Bookings { get; set; } = [];
}