using Microsoft.EntityFrameworkCore;
using ProjectIntelligence.Data;
using ProjectIntelligence.Models;

namespace ProjectIntelligence.Services;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _db;
    public ProjectService(ApplicationDbContext db) => _db = db;

    public async Task<List<Project>> GetAllAsync(string? name, ProjectStatus? status)
    {
        var query = _db.Projects.Include(p => p.Tasks).AsQueryable();
        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name));
        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);
        var projects = await query.AsNoTracking().ToListAsync();
        foreach (var p in projects)
            p.IsDelayed = p.Tasks.Any(t => !t.IsCompleted && t.DueDate.Date < DateTime.Today);
        return projects;
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        var p = await _db.Projects.Include(x => x.Tasks).FirstOrDefaultAsync(x => x.Id == id);
        if (p != null)
            p.IsDelayed = p.Tasks.Any(t => !t.IsCompleted && t.DueDate.Date < DateTime.Today);
        return p;
    }

    public async Task<Project> CreateAsync(Project p)
    {
        _db.Projects.Add(p);
        await _db.SaveChangesAsync();
        return p;
    }

    public async Task<bool> UpdateAsync(Project p)
    {
        _db.Projects.Update(p);
        try { await _db.SaveChangesAsync(); return true; }
        catch { return false; }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var p = await _db.Projects.FindAsync(id);
        if (p == null) return false;
        _db.Projects.Remove(p);
        await _db.SaveChangesAsync();
        return true;
    }
}