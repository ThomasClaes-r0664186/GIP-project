using Gip.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gip.Models
{
    public class FieldOfStudy
    {
        public int Id { get; set; }

        private string richtingCode;

        public string RichtingCode
        {
            get { return richtingCode; }
            set
            {
                if (value.Trim() == "")
                {
                    throw new DatabaseException("U heeft een lege richtingcode meegegeven.");
                }
                else
                {
                    string pattern = @"^[a-zA-Z]{3,3}$";
                    if (Regex.IsMatch(value, pattern))
                    {
                        richtingCode = value;
                    }
                    else
                    {
                        throw new DatabaseException("je hebt een foutief formaat van richtingcode of een ongeldig character ingegeven. Gelieve een richtingcode van het formaat AAA in te geven");
                    }
                }
            }
        }

        private string richtingTitel;
        public string RichtingTitel
        {
            get { return richtingTitel; }
            set
            {
                if (value.Trim() == "")
                {
                    throw new DatabaseException("De titel mag niet leeg zijn.");

                }
                else
                {
                    string pattern = @"[\\\/\<\>\;]";
                    if (Regex.IsMatch(value, pattern))
                    {
                        throw new DatabaseException("U heeft een verboden character ingegeven, gelieve dit niet te doen.");
                    }
                    else
                    {
                        richtingTitel = value;
                    }
                }
            }
        }

        private string type;
        public string Type
        {
            get { return type; }
            set
            {
                if (value.Trim() == "")
                {
                    throw new DatabaseException("De titel mag niet leeg zijn.");

                }
                else
                {
                    string pattern = @"[\\\/\<\>\;]";
                    if (Regex.IsMatch(value, pattern))
                    {
                        throw new DatabaseException("U heeft een verboden character ingegeven, gelieve dit niet te doen.");
                    }
                    else
                    {
                        type = value;
                    }
                }
            }
        }

        private int richtingStudiepunten;

        public int RichtingStudiepunten
        {
            get { return richtingStudiepunten; }
            set
            {
                if (value < 0)
                {
                    throw new DatabaseException("Het aantal studiepunten mag niet negatief zijn.");

                }
                else if (value > 120)
                {
                    throw new DatabaseException("Het aantal studiepunten mag niet hoger zijn dan 120.");
                }
                else
                {
                    string pattern = @"^[0-9]{0,3}\d$";
                    if (Regex.IsMatch(value.ToString(), pattern))
                    {
                        richtingStudiepunten = value;
                    }
                    else
                    {
                        throw new DatabaseException("U heeft een verboden character ingegeven, gelieve dit niet te doen.");
                    }
                }
            }
        }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
