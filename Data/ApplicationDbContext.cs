using Microsoft.EntityFrameworkCore;
using StudentPortal.Models.Entities;

namespace StudentPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectPreq> Prerequistes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SubjectPreq>()
                .HasKey(sp => new { sp.SubjectCode, sp.PreCode });

            modelBuilder.Entity<SubjectPreq>()
                .HasOne(sp => sp.Subject)
                .WithMany(s => s.Prerequisites)
                .HasForeignKey(sp => sp.SubjectCode);
        }
        public DbSet<SubjectSchedule> SubjectSchedules { get; set; }
    }
}
