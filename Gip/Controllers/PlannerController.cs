using Gip.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Gip.Models.ViewModels;
using Gip.Services.Interfaces;

namespace Gip.Controllers
{
    [Authorize(Roles ="Admin, Lector, Student")]
    public class PlannerController : Controller
    {
        private IPlannerService service;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public PlannerController(IPlannerService service, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.service = service; 
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        // GET /planner
        [HttpGet]
        [Route("planner")]
        public async Task<ActionResult> Index(int week)
        {
            int weekToUse = GetIso8601WeekOfYear(DateTime.Now)+week;
            try
            {
                List<Planner> planners; 
                var user = await userManager.GetUserAsync(User);

                if (User.IsInRole("Student"))
                {
                    planners = service.GetPlanningStudent(weekToUse, user);
                }
                else {
                    planners = service.GetPlanningLectAdmin(weekToUse);
                }

                ViewBag.maandag = FirstDayOfWeek(weekToUse).ToString("dd-MM-yyyy");
                ViewBag.vrijdag = FirstDayOfWeek(weekToUse).AddDays(4).ToString("dd-MM-yyyy");

                ViewBag.nextWeek = week += 1;
                ViewBag.prevWeek = week -= 2;

                if (TempData["error"] != null)
                {
                    ViewBag.error = TempData["error"].ToString();
                    TempData["error"] = null;
                }
                if (ViewBag.error == null || !ViewBag.error.Contains("addError") && !ViewBag.error.Contains("addGood") && !ViewBag.error.Contains("deleteError") && !ViewBag.error.Contains("deleteGood") && !ViewBag.error.Contains("editError") && !ViewBag.error.Contains("editGood") && !ViewBag.error.Contains("topicError"))
                {
                    ViewBag.error = "indexLokaalGood";
                }
                return View("../Planning/Index",planners);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TempData["error"] = "indexVakError" + "/" + "Er liep iets mis bij het ophalen van de planner.";
                return RedirectToAction("Index", "Home");
            }
        }

        //fixed - nog iets aan toevoegen
        [HttpPost]
        [Route("planner/add")]
        [Authorize(Roles = "Admin, Lector")]
        public async Task<IActionResult> Add(string dat, string uur, int lokaalId, double duratie, int vakid, string? lessenlijst,bool? checkbox, int lokaal2Id)
        {
            DateTime datum = DateTime.ParseExact(dat, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime tijd = new DateTime(1800, 1, 1, int.Parse(uur.Split(":")[0]), int.Parse(uur.Split(":")[1]), 0);
            double _duratie = Convert.ToDouble(duratie);
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            if (_duratie <=0) {
                TempData["error"] = "addError" + "/" + "De duratie mag niet negatief zijn, noch 0.";
                return RedirectToAction("Index", "Planner");
            }

            try
            {
                service.AddPlanning(user, datum, tijd, _duratie, lokaalId, vakid, lessenlijst, checkbox, lokaal2Id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TempData["error"] = "addError" + "/" + e.Message;
                return RedirectToAction("Index", "Planner");
            }
            TempData["error"] = "addGood";
            return RedirectToAction("Index", "Planner");
        }

        [HttpGet]
        [Route("planner/add")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Add()
        {
            try
            {
                List<Planner> planners = service.GetLokalen();

                return View("../Planner/Index", planners);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TempData["error"] = "indexVakError" + "/" + e.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Route("planner/delete")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Delete(int cmId) {
            try
            {
                service.DeletePlanning(cmId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TempData["error"] = "deleteError" + "/" + "Er is een databank probleem opgetreden. " + e.InnerException.Message == null ? " " : e.InnerException.Message;
                return RedirectToAction("Index", "Planner");
            }
            TempData["error"] = "deleteGood";
            return RedirectToAction("Index", "Planner");
        }

        //fixed - nog iets aan toevoegen.
        [HttpPost]
        [Route("planner/edit")]
        [Authorize(Roles = "Admin, Lector")]
        public async Task<ActionResult> Edit(int cmId,
            int newVakcode, 
            string newDatum, string newStartMoment, double newDuratie,
            int newLokaalid, string newLessenlijst) {
            try
            {
                var user = await userManager.GetUserAsync(User);
                service.EditPlanning(cmId, newVakcode, newDatum, newStartMoment, newDuratie, newLokaalid, newLessenlijst, user);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                TempData["error"] = "editError" + "/" + e.Message + " " + e.InnerException.Message == null ? " " : e.InnerException.Message;
                return RedirectToAction("Index","Planner");
            }
            TempData["error"] = "editGood";
            return RedirectToAction("Index", "Planner");
        }

        [HttpGet]
        [Route("planner/viewTopic")]
        public async Task<ActionResult> ViewTopic(int cmId)
        {
            try {
                if (TempData["error"] != null)
                {
                    ViewBag.error = TempData["error"].ToString();
                    TempData["error"] = null;
                }

                bool lector = User.IsInRole("Lector");

                var user = await userManager.FindByNameAsync(User.Identity.Name);

                var planner = service.GetTopic(cmId, lector, user);

                return View("../Planning/ViewTopi", planner);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                TempData["error"] = "topicError" + "/" + "Databank fout.";
                return RedirectToAction("Index", "Planner");
            }
        }

        [HttpGet]
        [Route("planner/viewCourseMoments")]
        public ActionResult ViewCourseMoments(int vakcode)
        {
            try
            {
                var planners = service.GetCourseMoments(vakcode);

                ViewBag.error = "coursemomentsGood";
                return View("../Planning/courseMomentsOffTopic", planners);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TempData["error"] = "topicError" + "/" + "Databank fout.";
                return RedirectToAction("Index", "Planner");
            }
        }

        [HttpGet]
        public IActionResult EditStudsInCm(int cmId)
        {
            ViewBag.cmId = cmId;

            try
            {
                var model = service.GetStudsInCm(cmId);

                return View("../Planning/EditStudsInCm", model);
            }
            catch (Exception e) 
            {
                Console.WriteLine(e);
                TempData["error"] = "topicError" + "/" + "Databank fout.";
                return RedirectToAction("Index", "Planner");
            }
        }

        [HttpPost]
        public IActionResult EditStudsInCm(List<EditStudInCmViewModel> model, int cmId)
        {
            try
            {
                service.EditStudsInCm(model, cmId);
            }
            catch (Exception e) 
            {
                TempData["error"] = e.Message;
            }

            return RedirectToAction("ViewTopic", new { cmId = cmId});
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult ViewCourseUsers(int vakcode) 
        {
            try
            {
                var cuvw = service.GetCourseUsers(vakcode);

                if (TempData["error"] != null)
                {
                    ViewBag.error = "Deze studenten konden niet toegevoegd worden aan bepaalde lesmomenten door overschrijden capaciteit:" + TempData["error"].ToString();
                    TempData["error"] = null;
                }

                return View("../Planning/ViewCourseUsers", cuvw);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TempData["error"] = "topicError" + "/" + "Databank fout.";
                return RedirectToAction("Index", "Vak");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult AddStudsToEachCm(int vakcode) 
        {
            ViewBag.VakCode = vakcode;

            var model = service.GetStudsInEachCm(vakcode);

            return View("../Planning/EditStudsToEachCm", model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Lector")]
        public async Task<ActionResult> AddStudsToEachCm(List<EditStudInCmViewModel> model, int vakcode) 
        {
            try {
                TempData["Error"] = await service.AddStudsToEachCm(model, vakcode);

                return RedirectToAction("ViewCourseUsers", "Planner", new { vakcode });
            }
            catch (Exception e) 
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("ViewCourseUsers", "Planner", vakcode);
            }
        }

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return (time.DayOfYear / 7);
        }

        public static DateTime FirstDayOfWeek(int weekOfYear)
        {
            DateTime jan1 = new DateTime(DateTime.Today.Year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var weekNum = weekOfYear + 1;
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
    }
}