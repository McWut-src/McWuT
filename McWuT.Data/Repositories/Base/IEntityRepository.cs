using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using McWuT.Core;
using System.Linq.Expressions;

namespace McWuT.Data.Repositories.Base;

public interface IEntityRepository<T> where T : class, IEntity
{
    Task<T> Create(T entity);

    Task<IEnumerable<T>> Create(IEnumerable<T> entities);

    Task<T> CreateOrUpdate(T entity);

    Task<IEnumerable<T>> CreateOrUpdate(IEnumerable<T> entities);

    Task<T> Delete(Guid uniqueId);

    Task<T> Delete(string uniqueId);

    Task<IEnumerable<T>> Delete(IEnumerable<Guid> uniqueIds);

    Task<IEnumerable<T>> Delete(IEnumerable<string> uniqueIds);

    Task<T?> Get(Guid uniqueId);

    Task<T?> Get(string uniqueId);

    Task<IEnumerable<T>> Get(IEnumerable<Guid> uniqueIds);

    Task<IEnumerable<T>> Get(IEnumerable<string> uniqueIds);

    Task<PagedResult<T>> GetAll(
            Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? page = null,
        int? pageSize = null);

    Task<T> Update(T entity);

    Task<IEnumerable<T>> Update(IEnumerable<T> entities);
}