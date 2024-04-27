using Manager_Service.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace Manager_Service.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ManagerModel> Managers { get; set; }

        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ManagerModel>().HasKey(x => x.Id);

            modelBuilder.Entity<UserModel>().HasKey(x => x.Id);
        }
    }
}
