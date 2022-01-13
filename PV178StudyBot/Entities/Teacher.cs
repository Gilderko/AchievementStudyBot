using System.Collections.Generic;

namespace PV178StudyBotDAL.Entities
{
    public class Teacher : BaseUlongEntity
    {
        public string RoleName { get; set; }

        public ulong RoleId { get; set; }

        public ICollection<Student> MyStudents { get; set; }

        public ICollection<Request> UnresolvedRequests { get; set; }
    }
}
