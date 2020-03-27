using System;
using Gip.Models.Exceptions;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gip.Models
{
    public partial class CourseMoment
    {
        public CourseMoment()
        {

        }
        public CourseMoment(string vakcode, DateTime datum, DateTime startmoment, string gebouw, int verdiep, string nummer, string userid, string lessenLijst, DateTime eindmoment)
        {
            this.Vakcode = vakcode;
            this.Datum = datum;
            this.Startmoment = startmoment;
            this.Gebouw = gebouw;
            this.Verdiep = verdiep;
            this.Nummer = nummer;
            this.Userid = userid;
            this.LessenLijst = lessenLijst;
            this.Eindmoment = eindmoment;
        }

        private string vakcode;
        private DateTime datum;
        private string gebouw;
        private int verdiep;
        private string nummer;
        private string userid;
        private DateTime startmoment;
        [Column(Order = 7), Key]
        public DateTime Eindmoment { get; set; }
        private string lessenLijst;


        [ForeignKey("Gebouw, Verdiep, Nummer")]
        public virtual Room Room { get; set; }
        [ForeignKey("Datum, Startmoment, Eindmoment")]
        public virtual Schedule Schedule { get; set; }
        public virtual User User { get; set; }
        public virtual Course VakcodeNavigation { get; set; }


        [Column(Order = 5), Key, ForeignKey("User")]
        public string Userid
        {
            get { return userid; }
            set
            {
                if (value == "")
                {
                    throw new DatabaseException("De userid mag niet leeg zijn.");

                }
                else
                {
                    string pattern = @"^[cru]\d{7}$";

                    if (Regex.IsMatch(value, pattern))
                    {
                        userid = value;
                    }
                    else
                    {
                        throw new DatabaseException("U heeft een verboden character ingegeven, gelieve dit niet te doen.");

                    }
                }
            }
        }

        [Column(Order = 0), Key, ForeignKey("VakcodeNavigation")]
        public string Vakcode
        {
            get { return vakcode; }
            set
            {
                if (value.Trim() == "")
                {
                    throw new DatabaseException("U heeft een lege vakcode meegegeven.");
                }
                else
                {
                                         
                    string pattern = @"^[a-zA-Z]{0,3}\d\d[a-zA-Z]$";
                    if (Regex.IsMatch(value, pattern))
                    {
                        vakcode = value;
                    }
                    else
                    {
                        throw new DatabaseException("je hebt een foutief formaat van vakcode of een ongeldig character ingegeven. Gelieve een vakcode van het formaat AAA11A in te geven");
                    }
                }
            }
        }

        [Column(Order = 3), Key]
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

        [Column(Order = 4), Key]
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
                    if (!Regex.IsMatch(value, pattern))
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

        [Column(Order = 2), Key]
        public string Gebouw
        {
            get { return gebouw; }

            set
            {
                if (value.Trim().Length <= 0 || value.Trim().Length > 1)
                {

                    throw new DatabaseException("U heeft niets meegegeven als gebouwcharacter.");
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

        public string? LessenLijst
        {
            get { return lessenLijst; }
            set
            {
                if (value == null || value.Trim() == "") { }
                else
                {
                    string pattern = @"[\\\/\<\>\;]";
                    if (Regex.IsMatch(value, pattern))
                    {
                        throw new DatabaseException("De lessenlijst bevat een verboden character, gelieve dit niet te doen.");
                    }
                    else
                    {
                        lessenLijst = value;
                    }
                }
            }
        }

        [Column(Order = 1), Key]
        public DateTime Datum
        {
            get { return datum; }
            set
            {

                if (value.Year > DateTime.Now.Year + 1)
                {
                    throw new DatabaseException("De gekozen datum is te ver in de toekomst.");
                }
                else
                {
                    if (value.DayOfWeek > DayOfWeek.Saturday && value.DayOfWeek > DayOfWeek.Sunday)
                    {
                        throw new DatabaseException("De school is gesloten in het weekend.");
                    }
                    else
                    {
                        datum = value;
                    }
                }
            }
        }

        [Column(Order = 6), Key]
        public DateTime Startmoment
        {
            get { return startmoment; }
            set
            {
                if (value.Hour < 6 || value.Hour > 22)
                {
                    throw new DatabaseException("De school is enkel open tussen 6:00 en 22:00");
                }
                else
                {
                    startmoment = value;
                }

            }
        }
    }
}
