using Gip.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Models
{
    public class RegisterViewModel
    {
        [Required]
        [ValidRNum(ErrorMessage = "R nummer moet aan het juist formaat voldoen. Vb: R0000001")]
        //[Remote(action: "RNumInUse", controller: "Account")]
        public string RNum { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string SurName { get; set; }

        [Required]
        [ValidBirthDate(ErrorMessage = "Geboortedatum mag zich niet in de toekomst bevinden.")]
        public DateTime GeboorteDatum { get; set; }

        [Required]
        [EmailAddress]
        //[Remote(action:"IsEmailInUse", controller: "Account")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Your password and confirm password do not match")]
        public string ConfirmPassword { get; set; }

    }
}