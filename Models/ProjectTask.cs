using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIntelligence.Models;

public enum TaskStatus { Pending, InProgress, Completed }

// Custom validation attribute: due date must be in future (if task not completed)
public class FutureAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
    {
        var task = ctx.ObjectInstance as ProjectTask;
        if (task != null && task.IsCompleted) return ValidationResult.Success;
        if (value is DateTime d && d.Date < DateTime.Today)
            return new ValidationResult("Due date must be in the future for incomplete tasks.");
        return ValidationResult.Success;
    }
}

public class ProjectTask
{
    public int Id { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required, Future]
    public DateTime DueDate { get; set; }

    public bool IsCompleted { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    // Foreign keys
    [Required]
    public int ProjectId { get; set; }
    public Project? Project { get; set; }

    public int? AssignedMemberId { get; set; }
    public Member? AssignedMember { get; set; }

    // Computed delay detection
    [NotMapped]
    public bool IsDelayed => !IsCompleted && DueDate.Date < DateTime.Today;
}