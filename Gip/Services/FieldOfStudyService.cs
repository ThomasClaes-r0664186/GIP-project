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


        public void DeleteRichting(int richtindId)
        {
            FieldOfStudy field = db.FieldOfStudy.Find(richtindId);

            if (field==null)
            {
                throw new ArgumentException("Deze richting werd niet in de databank gevonden.");
            }
            else
            {
                db.FieldOfStudy.Remove(field);
                db.SaveChanges();
            }
        }

        public void EditRichting(int richtindId, string richtingCode, string richtingTitel, string type, int richtingStudiepunten)
        {
            FieldOfStudy field = db.FieldOfStudy.Find(richtindId);

            if (field == null)
            {
                throw new ArgumentException("Deze richting werd niet gevonden in het systeem.");
            }

            FieldOfStudy newfield = new FieldOfStudy { Id = richtindId, RichtingCode = richtingCode, RichtingTitel = richtingTitel, Type = type, RichtingStudiepunten = richtingStudiepunten };

            field.Id = newfield.Id;
            field.RichtingCode = newfield.RichtingCode;
            field.RichtingTitel = newfield.RichtingTitel;
            field.Type = newfield.Type;
            field.RichtingStudiepunten = newfield.RichtingStudiepunten;

            db.SaveChanges();
            
        }
    }
}
