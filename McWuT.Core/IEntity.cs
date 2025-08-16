namespace McWuT.Core
{
    public interface IEntity
    {
        DateTime CreatedDate { get; set; }

        DateTime? DeletedDate { get; set; }

        int Id { get; set; }

        Guid UniqueId { get; set; }

        DateTime? UpdatedDate { get; set; }
    }
}