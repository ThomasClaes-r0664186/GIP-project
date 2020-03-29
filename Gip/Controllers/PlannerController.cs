using Gip.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Gip.Controllers
{
    [Authorize(Roles ="Admin, Lector, Student")]
    public class PlannerController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();

        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public PlannerController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        // GET /planner
        [HttpGet]
        [Route("planner")]
        public ActionResult Index(int week)
        {
            int weekToUse = GetIso8601WeekOfYear(DateTime.Now)+week;
            try
            {
                var _qry = from cm in db.CourseMoment
                           join c in db.Course on cm.Vakcode equals c.Vakcode
                           join s in db.Schedule
                                on new { cm.Datum, cm.Startmoment, cm.Eindmoment}
                                equals new { s.Datum, s.Startmoment, s.Eindmoment}
                           where (int)((cm.Datum.DayOfYear / 7.0) + 0.2) == weekToUse
                           orderby cm.Datum, cm.Startmoment, cm.Eindmoment, cm.Gebouw, cm.Verdiep, cm.Nummer
                           select new
                           {
                               datum = cm.Datum,
                               startmoment = cm.Startmoment,
                               gebouw = cm.Gebouw,
                               verdiep = cm.Verdiep,
                               nummer = cm.Nummer,
                               vakcode = c.Vakcode,
                               titel = c.Titel,
                               eindmoment = s.Eindmoment,
                               rNummer = cm.Userid
                           };
                var lokaalQry = from lok in db.Room
                                orderby lok.Gebouw, lok.Verdiep, lok.Nummer
                                select lok;
                var vakQry = from vak in db.Course
                             orderby vak.Vakcode
                             select vak;

                ViewBag.maandag = FirstDayOfWeek(weekToUse).ToString("dd-MM-yyyy");
                ViewBag.vrijdag = FirstDayOfWeek(weekToUse).AddDays(4).ToString("dd-MM-yyyy");

                List<Planner> planners = new List<Planner>();

                foreach (var qry in _qry)
                {
                    Planner planner = new Planner(qry.datum, qry.startmoment, qry.gebouw, qry.verdiep, qry.nummer, qry.rNummer, qry.vakcode, qry.titel, qry.eindmoment);
                    planners.Add(planner);
                }

                foreach (var qry in lokaalQry)
                {
                    Planner planner = new Planner(qry.Gebouw, qry.Verdiep, qry.Nummer, qry.Capaciteit);
                    planners.Add(planner);
                }

                foreach (var qry in vakQry)
                {
                    Planner planner = new Planner(qry.Vakcode, qry.Titel);
                    planners.Add(planner);
                }

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
        [HttpPost]
        [Route("planner/add")]
        [Authorize(Roles = "Admin, Lector")]
        public async Task<IActionResult> Add(string dat, string uur, string lokaalId, double duratie, string vakcode, string? lessenlijst,bool? checkbox, string lokaal2Id)
        {
            DateTime datum = DateTime.ParseExact(dat, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime tijd = new DateTime(1800, 1, 1, int.Parse(uur.Split(":")[0]), int.Parse(uur.Split(":")[1]), 0);
            double _duratie = Convert.ToDouble(duratie);

            if (_duratie <=0) {
                TempData["error"] = "addError" + "/" + "De duratie mag niet negatief zijn, noch 0.";
                return RedirectToAction("Index", "Planner");
            }

            DateTime eindmoment = tijd.AddHours(_duratie);

            lokaalId = lokaalId.Trim() + " ";
            string gebouw = lokaalId.Substring(0, 1);
            int verdieping = int.Parse(lokaalId.Substring(1, 1));
            string nummer = lokaalId.Substring(2, (lokaalId.Length - 2));

            try
            {
                Schedule schedule = db.Schedule.Find(datum, tijd, eindmoment);
                if (schedule == null) {
                    schedule = new Schedule();
                    schedule.Datum = datum;
                    schedule.Startmoment = tijd;
                    schedule.Eindmoment = eindmoment;
                    db.Schedule.Add(schedule);
                    db.SaveChanges();
                }

                var user = await userManager.GetUserAsync(User);

                CourseMoment moment = new CourseMoment();
                moment.Vakcode = vakcode;
                moment.Datum = datum;
                moment.Startmoment = schedule.Startmoment;
                moment.Eindmoment = schedule.Eindmoment;
                moment.Gebouw = gebouw;
                moment.Verdiep = verdieping;
                moment.Nummer = nummer;
                moment.Userid = user.UserName;
                moment.LessenLijst = lessenlijst;

                if (db.CourseMoment.Find(moment.Vakcode, moment.Datum, moment.Gebouw, moment.Verdiep, moment.Nummer, moment.Userid = user.UserName,moment.Startmoment, moment.Eindmoment) != null)
                {
                    TempData["error"] = "addError" + "/" + "Dit lesmoment staat reeds in de planning.";
                    return RedirectToAction("Index", "Planner");
                }
                else {
                    db.CourseMoment.Add(moment);
                    db.SaveChanges();
                }

                if (checkbox != null && checkbox == true)
                {
                    lokaal2Id = lokaal2Id.Trim() + " ";
                    string gebouw2 = lokaal2Id.Substring(0, 1);
                    int verdieping2 = int.Parse(lokaal2Id.Substring(1, 1));
                    string nummer2 = lokaal2Id.Substring(2, (lokaal2Id.Length - 2));

                    CourseMoment moment2 = new CourseMoment();
                    moment2.Vakcode = vakcode;
                    moment2.Datum = datum;
                    moment2.Startmoment = schedule.Startmoment;
                    moment2.Eindmoment = schedule.Eindmoment;
                    moment2.Gebouw = gebouw2;
                    moment2.Verdiep = verdieping2;
                    moment2.Nummer = nummer2;
                    moment2.Userid = user.UserName;
                    moment2.LessenLijst = lessenlijst;

                    if (db.CourseMoment.Find(moment2.Vakcode, moment2.Datum, moment2.Gebouw, moment2.Verdiep, moment2.Nummer, moment2.Userid = user.UserName, moment2.Startmoment, moment2.Eindmoment) != null)
                    {
                        TempData["error"] = "addError" + "/" + "Uw tweede lokaal is hetzelfde als het eerste lokaal, enkel het lesmoment in het eerste lokaal werd toegevoegd.";
                        return RedirectToAction("Index", "Planner");
                    }
                    else
                    {
                        db.CourseMoment.Add(moment2);
                        db.SaveChanges();
                    }
                }
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
                var lokaalQry = from lok in db.Room
                                orderby lok.Gebouw, lok.Verdiep, lok.Nummer
                                select lok;
                var vakQry = from vak in db.Course
                             orderby vak.Vakcode
                             select vak;

                List < Planner > planners = new List<Planner>();
                foreach (var qry in lokaalQry)
                {
                    Planner planner = new Planner(qry.Gebouw, qry.Verdiep, qry.Nummer, qry.Capaciteit);
                    planners.Add(planner);
                }
                foreach (var qry in vakQry) {
                    Planner planner = new Planner(qry.Vakcode, qry.Titel);
                    planners.Add(planner);
                }
                return View("../Planner/Index",planners);
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
        public ActionResult Delete(string vakcode, DateTime datum, DateTime startMoment, string gebouw, int verdiep, string nummer, string rNummer, DateTime eindMoment) {
            DateTime newStartMoment = new DateTime(1800, 1, 1, startMoment.Hour, startMoment.Minute, startMoment.Second);
            CourseMoment moment = db.CourseMoment.Find(vakcode, datum,gebouw, verdiep, nummer, rNummer, newStartMoment, eindMoment);
            if (moment == null) {
                TempData["error"] = "deleteError" + "/" + "Er is geen overeenkomend moment gevonden.";
                return RedirectToAction("Index", "Planner");
            }
            try
            {
                db.CourseMoment.Remove(moment);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                TempData["error"] = "deleteError" + "/" + "Er is een databank probleem opgetreden. " + e.InnerException.Message;
                return RedirectToAction("Index", "Planner");
            }
            TempData["error"] = "deleteGood";
            return RedirectToAction("Index", "Planner");
        }

        [HttpPost]
        [Route("planner/edit")]
        [Authorize(Roles = "Admin, Lector")]
        public async Task<ActionResult> EditAsync(string oldVakcode, 
            DateTime oldDatum, DateTime oldStartMoment, DateTime oldEindmoment,
            string oldGebouw, int oldVerdiep, string oldNummer, string OldRNummer,
            string newVakcode, 
            string newDatum, string newStartMoment, double newDuratie,
            string newLokaalid, string newLessenlijst) {
            try
            {
                CourseMoment oldMoment = db.CourseMoment.Find(oldVakcode, oldDatum, oldGebouw, oldVerdiep, oldNummer, OldRNummer, oldStartMoment, oldEindmoment);
                if (oldMoment == null)
                {
                    TempData["error"] = "deleteError" + "/" + "Er is geen overeenkomend moment gevonden in de databank.";
                    return RedirectToAction("Index", "Planner");
                }
                else
                {
                    db.CourseMoment.Remove(oldMoment);
                }
                newLokaalid = newLokaalid.Trim() + " ";
                string newGebouw = newLokaalid.Substring(0, 1);
                int newVerdiep = int.Parse(newLokaalid.Substring(1, 1));
                string newNummer = newLokaalid.Substring(2, (newLokaalid.Length - 2));

                double _duratie = Convert.ToDouble(newDuratie);

                DateTime datum = DateTime.ParseExact(newDatum, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime tijd = new DateTime(1800, 1, 1, int.Parse(newStartMoment.Split(":")[0]), int.Parse(newStartMoment.Split(":")[1]), 0);
                DateTime eindmoment = tijd.AddHours(_duratie);
                Schedule schedule = db.Schedule.Find(datum, tijd,eindmoment);

                if (schedule == null)
                {
                    schedule = new Schedule();
                    schedule.Datum = datum;
                    schedule.Startmoment = tijd;
                    schedule.Eindmoment = eindmoment;

                    db.Schedule.Add(schedule);
                    db.SaveChanges();
                }

                var user = await userManager.GetUserAsync(User);

                CourseMoment newMoment = new CourseMoment(newVakcode, datum, tijd, newGebouw, newVerdiep, newNummer, user.UserName, newLessenlijst,eindmoment);
                db.CourseMoment.Add(newMoment);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                TempData["error"] = "editError" + "/" + e.Message + " " + e.InnerException.Message;
                return RedirectToAction("Index","Planner");
            }
            TempData["error"] = "editGood";
            db.SaveChanges();
            return RedirectToAction("Index", "Planner");
        }

        [HttpGet]
        [Route("planner/viewTopic")]
        public ActionResult ViewTopic(string vakcode, DateTime datum, DateTime startMoment ,DateTime eindMoment, string gebouw, int verdiep, string nummer, string rNummer, int datumY, int datumM, int datumD)
        {
            try {
                /*
                 * om de één of andere reden wordt datum niet meer deftig doorgegeven, deze is dus vervangen door datumY, M, D
                 * 
                int Year = Convert.ToInt32(datum.ToString("dd/MM/yyyy").Split('/')[2]);
                int Month = Convert.ToInt32(datum.ToString("dd/MM/yyyy").Split('/')[0]);
                int Day = Convert.ToInt32(datum.ToString("dd/MM/yyyy").Split('/')[1]);
                DateTime dt = new DateTime(Year, Month, Day, datum.Hour, datum.Minute, datum.Second);
                */

                DateTime dt = new DateTime(datumY, datumM, datumD, datum.Hour, datum.Minute, datum.Second);

                DateTime newStartMoment = new DateTime(1800, 1, 1, startMoment.Hour, startMoment.Minute, startMoment.Second);
                
                Schedule schedule = db.Schedule.Find(dt, newStartMoment,eindMoment);
                CourseMoment moment = db.CourseMoment.Find(vakcode, dt, gebouw, verdiep, nummer, rNummer, newStartMoment, eindMoment);
                /* db.CourseMoment.Find(vakcode, datum,gebouw, verdiep, nummer, "r0664186", newStartMoment, eindMoment); */
                Course course = db.Course.Find(vakcode);
                Planner planner = new Planner(moment.Datum, schedule.Startmoment, moment.Gebouw, moment.Verdiep, moment.Nummer, course.Vakcode, course.Titel, schedule.Eindmoment, moment.LessenLijst);
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
        public ActionResult ViewCourseMoments(string vakcode)
        {
            try
            {
                var _qry = from cm in db.CourseMoment
                          join c in db.Course on cm.Vakcode equals c.Vakcode
                          join s in db.Schedule
                               on new { cm.Datum, cm.Startmoment, cm.Eindmoment }
                               equals new { s.Datum, s.Startmoment, s.Eindmoment}
                          where cm.Vakcode == vakcode
                          select new
                          {
                              datum = cm.Datum,
                              startmoment = cm.Startmoment,
                              gebouw = cm.Gebouw,
                              verdiep = cm.Verdiep,
                              nummer = cm.Nummer,
                              vakcode = c.Vakcode,
                              titel = c.Titel,
                              eindmoment = s.Eindmoment
                          };

                List<Planner> planners = new List<Planner>();
                foreach (var qry in _qry)
                {
                    Planner planner = new Planner(qry.datum, qry.startmoment, qry.gebouw, qry.verdiep, qry.nummer, qry.vakcode, qry.titel, qry.eindmoment);
                    planners.Add(planner);
                }

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

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return (time.DayOfYear / 7);
        }

        public static DateTime FirstDayOfWeek(int weekOfYear) {
            DateTime jan1 = new DateTime(DateTime.Today.Year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var weekNum = weekOfYear + 1 ;
            if (firstWeek == 1) {
                weekNum -= 1;
            }

            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
    }
}