using Gip.Models;
using Gip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Services
{
    public class FieldOfStudyService : IFieldOfStudyService
    {
        private gipDatabaseContext db;

        public FieldOfStudyService(gipDatabaseContext db) 
        {
            this.db = db;
        }

        public IQueryable<FieldOfStudy> GetAllFieldOfStudy()
        {
            IQueryable<FieldOfStudy> list = from fos in db.FieldOfStudy orderby fos.Type, fos.RichtingCode select fos;

            return list;
        }

        public void AddRichting(string code, string titel, string type, int studiepunten)
        {
            var rInUse = from r in db.FieldOfStudy
                         where r.RichtingCode == code.ToUpper()
                         select r;

            if (rInUse.Any())
            {
                throw new ArgumentException("Richting met deze code bestaat reeds.");
            }

            FieldOfStudy fos = new FieldOfStudy { RichtingCode = code.ToUpper(), RichtingTitel = titel.ToLower(), Type = type.ToLower(), RichtingStudiepunten = studiepunten};

            db.FieldOfStudy.Add(fos);
            db.SaveChanges();
        }
    }
}
