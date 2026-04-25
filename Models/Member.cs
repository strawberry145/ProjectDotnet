using System.ComponentModel.DataAnnotations;

namespace ProjectIntelligence.Models;

public class Member
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string Role { get; set; } = string.Empty;

    [Required]
    public DateTime JoinDate { get; set; }

    public List<ProjectTask> AssignedTasks { get; set; } = new();
}