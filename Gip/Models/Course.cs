using System;
using System.Collections.Generic;
using Gip.Models.Exceptions;

namespace Gip.Models
{
    public partial class Course
    {
        public Course()
        {
            CourseMoment = new HashSet<CourseMoment>();
            CourseUser = new HashSet<CourseUser>();
        }

        //public string Vakcode { get; set; }
        //public string Titel { get; set; }
        //public int Studiepunten { get; set; }

        private String vakcode;
        public String Vakcode
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
                    vakcode = value;
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
                    throw new DatabaseException("The chosen title is invalid!" + Environment.NewLine + "Please try again!");

                }
                else
                {
                    titel = value;
                }
            }
        }

        private int studiepunten;

        public int Studiepunten
        {
            get { return studiepunten; }
            set 
            {
                if (value <=0)
                {
                    throw new DatabaseException("The chosen amount of credits is invalid!" + Environment.NewLine + "Please try again!");

                }
                else if (value > 120)
                {
                    throw new DatabaseException("The chosen amount of credits is invalid!" + Environment.NewLine + "Please try again!");
                }
                else
                {
                    studiepunten = value;
                }
            }
        }


        public virtual ICollection<CourseMoment> CourseMoment { get; set; }
        public virtual ICollection<CourseUser> CourseUser { get; set; }
    }
}
