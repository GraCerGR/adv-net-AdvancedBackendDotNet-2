using Manager_Service.Models;
using Microsoft.EntityFrameworkCore;
using User_Service.Models.DTO;

namespace Manager_Service.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<QueueProgramsModel> QueuePrograms { get; set; }

        public DbSet<ApplicationModel> Applications { get; set; }

        //public DbSet<UserDto> UserDto { get; set; } //Миграция сама создала эту таблицу для хранения данных 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QueueProgramsModel>().HasKey(x => x.Id);

            modelBuilder.Entity<ApplicationModel>().HasKey(x => x.Id);

            //modelBuilder.Entity<UserDto>().HasKey(x => x.Id);
        }
    }
}
