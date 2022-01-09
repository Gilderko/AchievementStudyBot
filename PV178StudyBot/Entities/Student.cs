using PV178StudyBotDAL.Entities.ConnectionTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV178StudyBotDAL.Entities
{
    public class Student : BaseEntity
    {
        public Student()
        {
            AcquiredPoints = -1;
        }

        public int AcquiredPoints { get; set; }

        public ulong CurrentRankId { get; set; }
        
        public ulong? MyTeacherId { get; set; }

        [ForeignKey(nameof(MyTeacherId))]
        public Teacher MyTeacher { get; set; }
        
        // Many to many relationships

        public ICollection<StudentAndAchievement> ReachedAchievement { get; set; }
    }
}
