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
               .HasMany(s => s.EnrollmentDetails)
               .WithOne(sc => sc.EnrollmentHeader)
               .HasForeignKey(sc => sc.StudId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SubjectSchedule>()
                .HasMany(s => s.EnrollmentDetails)
                .WithOne(sc => sc.SubjectSchedule)
                .HasForeignKey(sc => sc.EDPCode)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
