using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McWuT.Data.Models.CrimeGenerator;

public enum CharacterType
{
    Victim = 0,
    Suspect = 1,
    Witness = 2
}

public enum Gender
{
    Male = 0,
    Female = 1,
    Other = 2
}

public class Character : BaseEntity
{
    [Required]
    public int GameSessionId { get; set; }

    [Required]
    [MaxLength(100)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public required string LastName { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    [Required]
    public int Age { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [Required]
    [MaxLength(150)]
    public required string Occupation { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [Required]
    public CharacterType Type { get; set; }

    [MaxLength(2000)]
    public string? Backstory { get; set; }

    [MaxLength(1000)]
    public string? Personality { get; set; }

    [MaxLength(200)]
    public string? Nationality { get; set; }

    // Location information
    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }

    // Navigation properties
    [ForeignKey("GameSessionId")]
    public virtual GameSession GameSession { get; set; } = null!;

    public virtual ICollection<Relationship> RelationshipsAsCharacter1 { get; set; } = new List<Relationship>();
    public virtual ICollection<Relationship> RelationshipsAsCharacter2 { get; set; } = new List<Relationship>();
    
    public virtual Victim? VictimDetails { get; set; }
    public virtual Suspect? SuspectDetails { get; set; }
    public virtual Witness? WitnessDetails { get; set; }
}