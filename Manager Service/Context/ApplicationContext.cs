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

        public DbSet<QueueProgramsModel> QueuePrograms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<QueueProgramsModel>().HasKey(x => x.Id);
        }
    }
}
