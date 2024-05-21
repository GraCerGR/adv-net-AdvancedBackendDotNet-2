using Microsoft.EntityFrameworkCore;
using User_Service.Models;

namespace User_Service.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<UserModel> Users { get; set; }

        public DbSet<ManagerModel> Managers { get; set; }

        public DbSet<AdminModel> Admins { get; set; }

        public DbSet<EditPasswordCode> Codes { get; set; }

        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasKey(x => x.Id);

            modelBuilder.Entity<ManagerModel>().HasKey(x => x.Id);
            //modelBuilder.Entity<ManagerModel>().HasIndex(x => new { x.UserId}).IsUnique();

            modelBuilder.Entity<AdminModel>().HasKey(x => x.Id);

            modelBuilder.Entity<EditPasswordCode>().HasKey(x => x.Id);

            modelBuilder.Entity<RefreshTokenModel>().HasKey(x => x.Id);
        }
    }
}
