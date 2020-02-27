using System;
using System.Collections.Generic;
using Gip.Models.Exceptions;
using System.Text.RegularExpressions;

namespace Gip.Models
{
    public partial class Course
    {
        public Course()
        {
            CourseMoment = new HashSet<CourseMoment>();
            CourseUser = new HashSet<CourseUser>();
        }
        public Course(string vakcode, string titel, int studiepunten)
        {
            this.Vakcode = vakcode;
            this.Titel = titel;
            this.Studiepunten = studiepunten;
        }
        //public string Vakcode { get; set; }
        //public string Titel { get; set; }
        //public int Studiepunten { get; set; }
        public virtual ICollection<CourseMoment> CourseMoment { get; set; }
        public virtual ICollection<CourseUser> CourseUser { get; set; }

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
                    string pattern = @"[\\\/\<\>\;]";
                    if (Regex.IsMatch(value, pattern))
                    {
                        throw new DatabaseException("The title you wish selected does not exist!" + Environment.NewLine + "Please do not include any special characters and try again.");
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
                if (value <=0)
                {
                    throw new DatabaseException("The chosen amount of credits is invalid!" + Environment.NewLine + "Please try again!");

                }
                else if (value > 60)
                {
                    throw new DatabaseException("The chosen amount of credits is invalid!" + Environment.NewLine + "Please try again!");
                }
                else
                {
                    string pattern = @"^\d$";
                    if (Regex.IsMatch(value.ToString(), pattern))
                    {
                        studiepunten = value;
                    }
                    else
                    {
                        throw new DatabaseException("The chosen amount of credits is invalid!" + Environment.NewLine + "Please do not include any special characters and try again.");
                    }
                }
            }
        }


       
    }
}
