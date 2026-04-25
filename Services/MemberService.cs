using Microsoft.EntityFrameworkCore;
using ProjectIntelligence.Data;
using ProjectIntelligence.Models;

namespace ProjectIntelligence.Services;

public class MemberService : IMemberService
{
    private readonly ApplicationDbContext _db;
    public MemberService(ApplicationDbContext db) => _db = db;

    public async Task<List<Member>> GetAllAsync() => await _db.Members.AsNoTracking().ToListAsync();
    public async Task<Member?> GetByIdAsync(int id) => await _db.Members.FindAsync(id);

    public async Task<Member> CreateAsync(Member m)
    {
        _db.Members.Add(m);
        await _db.SaveChangesAsync();
        return m;
    }

    public async Task<bool> UpdateAsync(Member m)
    {
        _db.Members.Update(m);
        try { await _db.SaveChangesAsync(); return true; }
        catch { return false; }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var m = await _db.Members.FindAsync(id);
        if (m == null) return false;
        _db.Members.Remove(m);
        await _db.SaveChangesAsync();
        return true;
    }
}