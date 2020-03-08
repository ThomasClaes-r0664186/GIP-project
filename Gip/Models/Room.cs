using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gip.Models.Exceptions;
using System.Text.RegularExpressions;

namespace Gip.Models
{
    public partial class Room
    {
        private string gebouw;
        private int verdiep;
        private string nummer;
        private string type;
        private int capaciteit;
        private string middelen;

        public virtual ICollection<CourseMoment> CourseMoment { get; set; }

        public Room(string middelen, string gebouw, int verdiep, string nummer, string type, int capaciteit)
        {
            this.Capaciteit = capaciteit;
            this.Gebouw = gebouw;
            this.Verdiep = verdiep;
            this.Nummer = nummer;
            this.Middelen = middelen;
            this.Type = type;
        }
        public Room()
        {
            CourseMoment = new HashSet<CourseMoment>();
        }

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
                    if (middelen1.Contains("geen middelen"))
                    {
                        gevonden++;
                    }
                    if (gevonden != aantal)
                    {
                        throw new DatabaseException("The resources you selected are not available!" + Environment.NewLine + "Please try again!");
                    }
                    else
                    {
                        middelen = value.Trim();
                    }
                }
                else
                {
                    middelen = "Geen middelen";
                }

            }
        }

        public string Gebouw
        {
            get { return gebouw; }

            set
            {
                if (value.Trim().Length <= 0)
                {

                    throw new DatabaseException("U heeft niets meegegeven als gebouwcharacter.");
                }
                if(value.Trim().Length > 1)
                {
                    throw new DatabaseException("U heeft meer als 1 character meegegeven voor gebouw, gelieve u te beperken to 1 letter.");
                }
                else
                {
                    string pattern = @"^[a-zA-Z]$";
                    if (Regex.IsMatch(value, pattern))
                    {
                        gebouw = value;
                    }
                    else
                    {
                        throw new DatabaseException("Dit gebouw bestaat niet of u heeft een verboden character ingegeven.");
                    }
                }
            }
        }

        public int Verdiep
        {
            get { return verdiep; }
            set
            {
                if (value < 0 || value > 9)
                {
                    throw new DatabaseException("Het verdiep mag niet negatief zijn noch boven 9.");
                }
                else
                {
                    string pattern = @"^\d$";
                    if (Regex.IsMatch(value.ToString(), pattern))
                    {
                        verdiep = value;
                    }
                    else
                    {
                        throw new DatabaseException("U heeft een verboden character ingegeven, gelieve dit niet te doen.");
                    }
                }
            }
        }

        public string Nummer
        {
            get { return nummer; }
            set
            {
                if (value.Trim().Length > 3 || value.Trim().Length < 0)
                {
                    throw new DatabaseException("Het nummer mag niet langer zijn dan 3 characters of u heeft een leeg nummer meegegeven.");
                }
                else
                {
                    string pattern = @"^\d\d[a-zA-Z]{0,1}$";
                    if (Regex.IsMatch(value, pattern))
                    {
                        nummer = value;
                    }
                    else
                    {
                        throw new DatabaseException("U heeft een verboden character ingegeven, gelieve dit niet te doen.");
                    }
                }
            }
        }

        public string Type //is dropdown
        {
            get { return type; }
            set
            {
                if (value == "Computerlokaal" || value == "Aula" || value == "Vergaderlokaal" || value == "Lokaal")
                {
                    type = value;
                }
                else
                {
                    throw new DatabaseException("the type of room you wish to select does not exist!" + Environment.NewLine + "Please try again.");
                }
            }
        }

        public int Capaciteit
        {
            get { return capaciteit; }
            set
            {
                if (value > 400)
                {
                    throw new DatabaseException("De capaciteit mag niet hoger zijn dan 400.");
                }
                else if (value < 0)
                {
                    throw new DatabaseException("De capaciteit mag niet negatief zijn.");
                }
                else
                {
                    string pattern = @"^\d$";
                    if (!Regex.IsMatch(value + "", pattern))
                    {
                        capaciteit = value;
                    }
                    else
                    {
                        throw new DatabaseException("U heeft een verboden character ingegeven, gelieve dit niet te doen.");
                    }
                }
            }
        }


    }
}
