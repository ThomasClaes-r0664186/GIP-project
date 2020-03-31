using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Models
{
    public partial class CourseMomentUsers
    {
        public int Id { get; set; }
        public string? ApplicationUserId { get; set; }
        public int? CoursMomentId { get; set; }

        public virtual ApplicationUser ApplicationUsers { get; set; }
        public virtual CourseMoment Coursemoments { get; set; }
    }
}
