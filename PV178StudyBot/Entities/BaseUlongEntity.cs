using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PV178StudyBotDAL.Entities
{
    public abstract class BaseUlongEntity : IEntity
    {
        // The name of property Id is hardcoded into QueryBase
        [Key]
        public ulong Id { get; set; }
    }
}
