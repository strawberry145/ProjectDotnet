using ProjectIntelligence.Models;
using TaskStatus = ProjectIntelligence.Models.TaskStatus;

namespace ProjectIntelligence.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        await db.Database.EnsureCreatedAsync();
        if (db.Projects.Any()) return;

        // Members
        var m1 = new Member { Name = "Alice", Email = "alice@uni.edu", Role = "Lead", JoinDate = DateTime.Today.AddMonths(-6) };
        var m2 = new Member { Name = "Bob", Email = "bob@uni.edu", Role = "Dev", JoinDate = DateTime.Today.AddMonths(-4) };
        var m3 = new Member { Name = "Carol", Email = "carol@uni.edu", Role = "Designer", JoinDate = DateTime.Today.AddMonths(-2) };
        db.Members.AddRange(m1, m2, m3);

        // Projects
        var p1 = new Project
        {
            Name = "Capstone A",
            Description = "Research project",
            StartDate = DateTime.Today.AddDays(-30),
            EndDate = DateTime.Today.AddDays(30),
            Status = ProjectStatus.InProgress
        };
        var p2 = new Project
        {
            Name = "Capstone B",
            Description = "Engineering project",
            StartDate = DateTime.Today.AddDays(-10),
            EndDate = DateTime.Today.AddDays(60),
            Status = ProjectStatus.InProgress
        };
        db.Projects.AddRange(p1, p2);
        await db.SaveChangesAsync();

        // Tasks
        db.Tasks.AddRange(
            new ProjectTask { Title = "Literature review", DueDate = DateTime.Today.AddDays(-2), IsCompleted = false, Status = TaskStatus.InProgress, ProjectId = p1.Id, AssignedMemberId = m1.Id },
            new ProjectTask { Title = "Draft report", DueDate = DateTime.Today.AddDays(10), IsCompleted = false, Status = TaskStatus.Pending, ProjectId = p1.Id, AssignedMemberId = m2.Id },
            new ProjectTask { Title = "Setup repo", DueDate = DateTime.Today.AddDays(-5), IsCompleted = true, Status = TaskStatus.Completed, ProjectId = p1.Id, AssignedMemberId = m2.Id },
            new ProjectTask { Title = "Wireframes", DueDate = DateTime.Today.AddDays(5), IsCompleted = false, Status = TaskStatus.InProgress, ProjectId = p2.Id, AssignedMemberId = m3.Id },
            new ProjectTask { Title = "Prototype", DueDate = DateTime.Today.AddDays(20), IsCompleted = false, Status = TaskStatus.Pending, ProjectId = p2.Id, AssignedMemberId = m1.Id }
        );
        await db.SaveChangesAsync();
    }
}