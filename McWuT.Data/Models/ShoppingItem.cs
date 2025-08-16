using McWuT.Core;
using System.ComponentModel.DataAnnotations;

namespace McWuT.Data.Models
{
    public class ShoppingItem : IUserEntity
    {
        [MaxLength(100)]
        public string? Category { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        [Key]
        public int Id { get; set; }

        public bool IsPurchased { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "Item Name")]
        public required string Name { get; set; }

        [MaxLength(50)]
        public string? Quantity { get; set; }

        [Required]
        public int ShoppingListId { get; set; }

        [Required]
        public Guid UniqueId { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [Required]
        [MaxLength(128)]
        public required string UserId { get; set; }

        // Navigation property
        public virtual ShoppingList? ShoppingList { get; set; }
    }
}