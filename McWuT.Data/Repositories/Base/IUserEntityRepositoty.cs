using McWuT.Core;
using System.Linq.Expressions;

namespace McWuT.Data.Repositories.Base;

public interface IUserEntityRepository<T> : IEntityRepository<T> where T : class, IUserEntity, IEntity
{
    Task<PagedResult<T>> SearchAll(
        string userId,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? page = null,
        int? pageSize = null);
}
