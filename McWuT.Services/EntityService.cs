using Microsoft.EntityFrameworkCore;
using McWuT.Core;
using McWuT.Data.Contexts;

namespace McWuT.Services;

public class EntityService<T> where T : class, IEntity
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public EntityService(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // Retrieve by GUID, excluding soft-deleted
    public async Task<T?> GetByGuidAsync(Guid uniqueId)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.UniqueId == uniqueId && e.DeletedDate == null);
    }

    // Query all non-deleted entities
    public IQueryable<T> GetAll()
    {
        return _dbSet.Where(e => e.DeletedDate == null);
    }

    // Add new entity (assign UniqueId if not set)
    public async Task AddAsync(T entity)
    {
        if (entity.UniqueId == Guid.Empty)
            entity.UniqueId = Guid.NewGuid();
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
    }

    // Soft delete by GUID
    public async Task<bool> SoftDeleteAsync(Guid uniqueId)
    {
        var entity = await GetByGuidAsync(uniqueId);
        if (entity == null) return false;
        entity.DeletedDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    // Update entity
    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }
}