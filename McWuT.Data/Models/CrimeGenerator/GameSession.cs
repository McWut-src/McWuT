using McWuT.Core;
using System.ComponentModel.DataAnnotations;

namespace McWuT.Data.Models.CrimeGenerator;

public enum GameSessionState
{
    InProgress = 0,
    Solved = 1,
    Failed = 2,
    Abandoned = 3
}

public enum DifficultyLevel
{
    Easy = 0,
    Medium = 1,
    Hard = 2
}

public class GameSession : BaseEntity, IUserEntity
{
    [Required]
    [MaxLength(128)]
    public required string UserId { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public GameSessionState State { get; set; } = GameSessionState.InProgress;

    [Required]
    public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Medium;

    public int Score { get; set; } = 0;

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int HintsUsed { get; set; } = 0;

    public bool IsAccusationMade { get; set; } = false;

    [MaxLength(128)]
    public string? AccusedSuspectId { get; set; }

    public bool IsCorrectAccusation { get; set; } = false;

    // Navigation properties
    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
    public virtual ICollection<Clue> Clues { get; set; } = new List<Clue>();
    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
    public virtual ICollection<Timeline> TimelineEvents { get; set; } = new List<Timeline>();
}