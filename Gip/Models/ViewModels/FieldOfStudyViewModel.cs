using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Models.ViewModels
{
    public class FieldOfStudyViewModel
    {
        public int Id { get; set; }
        public string RichtingCode { get; set; }
        public string RichtingTitel { get; set; }
        public string Type { get; set; }
        public int RichtingStudiepunten { get; set; }
    }
}
