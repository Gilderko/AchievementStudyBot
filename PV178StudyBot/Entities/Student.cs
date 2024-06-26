﻿using PV178StudyBotDAL.Entities.ConnectionTables;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PV178StudyBotDAL.Entities
{
    public class Student : BaseUlongEntity
    {
        public string OnRegisterName { get; set; }

        public int AcquiredPoints { get; set; }

        public ulong CurrentRankId { get; set; }

        [ForeignKey(nameof(CurrentRankId))]
        public Rank CurrentRank { get; set; }

        public ulong? MyTeacherId { get; set; }

        [ForeignKey(nameof(MyTeacherId))]
        public Teacher MyTeacher { get; set; }

        // Many to many relationships

        public ICollection<Request> MyRequests { get; set; }

        public ICollection<StudentAndAchievement> ReachedAchievements { get; set; }
    }
}
