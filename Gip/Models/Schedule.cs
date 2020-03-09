using System;
using System.Collections.Generic;
using Gip.Models.Exceptions;

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
        private DateTime startmoment;
        private DateTime eindmoment;

        public virtual ICollection<CourseMoment> CourseMoment { get; set; }

        public DateTime Datum
        {
            get { return datum; }
            set
            {

                if (value.Year > DateTime.Now.Year + 1)
                {
                    throw new DatabaseException("De gekozen datum is te ver in de toekomst.");
                }
                else if (value.DayOfWeek == DayOfWeek.Saturday || value.DayOfWeek == DayOfWeek.Sunday)
                {
                    throw new DatabaseException("De school is gesloten in het weekend.");
                }
                else if (value < DateTime.Now.AddDays(-1))
                {
                    throw new DatabaseException("Je kan het moment niet vroeger dan vandaag plannen.");
                }
                else
                {
                    datum = value;
                }
            }
        }

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

        public DateTime Eindmoment
        {
            get { return eindmoment; }
            set
            {
                if (value.Hour == 22 && value.Minute > 0) {
                    throw new DatabaseException("Uw eindmoment is te laat, de school is dan reeds gesloten.");
                }
                else if (value.Hour > 22)
                {
                    throw new DatabaseException("Uw eindmoment is te laat, de school is dan reeds gesloten.");
                }
                else
                {
                    eindmoment = value;
                }
            }
        }
    }
}
