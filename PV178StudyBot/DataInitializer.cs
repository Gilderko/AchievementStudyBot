using Microsoft.EntityFrameworkCore;
using PV178StudyBotDAL.Entities;

namespace PV178StudyBotDAL
{
    public static class DataInitializer
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Achievement>().HasData(
                new Achievement
                {
                    Id = 1,
                    Name = "Good Start",
                    Description = "Login into the achievement system."
                },
                new Achievement
                {
                    Id = 2,
                    Name = "Qualifier",
                    Description = " Visit the third seminar."
                },
                new Achievement
                {
                    Id = 3,
                    Name = "First Blood",
                    Description = "First answer to a relevant question in seminar."
                },
                new Achievement
                {
                    Id = 4,
                    Name = "Curious",
                    Description = "First question in seminar."
                },
                new Achievement
                {
                    Id = 5,
                    Name = "Yes, We Have a Forum",
                    Description = "Write a relevant post in the discussion forum, or discord channel."
                },
                new Achievement
                {
                    Id = 6,
                    Name = "See Sharp",
                    Description = "Create at least four unit tests in your homework."
                },
                new Achievement
                {
                    Id = 7,
                    Name = "Fanatic",
                    Description = "Visit 6 consecutive seminars."
                },
                new Achievement
                {
                    Id = 8,
                    Name = "Not Too Late",
                    Description = "Do not arrive late to a seminar."
                },
                new Achievement
                {
                    Id = 9,
                    Name = "Never Too Late",
                    Description = "Do not arrive late to any seminar."
                },
                new Achievement
                {
                    Id = 10,
                    Name = "Guest on a Quest",
                    Description = "Visit another seminar group."
                },
                new Achievement
                {
                    Id = 11,
                    Name = "Recruiter",
                    Description = "Invited friend visited your seminar group."
                },
                new Achievement
                {
                    Id = 12,
                    Name = "Lucker",
                    Description = "Correctly answers at least 2 test questionnaires on the first attempt."
                },
                new Achievement
                {
                    Id = 13,
                    Name = "Armed & Ready",
                    Description = "Open and correctly answer all test questionnaires."
                },
                new Achievement
                {
                    Id = 14,
                    Name = "Mozart",
                    Description = "In your third homework create additional song for the game and submit it with the homework."
                },
                new Achievement
                {
                    Id = 15,
                    Name = "Shark Expert",
                    Description = "Gain at least 90% points from the fourth homework."
                },
                new Achievement
                {
                    Id = 16,
                    Name = "Fast Logger",
                    Description = "Submit fifth homework at least 2 days before the deadline and get at least 80% points."
                },
                new Achievement
                {
                    Id = 17,
                    Name = "Half-perfectionist",
                    Description = "Get a full score from a homework at least 3 times."
                },
                new Achievement
                {
                    Id = 18,
                    Name = "Bullseye",
                    Description = "Present a complete project."
                },
                new Achievement
                {
                    Id = 19,
                    Name = "Leonardo",
                    Description = "Make a project presentation with nice slides."
                },
                new Achievement
                {
                    Id = 20,
                    Name = "Skiller",
                    Description = "Get 54 points total."
                }
                );
            

            modelBuilder.Entity<Admin>().HasData(
                new Admin()
                {
                    Id = 317634903959142401,
                }
                );
        }
    }
}
