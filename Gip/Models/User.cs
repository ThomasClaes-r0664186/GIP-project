using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Gip.Models.Exceptions;
namespace Gip.Models
{
    public partial class User
    {
        public User()
        {
            CourseMoment = new HashSet<CourseMoment>();
            CourseUser = new HashSet<CourseUser>();
        }
        public User(string naam, string mail, string userid)
        {
            this.Naam = naam;
            this.Mail = mail;
            this.Userid = userid;
        }
        //public string Userid { get; set; }
        //public string Naam { get; set; }
        //public string Mail { get; set; }

        private string naam;
        public string Naam
        {
            get { return naam; }
            set 
            {
                if (value == "")
                {
                    throw new DatabaseException("The chosen name is empty!" + Environment.NewLine + "Please try again.");

                }
                else
                {
                    string pattern = @"^[a-zA-Z&àáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ]+$";
                    if (Regex.IsMatch(value, pattern))
                    {
                        naam = value;
                    }
                    else
                    {
                        throw new DatabaseException("The chosen name is invalid!" + Environment.NewLine + "Please do not include anny special caracters and try again.");

                    }
                }
            }
        }

        private string mail;

        public string Mail
        {
            get { return mail; }
            set 
            {
                if (value.Trim() == "")
                {
                    throw new DatabaseException("The chosen Email address is empty!" + Environment.NewLine + "Please try again.");
                }
                else
                {
                    string pattern = @"^([a-zA-Z0-9_-.]+)@(([[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.)|(([a-zA-Z0-9-]+.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(]?)$";
                    if (Regex.IsMatch(value, pattern))
                    {
                            mail = value;
                    }
                    else
                    {
                        throw new DatabaseException("The chosen Email address is invalid!" + Environment.NewLine + "Please do not include special caracters and try again.");

                    }

                }
            }
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


        public virtual ICollection<CourseMoment> CourseMoment { get; set; }
        public virtual ICollection<CourseUser> CourseUser { get; set; }
    }
}
