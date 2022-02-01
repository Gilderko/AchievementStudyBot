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
                    Name = "Choose a starter",
                    Description = "Get picked by your seminar tutor.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/Starter.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 2,
                    Name = "First Blood",
                    Description = "First answer to a relevant question in seminar.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/FirstBlood.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 3,
                    Name = "Curious",
                    Description = "First relevant question in seminar.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/Curious.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 4,
                    Name = "Not Too Late",
                    Description = "Do not arrive late to a seminar.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/Nottoolate.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 5,
                    Name = "Yes, We Have a Forum",
                    Description = "Write a relevant post in Discord.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/Forum.png",
                    PointReward = 5
                },
                new Achievement
                {
                    Id = 6,
                    Name = "Qualifier",
                    Description = "Visit the third seminar.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/Qualifier.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 7,
                    Name = "Lucker",
                    Description = "Get full points from one test questionnaire.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/Lucker.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 8,
                    Name = "Lucker 2.0",
                    Description = "Get full points from four test questionnaires.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/Lucker.png",
                    PointReward = 15
                },
                new Achievement
                {
                    Id = 9,
                    Name = "Recruiter",
                    Description = "Come up with a new question for the test questionnaire",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/Recruiter.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 10,
                    Name = "See Sharp I",
                    Description = "Get at least 7 points from HW01.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/SeeSharp.png",
                    PointReward = 20
                },
                new Achievement
                {
                    Id = 11,
                    Name = "Heroic Mode I",
                    Description = "Finish HW01 on Heroic mode.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/HW01.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 12,
                    Name = "See Sharp II",
                    Description = "Get at least 7 points from HW02.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/SeeSharp.png",
                    PointReward = 25
                },
                new Achievement
                {
                    Id = 13,
                    Name = "Heroic Mode II",
                    Description = "Finish HW02 on Heroic mode.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/HW02.png",
                    PointReward = 15
                },
                new Achievement
                {
                    Id = 14,
                    Name = "See Sharp III",
                    Description = "Get at least 9 points from HW03.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/SeeSharp.png",
                    PointReward = 30
                },
                new Achievement
                {
                    Id = 15,
                    Name = "Heroic Mode III",
                    Description = "Finish HW03 on Heroic mode.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/SharkExpert.png",
                    PointReward = 20
                },
                new Achievement
                {
                    Id = 16,
                    Name = "See Sharp IV",
                    Description = "Get at least 9 points from HW04.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/SeeSharp.png",
                    PointReward = 35
                },
                new Achievement
                {
                    Id = 17,
                    Name = "Heroic Mode IV",
                    Description = "Finish HW04 on Heroic mode.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/HW04.png",
                    PointReward = 20
                },
                new Achievement
                {
                    Id = 18,
                    Name = "Fast Explorer",
                    Description = "Submit one homework at least 2 days before the deadline.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/FastExplorer.png",
                    PointReward = 20
                },
                new Achievement
                {
                    Id = 19,
                    Name = "Armed & Ready",
                    Description = "Get full points from all test questionnaires.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/ArmedandReady.png",
                    PointReward = 40
                },
                new Achievement
                {
                    Id = 20,
                    Name = "Guest on a Quest I",
                    Description = "Attend the first bonus lecture.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 21,
                    Name = "Guest on a Quest II",
                    Description = "Attend the second bonus lecture.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 22,
                    Name = "Guest on a Quest III",
                    Description = "Attend the third bonus lecture.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 23,
                    Name = "Guest on a Quest IV",
                    Description = "Attend the fourth bonus lecture.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 24,
                    Name = "Guest on a Quest V",
                    Description = "Attend the fifth bonus lecture.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 25,
                    Name = "Guest on a Quest VI",
                    Description = "Attend the sixth bonus lecture.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 26,
                    Name = "Never Too Late",
                    Description = "Do not arrive late to any seminar.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/Nevertoolate.png",
                    PointReward = 25
                },
                new Achievement
                {
                    Id = 27,
                    Name = "Fanatic",
                    Description = "Miss maximum 1 seminar without a health reason.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/Fanatic.png",
                    PointReward = 35
                },
                new Achievement
                {
                    Id = 28,
                    Name = "Leonardo",
                    Description = "Use a technology that was not taught in this course in your project.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/General.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 29,
                    Name = "Mozar",
                    Description = "Do not present your project longer than 5 minutes.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/General2.png",
                    PointReward = 10
                },
                new Achievement
                {
                    Id = 30,
                    Name = "Skiller",
                    Description = "Get at least 20 points from the project.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/Skiller.png",
                    PointReward = 25
                },
                new Achievement
                {
                    Id = 31,
                    Name = "Bullseye",
                    Description = "Submit your answers to the course survey.",
                    ImagePath = @"https://www.fi.muni.cz/~xmacak1/badges/Bullseye.png",
                    PointReward = 10
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
