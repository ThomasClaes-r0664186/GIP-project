using System;
using System.Collections.Generic;
using Gip.Models.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gip.Models
{
    public partial class Schedule
    {
        public Schedule()
        {
            Coursemoments = new HashSet<CourseMoment>();
        }
        public Schedule(DateTime datum, DateTime startmoment, DateTime eindmoment)
        {
            this.Datum = datum;
            this.Startmoment = startmoment;
            this.Eindmoment = eindmoment;
            Coursemoments = new HashSet<CourseMoment>();
        }

        public int Id { get; set; }

        private DateTime datum;
        private DateTime startmoment;
        private DateTime eindmoment;

        public virtual ICollection<CourseMoment> Coursemoments { get; set; }

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
                if (value.Hour < 6)
                {
                    throw new DatabaseException("Uw beginmoment is te vroeg, de school is nog niet open.");
                }
                else if (value.Hour > 22)
                {
                    throw new DatabaseException("Uw beginmoment is te laat, de school is dan reeds gesloten.");
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
