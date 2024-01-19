using Microsoft.EntityFrameworkCore;
using Kindergarten.Models;

namespace Kindergarten.Data
{
    public class KindergartenContext : DbContext
    {
        public KindergartenContext(DbContextOptions<KindergartenContext> options) : base(options) { }

        public DbSet<Child> Children { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<KActivity> KActivities { get; set; }
        public DbSet<ClassActivity> ClassesActivities { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Child>().ToTable("Children");
            modelBuilder.Entity<Class>().ToTable("Classes");
            modelBuilder.Entity<Teacher>().ToTable("Teachers");
            modelBuilder.Entity<KActivity>().ToTable("KActivities");
            modelBuilder.Entity<ClassActivity>().ToTable("ClassesActivities");
            modelBuilder.Entity<Attendance>().ToTable("Attendances");
            modelBuilder.Entity<ClassActivity>().HasKey(ca => new { ca.ClassID, ca.KActivityID });

            // Additional code for identity configurations
        }

    }
}
