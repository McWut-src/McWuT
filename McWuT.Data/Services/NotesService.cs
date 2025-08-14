using McWuT.Data.Contexts;
using McWuT.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace McWuT.Data.Services;

public class NotesService(ApplicationDbContext db) : INotesService
{
    private readonly ApplicationDbContext _db = db;

    public async Task<List<Note>> GetNotesAsync(string userId, string? search = null)
    {
        var q = _db.Notes.AsNoTracking().Where(n => n.UserId == userId);
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            q = q.Where(n => (n.Title ?? "").Contains(s) || (n.Content ?? "").Contains(s));
        }
        return await q.OrderByDescending(n => n.UpdatedDate ?? n.CreatedDate).ToListAsync();
    }

    public async Task<Note?> GetByIdAsync(string userId, int id)
    {
        return await _db.Notes.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
    }

    public async Task<int> CreateAsync(string userId, string? title, string? content)
    {
        var now = DateTime.UtcNow;
        var note = new Note
        {
            UserId = userId,
            Title = string.IsNullOrWhiteSpace(title) ? null : title,
            Content = string.IsNullOrWhiteSpace(content) ? null : content,
            CreatedDate = now,
            UpdatedDate = now,
            UniqueId = Guid.NewGuid()
        };
        _db.Notes.Add(note);
        await _db.SaveChangesAsync();
        return note.Id;
    }

    public async Task<bool> UpdateAsync(string userId, int id, string? title, string? content)
    {
        var note = await _db.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        if (note is null) return false;
        note.Title = string.IsNullOrWhiteSpace(title) ? null : title;
        note.Content = string.IsNullOrWhiteSpace(content) ? null : content;
        note.UpdatedDate = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SoftDeleteAsync(string userId, int id)
    {
        var note = await _db.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        if (note is null) return false;
        note.DeletedDate = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return true;
    }
}
