using Microsoft.EntityFrameworkCore;
using PV178StudyBotDAL.Entities;
using PV178StudyBotDAL.Entities.ConnectionTables;
using System;

namespace PV178StudyBotDAL
{
    public class PV178StudyBotDbContext : DbContext
    {
        // Entity Tables

        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Achievement> Achievements { get; set; }

        public DbSet<Rank> Ranks { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<Admin> Admins { get; set; }

        // Connection Tables

        public DbSet<StudentAndAchievement> StudentAndAchievements { get; set; }

        //This should be injected
        private string ConnectionString = Environment.GetEnvironmentVariable("PV178StudyBot_ConnectionString");

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));  

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentAndAchievement>()
                .HasKey(obj => new { obj.AchievementId, obj.StudentId });

            modelBuilder.Seed();

            base.OnModelCreating(modelBuilder);
        }
    }
}
