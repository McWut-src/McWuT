using McWuT.Core;
using McWuT.Data.Contexts;
using McWuT.Common.Extensions;
using System.Linq.Expressions;

namespace McWuT.Data.Repositories.Base;

public class UserEntityRepository<T> : EntityRepository<T>, IUserEntityRepository<T> where T : class, IUserEntity, IEntity
{
    public UserEntityRepository(ApplicationDbContext context) : base(context) { }

    public async Task<PagedResult<T>> SearchAll(
        string userId,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        int? page = null,
        int? pageSize = null)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));

        Expression<Func<T, bool>> userFilter = e => e.UserId == userId;

        var combinedFilter = filter == null
            ? userFilter
            : userFilter.And(filter);

        return await GetAll(combinedFilter, orderBy, page, pageSize);
    }
}