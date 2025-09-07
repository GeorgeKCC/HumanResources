using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace ColaboratorModule.Data.Context
{
    internal class ColaboratorContext(DbContextOptions<ColaboratorContext> options) : DbContext(options)
    {
        public DbSet<Colaborator> Colaborators { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Colaborator>()
                         .Property(e => e.Version)
                         .IsRowVersion();

            // Configure your entities here
            base.OnModelCreating(modelBuilder);
        }
    }
}
