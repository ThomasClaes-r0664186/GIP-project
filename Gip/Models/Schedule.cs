using System;
using System.Collections.Generic;
using Gip.Models.Exceptions;
using System.Text.RegularExpressions;

namespace Gip.Models
{
    public partial class Schedule
    {
        public Schedule()
        {
            CourseMoment = new HashSet<CourseMoment>();
        }
        public Schedule(DateTime datum, DateTime startmoment, DateTime eindmoment)
        {
            this.Datum = datum;
            this.Startmoment = startmoment;
            this.Eindmoment = eindmoment;
            CourseMoment = new HashSet<CourseMoment>();

        }

        private DateTime datum;
        public DateTime Datum
        {
            get { return datum; }
            set
            {

                if (value.Year > DateTime.Now.Year + 1)
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
                    throw new DatabaseException("The chosen starting time is invalid!" + Environment.NewLine + "Please select a different starting time.");
                }
                else
                {
                    startmoment = value;
                }

            }
        }

        private DateTime eindmoment;
        public DateTime Eindmoment
        {
            get { return eindmoment; }
            set 
            {
                if (value.Hour > 22)
                {
                    throw new DatabaseException("The ending time is invalid!" + Environment.NewLine + "Please select an earlier time.");

                }
                else
                {
                    eindmoment = value;
                }
            }
        }


        public virtual ICollection<CourseMoment> CourseMoment { get; set; }
    }
}
