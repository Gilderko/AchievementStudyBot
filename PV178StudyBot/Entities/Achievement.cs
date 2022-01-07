using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV178StudyBotDAL.Entities
{
    public class Achievement : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int PointReward { get; set; }

        public string ImagePath { get; set; }
    }
}
