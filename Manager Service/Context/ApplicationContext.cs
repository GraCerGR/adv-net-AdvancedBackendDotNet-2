using Manager_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Manager_Service.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ManagerModel> Managers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ManagerModel>().HasKey(x => x.Id);
        }
    }
}
