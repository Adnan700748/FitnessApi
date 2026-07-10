namespace FitnessApi.Entities;

public class Instructor
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public string? Bio { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation: one instructor teaches many classes
    public ICollection<FitnessClass> Classes { get; set; } = [];
}