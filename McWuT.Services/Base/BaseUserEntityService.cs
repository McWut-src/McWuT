using McWuT.Core;
using McWuT.Data.Repositories.Base;

namespace McWuT.Services;

public class BaseUserEntityService<T> where T : class, IUserEntity
{
    protected readonly IUserEntityRepository<T> Repository;

    public BaseUserEntityService(IUserEntityRepository<T> repository)
    {
        Repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
}