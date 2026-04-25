namespace ProjectIntelligence.Dtos;

public class ProjectHealthDto
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = "";
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int DelayedTasks { get; set; }
    public double PercentageCompleted { get; set; }
    public double HealthScore { get; set; }
}

public class MemberPerformanceDto
{
    public int MemberId { get; set; }
    public string MemberName { get; set; } = "";
    public int TotalAssigned { get; set; }
    public int Completed { get; set; }
    public double CompletionRate { get; set; }
    public int DelayedTasks { get; set; }
    public int CurrentWorkload { get; set; }
}

public class ProjectChartDto
{
    public string ProjectName { get; set; } = "";
    public int Completed { get; set; }
    public int Pending { get; set; }
}