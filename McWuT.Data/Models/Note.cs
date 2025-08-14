using McWuT.Core;
using System.ComponentModel.DataAnnotations;

namespace McWuT.Data.Models
{
    public class Note : IEntity
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

     
        [MaxLength(256)]
        public string? Title { get; set; }

       
        [MaxLength(4000)]
        public string? Content { get; set; }
    }
}