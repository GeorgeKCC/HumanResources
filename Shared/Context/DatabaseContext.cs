namespace Shared.Context
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        public DbSet<Colaborator> Colaborators { get; set; }
        public DbSet<Security> Securities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Colaborator>()
                         .Property(e => e.Version)
                         .IsRowVersion();

            base.OnModelCreating(modelBuilder);
        }
    }
}