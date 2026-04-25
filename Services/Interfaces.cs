using ProjectIntelligence.Dtos;
using ProjectIntelligence.Models;
using TaskStatus = ProjectIntelligence.Models.TaskStatus;

namespace ProjectIntelligence.Services;

public interface IProjectService
{
    Task<List<Project>> GetAllAsync(string? name, ProjectStatus? status);
    Task<Project?> GetByIdAsync(int id);
    Task<Project> CreateAsync(Project p);
    Task<bool> UpdateAsync(Project p);
    Task<bool> DeleteAsync(int id);
}

public interface ITaskService
{
    Task<List<ProjectTask>> GetAllAsync(int? projectId, TaskStatus? status, int? memberId, DateTime? from, DateTime? to);
    Task<ProjectTask?> GetByIdAsync(int id);
    Task<ProjectTask> CreateAsync(ProjectTask t);
    Task<bool> UpdateAsync(ProjectTask t);
    Task<bool> DeleteAsync(int id);
}

public interface IMemberService
{
    Task<List<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(int id);
    Task<Member> CreateAsync(Member m);
    Task<bool> UpdateAsync(Member m);
    Task<bool> DeleteAsync(int id);
}

public interface IDashboardService
{
    Task<List<ProjectHealthDto>> GetProjectHealthAsync();
    Task<List<MemberPerformanceDto>> GetMemberPerformanceAsync();
    Task<List<ProjectChartDto>> GetProjectChartDataAsync();
}