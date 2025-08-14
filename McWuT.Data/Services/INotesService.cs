using McWuT.Data.Models;

namespace McWuT.Data.Services;

public interface INotesService
{
    Task<List<Note>> GetNotesAsync(string userId, string? search = null);
    Task<Note?> GetByIdAsync(string userId, int id);
    Task<int> CreateAsync(string userId, string? title, string? content);
    Task<bool> UpdateAsync(string userId, int id, string? title, string? content);
    Task<bool> SoftDeleteAsync(string userId, int id);
}
