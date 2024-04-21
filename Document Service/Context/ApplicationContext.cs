using Microsoft.EntityFrameworkCore;
using Document_Service.Models;

namespace WebApplication1.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<FilePassportModel> PassportFiles { get; set; }

        public DbSet<FileEducationModel> EducationFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilePassportModel>().HasKey(x => x.Id);

            modelBuilder.Entity<FileEducationModel>().HasKey(x => x.Id);

        }
    }
}
