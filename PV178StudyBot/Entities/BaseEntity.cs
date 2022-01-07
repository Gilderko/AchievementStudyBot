using System.ComponentModel.DataAnnotations;

namespace PV178StudyBotDAL.Entities
{
    public abstract class BaseEntity : IEntity
    {
        // The name of property Id is hardcoded into QueryBase
        [Key]
        public ulong Id { get; set; }
    }
}
