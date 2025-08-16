using System;

namespace McWuT.Core
{
    public interface IUserEntity : IEntity
    {
        string UserId { get; set; }
    }
}
