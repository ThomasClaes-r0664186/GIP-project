using System;
using System.Collections.Generic;
using Gip.Models.Exceptions;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gip.Models
{
    public partial class CourseUser
    {
        public int Id { get; set; }
        public bool? GoedGekeurd { get; set; }
        public string? ApplicationUserId { get; set; }
        public int? CourseId { get; set; }
        public string? AfwijzingBeschr { get; set; }

        public virtual ApplicationUser ApplicationUsers { get; set; }
        public virtual Course Courses { get; set; }
    }
}
