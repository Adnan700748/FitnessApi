namespace FitnessApi.Entities;

public class Assessment
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public decimal MaxScore { get; set; }
    public decimal Weight { get; set; }

    public int FitnessClassId { get; set; }
    public FitnessClass FitnessClass { get; set; } = null!;
}