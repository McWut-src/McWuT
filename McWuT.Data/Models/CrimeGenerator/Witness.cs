using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McWuT.Data.Models.CrimeGenerator;

public class Witness : BaseEntity
{
    [Required]
    public int CharacterId { get; set; }

    [MaxLength(2000)]
    public string? Statement { get; set; }

    [Range(0, 100)]
    public int ReliabilityScore { get; set; } = 75; // 0-100 scale

    public bool HasBeenInterviewed { get; set; } = false;

    public int InterviewCount { get; set; } = 0;

    [MaxLength(500)]
    public string? WitnessedLocation { get; set; }

    public DateTime? WitnessedTime { get; set; }

    [MaxLength(1000)]
    public string? WhatWitnessed { get; set; }

    [MaxLength(500)]
    public string? SuspiciousActivity { get; set; }

    [MaxLength(2000)]
    public string? InterviewNotes { get; set; }

    public bool IsCredible { get; set; } = true;

    [MaxLength(500)]
    public string? Concerns { get; set; }

    // Navigation properties
    [ForeignKey("CharacterId")]
    public virtual Character Character { get; set; } = null!;
}