using McWuT.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using McWuT.Data.Models;

namespace McWuT.Data.Contexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
               : IdentityDbContext<IdentityUser>(options)
    {
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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

        private static void SetSoftDeleteFilter<TEntity>(ModelBuilder builder) where TEntity : class, IEntity
        {
            builder.Entity<TEntity>().HasQueryFilter(e => e.DeletedDate == null);
        }


    }
}
