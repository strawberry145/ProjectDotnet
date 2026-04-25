using Microsoft.EntityFrameworkCore;
using ProjectIntelligence.Models;

namespace ProjectIntelligence.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectTask> Tasks => Set<ProjectTask>();
    public DbSet<Member> Members => Set<Member>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Configure relationships
        builder.Entity<ProjectTask>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProjectTask>()
            .HasOne(t => t.AssignedMember)
            .WithMany(m => m.AssignedTasks)
            .HasForeignKey(t => t.AssignedMemberId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}