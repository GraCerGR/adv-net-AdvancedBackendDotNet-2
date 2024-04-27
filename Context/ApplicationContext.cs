using Handbook_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Handbook_Service.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }


        public DbSet<FacultyModel> Faculties { get; set; }

        public DbSet<EducationLevelModel> EducationLevels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FacultyModel>().HasKey(x => x.Id);

            /*modelBuilder.Entity<EducationLevelModel>().HasKey(x => x.Id);*/
        }

    }
}
