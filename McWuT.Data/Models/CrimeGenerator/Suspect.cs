using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McWuT.Data.Models.CrimeGenerator;

public class Suspect : BaseEntity
{
    [Required]
    public int CharacterId { get; set; }

    [MaxLength(1000)]
    public string? Motive { get; set; }

    [MaxLength(1000)]
    public string? Alibi { get; set; }

    [Required]
    public bool IsGuilty { get; set; }

    [Range(0, 100)]
    public int SuspicionLevel { get; set; } = 50; // 0-100 scale

    [MaxLength(500)]
    public string? Opportunity { get; set; }

    [MaxLength(500)]
    public string? Means { get; set; }

    public DateTime? LastSeenVictimTime { get; set; }

    [MaxLength(500)]
    public string? LastSeenVictimLocation { get; set; }

    [MaxLength(1000)]
    public string? PreviousRecord { get; set; }

    [MaxLength(2000)]
    public string? InterrogationNotes { get; set; }

    public bool HasBeenInterrogated { get; set; } = false;

    public int InterrogationCount { get; set; } = 0;

    // Navigation properties
    [ForeignKey("CharacterId")]
    public virtual Character Character { get; set; } = null!;
}