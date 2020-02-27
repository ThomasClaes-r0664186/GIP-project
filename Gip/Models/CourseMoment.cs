using System;
using System.Collections.Generic;

namespace Gip.Models
{
    public partial class CourseMoment
    {
        public string Vakcode { get; set; }
        public DateTime Datum { get; set; }
        public DateTime Startmoment { get; set; }
        public string Gebouw { get; set; }
        public int Verdiep { get; set; }
        public string Nummer { get; set; }
        public string Userid { get; set; }
        public string LessenLijst { get; set; }

        public virtual Room Room { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual User User { get; set; }
        public virtual Course VakcodeNavigation { get; set; }
    }
}
