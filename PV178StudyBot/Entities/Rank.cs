using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV178StudyBotDAL.Entities
{
    public class Rank : BaseEntity
    {
        public int PointsRequired { get; set; }

        public string Description { get; set; }

        public string AwardedTitle { get; set; }

        public int ColorR { get; set; }

        public int ColorG { get; set; }

        public int ColorB { get; set; }
    }
}
