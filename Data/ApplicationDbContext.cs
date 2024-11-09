using Microsoft.EntityFrameworkCore;
using StudentPortal.Models.Entities;

namespace StudentPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectPreq> Prerequisites { get; set; }
        public DbSet<SubjectSchedule> SubjectSchedules { get; set; }
        public DbSet<EnrollmentDetail> EnrollmentDetails { get; set; }
        public DbSet<EnrollmentHeader> EnrollmentHeaders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SubjectPreq>()
                .HasKey(sp => new { sp.SubjectCode, sp.PreCode });

            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Prerequisites)
                .WithOne(sp => sp.Subject)
                .HasForeignKey<SubjectPreq>(sp => sp.SubjectCode)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subject>()
                .HasMany(s => s.Schedules)
                .WithOne(sc => sc.Subject)
                .HasForeignKey(sc => sc.SubjectCode)  
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EnrollmentDetail>()
               .HasKey(e => new { e.StudId, e.EDPCode });

            modelBuilder.Entity<EnrollmentHeader>()
                .HasOne(eh => eh.Student)              // Each EnrollmentHeader has one Student
                .WithOne(s => s.EnrollmentHeader)      // Each Student has one EnrollmentHeader
                .HasForeignKey<EnrollmentHeader>(eh => eh.StudId) // StudId is the foreign key in EnrollmentHeader
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EnrollmentHeader>()
               .HasMany(eh => eh.EnrollmentDetails)
               .WithOne(ed => ed.EnrollmentHeader)
               .HasForeignKey(ed => ed.StudId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubjectSchedule>()
                .HasMany(ss => ss.EnrollmentDetails)
                .WithOne(ed => ed.SubjectSchedule)
                .HasForeignKey(ed => ed.EDPCode)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
