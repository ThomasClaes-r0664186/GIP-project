using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Models.ViewModels
{
    public class CourseUsersViewModel
    {
        public int cId { get; set; }
        public string Vakcode {get; set;}
        public string Titel { get; set; }
        public List<ApplicationUser> users { get; set; }
    }
}
