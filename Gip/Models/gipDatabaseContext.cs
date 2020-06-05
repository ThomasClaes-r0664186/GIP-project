using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Gip.Models
{
    public partial class gipDatabaseContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;

        public gipDatabaseContext(DbContextOptions<gipDatabaseContext> options, IConfiguration configuration)
            : base(options)
        {
            this._configuration = configuration;
        }

        public gipDatabaseContext(DbContextOptions<gipDatabaseContext> options) : base(options) {}

        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<CourseUser> CourseUser { get; set; }
        public virtual DbSet<CourseMoment> CourseMoment { get; set; }
        public virtual DbSet<CourseMomentUsers> CourseMomentUsers { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<FieldOfStudy> FieldOfStudy { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("database"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //testen of dit werkt => zorgt ervoor dat wanneer er iets verwijderd wordt, elke lijn waarin ernaar verwezen is, ook wordt verwijderd.
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
            }
        }
    }
}
