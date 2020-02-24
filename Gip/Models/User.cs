using System;
using System.Collections.Generic;

namespace Gip.Models
{
    public partial class User
    {
        public User()
        {
            CourseMoment = new HashSet<CourseMoment>();
            CourseUser = new HashSet<CourseUser>();
        }

        public string Userid { get; set; }
        public string Naam { get; set; }
        public string Mail { get; set; }

        public virtual ICollection<CourseMoment> CourseMoment { get; set; }
        public virtual ICollection<CourseUser> CourseUser { get; set; }
    }
}
