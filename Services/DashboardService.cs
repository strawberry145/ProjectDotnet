using Microsoft.EntityFrameworkCore;
using ProjectIntelligence.Data;
using ProjectIntelligence.Dtos;

namespace ProjectIntelligence.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _db;
    public DashboardService(ApplicationDbContext db) => _db = db;

    public async Task<List<ProjectHealthDto>> GetProjectHealthAsync()
    {
        var projects = await _db.Projects.Include(p => p.Tasks).AsNoTracking().ToListAsync();
        var today = DateTime.Today;
        return projects.Select(p =>
        {
            var total = p.Tasks.Count;
            var completed = p.Tasks.Count(t => t.IsCompleted);
            var delayed = p.Tasks.Count(t => !t.IsCompleted && t.DueDate.Date < today);
            var pct = total == 0 ? 0 : (double)completed / total * 100;
            var score = Math.Clamp(pct * 0.6 - delayed * 0.1, 0, 100);
            return new ProjectHealthDto
            {
                ProjectId = p.Id,
                ProjectName = p.Name,
                TotalTasks = total,
                CompletedTasks = completed,
                DelayedTasks = delayed,
                PercentageCompleted = Math.Round(pct, 2),
                HealthScore = Math.Round(score, 2)
            };
        }).ToList();
    }

    public async Task<List<MemberPerformanceDto>> GetMemberPerformanceAsync()
    {
        var members = await _db.Members.Include(m => m.AssignedTasks).AsNoTracking().ToListAsync();
        var today = DateTime.Today;
        return members.Select(m =>
        {
            var total = m.AssignedTasks.Count;
            var completed = m.AssignedTasks.Count(t => t.IsCompleted);
            var delayed = m.AssignedTasks.Count(t => !t.IsCompleted && t.DueDate.Date < today);
            var workload = m.AssignedTasks.Count(t => !t.IsCompleted);
            var rate = total == 0 ? 0 : (double)completed / total * 100;
            return new MemberPerformanceDto
            {
                MemberId = m.Id,
                MemberName = m.Name,
                TotalAssigned = total,
                Completed = completed,
                CompletionRate = Math.Round(rate, 2),
                DelayedTasks = delayed,
                CurrentWorkload = workload
            };
        }).ToList();
    }

    public async Task<List<ProjectChartDto>> GetProjectChartDataAsync()
    {
        var projects = await _db.Projects.Include(p => p.Tasks).AsNoTracking().ToListAsync();
        return projects.Select(p => new ProjectChartDto
        {
            ProjectName = p.Name,
            Completed = p.Tasks.Count(t => t.IsCompleted),
            Pending = p.Tasks.Count(t => !t.IsCompleted)
        }).ToList();
    }
}