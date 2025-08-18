using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McWuT.Data.Models.CrimeGenerator;

public enum ClueType
{
    Physical = 0,
    Digital = 1,
    Testimonial = 2,
    Circumstantial = 3,
    Forensic = 4
}

public enum ClueRelevance
{
    RedHerring = 0,
    Minor = 1,
    Important = 2,
    Critical = 3
}

public class Clue : BaseEntity
{
    [Required]
    public int GameSessionId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(1000)]
    public required string Description { get; set; }

    [Required]
    public ClueType Type { get; set; }

    [Required]
    public ClueRelevance Relevance { get; set; }

    [Required]
    public int LocationFoundId { get; set; }

    public DateTime? DiscoveredTime { get; set; }

    public bool IsDiscovered { get; set; } = false;

    [Range(0, 100)]
    public int DifficultyToFind { get; set; } = 50; // 0 = easy to find, 100 = very hard

    [MaxLength(1000)]
    public string? Analysis { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public int? RelatedCharacterId { get; set; }

    [MaxLength(1000)]
    public string? HowItHelps { get; set; }

    public bool RequiresSpecialEquipment { get; set; } = false;

    [MaxLength(200)]
    public string? SpecialEquipmentNeeded { get; set; }

    // Navigation properties
    [ForeignKey("GameSessionId")]
    public virtual GameSession GameSession { get; set; } = null!;

    [ForeignKey("LocationFoundId")]
    public virtual Location LocationFound { get; set; } = null!;

    [ForeignKey("RelatedCharacterId")]
    public virtual Character? RelatedCharacter { get; set; }
}