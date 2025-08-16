using McWuT.Core;
using System.ComponentModel.DataAnnotations;

namespace McWuT.Data.Models
{
    public class PasswordEntry : IUserEntity
    {
        [MaxLength(100)]
        public string? Category { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string? EncryptedPassword { get; set; }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        [Display(Name = "Entry Name")]
        public required string Name { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        [Required]
        public Guid UniqueId { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [Required]
        [MaxLength(128)]
        public required string UserId { get; set; }

        [MaxLength(256)]
        public string? Username { get; set; }

        [MaxLength(256)]
        public string? Website { get; set; }
    }
}