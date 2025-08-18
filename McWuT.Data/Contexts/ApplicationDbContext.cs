using McWuT.Core;
using McWuT.Data.Models;
using McWuT.Data.Models.CrimeGenerator;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace McWuT.Data.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
               : IdentityDbContext<IdentityUser>(options)
    {
        public DbSet<Note> Notes { get; set; }

        public DbSet<PasswordEntry> PasswordEntries { get; set; }

        public DbSet<ShoppingList> ShoppingLists { get; set; }

        public DbSet<ShoppingItem> ShoppingItems { get; set; }

        // CrimeGenerator entities
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Victim> Victims { get; set; }
        public DbSet<Suspect> Suspects { get; set; }
        public DbSet<Witness> Witnesses { get; set; }
        public DbSet<Clue> Clues { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Timeline> TimelineEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // CrimeGenerator entity configurations
            ConfigureCrimeGeneratorEntities(builder);

            // Apply global query filter for soft delete
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(IEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ApplicationDbContext)
                                 .GetMethod(nameof(SetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)
                                ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, [builder]);
                }
            }
        }

        private void ConfigureCrimeGeneratorEntities(ModelBuilder builder)
        {
            // Character relationships configuration
            builder.Entity<Relationship>()
                .HasOne(r => r.Character1)
                .WithMany(c => c.RelationshipsAsCharacter1)
                .HasForeignKey(r => r.Character1Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Relationship>()
                .HasOne(r => r.Character2)
                .WithMany(c => c.RelationshipsAsCharacter2)
                .HasForeignKey(r => r.Character2Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Character type-specific details configuration
            builder.Entity<Character>()
                .HasOne(c => c.VictimDetails)
                .WithOne(v => v.Character)
                .HasForeignKey<Victim>(v => v.CharacterId);

            builder.Entity<Character>()
                .HasOne(c => c.SuspectDetails)
                .WithOne(s => s.Character)
                .HasForeignKey<Suspect>(s => s.CharacterId);

            builder.Entity<Character>()
                .HasOne(c => c.WitnessDetails)
                .WithOne(w => w.Character)
                .HasForeignKey<Witness>(w => w.CharacterId);

            // Ensure unique relationship constraints
            builder.Entity<Relationship>()
                .HasIndex(r => new { r.Character1Id, r.Character2Id, r.Type })
                .IsUnique();

            // Decimal precision for coordinates
            builder.Entity<Location>()
                .Property(l => l.Latitude)
                .HasColumnType("decimal(10,8)");

            builder.Entity<Location>()
                .Property(l => l.Longitude)
                .HasColumnType("decimal(11,8)");
        }

        private static void SetSoftDeleteFilter<TEntity>(ModelBuilder builder) where TEntity : class, IEntity
        {
            builder.Entity<TEntity>().HasQueryFilter(e => e.DeletedDate == null);
        }
    }
}