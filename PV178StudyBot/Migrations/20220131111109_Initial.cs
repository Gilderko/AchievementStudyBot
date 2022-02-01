using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PV178StudyBotDAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointReward = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ranks",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    PointsRequired = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwardedTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorR = table.Column<float>(type: "real", nullable: false),
                    ColorG = table.Column<float>(type: "real", nullable: false),
                    ColorB = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    OnRegisterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcquiredPoints = table.Column<int>(type: "int", nullable: false),
                    CurrentRankId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    MyTeacherId = table.Column<decimal>(type: "decimal(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Ranks_CurrentRankId",
                        column: x => x.CurrentRankId,
                        principalTable: "Ranks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Teachers_MyTeacherId",
                        column: x => x.MyTeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    AchievmentId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Achievements_AchievmentId",
                        column: x => x.AchievmentId,
                        principalTable: "Achievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentAndAchievements",
                columns: table => new
                {
                    StudentId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    AchievementId = table.Column<int>(type: "int", nullable: false),
                    ReceivedWhen = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAndAchievements", x => new { x.AchievementId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_StudentAndAchievements_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "Achievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAndAchievements_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Achievements",
                columns: new[] { "Id", "Description", "ImagePath", "Name", "PointReward" },
                values: new object[,]
                {
                    { 1, "Get picked by your seminar tutor.", "https://www.fi.muni.cz/~xmacak1/badges/Starter.png", "Choose a starter", 10 },
                    { 30, "Get at least 20 points from the project.", "https://www.fi.muni.cz/~xmacak1/badges/Skiller.png", "Skiller", 25 },
                    { 29, "Do not present your project longer than 5 minutes.", "https://www.fi.muni.cz/~xmacak1/badges/General2.png", "Mozar", 10 },
                    { 28, "Use a technology that was not taught in this course in your project.", "https://www.fi.muni.cz/~xmacak1/badges/General.png", "Leonardo", 10 },
                    { 27, "Miss maximum 1 seminar without a health reason.", "https://www.fi.muni.cz/~xmacak1/badges/Fanatic.png", "Fanatic", 35 },
                    { 26, "Do not arrive late to any seminar.", "https://www.fi.muni.cz/~xmacak1/badges/Nevertoolate.png", "Never Too Late", 25 },
                    { 25, "Attend the sixth bonus lecture.", "https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png", "Guest on a Quest VI", 10 },
                    { 24, "Attend the fifth bonus lecture.", "https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png", "Guest on a Quest V", 10 },
                    { 23, "Attend the fourth bonus lecture.", "https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png", "Guest on a Quest IV", 10 },
                    { 22, "Attend the third bonus lecture.", "https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png", "Guest on a Quest III", 10 },
                    { 21, "Attend the second bonus lecture.", "https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png", "Guest on a Quest II", 10 },
                    { 20, "Attend the first bonus lecture.", "https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png", "Guest on a Quest I", 10 },
                    { 19, "Get full points from all test questionnaires.", "https://www.fi.muni.cz/~xmacak1/badges/ArmedandReady.png", "Armed & Ready", 40 },
                    { 18, "Submit one homework at least 2 days before the deadline.", "https://www.fi.muni.cz/~xmacak1/badges/FastExplorer.png", "Fast Explorer", 20 },
                    { 17, "Finish HW04 on Heroic mode.", "https://www.fi.muni.cz/~xmacak1/badges/HW04.png", "Heroic Mode IV", 20 },
                    { 16, "Get at least 9 points from HW04.", "https://www.fi.muni.cz/~xmacak1/badges/SeeSharp.png", "See Sharp IV", 35 },
                    { 15, "Finish HW03 on Heroic mode.", "https://www.fi.muni.cz/~xmacak1/badges/SharkExpert.png", "Heroic Mode III", 20 },
                    { 14, "Get at least 9 points from HW03.", "https://www.fi.muni.cz/~xmacak1/badges/SeeSharp.png", "See Sharp III", 30 },
                    { 13, "Finish HW02 on Heroic mode.", "https://www.fi.muni.cz/~xmacak1/badges/HW02.png", "Heroic Mode II", 15 },
                    { 12, "Get at least 7 points from HW02.", "https://www.fi.muni.cz/~xmacak1/badges/SeeSharp.png", "See Sharp II", 25 },
                    { 11, "Finish HW01 on Heroic mode.", "https://www.fi.muni.cz/~xmacak1/badges/HW01.png", "Heroic Mode I", 10 },
                    { 10, "Get at least 7 points from HW01.", "https://www.fi.muni.cz/~xmacak1/badges/SeeSharp.png", "See Sharp I", 20 },
                    { 9, "Come up with a new question for the test questionnaire", "https://www.fi.muni.cz/~xmacak1/badges/Recruiter.png", "Recruiter", 10 },
                    { 8, "Get full points from four test questionnaires.", "https://www.fi.muni.cz/~xmacak1/badges/Lucker.png", "Lucker 2.0", 15 },
                    { 7, "Get full points from one test questionnaire.", "https://www.fi.muni.cz/~xmacak1/badges/Lucker.png", "Lucker", 10 },
                    { 6, "Visit the third seminar.", "https://www.fi.muni.cz/~xmacak1/badges/Qualifier.png", "Qualifier", 10 },
                    { 5, "Write a relevant post in Discord.", "https://www.fi.muni.cz/~xmacak1/badges/Forum.png", "Yes, We Have a Forum", 5 },
                    { 4, "Do not arrive late to a seminar.", "https://www.fi.muni.cz/~xmacak1/badges/Nottoolate.png", "Not Too Late", 10 },
                    { 3, "First relevant question in seminar.", "https://www.fi.muni.cz/~xmacak1/badges/Curious.png", "Curious", 10 },
                    { 2, "First answer to a relevant question in seminar.", "https://www.fi.muni.cz/~xmacak1/badges/FirstBlood.png", "First Blood", 10 },
                    { 31, "Submit your answers to the course survey.", "https://www.fi.muni.cz/~xmacak1/badges/Bullseye.png", "Bullseye", 10 }
                });

            migrationBuilder.InsertData(
                table: "Admins",
                column: "Id",
                value: 317634903959142401m);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AchievmentId",
                table: "Requests",
                column: "AchievmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_StudentId",
                table: "Requests",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_TeacherId",
                table: "Requests",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAndAchievements_StudentId",
                table: "StudentAndAchievements",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CurrentRankId",
                table: "Students",
                column: "CurrentRankId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_MyTeacherId",
                table: "Students",
                column: "MyTeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "StudentAndAchievements");

            migrationBuilder.DropTable(
                name: "Achievements");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Ranks");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
