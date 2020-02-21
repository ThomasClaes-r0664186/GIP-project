using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Models.Model
{
    public class Course
    {
        private int _credits;
        private string _title;
        private String _code;

        public Course(String title, String code, int cred) {
            Title = title;
            Code = code;
            Credits = cred;
        }

        public int Credits
        {
            get => _credits;
            set 
            {
                if (value < 0)
                {
                    throw new ArgumentException("De code mag niet negatief zijn.");
                }
                else {
                    _credits = value;
                }
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (value == null || value.ToLower().Trim().Length <= 0)
                {
                    throw new ArgumentException("De titel mag niet leeg zijn.");
                }
                else
                {
                    _title = value;
                }
            }
        }

        public string Code
        {
            get => _title;
            set
            {
                if (value == null || value.ToLower().Trim().Length <= 0)
                {
                    throw new ArgumentException("De code mag niet leeg zijn.");
                }
                else
                {
                    _title = value;
                }
            }
        }

    }
}
