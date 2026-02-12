namespace Shared.Context.HumanResource_Context
{
    public class DatabaseHumanResourceContext(DbContextOptions<DatabaseHumanResourceContext> options) : DbContext(options)
    {
        public DbSet<Colaborator> Colaborators { get; set; }
        public DbSet<Security> Securities { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Colaborator>()
                         .Property(e => e.Version)
                         .IsRowVersion();

            base.OnModelCreating(modelBuilder);
        }
    }
}