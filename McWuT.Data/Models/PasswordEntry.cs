using McWuT.Core;
using System.ComponentModel.DataAnnotations;

namespace McWuT.Data.Models
{
    public class PasswordEntry : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        [Required]
        public Guid UniqueId { get; set; }

        [Required]
        [MaxLength(128)]
        public required string UserId { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "Entry Name")]
        public required string Name { get; set; }

        [MaxLength(256)]
        public string? Website { get; set; }

        [MaxLength(256)]
        public string? Username { get; set; }

        public string? EncryptedPassword { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}