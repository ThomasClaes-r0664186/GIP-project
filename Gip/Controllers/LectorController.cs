using Gip.Models;
using Gip.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gip.Controllers
{
    [Authorize(Roles="Lector")]
    public class LectorController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();

        [HttpGet]
        [Route("Lector")]
        public ActionResult Index()
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
                      select new { cuId = cu.Id, cId = vak.Id,titel = vak.Titel, vakCode = vak.Vakcode, RNum = user.UserName,naam = user.Naam, voorNaam = user.VoorNaam};

            foreach (var vakI in vakL)
            {
                StudentRequestsViewModel studReq = new StudentRequestsViewModel { cuId = -1,Titel = vakI.Titel, VakCode = vakI.Vakcode };
                studentRequests.Add(studReq);

                var cuL2 = cuL.Where(c => c.cId.Equals(vakI.Id));

                foreach (var res in cuL2)
                {
                    studReq = new StudentRequestsViewModel { RNum = res.RNum,cuId = res.cuId, VakCode = res.vakCode, Titel = res.titel, Naam = res.naam, VoorNaam = res.voorNaam};
                    studentRequests.Add(studReq);
                }
            }
            return View(studentRequests);
        }

        [HttpPost]
        public ActionResult ApproveStudent(int cuId) 
        {
            //db.CourseUser.Find(cuId).GoedGekeurd = true;
            //db.SaveChanges();
            try
            {
                db.CourseUser.Find(cuId).GoedGekeurd = true;
                db.SaveChanges();

                TempData["error"] = "approveGood";
            }
            catch (Exception e)
            {
                TempData["error"] = "error" + "/" + e.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DenyStudent(int cuId, string beschrijving)
        {
            try
            {
                db.CourseUser.Find(cuId).GoedGekeurd = null;
                db.CourseUser.Find(cuId).AfwijzingBeschr = beschrijving;
                db.SaveChanges();

                TempData["error"] = "denyGood";
            }
            catch (Exception e)
            {
                TempData["error"] = "error" + "/" + e.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
