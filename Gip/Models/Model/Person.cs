using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Models.Model
{
    public class Person
    {
        private string _name, _firstname, _email, _id;

        public Person(string name, string firstname, string email, string id) {
            Name = name;
            Firstname = firstname;
            Email = email;
            Id = id;
        }

        public string Name
        {
            get => _name;
            set
            {
                if (value == null || value.ToLower().Trim().Length <= 0)
                {
                    throw new ArgumentException("De naam mag niet leeg zijn.");
                }
                else
                {
                    _name = value;
                }
            }
        }

        public string Firstname
        {
            get => _firstname;
            set
            {
                if (value == null || value.ToLower().Trim().Length <= 0)
                {
                    throw new ArgumentException("De voornaam mag niet leeg zijn.");
                }
                else
                {
                    _firstname = value;
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (value == null || value.ToLower().Trim().Length <= 0)
                {
                    throw new ArgumentException("De email mag niet leeg zijn.");
                }
                else
                {
                    _email = value;
                }
            }
        }

        //student/prof nummer
        public string Id
        {
            get => _id;
            set
            {
                if (value == null || value.ToLower().Trim().Length <= 0)
                {
                    throw new ArgumentException("De id mag niet leeg zijn.");
                }
                else
                {
                    _id = value;
                }
            }
        }
    }
}
