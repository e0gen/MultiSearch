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

        public virtual DbSet<ItemDb> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
            }
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer("Server=SQL2\\MAIN;Database=WorkDB;Trusted_Connection=True;");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
