using Microsoft.EntityFrameworkCore;

namespace Gip.Models
{
    public partial class gipDatabaseContext : DbContext
    {
        public gipDatabaseContext()
        {
        }

        public gipDatabaseContext(DbContextOptions<gipDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<CourseMoment> CourseMoment { get; set; }
        public virtual DbSet<CourseUser> CourseUser { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=diskstation.desmet.net,32850;Database=gipDatabase;User Id=SA;Password=Str0ngP4ssw0rd");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Vakcode);

                entity.Property(e => e.Vakcode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Titel)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CourseMoment>(entity =>
            {
                entity.HasKey(e => new { e.Vakcode, e.Datum, e.Startmoment, e.Gebouw, e.Verdiep, e.Nummer, e.Userid });

                entity.Property(e => e.Vakcode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                
                entity.Property(e => e.LessenLijst)
                    .IsUnicode(false);

                entity.Property(e => e.Datum).HasColumnType("date");

                entity.Property(e => e.Startmoment).HasColumnType("datetime");

                entity.Property(e => e.Gebouw)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Nummer)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Userid)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CourseMoment)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseMoment_User");

                entity.HasOne(d => d.VakcodeNavigation)
                    .WithMany(p => p.CourseMoment)
                    .HasForeignKey(d => d.Vakcode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseMoment_Course");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.CourseMoment)
                    .HasForeignKey(d => new { d.Datum, d.Startmoment })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseMoment_Schedule");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.CourseMoment)
                    .HasForeignKey(d => new {d.Gebouw, d.Verdiep, d.Nummer})
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseMoment_Room");
            });

            modelBuilder.Entity<CourseUser>(entity =>
            {
                entity.HasKey(e => new { e.Vakcode, e.Userid });

                entity.Property(e => e.Vakcode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Userid)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CourseUser)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseUser_User");

                entity.HasOne(d => d.VakcodeNavigation)
                    .WithMany(p => p.CourseUser)
                    .HasForeignKey(d => d.Vakcode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CourseUser_Course");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => new { e.Gebouw, e.Verdiep, e.Nummer });

                entity.Property(e => e.Gebouw)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Nummer)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Middelen).IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(e => new { e.Datum, e.Startmoment });

                entity.Property(e => e.Datum).HasColumnType("date");

                entity.Property(e => e.Startmoment).HasColumnType("datetime");

                entity.Property(e => e.Eindmoment).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Userid)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Naam)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
