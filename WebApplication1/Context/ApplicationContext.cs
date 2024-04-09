using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<UserModel> Users { get; set; }

        public DbSet<ManagerModel> Managers { get; set; }

        public DbSet<EditPasswordCode> Codes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasKey(x => x.Id);

            modelBuilder.Entity<ManagerModel>().HasKey(x => x.Id);
            //modelBuilder.Entity<ManagerModel>().HasIndex(x => new { x.UserId}).IsUnique();
            modelBuilder.Entity<EditPasswordCode>().HasKey(x => x.Id);
        }
    }
}
