ALTER DATABASE CHARACTER SET utf8mb4;
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Admins` (
    `Id` bigint unsigned NOT NULL AUTO_INCREMENT,
    CONSTRAINT `PK_Admins` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `Achievements` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    `Description` longtext CHARACTER SET utf8mb4 NULL,
    `PointReward` int NOT NULL,
    `ImagePath` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Achievements` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `Ranks` (
    `Id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `PointsRequired` int NOT NULL,
    `Description` longtext CHARACTER SET utf8mb4 NULL,
    `AwardedTitle` longtext CHARACTER SET utf8mb4 NULL,
    `ColorR` float NOT NULL,
    `ColorG` float NOT NULL,
    `ColorB` float NOT NULL,
    CONSTRAINT `PK_Ranks` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `Teachers` (
    `Id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `RoleId` bigint unsigned NOT NULL,
    CONSTRAINT `PK_Teachers` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `Students` (
    `Id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `OnRegisterName` longtext CHARACTER SET utf8mb4 NULL,
    `AcquiredPoints` int NOT NULL,
    `CurrentRankId` bigint unsigned NOT NULL,
    `MyTeacherId` bigint unsigned NULL,
    CONSTRAINT `PK_Students` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Students_Ranks_CurrentRankId` FOREIGN KEY (`CurrentRankId`) REFERENCES `Ranks` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Students_Teachers_MyTeacherId` FOREIGN KEY (`MyTeacherId`) REFERENCES `Teachers` (`Id`) ON DELETE RESTRICT
) CHARACTER SET utf8mb4;

CREATE TABLE `Requests` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `StudentId` bigint unsigned NOT NULL,
    `AchievmentId` int NOT NULL,
    `TeacherId` bigint unsigned NOT NULL,
    CONSTRAINT `PK_Requests` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Requests_Achievements_AchievmentId` FOREIGN KEY (`AchievmentId`) REFERENCES `Achievements` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Requests_Students_StudentId` FOREIGN KEY (`StudentId`) REFERENCES `Students` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Requests_Teachers_TeacherId` FOREIGN KEY (`TeacherId`) REFERENCES `Teachers` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

CREATE TABLE `StudentAndAchievements` (
    `StudentId` bigint unsigned NOT NULL,
    `AchievementId` int NOT NULL,
    `ReceivedWhen` Date NOT NULL,
    CONSTRAINT `PK_StudentAndAchievements` PRIMARY KEY (`AchievementId`, `StudentId`),
    CONSTRAINT `FK_StudentAndAchievements_Achievements_AchievementId` FOREIGN KEY (`AchievementId`) REFERENCES `Achievements` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_StudentAndAchievements_Students_StudentId` FOREIGN KEY (`StudentId`) REFERENCES `Students` (`Id`) ON DELETE CASCADE
) CHARACTER SET utf8mb4;

INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (1, 'Get picked by your seminar tutor.', 'https://www.fi.muni.cz/~xmacak1/badges/Starter.png', 'Choose a starter', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (30, 'Get at least 20 points from the project.', 'https://www.fi.muni.cz/~xmacak1/badges/Skiller.png', 'Skiller', 25);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (29, 'Do not present your project longer than 5 minutes.', 'https://www.fi.muni.cz/~xmacak1/badges/General2.png', 'Mozar', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (28, 'Use a technology that was not taught in this course in your project.', 'https://www.fi.muni.cz/~xmacak1/badges/General.png', 'Leonardo', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (27, 'Miss maximum 1 seminar without a health reason.', 'https://www.fi.muni.cz/~xmacak1/badges/Fanatic.png', 'Fanatic', 35);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (26, 'Do not arrive late to any seminar.', 'https://www.fi.muni.cz/~xmacak1/badges/Nevertoolate.png', 'Never Too Late', 25);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (24, 'Attend the fifth bonus lecture.', 'https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png', 'Guest on a Quest V', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (23, 'Attend the fourth bonus lecture.', 'https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png', 'Guest on a Quest IV', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (22, 'Attend the third bonus lecture.', 'https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png', 'Guest on a Quest III', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (21, 'Attend the second bonus lecture.', 'https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png', 'Guest on a Quest II', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (20, 'Attend the first bonus lecture.', 'https://www.fi.muni.cz/~xmacak1/badges/GuestonQuest.png', 'Guest on a Quest I', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (19, 'Get full points from all test questionnaires.', 'https://www.fi.muni.cz/~xmacak1/badges/ArmedandReady.png', 'Armed & Ready', 40);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (18, 'Submit one homework at least 2 days before the deadline.', 'https://www.fi.muni.cz/~xmacak1/badges/FastExplorer.png', 'Fast Explorer', 20);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (17, 'Finish HW04 on Heroic mode.', 'https://www.fi.muni.cz/~xmacak1/badges/HW04.png', 'Heroic Mode IV', 20);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (31, 'Submit your answers to the course survey.', 'https://www.fi.muni.cz/~xmacak1/badges/Bullseye.png', 'Bullseye', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (16, 'Get at least 9 points from HW04.', 'https://www.fi.muni.cz/~xmacak1/badges/SeeSharp.png', 'See Sharp IV', 35);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (14, 'Get at least 9 points from HW03.', 'https://www.fi.muni.cz/~xmacak1/badges/SeeSharp.png', 'See Sharp III', 30);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (13, 'Finish HW02 on Heroic mode.', 'https://www.fi.muni.cz/~xmacak1/badges/HW02.png', 'Heroic Mode II', 15);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (12, 'Get at least 7 points from HW02.', 'https://www.fi.muni.cz/~xmacak1/badges/SeeSharp.png', 'See Sharp II', 25);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (11, 'Finish HW01 on Heroic mode.', 'https://www.fi.muni.cz/~xmacak1/badges/HW01.png', 'Heroic Mode I', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (10, 'Get at least 7 points from HW01.', 'https://www.fi.muni.cz/~xmacak1/badges/SeeSharp.png', 'See Sharp I', 20);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (9, 'Come up with a new question for the test questionnaire', 'https://www.fi.muni.cz/~xmacak1/badges/Recruiter.png', 'Recruiter', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (8, 'Get full points from four test questionnaires.', 'https://www.fi.muni.cz/~xmacak1/badges/Lucker.png', 'Lucker 2.0', 15);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (7, 'Get full points from one test questionnaire.', 'https://www.fi.muni.cz/~xmacak1/badges/Lucker.png', 'Lucker', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (6, 'Visit three seminars in a row.', 'https://www.fi.muni.cz/~xmacak1/badges/Qualifier.png', 'Qualifier', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (5, 'Write a relevant post in Discord.', 'https://www.fi.muni.cz/~xmacak1/badges/Forum.png', 'Yes, We Have a Forum', 5);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (4, 'Do not arrive late to a seminar.', 'https://www.fi.muni.cz/~xmacak1/badges/Nottoolate.png', 'Not Too Late', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (3, 'First relevant question in seminar.', 'https://www.fi.muni.cz/~xmacak1/badges/Curious.png', 'Curious', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (2, 'First answer to a relevant question in seminar.', 'https://www.fi.muni.cz/~xmacak1/badges/FirstBlood.png', 'First Blood', 10);
INSERT INTO `Achievements` (`Id`, `Description`, `ImagePath`, `Name`, `PointReward`)
VALUES (15, 'Finish HW03 on Heroic mode.', 'https://www.fi.muni.cz/~xmacak1/badges/SharkExpert.png', 'Heroic Mode III', 20);

INSERT INTO `Admins` (`Id`)
VALUES (317634903959142401);

CREATE INDEX `IX_Requests_AchievmentId` ON `Requests` (`AchievmentId`);

CREATE INDEX `IX_Requests_StudentId` ON `Requests` (`StudentId`);

CREATE INDEX `IX_Requests_TeacherId` ON `Requests` (`TeacherId`);

CREATE INDEX `IX_StudentAndAchievements_StudentId` ON `StudentAndAchievements` (`StudentId`);

CREATE INDEX `IX_Students_CurrentRankId` ON `Students` (`CurrentRankId`);

CREATE INDEX `IX_Students_MyTeacherId` ON `Students` (`MyTeacherId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220211161958_Initial', '5.0.8');

COMMIT;