using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace AcademycManager.Data
{
    public class AcademycDbContext : DbContext
    {
        public DbSet<SubjectEntity> Subjects { get; set; } = null!;
        public DbSet<ActivityEntity> Activities { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AcademycManager");

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            string dbPath = Path.Combine(appDataPath, "AcademycManager.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Subject configuration
            modelBuilder.Entity<SubjectEntity>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<SubjectEntity>()
                .HasMany(s => s.Activities)
                .WithOne(a => a.Subject)
                .HasForeignKey(a => a.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Activity configuration
            modelBuilder.Entity<ActivityEntity>()
                .HasKey(a => a.Id);
        }
    }
}
