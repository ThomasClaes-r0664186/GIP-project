using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gip.Models.Exceptions;

namespace Gip.Models
{
    public partial class Room
    {
        public Room()
        {
            CourseMoment = new HashSet<CourseMoment>();
        }

       // public string Gebouw { get; set; } //if(value.toLower.trim().length <= 0) throw new exeption, else gebouw = value
       // public int Verdiep { get; set; }
       // public string Nummer { get; set; }
       // public string Type { get; set; }
       // public int Capaciteit { get; set; }
        
        private String middelen;
        public Utils Middelen
        {
            get
            {

                string[] items = middelen.Split('/');

                bool Projectorsetup =false;
                bool Schermen = false;
                bool Scherm = false;
                bool Wifi = false;

                foreach (string item in items)
                {
                    if (!Projectorsetup)
                        Projectorsetup = item.Contains("Projectorsetup");
                    if (!Schermen)
                        Schermen = item.Contains("Schermen");
                    if (!Scherm)
                        Schermen = item.Contains("Scherm");
                    if (!Wifi)
                        Wifi = item.Contains("wifi");
                }
                Utils u = new Utils(Projectorsetup, Scherm, Schermen, Wifi);
                return u;
            }
            set
            {
                string middelen1 = string.Empty;
                
                Utils utils = (Utils)value;

                if (utils.Projectorsetup)                
                    middelen1 += "Projectorsetup/";
                if (utils.Scherm)
                    middelen1 += "Scherm/";
                if (utils.Schermen)
                    middelen1 += "Schermen/";
                if (utils.Wifi)
                    middelen1 += "Wifi/";
                middelen1 = middelen1.Substring(0, middelen1.Length - 1);
            }
            
        } //gooit nog geen exception

        private string gebouw;
        public string Gebouw 
        {
            get { return gebouw; }

            set 
            {
                if (value.Trim().Length <= 0 || value.Trim().Length > 3)
                {
                    throw new RoomException("The building you wish selected does not exist!" + Environment.NewLine + "Please try again.");
                }
                else
                {
                    gebouw = value;
                }
            }
        }

        private int verdiep;
        public int Verdiep
        {
            get { return verdiep; }
            set
            {
                if (value < 0 || value > 3)
                {
                    throw new RoomException("The floor you wish to select does not exist!" + Environment.NewLine + "Please try again.");
                }
                else
                {
                    verdiep = value;
                }
            }
        }

        private string nummer;
        public string Nummer
        {
            get { return nummer; }
            set
            {
                if (value.Trim().Length > 3 || value.Trim().Length < 0)
                {
                    throw new RoomException("The number you wish to select does not exist!" + Environment.NewLine + "Please try again.");
                }
                else
                {
                    nummer = value;
                }
            }
        }

        private string type;
        public string Type
        {
            get { return type; }
            set
            {
                if (value == "Computerlokaal" || value == "Aula" || value == "Vergaderlokaal")
                {
                    type = value;
                }
                else
                {
                    throw new RoomException("the type of room you wish to select does not exist!" + Environment.NewLine + "Please try again.");
                }
            }
        }

        private int capaciteit;

        public int Capaciteit   
        {
            get { return capaciteit; }
            set 
            {
                if (value > 400)
                {
                    throw new RoomException("The chosen capacity was to high!" + Environment.NewLine + "Please try again.");
                }
                else if (value < 0)
                {
                    throw new RoomException("The chosen capacity was to low!" + Environment.NewLine + "Please try again.");
                }
                else
                {
                    capaciteit = value;
                }
            }
        }

        public virtual ICollection<CourseMoment> CourseMoment { get; set; }
    }

    /*
       public class Items
    {
        public bool Projectorsetup;
        public bool Scherm;
        public bool Schermen;
        public bool Wifi;

        public Items(bool projectorsetup,bool scherm,bool schermen,bool wifi)
        {
            Projectorsetup = projectorsetup;
            Schermen = schermen;
            Scherm = scherm;
            Wifi = wifi;
        }
    }
     */

}
