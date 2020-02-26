﻿using System;
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
        public string Middelen
        {
            get
            {
                return middelen;
            }
            set
            {
                string middelen1 = value.ToLower();
                int aantal = 1;
                if (middelen1.Contains('/'))
                {
                    aantal = middelen1.Split('/').Length;
                }
                if (aantal > 3)
                {
                    throw new DatabaseException("The amount of resources for a single room is to high!" + Environment.NewLine + "Please try again!");
                }
                int gevonden = 0;
                if (!middelen1.Trim().Equals(""))
                {
                    if (middelen1.Contains("projectorsetup"))
                    {
                        gevonden++;
                    }
                    if (middelen1.Contains("schermen"))
                    {
                        gevonden++;
                    }
                    else if (middelen1.Contains("scherm"))
                    {
                        gevonden++;
                    }
                    if (middelen1.Contains("wifi"))
                    {
                        gevonden++;
                    }
                    if (gevonden != aantal)
                    {
                        throw new DatabaseException("The resources you selected are not available!" + Environment.NewLine + "Please try again!");
                    }
                    middelen = value;
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
                    throw new DatabaseException("The building you wish selected does not exist!" + Environment.NewLine + "Please try again.");
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
                    throw new DatabaseException("The floor you wish to select does not exist!" + Environment.NewLine + "Please try again.");
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
                    throw new DatabaseException("The number you wish to select does not exist!" + Environment.NewLine + "Please try again.");
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
                    throw new DatabaseException("the type of room you wish to select does not exist!" + Environment.NewLine + "Please try again.");
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
                    throw new DatabaseException("The chosen capacity was to high!" + Environment.NewLine + "Please try again.");
                }
                else if (value < 0)
                {
                    throw new DatabaseException("The chosen capacity was to low!" + Environment.NewLine + "Please try again.");
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
