using EAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace EAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext)
                .Assembly);
        }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<City> Cities => Set<City>();
        public DbSet<Country> Countries => Set<Country>();



        // END
    }
}
