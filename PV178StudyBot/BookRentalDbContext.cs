﻿using PV178StudyBotDAL.Entities;
using PV178StudyBotDAL.Entities.ConnectionTables;
using Microsoft.EntityFrameworkCore;

namespace PV178StudyBotDAL
{
    public class BookRentalDbContext : DbContext
    {
        // Entity Tables

        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Achievement> Achievements { get; set; }

        public DbSet<Rank> Ranks { get; set; }

        public DbSet<Request> Requests { get; set; }

        // Connection Tables

        public DbSet<StudentAndAchievement> StudentAndAchievements { get; set; }


        private string ConnectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=BookReservation";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);

            optionsBuilder.EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Seed();

            base.OnModelCreating(modelBuilder);
        }
    }
}
