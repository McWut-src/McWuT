using McWuT.Core;
using McWuT.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace McWuT.Data.Repositories.Base;

public class EntityRepository<T> : IEntityRepository<T> where T : class, IEntity
{
    public EntityRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<T> Create(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        if (entity.UniqueId == Guid.Empty)
        {
            entity.UniqueId = Guid.NewGuid();
        }

        entity.CreatedDate = DateTime.UtcNow;
        entity.UpdatedDate = null;
        entity.DeletedDate = null;

        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<IEnumerable<T>> Create(IEnumerable<T> entities)
    {
        if (entities == null) throw new ArgumentNullException(nameof(entities));

        foreach (var entity in entities)
        {
            if (entity.UniqueId == Guid.Empty)
            {
                entity.UniqueId = Guid.NewGuid();
            }

            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedDate = null;
            entity.DeletedDate = null;
        }

        _context.Set<T>().AddRange(entities);
        await _context.SaveChangesAsync();

        return entities;
    }

    public async Task<T> CreateOrUpdate(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var existing = await _context.Set<T>().FirstOrDefaultAsync(e => e.UniqueId == entity.UniqueId);

        if (existing != null)
        {
            var originalId = existing.Id;
            var originalUniqueId = existing.UniqueId;
            var originalCreatedDate = existing.CreatedDate;

            _context.Entry(existing).CurrentValues.SetValues(entity);

            existing.Id = originalId;
            existing.UniqueId = originalUniqueId;
            existing.CreatedDate = originalCreatedDate;
            existing.DeletedDate = null; // Undelete if previously soft-deleted
            existing.UpdatedDate = DateTime.UtcNow;
        }
        else
        {
            if (entity.UniqueId == Guid.Empty)
            {
                entity.UniqueId = Guid.NewGuid();
            }

            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedDate = null;
            entity.DeletedDate = null;

            _context.Set<T>().Add(entity);
            existing = entity;
        }

        await _context.SaveChangesAsync();

        return existing;
    }

    public async Task<IEnumerable<T>> CreateOrUpdate(IEnumerable<T> entities)
    {
        if (entities == null) throw new ArgumentNullException(nameof(entities));

        var uniqueIds = entities.Select(e => e.UniqueId).ToList();
        var existings = await _context.Set<T>().Where(e => uniqueIds.Contains(e.UniqueId)).ToDictionaryAsync(e => e.UniqueId);

        var results = new List<T>();

        foreach (var entity in entities)
        {
            if (existings.TryGetValue(entity.UniqueId, out var existing))
            {
                var originalId = existing.Id;
                var originalUniqueId = existing.UniqueId;
                var originalCreatedDate = existing.CreatedDate;

                _context.Entry(existing).CurrentValues.SetValues(entity);

                existing.Id = originalId;
                existing.UniqueId = originalUniqueId;
                existing.CreatedDate = originalCreatedDate;
                existing.DeletedDate = null; // Undelete if previously soft-deleted
                existing.UpdatedDate = DateTime.UtcNow;

                results.Add(existing);
            }
            else
            {
                if (entity.UniqueId == Guid.Empty)
                {
                    entity.UniqueId = Guid.NewGuid();
                }

                entity.CreatedDate = DateTime.UtcNow;
                entity.UpdatedDate = null;
                entity.DeletedDate = null;

                _context.Set<T>().Add(entity);
                results.Add(entity);
            }
        }

        await _context.SaveChangesAsync();

        return results;
    }

    public async Task<T> Delete(Guid uniqueId)
    {
        var entity = await _context.Set<T>().FirstOrDefaultAsync(e => e.UniqueId == uniqueId && !e.DeletedDate.HasValue);

        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with UniqueId {uniqueId} not found or already deleted.");
        }

        entity.DeletedDate = DateTime.UtcNow;
        entity.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<IEnumerable<T>> Delete(IEnumerable<Guid> uniqueIds)
    {
        if (uniqueIds == null) throw new ArgumentNullException(nameof(uniqueIds));

        var uniqueIdsList = uniqueIds.ToList();
        var entitiesToDelete = await _context.Set<T>().Where(e => uniqueIdsList.Contains(e.UniqueId) && !e.DeletedDate.HasValue).ToListAsync();

        foreach (var entity in entitiesToDelete)
        {
            entity.DeletedDate = DateTime.UtcNow;
            entity.UpdatedDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return entitiesToDelete;
    }

    public async Task<T?> Get(Guid uniqueId)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(e => e.UniqueId == uniqueId && !e.DeletedDate.HasValue);
    }

    public async Task<IEnumerable<T>> Get(IEnumerable<Guid> uniqueIds)
    {
        if (uniqueIds == null) throw new ArgumentNullException(nameof(uniqueIds));

        return await _context.Set<T>().Where(e => uniqueIds.Contains(e.UniqueId) && !e.DeletedDate.HasValue).ToListAsync();
    }

    public async Task<PagedResult<T>> GetAll(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? page = null,
        int? pageSize = null)
    {
        var query = _context.Set<T>().Where(e => !e.DeletedDate.HasValue);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        var totalCount = await query.CountAsync();

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (page.HasValue && pageSize.HasValue)
        {
            var skip = (page.Value - 1) * pageSize.Value;
            query = query.Skip(skip).Take(pageSize.Value);
        }

        var items = await query.ToListAsync();

        return new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page ?? 1,
            PageSize = pageSize ?? totalCount
        };
    }

    public async Task<T> Update(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var existing = await _context.Set<T>().FirstOrDefaultAsync(e => e.UniqueId == entity.UniqueId && !e.DeletedDate.HasValue);

        if (existing == null)
        {
            throw new KeyNotFoundException($"Entity with UniqueId {entity.UniqueId} not found or deleted.");
        }

        var originalId = existing.Id;
        var originalUniqueId = existing.UniqueId;
        var originalCreatedDate = existing.CreatedDate;
        var originalDeletedDate = existing.DeletedDate;

        _context.Entry(existing).CurrentValues.SetValues(entity);

        existing.Id = originalId;
        existing.UniqueId = originalUniqueId;
        existing.CreatedDate = originalCreatedDate;
        existing.DeletedDate = originalDeletedDate;
        existing.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return existing;
    }

    public async Task<IEnumerable<T>> Update(IEnumerable<T> entities)
    {
        if (entities == null) throw new ArgumentNullException(nameof(entities));

        var uniqueIds = entities.Select(e => e.UniqueId).ToList();
        var existings = await _context.Set<T>().Where(e => uniqueIds.Contains(e.UniqueId) && !e.DeletedDate.HasValue).ToListAsync();

        if (existings.Count != entities.Count())
        {
            throw new KeyNotFoundException("One or more entities not found or deleted.");
        }

        var existingDict = existings.ToDictionary(e => e.UniqueId);
        var results = new List<T>();

        foreach (var entity in entities)
        {
            var existing = existingDict[entity.UniqueId];

            var originalId = existing.Id;
            var originalUniqueId = existing.UniqueId;
            var originalCreatedDate = existing.CreatedDate;
            var originalDeletedDate = existing.DeletedDate;

            _context.Entry(existing).CurrentValues.SetValues(entity);

            existing.Id = originalId;
            existing.UniqueId = originalUniqueId;
            existing.CreatedDate = originalCreatedDate;
            existing.DeletedDate = originalDeletedDate;
            existing.UpdatedDate = DateTime.UtcNow;

            results.Add(existing);
        }

        await _context.SaveChangesAsync();

        return results;
    }

    public async Task<T?> Get(string uniqueId)
    {
        if (!Guid.TryParse(uniqueId, out var guid))
            throw new ArgumentException("Invalid GUID format.", nameof(uniqueId));
        return await Get(guid);
    }

    public async Task<IEnumerable<T>> Get(IEnumerable<string> uniqueIds)
    {
        var guidList = uniqueIds.Select(id => Guid.TryParse(id, out var guid) ? guid : throw new ArgumentException($"Invalid GUID format: {id}", nameof(uniqueIds))).ToList();
        return await Get(guidList);
    }

    public async Task<T> Delete(string uniqueId)
    {
        if (!Guid.TryParse(uniqueId, out var guid))
            throw new ArgumentException("Invalid GUID format.", nameof(uniqueId));
        return await Delete(guid);
    }

    public async Task<IEnumerable<T>> Delete(IEnumerable<string> uniqueIds)
    {
        var guidList = uniqueIds.Select(id => Guid.TryParse(id, out var guid) ? guid : throw new ArgumentException($"Invalid GUID format: {id}", nameof(uniqueIds))).ToList();
        return await Delete(guidList);
    }

    private readonly ApplicationDbContext _context;
}
