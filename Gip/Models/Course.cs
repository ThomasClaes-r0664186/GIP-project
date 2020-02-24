using System;
using System.Collections.Generic;

namespace Gip.Models
{
    public partial class Course
    {
        public Course()
        {
            CourseMoment = new HashSet<CourseMoment>();
            CourseUser = new HashSet<CourseUser>();
        }

        public string Vakcode { get; set; }
        public string Titel { get; set; }
        public int Studiepunten { get; set; }

        public virtual ICollection<CourseMoment> CourseMoment { get; set; }
        public virtual ICollection<CourseUser> CourseUser { get; set; }
    }
}
