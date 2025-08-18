using McWuT.Core;
using System.ComponentModel.DataAnnotations;

namespace McWuT.Data.Models.CrimeGenerator;

public abstract class BaseEntity : IEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public Guid UniqueId { get; set; } = Guid.NewGuid();

    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedDate { get; set; }

    public DateTime? DeletedDate { get; set; }
}