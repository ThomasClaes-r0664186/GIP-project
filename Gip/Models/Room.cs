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
        public int ProjectorSetup { get; set; }
        public bool Scherm { get; set; }
        public bool Wifi { get; set; }
        // middelen is veranderd naar --> int projectorSetup, bool scherm, bool wifi
        /// <summary>
        /// getter en setters voor alles inplementeren: bijvoorbeeld mail mag niet leeg zijn of moet @ bevatten
        /// exceptions aanmaken om dan te gooien       
        /// </summary>

        public virtual ICollection<CourseMoment> CourseMoment { get; set; }
    }
}
