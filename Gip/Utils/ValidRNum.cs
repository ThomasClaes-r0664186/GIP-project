using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gip.Utils
{
    public class ValidRNum : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || value.ToString() == "")
            {
                return false;
            }
            else
            {
                string pattern = @"^[CRUSMXcrusmx]\d{7}$";
                if (Regex.IsMatch(value.ToString(), pattern))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
