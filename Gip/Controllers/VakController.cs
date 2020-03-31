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

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public VakController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        // GET
        //fixed
        [HttpGet]
        [Route("vak")]
        public async Task<ActionResult> Index()
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
                               where c.ApplicationUserId == user.Id
                               select c;

                    //alle vakken aflopen
                    foreach (var vak in qry) 
                    {
                        //Als het vak voorkomt in de list<CourseUser> qry2, dan maak je een VakViewModel aan
                        //      waar ingeschreven == 1 staat voor: de student is geaccepteerd door lector (goedgekeurd == true)
                        //      en ingeschreven == 2 staat voor: de student heeft aanvraag gedaan maar is nog niet geaccepteerd (goedgekeurd == false)
                        var q2 = qry2.Where(cu => cu.CourseId.Equals(vak.Id));
                        if (q2.Any())
                        {
                            var temp = new VakViewModel { courseId = vak.Id, Vakcode = vak.Vakcode, Titel = vak.Titel, Studiepunten = vak.Studiepunten, Ingeschreven = q2.First().GoedGekeurd ? 1 : 2 };
                            vakViewModels.Add(temp);
                        }
                        //als het vak daar niet in voorkomt, maak je een VakViewModel aan met ingeschreven op 0, 
                        //dit betekent dat je geen aanvraag hebt gedaan voor de inschrijving noch ingeschreven bent.
                        else
                        {
                            var temp = new VakViewModel { courseId = vak.Id ,Vakcode = vak.Vakcode, Titel = vak.Titel, Studiepunten = vak.Studiepunten, Ingeschreven = 0 };
                            vakViewModels.Add(temp);
                        }
                    }
                }
                //als je geen student bent, maar wel admin of lector, krijg je gewoon een overzicht van alle vakken. Die kan je dan bewerken of verwijderen.
                else 
                {
                    foreach (var vak in qry)
                    {
                        var temp = new VakViewModel { courseId = vak.Id, Vakcode = vak.Vakcode, Titel = vak.Titel, Studiepunten = vak.Studiepunten };
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

        //fixed, zorg er nog voor dat vak niet kan toegevoegd worden wanneer vakcode reeds in gebruik
        [HttpPost]
        [Route("vak/add")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Add(string vakcode, string titel, int studiepunten)
        {
            try{
                Course course = new Course { Vakcode = vakcode, Titel = titel, Studiepunten = studiepunten};
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

        //fixed
        [HttpGet]
        [Route("vak/add")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Add()
        {
            return View();
        }

        //fixed 
        [HttpPost]
        [Route("vak/delete")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Delete(int vakcode)
        {
            if (vakcode < 0)
            {
                TempData["error"] = "deleteError" + "/" + "Vakcode is foutief.";
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

        //fixed, zorg er nog voor dat vak niet kan toegevoegd worden wanneer vakcode reeds in gebruik + verwijder bij gebruik lesmoment?
        [HttpPost]
        [Route("vak/edit")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Edit(int vakcodeOld, string vakcodeNew, string titel, int studiepunten)
        {
            TempData["error"] = "";
            if (vakcodeOld < 0)
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

                course.Vakcode = newCourse.Vakcode;
                course.Titel = newCourse.Titel;
                course.Studiepunten = newCourse.Studiepunten;
                db.SaveChanges();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                TempData["error"] = "editError" + "/" + e.Message;
                return RedirectToAction("Index", "Vak");
            }
            TempData["error"] = "editGood";
            return RedirectToAction("Index", "Vak");
        }

        //fixed
        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> Subscribe(int vakCode) 
        {
            try
            {
                var vak = db.Course.Find(vakCode);
                var user = await userManager.GetUserAsync(User);

                CourseUser cu = new CourseUser { CourseId = vak.Id, ApplicationUserId = user.Id};
                db.CourseUser.Add(cu);

                db.SaveChanges();
            }
            catch (Exception e) {
                ViewBag.error = e.Message + " " + e.InnerException.Message==null?" ": e.InnerException.Message;
            }
            return RedirectToAction("Index");
        }

        //fixed
        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> UnSubscribe(int vakCode)
        {
            try
            {
                var vak = db.Course.Find(vakCode);
                var user = await userManager.GetUserAsync(User);
                CourseUser cu = db.CourseUser.Find(user.UserName, vak.Vakcode);

                db.CourseUser.Remove(cu);

                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message + " " + e.InnerException.Message == null ? " " : e.InnerException.Message;
            }
            return RedirectToAction("Index");
        }
    }
}