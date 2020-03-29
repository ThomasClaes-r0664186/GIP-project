using Gip.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Gip.Models.ViewModels;
using System.Collections.Generic;

namespace Gip.Controllers
{
    [Authorize(Roles = "Admin, Lector, Student")]
    public class VakController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();

        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public VakController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        // GET
        [HttpGet]
        [Route("vak")]
        public async Task<ActionResult> IndexAsync()
        {
            try{
                //aflopen databank en alle vakken in een list<Course> qry steken
                var qry = from d in db.Course
                          orderby d.Vakcode
                          select d;

                List<VakViewModel> vakViewModels = new List<VakViewModel>();

                // wanneer je een student bent, moet je het inschrijfgedeelte kunnen zien.
                if (User.IsInRole("Student"))
                {
                    var user = await userManager.GetUserAsync(User);

                    //aflopen databank en alle rijen, waar de student in voorkomt in de tabel CourseUser, in een list<CourseUser> steken.
                    var qry2 = from c in db.CourseUser
                               orderby c.Vakcode
                               where c.Userid == user.UserName
                               select c;

                    //alle vakken aflopen
                    foreach (var vak in qry) 
                    {
                        //Als het vak voorkomt in de list<CourseUser> qry2, dan maak je een VakViewModel aan
                        //      waar ingeschreven == 1 staat voor: de student is geaccepteerd door lector (goedgekeurd == true)
                        //      en ingeschreven == 2 staat voor: de student heeft aanvraag gedaan maar is nog niet geaccepteerd (goedgekeurd == false)
                        var q2 = qry2.Where(cu => cu.Vakcode.Equals(vak.Vakcode));
                        if (q2.Any())
                        {
                            var temp = new VakViewModel { Vakcode = vak.Vakcode, Titel = vak.Titel, Studiepunten = vak.Studiepunten, Ingeschreven = q2.First().GoedGekeurd ? 1 : 2 };
                            vakViewModels.Add(temp);
                        }
                        //als het vak daar niet in voorkomt, maak je een VakViewModel aan met ingeschreven op 0, 
                        //dit betekent dat je geen aanvraag hebt gedaan voor de inschrijving noch ingeschreven bent.
                        else
                        {
                            var temp = new VakViewModel { Vakcode = vak.Vakcode, Titel = vak.Titel, Studiepunten = vak.Studiepunten, Ingeschreven = 0 };
                            vakViewModels.Add(temp);
                        }
                    }
                }
                //als je geen student bent, maar wel admin of lector, krijg je gewoon een overzicht van alle vakken. Die kan je dan bewerken of verwijderen.
                else 
                {
                    foreach (var vak in qry)
                    {
                        var temp = new VakViewModel { Vakcode = vak.Vakcode, Titel = vak.Titel, Studiepunten = vak.Studiepunten };
                        vakViewModels.Add(temp);
                    }
                }

                if (TempData["error"] != null)
                {
                    ViewBag.error = TempData["error"].ToString();
                    TempData["error"] = null;
                }
                if (ViewBag.error == null || !ViewBag.error.Contains("addError") && !ViewBag.error.Contains("addGood") && !ViewBag.error.Contains("deleteError") && !ViewBag.error.Contains("deleteGood") && !ViewBag.error.Contains("editError") && !ViewBag.error.Contains("editGood"))
                {
                    ViewBag.error = "indexVakGood";
                }
                
                return View(vakViewModels);
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

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> SubscribeAsync(string vakCode) 
        {
            try
            {
                var vak = db.Course.Find(vakCode);
                var user = await userManager.GetUserAsync(User);
                var oldUser = db.User.Find(user.UserName);

                CourseUser courseUser = new CourseUser { Vakcode = vakCode, Userid = user.UserName};
                db.CourseUser.Add(courseUser);
                db.SaveChanges();
            }
            catch (Exception e) {
                ViewBag.error = e.Message + " " + e.InnerException.Message==null?" ": e.InnerException.Message;
            }
            return RedirectToAction("Index");
        }
    }
}