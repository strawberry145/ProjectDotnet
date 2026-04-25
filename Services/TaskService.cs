using Microsoft.EntityFrameworkCore;
using ProjectIntelligence.Data;
using ProjectIntelligence.Models;
using TaskStatus = ProjectIntelligence.Models.TaskStatus;

namespace ProjectIntelligence.Services;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _db;
    public TaskService(ApplicationDbContext db) => _db = db;

    public async Task<List<ProjectTask>> GetAllAsync(int? projectId, TaskStatus? status, int? memberId, DateTime? from, DateTime? to)
    {
        var query = _db.Tasks.Include(t => t.Project).Include(t => t.AssignedMember).AsQueryable();
        if (projectId.HasValue) query = query.Where(t => t.ProjectId == projectId.Value);
        if (status.HasValue) query = query.Where(t => t.Status == status.Value);
        if (memberId.HasValue) query = query.Where(t => t.AssignedMemberId == memberId.Value);
        if (from.HasValue) query = query.Where(t => t.DueDate >= from.Value);
        if (to.HasValue) query = query.Where(t => t.DueDate <= to.Value);
        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<ProjectTask?> GetByIdAsync(int id) =>
        await _db.Tasks.Include(t => t.Project).Include(t => t.AssignedMember).FirstOrDefaultAsync(t => t.Id == id);

    public async Task<ProjectTask> CreateAsync(ProjectTask t)
    {
        _db.Tasks.Add(t);
        await _db.SaveChangesAsync();
        return t;
    }

    public async Task<bool> UpdateAsync(ProjectTask t)
    {
        _db.Tasks.Update(t);
        try { await _db.SaveChangesAsync(); return true; }
        catch { return false; }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var t = await _db.Tasks.FindAsync(id);
        if (t == null) return false;
        _db.Tasks.Remove(t);
        await _db.SaveChangesAsync();
        return true;
    }
}