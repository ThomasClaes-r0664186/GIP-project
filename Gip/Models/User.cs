using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Gip.Models.Exceptions;
using System.ComponentModel.DataAnnotations;
namespace Gip.Models
{
    public partial class User
    {
        public User()
        {
            CourseMoment = new HashSet<CourseMoment>();
            CourseUser = new HashSet<CourseUser>();
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
                string pattern = @"^[a-zA-Z]+$";               
                if (value != "")
                {                    
                    if (Regex.IsMatch(value, pattern))
                    {
                        naam = value;
                    }
                    
                }
                else
                {
                    throw new DatabaseException("The chosen name is invalid!" + Environment.NewLine + "Please do not include special caracters and try again.");
                }
            }
        }

        private string mail;

        public string Mail
        {
            get { return mail; }
            set 
            {
                string pattern = @"^([a-zA-Z0-9_-.]+)@(([[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.)|(([a-zA-Z0-9-]+.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(]?)$";
                if (value != "")
                {
                    if (Regex.IsMatch(value, pattern))
                    {
                        mail = value;
                    }
                }
                else
                {
                    throw new DatabaseException("The chosen Email address is invalid!" + Environment.NewLine + "Please do not include special caracters and try again.");
                }
            }
        }

        private string userid;

        public string Userid
        {
            get { return userid; }
            set 
            {
                string pattern = @"^[a-zA-Z]+$";
                userid = value; 
            }//verder af maken
        }


        public virtual ICollection<CourseMoment> CourseMoment { get; set; }
        public virtual ICollection<CourseUser> CourseUser { get; set; }
    }
}
