using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McWuT.Data.Models.CrimeGenerator;

public enum RelationshipType
{
    Family = 0,
    Friend = 1,
    Enemy = 2,
    Romantic = 3,
    Professional = 4,
    Acquaintance = 5,
    Stranger = 6
}

public class Relationship : BaseEntity
{
    [Required]
    public int Character1Id { get; set; }

    [Required]
    public int Character2Id { get; set; }

    [Required]
    public RelationshipType Type { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Description { get; set; }

    [Range(0, 10)]
    public int Strength { get; set; } = 5; // 0 = weak, 10 = very strong

    [Range(-10, 10)]
    public int Sentiment { get; set; } = 0; // -10 = hate, 0 = neutral, 10 = love

    public DateTime? RelationshipStart { get; set; }

    public DateTime? RelationshipEnd { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public bool IsPublicKnowledge { get; set; } = true;

    [MaxLength(500)]
    public string? LastInteraction { get; set; }

    public DateTime? LastInteractionDate { get; set; }

    // Navigation properties
    [ForeignKey("Character1Id")]
    public virtual Character Character1 { get; set; } = null!;

    [ForeignKey("Character2Id")]
    public virtual Character Character2 { get; set; } = null!;
}