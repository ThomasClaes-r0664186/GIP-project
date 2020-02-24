using System;
using System.Collections.Generic;

namespace Gip.Models
{
    public partial class Room
    {
        public Room()
        {
            CourseMoment = new HashSet<CourseMoment>();
        }

        public string Gebouw { get; set; }
        public int Verdiep { get; set; }
        public string Nummer { get; set; }
        public string Type { get; set; }
        public int Capaciteit { get; set; }
        public string Middelen { get; set; }

        public virtual ICollection<CourseMoment> CourseMoment { get; set; }
    }
}
