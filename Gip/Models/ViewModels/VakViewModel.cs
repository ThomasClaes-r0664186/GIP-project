using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Models.ViewModels
{
    public class VakViewModel
    {
        public int courseId { get; set; }
        public string Vakcode { get; set; }
        public string Titel { get; set; }
        public int Studiepunten { get; set; }
        public int Ingeschreven { get; set; }
        public string afwijzingBeschrijving { get; set; }
    }
}
