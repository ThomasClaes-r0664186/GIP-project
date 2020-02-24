using System;
using System.Collections.Generic;

namespace Gip.Models
{
    public partial class CourseUser
    {
        public string Vakcode { get; set; }
        public string Userid { get; set; }

        public virtual User User { get; set; }
        public virtual Course VakcodeNavigation { get; set; }
    }
}
