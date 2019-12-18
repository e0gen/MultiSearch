using Microsoft.EntityFrameworkCore;

namespace MultiSearch.DataAccess
{
    public class WorkDbContext : DbContext
    {
        public WorkDbContext()
        {
        }

        public WorkDbContext(DbContextOptions<WorkDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<WebPageEntity> WebPages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=MultiSearchDB; Integrated Security = true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WebPageEntity>()
                .Property(b => b.WebPageEntityId)
                .IsRequired();
        }
    }
}
