using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV178StudyBotDAL.Entities
{
    public class Request : BaseEntity
    {
        public ulong StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }

        public ulong AchievmentId { get; set; }

        [ForeignKey(nameof(AchievmentId))]
        public Achievement RequestedAchievement { get; set; }

        public ulong TeacherId { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public Teacher Teacher { get; set; }
    }
}
