using System;
using Gip.Models.Exceptions;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gip.Models
{
    public partial class CourseMoment
    {
        public CourseMoment(){}

        private string? lessenLijst;

        public int Id { get; set; }

        public string? LessenLijst
        {
            get { return lessenLijst; }
            set
            {
                if (value == null || value.Trim() == "") { }
                else
                {
                    string pattern = @"[\\\/\<\>\;]";
                    if (Regex.IsMatch(value, pattern))
                    {
                        throw new DatabaseException("De lessenlijst bevat een verboden character, gelieve dit niet te doen.");
                    }
                    else
                    {
                        lessenLijst = value;
                    }
                }
            }
        }

        public int? RoomId { get; set; }

        public int? ScheduleId { get; set; }
        public string? ApplicationUserId { get; set; }
        public int? CourseId { get; set; }

        public virtual Room Room { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual ApplicationUser ApplicationUsers { get; set; }
        public virtual Course Courses { get; set; }
    }
}
