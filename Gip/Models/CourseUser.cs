using System;
using System.Collections.Generic;
using Gip.Models.Exceptions;
using System.Text.RegularExpressions;
namespace Gip.Models
{
    public partial class CourseUser
    {
        //public string Vakcode { get; set; }
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

        public CourseUser(string userid, string vakcode)
        {
            this.Userid = userid;
            this.Vakcode = vakcode; 
        }

        public virtual User User { get; set; }
        public virtual Course VakcodeNavigation { get; set; }
    }
}
