using Gip.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Gip.Controllers
{
    [Authorize(Roles = "Admin, Lector, Student")]
    public class VakController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();
        
        // GET
        [HttpGet]
        [Route("vak")]
        public ActionResult Index()
        {
            try{
                var qry = from d in db.Course
                          orderby d.Vakcode
                          select d;
                if (TempData["error"] != null)
                {
                    ViewBag.error = TempData["error"].ToString();
                    TempData["error"] = null;
                }
                if (ViewBag.error == null || !ViewBag.error.Contains("addError") && !ViewBag.error.Contains("addGood") && !ViewBag.error.Contains("deleteError") && !ViewBag.error.Contains("deleteGood") && !ViewBag.error.Contains("editError") && !ViewBag.error.Contains("editGood"))
                {
                    ViewBag.error = "indexVakGood";
                }
                
                return View(qry);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ViewBag.error = "indexVakError" + "/" + e.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Route("vak/add")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Add(string vakcode, string titel, int studiepunten)
        {
            try{
                Course course = new Course();
                course.Vakcode = vakcode;
                course.Titel = titel;
                course.Studiepunten = studiepunten;
                db.Course.Add(course);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                if (e.InnerException != null && e.InnerException.Message.ToLower().Contains("primary"))
                {
                    TempData["error"] = "addError" + "/" + "De vakcode die u heeft ingegeven, is reeds in gebruik. Gelieve een andere vakcode te gebruiken.";
                    return RedirectToAction("Index", "Vak");
                }
                Console.WriteLine(e);
                TempData["error"] = "addError" + "/" + e.Message;
                return RedirectToAction("Index", "Vak");
            }
            TempData["error"] = "addGood";
            return RedirectToAction("Index", "Vak");
        }

        [HttpGet]
        [Route("vak/add")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Route("vak/delete")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Delete(string vakcode)
        {
            if (vakcode == null || vakcode.Trim().Equals(""))
            {
                TempData["error"] = "deleteError" + "/" + "Vakcode mag niet leeg zijn.";
                return  RedirectToAction("Index", "Vak");
            }

            Course course = db.Course.Find(vakcode);

            if (course == null) {
                TempData["error"] = "deleteError" + "/" + "Vak niet gevonden in databank";
                return RedirectToAction("Index", "Vak");
            }

            try
            {
                db.Course.Remove(course);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TempData["error"] = "deleteError" + "/" + "Dit vak is gebruikt in een lesmoment, pas dat eerst aan voordat je dit kan verwijderen.";
                return RedirectToAction("Index", "Vak");
            }

            TempData["error"] = "deleteGood";
            return RedirectToAction("Index", "Vak");
        }
        
        [HttpPost]
        [Route("vak/edit")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Edit(string vakcodeOld, string vakcodeNew, string titel, int studiepunten)
        {
            TempData["error"] = "";
            if (vakcodeOld == null || vakcodeOld.Trim().Equals(""))
            {
                TempData["error"] = "editError" + "/" + "De oude vakcode is niet goed doorgegeven want deze is leeg.";
                return RedirectToAction("Index", "Vak");
            }
            try{
                Course course = db.Course.Find(vakcodeOld);
                Course newCourse = new Course();
                newCourse.Vakcode = vakcodeNew;
                newCourse.Titel = titel;
                newCourse.Studiepunten = studiepunten;

                if (course.Vakcode.Equals(newCourse.Vakcode)) {
                    db.Course.Find(vakcodeOld).Titel = newCourse.Titel;
                    db.Course.Find(vakcodeOld).Studiepunten = newCourse.Studiepunten;
                    db.SaveChanges();
                }
                else
                {
                    db.Course.Add(newCourse);
                    db.SaveChanges();
                    Delete(vakcodeOld);
                }
                if (TempData["error"].ToString().Contains("deleteError"))
                {
                    TempData["error"] = "editError" + "/" + "Dit vak werd gebruikt in een lesmoment. Er werd een nieuw vak aangemaakt met de nieuwe waarden, gelieve dit aan te passen in de planning.";
                    return RedirectToAction("Index", "Vak");
                }
            }
            catch (Exception e) {
                if (e.InnerException != null && e.InnerException.Message.ToLower().Contains("primary"))
                {
                    TempData["error"] = "editError" + "/" + "De vakcode die u heeft ingegeven, is reeds in gebruik. Gelieve een andere vakcode te gebruiken.";
                    return RedirectToAction("Index", "Vak");
                }
                Console.WriteLine(e.Message);
                TempData["error"] = "editError" + "/" + e.Message;
                return RedirectToAction("Index", "Vak");

                /**
                 * lesmoment mee updaten met verandering lokaal, poging 1:
                 * 
                 * if (e.Message.ToLower().Contains("updating")) {
                    Course newVak = db.Course.Find(vakcodeNew);

                    var query = db.CourseMoment.Where(l => l.Vakcode == vakcodeOld);
                    foreach (var q in query) {
                        db.CourseMoment.Find(q.Vakcode, q.Datum, q.Gebouw, q.Verdiep, q.Nummer, q.Userid, q.Startmoment, q.Eindmoment).Vakcode = newVak.Vakcode;

                        PlannerController.Edit(q.Vakcode, q.Datum, q.Startmoment, q.Gebouw, q.Verdiep, q.Nummer, newVak.Vakcode, );
                    }

                    db.SaveChanges();
                }*/
            }
            TempData["error"] = "editGood";
            return RedirectToAction("Index", "Vak");
        }
    }
}