using System;
using System.Collections.Generic;

namespace Gip.Models
{
    public class Planner
    {
        public int cmId {get; set;}
        public int cId { get; set; }
        public int rId { get; set; }
        private DateTime _datum;
        private DateTime _startmoment;
        private string _gebouw;
        private int _verdiep;
        private string _nummer;
        private string _vakcode;
        private string _titel;
        private string _rNummer;
        private DateTime _eindmoment;
        private string _lessenLijst;
        private int _capaciteit;

        public List<ApplicationUser> users { get; set; }

        public Planner() { }

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

        public Planner(DateTime datum, DateTime startmoment, string gebouw, int verdiep, string nummer, string vakcode, string titel, DateTime eindmoment, string lessenLijst)
        {
            Datum = datum;
            Startmoment = startmoment;
            Gebouw = gebouw;
            Verdiep = verdiep;
            Nummer = nummer;
            Vakcode = vakcode;
            Titel = titel;
            Eindmoment = eindmoment;
            LessenLijst = lessenLijst;
        }

        public Planner(DateTime datum, DateTime startmoment, string gebouw, int verdiep, string nummer, string rNummer, string vakcode, string titel, DateTime eindmoment)
        {
            Datum = datum;
            Startmoment = startmoment;
            Gebouw = gebouw;
            Verdiep = verdiep;
            Nummer = nummer;
            Vakcode = vakcode;
            Titel = titel;
            Eindmoment = eindmoment;
            RNummer = rNummer;
        }

        public Planner(DateTime datum, DateTime startmoment, string gebouw, int verdiep, string nummer, string rNummer, string vakcode, string titel, DateTime eindmoment, string lessenLijst)
        {
            Datum = datum;
            Startmoment = startmoment;
            Gebouw = gebouw;
            Verdiep = verdiep;
            Nummer = nummer;
            Vakcode = vakcode;
            Titel = titel;
            Eindmoment = eindmoment;
            LessenLijst = lessenLijst;
            RNummer = rNummer;
        }

        public Planner(string gebouw, int verdiep, string nummer, int capaciteit) {
            Datum = new DateTime();
            Startmoment = new DateTime();
            Gebouw = gebouw;
            Verdiep = verdiep;
            Nummer = nummer;
            Vakcode = null;
            Titel = null;
            Eindmoment = new DateTime();
            Capaciteit = capaciteit;
        }

        public Planner(string vakcode, string titel) {
            Datum = new DateTime();
            Startmoment = new DateTime();
            Gebouw = null;
            Verdiep = -1;
            Nummer = null;
            Vakcode = vakcode;
            Titel = titel;
            Eindmoment = new DateTime();
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

        public string RNummer
        {
            get { return _rNummer; }
            set { this._rNummer = value; }
        }

        public DateTime Eindmoment {
            get { return _eindmoment; }
            set { this._eindmoment = value; }
        }

        public string LessenLijst {
            get { return _lessenLijst; }
            set { this._lessenLijst = value; }
        }
        public int Capaciteit
        {
            get { return _capaciteit; }
            set { this._capaciteit = value; }
        }
    }
}
