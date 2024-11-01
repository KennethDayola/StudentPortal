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

        }
        public DbSet<SubjectSchedule> SubjectSchedules { get; set; }
    }
}
