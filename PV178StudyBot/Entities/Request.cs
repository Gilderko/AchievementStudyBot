using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PV178StudyBotDAL.Entities
{
    public class Request : BaseEntity, ITextDisplayable
    {
        public ulong StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }

        public int AchievmentId { get; set; }

        [ForeignKey(nameof(AchievmentId))]
        public Achievement RequestedAchievement { get; set; }

        public ulong TeacherId { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public Teacher Teacher { get; set; }

        public override string ToString()
        {
            return $"**{Student.OnRegisterName}** requested: **{RequestedAchievement.Name}**\n" +
                $"**Description**: {new String(RequestedAchievement.Description.Take(50).ToArray())}\n" +
                $"**Reward**: {RequestedAchievement.PointReward} caps";
        }

        public string ToStringShort()
        {
            return $"**{Student.OnRegisterName}** requested: **{RequestedAchievement.Name}**";
        }
    }
}
