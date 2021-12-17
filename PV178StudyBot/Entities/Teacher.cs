using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV178StudyBotDAL.Entities
{
    public class Teacher : BaseEntity
    {
        public ICollection<Student> MyStudents { get; set; }

        public ICollection<Request> UnresolvedRequests { get; set; }
    }
}
