using Microsoft.EntityFrameworkCore;
using FitnessApi.Entities;

namespace FitnessApi.Data;

public static class DataSeeder
{
    private static readonly (string Code, string Title, int MaxCapacity)[] Classes =
    {
        ("YOG-101", "Beginner Yoga", 30),
        ("YOG-102", "Vinyasa Flow", 30),
        ("YOG-103", "Yoga for Athletes", 25),
        ("CROSS-201", "CrossFit Fundamentals", 28),
        ("CROSS-202", "Advanced CrossFit", 28),
        ("CROSS-203", "CrossFit Endurance", 28),
        ("HIIT-301", "High Intensity Interval Training", 24),
        ("HIIT-302", "Tabata Blast", 26),
        ("PIL-101", "Pilates Mat", 24),
        ("PIL-102", "Reformer Pilates", 22),
        ("STR-201", "Strength Training I", 22),
        ("STR-202", "Strength Training II", 20),
        ("BOX-101", "Boxing Basics", 30),
        ("BOX-102", "Sparring Techniques", 25),
        ("DANCE-101", "Zumba", 40),
        ("DANCE-102", "Hip Hop Dance", 35),
        ("SWIM-101", "Beginner Swimming", 20),
        ("SWIM-102", "Advanced Swimming", 20),
        ("RUN-101", "Running Technique", 30),
        ("RUN-102", "Marathon Prep", 25),
        ("CAL-101", "Calisthenics", 30),
        ("CAL-102", "Gymnastics Rings", 25),
        ("MOB-101", "Mobility & Flexibility", 30),
        ("MOB-102", "Active Recovery", 30),
        ("NUTR-101", "Sports Nutrition", 40)
    };

    public static async Task SeedAsync(FitnessDbContext context, CancellationToken ct = default)
    {
        await context.Database.MigrateAsync(ct);

        if (await context.FitnessClasses.AnyAsync(ct))
            return;

        foreach (var (code, title, maxCapacity) in Classes)
        {
            context.FitnessClasses.Add(new FitnessClass
            {
                Code = code,
                Title = title,
                MaxCapacity = maxCapacity
            });
        }

        await context.SaveChangesAsync(ct);
    }
}