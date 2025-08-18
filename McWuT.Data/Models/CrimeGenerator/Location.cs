using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McWuT.Data.Models.CrimeGenerator;

public enum LocationType
{
    CrimeScene = 0,
    Residence = 1,
    Workplace = 2,
    Public = 3,
    Recreation = 4,
    Transportation = 5,
    Other = 6
}

public class Location : BaseEntity
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
    public LocationType Type { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public bool IsAccessible { get; set; } = true;

    [MaxLength(500)]
    public string? AccessRequirements { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public bool IsSearched { get; set; } = false;

    public DateTime? LastSearchTime { get; set; }

    [MaxLength(2000)]
    public string? SearchNotes { get; set; }

    public bool HasEvidence { get; set; } = false;

    [MaxLength(1000)]
    public string? AtmosphericDescription { get; set; }

    // Navigation properties
    [ForeignKey("GameSessionId")]
    public virtual GameSession GameSession { get; set; } = null!;

    public virtual ICollection<Clue> CluesFound { get; set; } = new List<Clue>();
    public virtual ICollection<Victim> VictimsFound { get; set; } = new List<Victim>();
}