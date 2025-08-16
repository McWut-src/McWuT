using McWuT.Data.Models;

namespace McWuT.Services.Notes;

public interface INotesService
{
    Task<Note> Create(string userId, string? title, string? content);
    Task<Note> Get(string uniqueId);
    Task<Note> Delete(string uniqueId);
    Task<Note> Update(string uniqueId, string? title, string? content);
    Task<List<Note>> GetNotes(string userId, string? query);
}