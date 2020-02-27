﻿using System;

namespace Gip.Models
{
    public class Planner
    {
        private DateTime _datum;
        private DateTime _startmoment;
        private string _gebouw;
        private int _verdiep;
        private string _nummer;
        private string _vakcode;
        private string _titel;
        private DateTime _eindmoment;

        public Planner(DateTime datum, DateTime startmoment, string gebouw, int verdiep, string nummer, string vakcode, string titel, DateTime eindmoment)
        {
            Datum = datum;
            Startmoment = startmoment;
            Gebouw = gebouw;
            Verdiep = verdiep;
            Nummer = nummer;
            Vakcode = vakcode;
            Titel = titel;
            Eindmoment = eindmoment;
        }

        public DateTime Datum {
            get { return _datum; }
            set { this._datum = value; } 
        }

        public DateTime Startmoment {
            get { return _startmoment; }
            set { this._startmoment = value; }
        }

        public string Gebouw {
            get { return _gebouw; }
            set { this._gebouw = value; }
        }

        public int Verdiep {
            get { return _verdiep; }
            set { this._verdiep = value; }
        }

        public string Nummer {
            get { return _nummer; }
            set { this._nummer = value; }
        }

        public string Vakcode {
            get { return _vakcode; }
            set { this._vakcode = value; }
        }

        public string Titel {
            get { return _titel;  }
            set { this._titel = value; }
        }

        public DateTime Eindmoment {
            get { return _eindmoment; }
            set { this._eindmoment = value; }
        }
    }
}
