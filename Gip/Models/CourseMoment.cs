using System;
using System.Collections.Generic;
using Gip.Models.Exceptions;
using System.Text.RegularExpressions;
namespace Gip.Models
{
    public partial class CourseMoment
    {
<<<<<<< HEAD
        public string Vakcode { get; set; }
        public DateTime Datum { get; set; }
        public DateTime Startmoment { get; set; }
        public string Gebouw { get; set; }
        public int Verdiep { get; set; }
        public string Nummer { get; set; }
        public string Userid { get; set; }
        public string LessenLijst { get; set; }

        public CourseMoment()
        {

        }
        public CourseMoment(string vakcode, DateTime datum, DateTime startmoment, string gebouw, int verdiep, string nummer, string userid, string lessenLijst)
        {
            this.Vakcode = vakcode;
            this.Datum = datum;
            this.Startmoment = startmoment;
            this.Gebouw = gebouw;
            this.Verdiep = verdiep;
            this.Nummer = nummer;
            this.Userid = userid;
            this.LessenLijst = lessenLijst;
        }

        private string userid;
        public string Userid
        {
            get { return userid; }
            set
            {
                if (value == "")
                {
                    throw new DatabaseException("The chosen User identification number is empty!" + Environment.NewLine + "Please try again.");

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
                        throw new DatabaseException("The chosen User identification number is invalid!" + Environment.NewLine + "Please do not include special caracters and try again.");

                    }
                }
            }
        }

        private string vakcode;
        public string Vakcode
        {
            get { return vakcode; }
            set
            {
                if (value.Trim() == "")
                {
                    throw new DatabaseException("The chosen course code is invalid!" + Environment.NewLine + "Please try again!");
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
                        throw new DatabaseException("The chosen course code is invalid!" + Environment.NewLine + "Please do not include any special characters and try again!");
                    }
                }
            }
        }

        private int verdiep;
        public int Verdiep
        {
            get { return verdiep; }
            set
            {
                if (value < 0 || value > 9)
                {
                    throw new DatabaseException("The floor you wish to select does not exist!" + Environment.NewLine + "Please try again.");
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
                        throw new DatabaseException("The floor you wish to select does not exist!" + Environment.NewLine + "Please do not include any special characters and try again.");
                    }
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
                    throw new DatabaseException("The number you wish to select is invalid!" + Environment.NewLine + "Please try again.");
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
                        throw new DatabaseException("The number you wish to select is invalid!" + Environment.NewLine + "Please do not include any special characters and try again.");
                    }
                }
            }
        }

        private string gebouw;
        public string Gebouw
        {
            get { return gebouw; }

            set
            {
                if (value.Trim().Length <= 0 || value.Trim().Length > 1)
                {

                    throw new DatabaseException("The building you wish selected does not exist!" + Environment.NewLine + "Please try again.");
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
                        throw new DatabaseException("The building you wish selected does not exist!" + Environment.NewLine + "Please do not include any special characters and try again.");
                    }
                }
            }
        }

        private string lessenLijst;

        public string LessenLijst
        {
            get { return lessenLijst; }
            set
            {
                if (value == "")
                {
                    throw new DatabaseException("The chosen topic list is empty!" + Environment.NewLine + "Please try again!");
                }
                else
                {
                    string pattern = @"[\\\/\<\>\;]";
                    if (Regex.IsMatch(value, pattern))
                    {
                        throw new DatabaseException("The topic list you wish select is invalid!" + Environment.NewLine + "Please do not include any special characters and try again.");
                    }
                    else
                    {
                        lessenLijst = value;
                    }
                }
            }
        }

        private DateTime datum;

        public DateTime Datum
        {
            get { return datum; }
            set 
            {
              
                if (value.Year > DateTime.Now.Year +1)
                {
                    throw new DatabaseException("The selected date is to far in the future!" + Environment.NewLine + "Please try again.");
                }
                else
                {
                    if (value.DayOfWeek > DayOfWeek.Saturday && value.DayOfWeek > DayOfWeek.Sunday)
                    {
                        throw new DatabaseException("The school is closed on weekends!" + Environment.NewLine + "Please select a different date!");
                    }
                    else
                    {
                        datum = value;
                    }
                }
            }
        }

        private DateTime startmoment;

        public DateTime Startmoment
        {
            get { return startmoment; }
            set 
            {
                if (value.Hour < 6 || value.Hour > 22)
                {
                    throw new DatabaseException("The chosen starting time is invalid!" + Environment.NewLine + "Please select a different starting time!");
                }
                else
                {
                    startmoment = value;
                }
                
            }
        }

        public virtual Room Room { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual User User { get; set; }
        public virtual Course VakcodeNavigation { get; set; }
    }
}
