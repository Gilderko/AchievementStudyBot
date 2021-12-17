using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
