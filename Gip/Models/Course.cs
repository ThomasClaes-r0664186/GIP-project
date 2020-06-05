using System.Collections.Generic;
using Gip.Models.Exceptions;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gip.Models
{
    public partial class Course
    {
        public Course()
        {
            CourseMoments = new HashSet<CourseMoment>();
            CourseUsers = new HashSet<CourseUser>();
        }
        public Course(string vakcode, string titel, int studiepunten)
        {
            CourseMoments = new HashSet<CourseMoment>();
            CourseUsers = new HashSet<CourseUser>();
            this.Vakcode = vakcode;
            this.Titel = titel;
            this.Studiepunten = studiepunten;
        }

        public int Id { get; set; }

        private string vakcode;

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

        private string titel;
        public string Titel
        {
            get { return titel; }
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
                        titel = value;
                    }
                }
            }
        }

        private int studiepunten;

        public int Studiepunten
        {
            get { return studiepunten; }
            set
            {
                if (value <= 0)
                {
                    throw new DatabaseException("Het aantal studiepunten mag niet negatief zijn.");

                }
                else if (value > 60)
                {
                    throw new DatabaseException("Het aantal studiepunten mag niet hoger zijn dan 60.");
                }
                else
                {
                    string pattern = @"^[1-9]{0,1}\d$";
                    if (Regex.IsMatch(value.ToString(), pattern))
                    {
                        studiepunten = value;
                    }
                    else
                    {
                        throw new DatabaseException("U heeft een verboden character ingegeven, gelieve dit niet te doen.");
                    }
                }
            }
        }

        public int? FieldOfStudyId { get; set; }

        public virtual ICollection<CourseMoment> CourseMoments { get; set; }
        public virtual ICollection<CourseUser> CourseUsers { get; set; }
        public virtual FieldOfStudy FieldOfStudy { get; set; }
    }
}
