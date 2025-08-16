using McWuT.Core;
using McWuT.Data.Repositories;
using McWuT.Data.Repositories.Base;
using System;
using System.Threading.Tasks;

namespace McWuT.Services;

public class BaseEntityService<T> where T : class, IEntity
{
    protected readonly IEntityRepository<T> Repository;

    public BaseEntityService(IEntityRepository<T> repository)
    {
        Repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
}
