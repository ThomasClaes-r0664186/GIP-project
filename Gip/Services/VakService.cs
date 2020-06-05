using Gip.Models;
using Gip.Models.ViewModels;
using Gip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Services
{
    public class VakService : IVakService
    {
        private gipDatabaseContext db;
        public VakService(gipDatabaseContext db) 
        {
            this.db = db;
        }

        public List<VakViewModel> GetVakkenStudent(ApplicationUser user)
        {
            var qry = from d in db.Course
                      orderby d.Vakcode
                      select d;

            List<VakViewModel> vakViewModels = new List<VakViewModel>();

            //aflopen databank en alle rijen, waar de student in voorkomt in de tabel CourseUser, in een list<CourseUser> steken.

            var qry2 = from c in db.CourseUser
                       where c.ApplicationUserId == user.Id
                       select c;

            //alle vakken aflopen
            foreach (var vak in qry)
            {
                //Als het vak voorkomt in de list<CourseUser> qry2, dan maak je een VakViewModel aan
                //      waar ingeschreven == 1 staat voor: de student is geaccepteerd door lector (goedgekeurd == true)
                //      en ingeschreven == 2 staat voor: de student heeft aanvraag gedaan maar is nog niet geaccepteerd (goedgekeurd == false)
                //      voor een bescrhijving kunnen we dan hieraan toevoegen == 3 waarin je dan bent afgekeurd en je u niet meteen terug kan inschrijven.
                var q2 = qry2.Where(cu => cu.CourseId.Equals(vak.Id));
                if (q2.Any())
                {
                    var temp = new VakViewModel { courseId = vak.Id, Vakcode = vak.Vakcode, Titel = vak.Titel, Studiepunten = vak.Studiepunten, Ingeschreven = q2.First().GoedGekeurd == true ? 1 : q2.First().GoedGekeurd == false ? 2 : 3, afwijzingBeschrijving = q2.First().AfwijzingBeschr };
                    vakViewModels.Add(temp);
                }
                //als het vak daar niet in voorkomt, maak je een VakViewModel aan met ingeschreven op 0, 
                //dit betekent dat je geen aanvraag hebt gedaan voor de inschrijving noch ingeschreven bent.
                else
                {
                    var temp = new VakViewModel { courseId = vak.Id, Vakcode = vak.Vakcode, Titel = vak.Titel, Studiepunten = vak.Studiepunten, Ingeschreven = 0 };
                    vakViewModels.Add(temp);
                }
            }

            return vakViewModels;
        }

        public List<VakViewModel> GetVakkenLectAdm()
        {
            //aflopen databank en alle vakken in een list<Course> qry steken
            var qry = from d in db.Course
                      orderby d.Vakcode
                      select d;

            List<VakViewModel> vakViewModels = new List<VakViewModel>();

            foreach (var vak in qry)
            {
                var temp = new VakViewModel { courseId = vak.Id, Vakcode = vak.Vakcode, Titel = vak.Titel, Studiepunten = vak.Studiepunten };
                vakViewModels.Add(temp);
            }

            return vakViewModels;
        }

        public void AddVak(string vakcode, string titel, int studiepunten)
        {
            var cInUse = from c in db.Course
                         where c.Vakcode == vakcode
                         select c;

            if (cInUse.Any())
            {
                throw new ArgumentException("Vak met deze vakcode bestaat reeds.");
            }

            Course course = new Course { Vakcode = vakcode.ToUpper(), Titel = titel, Studiepunten = studiepunten };

            FieldOfStudy FOS = db.FieldOfStudy.Where(fos => fos.RichtingCode == vakcode.Substring(0, 3)).FirstOrDefault();
                

            if (FOS != null) {
                course.FieldOfStudyId = FOS.Id;
                FOS.RichtingStudiepunten += course.Studiepunten;
            }
            
            db.Course.Add(course);
            db.SaveChanges();
        }

        public void DeleteVak(int vakcode)
        {
            Course course = db.Course.Find(vakcode);

            if (course == null)
            {
                throw new ArgumentException("Er bestaat geen vak met deze vakcode");
            }

            var qryDelC = from cu in db.CourseUser
                          where cu.CourseId == vakcode
                          select cu;

            if (qryDelC.Any())
            {
                foreach (var CoUs in qryDelC)
                {
                    db.CourseUser.Remove(CoUs);
                }
                db.SaveChanges();
            }

            db.Course.Remove(course);
            db.SaveChanges();
        }

        public void EditVak(int vakcodeOld, string vakcodeNew, string titel, int studiepunten)
        {
            //vakcode in de parameters is eigenlijk CourseId
            Course course = db.Course.Find(vakcodeOld);

            if (course == null)
            {
                throw new ArgumentException("Het oude vakcode is niet correct doorgegeven.");
            }

            var cInUse = from c in db.Course
                         where c.Vakcode == vakcodeNew
                         select c;

            if (cInUse.Any())
            {
                foreach (var c in cInUse)
                {
                    if (c != course)
                    {
                        throw new ArgumentException("Het nieuwe vakcode dat u heeft ingegeven is reeds in gebruik");
                    }
                }
            }

            Course newCourse = new Course();
            newCourse.Vakcode = vakcodeNew.ToUpper();
            newCourse.Titel = titel;
            newCourse.Studiepunten = studiepunten;

            course.Vakcode = newCourse.Vakcode.ToUpper();
            course.Titel = newCourse.Titel;
            course.Studiepunten = newCourse.Studiepunten;
            db.SaveChanges();
        }

        public void Subscribe(int vakCode, ApplicationUser user)
        {
            var vak = db.Course.Find(vakCode);

            CourseUser cu = new CourseUser { CourseId = vak.Id, ApplicationUserId = user.Id, GoedGekeurd = false };
            db.CourseUser.Add(cu);

            db.SaveChanges();
        }

        public void UnSubscribe(int vakcode, ApplicationUser user)
        {
            var vak = db.Course.Find(vakcode);

            var cu = from cus in db.CourseUser
                     where cus.ApplicationUserId == user.Id
                     where cus.CourseId == vak.Id
                     select cus;

            CourseUser courseUser = cu.FirstOrDefault();

            if (vak == null || user == null || courseUser == null)
            {
                throw new ArgumentException("Het vak of de gebruiker dat werd meegegeven, is verkeerd meegegeven.");
            }

            var CMUL = from cmus in db.CourseMomentUsers
                       join cm in db.CourseMoment on cmus.CoursMomentId equals cm.Id
                       join c in db.Course on cm.CourseId equals c.Id
                       where cmus.ApplicationUserId == user.Id
                       where c.Id == vak.Id
                       select cmus;

            foreach (var cmu in CMUL)
            {
                db.CourseMomentUsers.Remove(cmu);
            }

            db.SaveChanges();

            db.CourseUser.Remove(courseUser);

            db.SaveChanges();
        }
    }
}