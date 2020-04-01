using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Models.ViewModels
{
    public class EditStudInCmViewModel
    {
        public string userId { get; set; }
        public string RNum { get; set; }
        public string Naam { get; set; }
        public string VoorNaam { get; set; }
        public bool IsSelected { get; set; }
    }
}
