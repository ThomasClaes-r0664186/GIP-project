using System;
using System.Collections.Generic;
using Gip.Models.Exceptions;
using System.Text.RegularExpressions;

namespace Gip.Models
{
    public partial class CourseUser
    {
        private string vakcode;
        private string userid;

        public virtual User User { get; set; }
        public virtual Course VakcodeNavigation { get; set; }

        public string Userid
        {
            get { return userid; }
            set
            {
                if (value == "")
                {
                    throw new DatabaseException("De user id mag niet leeg zijn.");

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
                        throw new DatabaseException("U heeft een verkeerd formaat ingegeven. gelieve een C, R of U in te geven met erachter 7 cijfers.");

                    }
                }
            }
        }

        public string Vakcode
        {
            get { return vakcode; }
            set
            {
                if (value.Trim() == "")
                {
                    throw new DatabaseException("Vakcode mag niet leeg zijn.");
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
                        throw new DatabaseException("U heeft een verboden character ingegeven of de vakcode is van een verkeerd formaat.");
                    }
                }
            }
        }
    }
}
