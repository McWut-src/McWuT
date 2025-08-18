using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McWuT.Data.Models.CrimeGenerator;

public enum TimelineEventType
{
    VictimActivity = 0,
    SuspectActivity = 1,
    WitnessObservation = 2,
    CrimeOccurrence = 3,
    EvidenceCreation = 4,
    Other = 5
}

public class Timeline : BaseEntity
{
    [Required]
    public int GameSessionId { get; set; }

    [Required]
    public DateTime EventTime { get; set; }

    [Required]
    public TimelineEventType EventType { get; set; }

    [Required]
    [MaxLength(500)]
    public required string Description { get; set; }

    public int? ActorCharacterId { get; set; }

    public int? LocationId { get; set; }

    [Range(0, 100)]
    public int Certainty { get; set; } = 100; // 0 = uncertain, 100 = certain

    public bool IsConfirmed { get; set; } = false;

    [MaxLength(1000)]
    public string? Details { get; set; }

    public int? RelatedClueId { get; set; }

    [MaxLength(200)]
    public string? Source { get; set; } // Who provided this information

    public bool IsPublicKnowledge { get; set; } = true;

    [Range(0, 10)]
    public int Importance { get; set; } = 5; // 0 = not important, 10 = crucial

    // Navigation properties
    [ForeignKey("GameSessionId")]
    public virtual GameSession GameSession { get; set; } = null!;

    [ForeignKey("ActorCharacterId")]
    public virtual Character? ActorCharacter { get; set; }

    [ForeignKey("LocationId")]
    public virtual Location? Location { get; set; }

    [ForeignKey("RelatedClueId")]
    public virtual Clue? RelatedClue { get; set; }
}