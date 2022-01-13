using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PV178StudyBotDAL.Entities.ConnectionTables
{
    public class StudentAndAchievement
    {
        public ulong StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }

        public ulong AchievementId { get; set; }

        [ForeignKey(nameof(AchievementId))]
        public Achievement Achievement { get; set; }

        [Column(TypeName = "Date")]
        public DateTime ReceivedWhen { get; set; }
    }
}
