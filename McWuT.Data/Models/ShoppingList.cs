using McWuT.Core;
using System.ComponentModel.DataAnnotations;

namespace McWuT.Data.Models
{
    public class ShoppingList : IUserEntity
    {
        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "List Name")]
        public required string Name { get; set; }

        [Required]
        public Guid UniqueId { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [Required]
        [MaxLength(128)]
        public required string UserId { get; set; }

        // Navigation property
        public virtual ICollection<ShoppingItem> Items { get; set; } = new List<ShoppingItem>();
    }
}