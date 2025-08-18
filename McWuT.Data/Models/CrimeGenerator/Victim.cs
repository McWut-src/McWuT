using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McWuT.Data.Models.CrimeGenerator;

public class Victim : BaseEntity
{
    [Required]
    public int CharacterId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string CauseOfDeath { get; set; }

    [Required]
    public DateTime TimeOfDeath { get; set; }

    [MaxLength(200)]
    public string? WeaponUsed { get; set; }

    [Required]
    public int LocationFoundId { get; set; }

    [MaxLength(1000)]
    public string? CircumstancesOfDeath { get; set; }

    [MaxLength(500)]
    public string? LastSeenWith { get; set; }

    public DateTime? LastSeenTime { get; set; }

    [MaxLength(500)]
    public string? LastSeenLocation { get; set; }

    // Navigation properties
    [ForeignKey("CharacterId")]
    public virtual Character Character { get; set; } = null!;

    [ForeignKey("LocationFoundId")]
    public virtual Location LocationFound { get; set; } = null!;
}