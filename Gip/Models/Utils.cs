using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Models
{
    public class Utils
    {
        public bool Projectorsetup { get; set; }
        public bool Scherm { get; set; }
        public bool Schermen { get; set; }
        public bool Wifi { get; set; }

        public Utils(bool projectorsetup, bool scherm, bool schermen, bool wifi)
        {
            this.Scherm = scherm;
            this.Schermen = Schermen;
            this.Projectorsetup = projectorsetup;
            this.Wifi = wifi;
        }
    }
}
