using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gip.Models
{
    public partial class gipDatabaseContext : IdentityDbContext
    {
        public gipDatabaseContext()
        {
        }

        public gipDatabaseContext(DbContextOptions<gipDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<CourseUser> CourseUser { get; set; }
        public virtual DbSet<CourseMoment> CourseMoment { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=diskstation.desmet.net,32850;Database=gipDatabase;User Id=SA;Password=Str0ngP4ssw0rd;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Room>().HasKey(x => new {
                x.Gebouw,
                x.Verdiep,
                x.Nummer
            });

            modelBuilder.Entity<CourseUser>().HasKey(x => new {
                x.Userid,
                x.Vakcode
            });

            modelBuilder.Entity<Schedule>().HasKey(x => new {
                x.Datum,
                x.Startmoment,
                x.Eindmoment
            });

            modelBuilder.Entity<CourseMoment>().HasKey(x => new { 
                x.Vakcode, 
                x.Datum, 
                x.Gebouw, 
                x.Verdiep, 
                x.Nummer, 
                x.Userid, 
                x.Startmoment, 
                x.Eindmoment });
        }
    }
}
