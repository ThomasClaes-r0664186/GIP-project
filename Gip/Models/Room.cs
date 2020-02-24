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

        public string Gebouw { get; set; } //if(value.toLower.trim().length <= 0) throw new exeption, else gebouw = value
        public int Verdiep { get; set; }
        public string Nummer { get; set; }
        public string Type { get; set; }
        public int Capaciteit { get; set; }
       // public string Middelen { get; set; }
        //get splitst middelen op en returned utils object -->(nakijken in die string of er projectorsetup,... return true else false)
        //opzoeken hoe je string met / 

        private String middelen;



        public String Middelen
        {
            get
            {
                return middelen;

                /*  bool Projectorsetup;
                bool Schermen;
                bool Wifi;
                if (middelen =="/Projectorsetup") //
                {
                    Projectorsetup = true;
                }*/
            }
            set
            { 
                middelen = value;
            }
            
        }


        public virtual ICollection<CourseMoment> CourseMoment { get; set; }
    }
}
