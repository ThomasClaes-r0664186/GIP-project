using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Gip.Models.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Gip.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        private string naam;
        private string voorNaam;
        private DateTime geboorteDatum;

        public virtual ICollection<CourseUser> CourseUsers { get; set; }
        public virtual ICollection<CourseMoment> Coursemoments { get; set; }

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
                        throw new DatabaseException("U heeft verboden characters ingegeven voor de naam, gelieve dit niet te doen.");

                    }
                }
            }
        }

        public string VoorNaam
        {
            get { return voorNaam; }
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
                        voorNaam = value;
                    }
                    else
                    {
                        throw new DatabaseException("U heeft verboden characters ingegeven voor de naam, gelieve dit niet te doen.");

                    }
                }
            }
        }

        public DateTime GeboorteDatum 
        {
            get { return geboorteDatum; }
            set {
                if (value.Year < 1920) { throw new DatabaseException("De persoon mag niet ouder zijn dan 100jaar."); }
                else 
                {
                    geboorteDatum = value;
                }
            }
        }
    }
}
