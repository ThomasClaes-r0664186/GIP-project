using Gip.Models;
using Gip.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public int GetStudAlreadySubscribed(ApplicationUser user)
        {
            // we kijken voor welke vakken de student is ingeschreven
            var cuL = db.CourseUser.Where(cu => cu.ApplicationUserId == user.Id).Include("Courses");

            //indien deze is ingeschreven in een vak
            if (cuL.Any())
            {
                //gaan we nakijken of dat vak tot een richting behoort en de eerste richting dat we tegenkomen, daarvan returnen we de id.
                foreach (CourseUser cu in cuL)
                {
                    if (cu.Courses.FieldOfStudyId != null)
                    {
                        return (int)cu.Courses.FieldOfStudyId;
                    }
                }
            }
            //anders returnen we dat student niet is ingeschreven. 
            return -1;
        }

        public void AddRichting(string code, string titel, string type)
        {
            var rInUse = from r in db.FieldOfStudy
                         where r.RichtingCode == code.ToUpper()
                         select r;

            if (rInUse.Any())
            {
                throw new ArgumentException("Richting met deze code bestaat reeds.");
            }

            FieldOfStudy fos = new FieldOfStudy { RichtingCode = code.ToUpper(), RichtingTitel = titel.ToLower(), Type = type.ToLower(), RichtingStudiepunten = 0};

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

        public FieldOfStudy GetRichting(int richtingId) 
        {
            return db.FieldOfStudy.Find(richtingId);
        }

        public void EditRichting(int richtindId, string richtingCode, string richtingTitel, string type)
        {
            FieldOfStudy field = db.FieldOfStudy.Find(richtindId);

            if (field == null)
            {
                throw new ArgumentException("Deze richting werd niet gevonden in het systeem.");
            }

            try 
            {
                FieldOfStudy newfield = new FieldOfStudy { Id = richtindId, RichtingCode = richtingCode, RichtingTitel = richtingTitel, Type = type, RichtingStudiepunten = 0 };

                field.Id = newfield.Id;
                field.RichtingCode = newfield.RichtingCode;
                field.RichtingTitel = newfield.RichtingTitel;
                field.Type = newfield.Type;

                db.SaveChanges();
            }
            catch (Exception e) 
            { throw new ArgumentException("Er liep iets mis met het updaten van de data: " + e.Message); }
        }

        public void SubscribeFos(int fosId, ApplicationUser user)
        {
            var courseList = db.Course.Where(c => c.FieldOfStudyId == fosId).ToList<Course>();

            if (courseList == null)
            {
                throw new ArgumentException("Deze richting heeft nog geen vakken of bestaat niet.");
            }

            VakService v = new VakService(db);

            foreach (Course course in courseList)
            {
                v.Subscribe(course.Id, user);
                db.SaveChanges();
            }
        }
    }
}
