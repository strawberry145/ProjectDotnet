using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIntelligence.Models;

public enum ProjectStatus { NotStarted, InProgress, Completed }

public class Project
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required] public DateTime StartDate { get; set; }
    [Required] public DateTime EndDate { get; set; }

    public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;

    // Navigation property: one project has many tasks
    public List<ProjectTask> Tasks { get; set; } = new();

    // Computed property (not stored in database)
    [NotMapped]
    public bool IsDelayed { get; set; }
}