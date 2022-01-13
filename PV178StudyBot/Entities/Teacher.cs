using System.Collections.Generic;

namespace PV178StudyBotDAL.Entities
{
    public class Teacher : BaseEntity
    {
        public string RoleName;

        public ulong RoleId;

        public ICollection<Student> MyStudents { get; set; }

        public ICollection<Request> UnresolvedRequests { get; set; }
    }
}
