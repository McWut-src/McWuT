using McWuT.Data.Models;
using McWuT.Data.Repositories.Base;
using McWuT.Services.Notes;
using System.Linq.Expressions;

namespace McWuT.Services;

public class NotesService(IUserEntityRepository<Note> repository)
           : BaseUserEntityService<Note>(repository), INotesService
{
    public async Task<Note> Create(string userId, string? title, string? content)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));

        var note = new Note
        {
            UniqueId = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Content = content
        };

        return await Repository.Create(note);
    }

    public async Task<Note> Delete(string uniqueId)
    {
        return await Repository.Delete(uniqueId);
    }

    public async Task<Note> Get(string uniqueId)
    {
        var note = await Repository.Get(uniqueId);
        return note ?? throw new KeyNotFoundException($"Note with UniqueId {uniqueId} not found.");
    }

    public async Task<List<Note>> GetNotes(string userId, string? query)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));

        Expression<Func<Note, bool>>? filter = null;
        if (!string.IsNullOrWhiteSpace(query))
        {
            filter = n => (n.Title != null && n.Title.Contains(query)) || (n.Content != null && n.Content.Contains(query));
        }

        var result = await Repository.SearchAll(
            userId,
            filter: filter,
            orderBy: q => q.OrderByDescending(n => n.CreatedDate)
        );

        return result.Items.ToList();
    }

    public async Task<Note> Update(string uniqueId, string? title, string? content)
    {
        var note = await Get(uniqueId); // Ensures it exists and fetches it

        note.Title = title;
        note.Content = content;

        return await Repository.Update(note);
    }
}