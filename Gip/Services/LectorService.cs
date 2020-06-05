using Gip.Models;
using Gip.Models.ViewModels;
using Gip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Services
{
    public class LectorService : ILectorService
    {
        private gipDatabaseContext db;

        public LectorService(gipDatabaseContext db) 
        {
            this.db = db;
        }

        public List<StudentRequestsViewModel> GetStudentRequests()
        {
            List<StudentRequestsViewModel> studentRequests = new List<StudentRequestsViewModel>();

            var vakL = from course in db.Course
                       orderby course.Vakcode
                       select course;

            var cuL = from cu in db.CourseUser
                      join user in db.Users on cu.ApplicationUserId equals user.Id
                      join vak in db.Course on cu.CourseId equals vak.Id
                      orderby user.UserName
                      where cu.GoedGekeurd == false
                      select new { cuId = cu.Id, cId = vak.Id, titel = vak.Titel, vakCode = vak.Vakcode, RNum = user.UserName, naam = user.Naam, voorNaam = user.VoorNaam };

            foreach (var vakI in vakL)
            {
               // StudentRequestsViewModel studReq = new StudentRequestsViewModel { cuId = -1, Titel = vakI.Titel, VakCode = vakI.Vakcode };
                //studentRequests.Add(studReq);

                var cuL2 = cuL.Where(c => c.cId.Equals(vakI.Id));
                
                foreach (var res in cuL2)
                {
                    StudentRequestsViewModel studReq = new StudentRequestsViewModel
                    {
                        RNum = res.RNum, cuId = res.cuId, VakCode = res.vakCode, Titel = res.titel, Naam = res.naam,
                        VoorNaam = res.voorNaam
                    };
                    studentRequests.Add(studReq);
                }
            }
            return studentRequests;
        }

        public void ApproveStudent(int cuId)
        {
            db.CourseUser.Find(cuId).GoedGekeurd = true;
            db.SaveChanges();
        }

        public void DenyStudent(int cuId, string beschrijving)
        {
            db.CourseUser.Find(cuId).GoedGekeurd = null;
            db.CourseUser.Find(cuId).AfwijzingBeschr = beschrijving;
            db.SaveChanges();
        }
    }
}
