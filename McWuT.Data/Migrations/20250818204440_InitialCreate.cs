using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McWuT.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    Difficulty = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    HintsUsed = table.Column<int>(type: "INTEGER", nullable: false),
                    IsAccusationMade = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccusedSuspectId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    IsCorrectAccusation = table.Column<bool>(type: "INTEGER", nullable: false),
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PasswordEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EncryptedPassword = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Website = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameSessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    Occupation = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Backstory = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Personality = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Nationality = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_GameSessions_GameSessionId",
                        column: x => x.GameSessionId,
                        principalTable: "GameSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameSessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,8)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(11,8)", nullable: true),
                    IsAccessible = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessRequirements = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    IsSearched = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastSearchTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SearchNotes = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    HasEvidence = table.Column<bool>(type: "INTEGER", nullable: false),
                    AtmosphericDescription = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_GameSessions_GameSessionId",
                        column: x => x.GameSessionId,
                        principalTable: "GameSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsPurchased = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Quantity = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ShoppingListId = table.Column<int>(type: "INTEGER", nullable: false),
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingItems_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Character1Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Character2Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Strength = table.Column<int>(type: "INTEGER", nullable: false),
                    Sentiment = table.Column<int>(type: "INTEGER", nullable: false),
                    RelationshipStart = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RelationshipEnd = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    IsPublicKnowledge = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastInteraction = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    LastInteractionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relationships_Characters_Character1Id",
                        column: x => x.Character1Id,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Relationships_Characters_Character2Id",
                        column: x => x.Character2Id,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Suspects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CharacterId = table.Column<int>(type: "INTEGER", nullable: false),
                    Motive = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Alibi = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    IsGuilty = table.Column<bool>(type: "INTEGER", nullable: false),
                    SuspicionLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    Opportunity = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Means = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    LastSeenVictimTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastSeenVictimLocation = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    PreviousRecord = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    InterrogationNotes = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    HasBeenInterrogated = table.Column<bool>(type: "INTEGER", nullable: false),
                    InterrogationCount = table.Column<int>(type: "INTEGER", nullable: false),
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suspects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suspects_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Witnesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CharacterId = table.Column<int>(type: "INTEGER", nullable: false),
                    Statement = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    ReliabilityScore = table.Column<int>(type: "INTEGER", nullable: false),
                    HasBeenInterviewed = table.Column<bool>(type: "INTEGER", nullable: false),
                    InterviewCount = table.Column<int>(type: "INTEGER", nullable: false),
                    WitnessedLocation = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    WitnessedTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    WhatWitnessed = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    SuspiciousActivity = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    InterviewNotes = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    IsCredible = table.Column<bool>(type: "INTEGER", nullable: false),
                    Concerns = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Witnesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Witnesses_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameSessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Relevance = table.Column<int>(type: "INTEGER", nullable: false),
                    LocationFoundId = table.Column<int>(type: "INTEGER", nullable: false),
                    DiscoveredTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDiscovered = table.Column<bool>(type: "INTEGER", nullable: false),
                    DifficultyToFind = table.Column<int>(type: "INTEGER", nullable: false),
                    Analysis = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    RelatedCharacterId = table.Column<int>(type: "INTEGER", nullable: true),
                    HowItHelps = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    RequiresSpecialEquipment = table.Column<bool>(type: "INTEGER", nullable: false),
                    SpecialEquipmentNeeded = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clues_Characters_RelatedCharacterId",
                        column: x => x.RelatedCharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clues_GameSessions_GameSessionId",
                        column: x => x.GameSessionId,
                        principalTable: "GameSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clues_Locations_LocationFoundId",
                        column: x => x.LocationFoundId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Victims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CharacterId = table.Column<int>(type: "INTEGER", nullable: false),
                    CauseOfDeath = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    TimeOfDeath = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WeaponUsed = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    LocationFoundId = table.Column<int>(type: "INTEGER", nullable: false),
                    CircumstancesOfDeath = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    LastSeenWith = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    LastSeenTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastSeenLocation = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Victims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Victims_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Victims_Locations_LocationFoundId",
                        column: x => x.LocationFoundId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimelineEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameSessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    EventTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EventType = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ActorCharacterId = table.Column<int>(type: "INTEGER", nullable: true),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: true),
                    Certainty = table.Column<int>(type: "INTEGER", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Details = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    RelatedClueId = table.Column<int>(type: "INTEGER", nullable: true),
                    Source = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    IsPublicKnowledge = table.Column<bool>(type: "INTEGER", nullable: false),
                    Importance = table.Column<int>(type: "INTEGER", nullable: false),
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelineEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimelineEvents_Characters_ActorCharacterId",
                        column: x => x.ActorCharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimelineEvents_Clues_RelatedClueId",
                        column: x => x.RelatedClueId,
                        principalTable: "Clues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimelineEvents_GameSessions_GameSessionId",
                        column: x => x.GameSessionId,
                        principalTable: "GameSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimelineEvents_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_GameSessionId",
                table: "Characters",
                column: "GameSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Clues_GameSessionId",
                table: "Clues",
                column: "GameSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Clues_LocationFoundId",
                table: "Clues",
                column: "LocationFoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Clues_RelatedCharacterId",
                table: "Clues",
                column: "RelatedCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_GameSessionId",
                table: "Locations",
                column: "GameSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_Character1Id_Character2Id_Type",
                table: "Relationships",
                columns: new[] { "Character1Id", "Character2Id", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_Character2Id",
                table: "Relationships",
                column: "Character2Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItems_ShoppingListId",
                table: "ShoppingItems",
                column: "ShoppingListId");

            migrationBuilder.CreateIndex(
                name: "IX_Suspects_CharacterId",
                table: "Suspects",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEvents_ActorCharacterId",
                table: "TimelineEvents",
                column: "ActorCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEvents_GameSessionId",
                table: "TimelineEvents",
                column: "GameSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEvents_LocationId",
                table: "TimelineEvents",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelineEvents_RelatedClueId",
                table: "TimelineEvents",
                column: "RelatedClueId");

            migrationBuilder.CreateIndex(
                name: "IX_Victims_CharacterId",
                table: "Victims",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Victims_LocationFoundId",
                table: "Victims",
                column: "LocationFoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Witnesses_CharacterId",
                table: "Witnesses",
                column: "CharacterId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "PasswordEntries");

            migrationBuilder.DropTable(
                name: "Relationships");

            migrationBuilder.DropTable(
                name: "ShoppingItems");

            migrationBuilder.DropTable(
                name: "Suspects");

            migrationBuilder.DropTable(
                name: "TimelineEvents");

            migrationBuilder.DropTable(
                name: "Victims");

            migrationBuilder.DropTable(
                name: "Witnesses");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ShoppingLists");

            migrationBuilder.DropTable(
                name: "Clues");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "GameSessions");
        }
    }
}
