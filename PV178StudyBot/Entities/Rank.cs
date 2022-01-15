namespace PV178StudyBotDAL.Entities
{
    public class Rank : BaseUlongEntity
    {
        public int PointsRequired { get; set; }

        public string Description { get; set; }

        public string AwardedTitle { get; set; }

        public float ColorR { get; set; }

        public float ColorG { get; set; }

        public float ColorB { get; set; }
    }
}
