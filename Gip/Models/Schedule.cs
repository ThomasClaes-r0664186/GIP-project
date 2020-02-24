using System;
using System.Collections.Generic;

namespace Gip.Models
{
    public partial class Schedule
    {
        public Schedule()
        {
            CourseMoment = new HashSet<CourseMoment>();
        }

        public DateTime Datum { get; set; }
        public DateTime Startmoment { get; set; }
        public DateTime Eindmoment { get; set; }

        public virtual ICollection<CourseMoment> CourseMoment { get; set; }
    }
}
