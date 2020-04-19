using Gip.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Gip.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Gip.Controllers
{
    [Authorize(Roles ="Admin, Lector, Student")]
    public class PlannerController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public PlannerController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
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
                List<Planner> planners = new List<Planner>();
                var user = await userManager.GetUserAsync(User);

                if (User.IsInRole("Student"))
                {
                    var qry2 = from c in db.CourseUser
                               where c.ApplicationUserId == user.Id
                               select c;

                    List<int?> vakMetStud = new List<int?>();
                    
                    foreach (var cu in qry2) 
                    {
                        if (cu.GoedGekeurd) 
                        {
                            vakMetStud.Add(cu.CourseId);
                        }
                    }

                    var _qry = from cm in db.CourseMoment
                               join c in db.Course on cm.CourseId equals c.Id
                               join s in db.Schedule on cm.ScheduleId equals s.Id
                               join r in db.Room on cm.RoomId equals r.Id
                               join u in db.Users on cm.ApplicationUserId equals u.Id
                               where (int)((s.Datum.DayOfYear / 7.0) + 0.2) == weekToUse
                               where vakMetStud.Contains(cm.CourseId)
                               orderby s.Datum, s.Startmoment, s.Eindmoment, r.Gebouw, r.Verdiep, r.Nummer
                               select new
                               {
                                   cmId = cm.Id,
                                   datum = s.Datum,
                                   startmoment = s.Startmoment,
                                   gebouw = r.Gebouw,
                                   verdiep = r.Verdiep,
                                   nummer = r.Nummer,
                                   vakcode = c.Vakcode,
                                   titel = c.Titel,
                                   eindmoment = s.Eindmoment,
                               };
                    foreach (var qry in _qry)
                    {
                        Planner planner = new Planner { cmId = qry.cmId, 
                                                        Datum = qry.datum, 
                                                        Startmoment = qry.startmoment, 
                                                        Gebouw = qry.gebouw, 
                                                        Verdiep = qry.verdiep, 
                                                        Nummer = qry.nummer, 
                                                        Vakcode = qry.vakcode, 
                                                        Titel = qry.titel, 
                                                        Eindmoment = qry.eindmoment};
                        planners.Add(planner);
                    }
                }
                else {
                    var _qry = from cm in db.CourseMoment
                               join c in db.Course on cm.CourseId equals c.Id
                               join s in db.Schedule on cm.ScheduleId equals s.Id
                               join r in db.Room on cm.RoomId equals r.Id
                               join u in db.Users on cm.ApplicationUserId equals u.Id
                               where (int)((s.Datum.DayOfYear / 7.0) + 0.2) == weekToUse
                               orderby s.Datum, s.Startmoment, s.Eindmoment, r.Gebouw, r.Verdiep, r.Nummer
                               select new
                               {
                                   cmId = cm.Id,
                                   cId = c.Id,
                                   datum = s.Datum,
                                   startmoment = s.Startmoment,
                                   gebouw = r.Gebouw,
                                   verdiep = r.Verdiep,
                                   nummer = r.Nummer,
                                   vakcode = c.Vakcode,
                                   titel = c.Titel,
                                   eindmoment = s.Eindmoment,
                                   lessenlijst = cm.LessenLijst
                               };
                    var lokaalQry = from lok in db.Room
                                    orderby lok.Gebouw, lok.Verdiep, lok.Nummer
                                    select lok;
                    var vakQry = from vak in db.Course
                                 orderby vak.Vakcode
                                 select vak;

                    foreach (var qry in _qry)
                    {
                        Planner planner = new Planner { cmId = qry.cmId, 
                                                        cId = qry.cId,
                                                        Datum = qry.datum, 
                                                        Startmoment = qry.startmoment, 
                                                        Gebouw = qry.gebouw, 
                                                        Verdiep = qry.verdiep, 
                                                        Nummer = qry.nummer, 
                                                        Vakcode = qry.vakcode, 
                                                        Titel = qry.titel, 
                                                        Eindmoment = qry.eindmoment,
                                                        LessenLijst = qry.lessenlijst};
                        planners.Add(planner);
                    }

                    foreach (var qry in lokaalQry)
                    {
                        Planner planner = new Planner { rId = qry.Id, Gebouw = qry.Gebouw, Verdiep = qry.Verdiep, Nummer = qry.Nummer, Capaciteit = qry.Capaciteit};
                        planners.Add(planner);
                    }

                    foreach (var qry in vakQry)
                    {
                        Planner planner = new Planner { cId = qry.Id, Vakcode = qry.Vakcode, Titel = qry.Titel};
                        planners.Add(planner);
                    }
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

            if (_duratie <=0) {
                TempData["error"] = "addError" + "/" + "De duratie mag niet negatief zijn, noch 0.";
                return RedirectToAction("Index", "Planner");
            }

            DateTime eindmoment = tijd.AddHours(_duratie);

            if (DubbeleBoeking(datum, tijd, eindmoment, lokaalId))
            {
                TempData["error"] = "addError" + "/" + "Dit lokaal wordt reeds gebruikt in een anders lesmoment op dit tijdstip.";
                return RedirectToAction("Index", "Planner");
            }

            try
            {
                //hier code schrijven zodat er niet altijd een nieuwe schedule wordt aangemaakt
                Schedule schedule = new Schedule { Datum = datum, Startmoment = tijd, Eindmoment = eindmoment };

                db.Schedule.Add(schedule);
                db.SaveChanges();

                var user = await userManager.GetUserAsync(User);

                CourseMoment moment = new CourseMoment { CourseId = vakid, ScheduleId = schedule.Id, RoomId = lokaalId, ApplicationUserId = user.Id, LessenLijst = lessenlijst};

                //hier code schrijven zodat er geen tweede dezelfde coursemoment aangemaakt kan worden

                db.CourseMoment.Add(moment);
                db.SaveChanges();

                if (checkbox != null && checkbox == true)
                {
                    if (DubbeleBoeking(datum, tijd, eindmoment, lokaal2Id))
                    {
                        TempData["error"] = "addError" + "/" + "Het lokaal voor de tweede les wordt reeds gebruikt in een anders lesmoment op dit tijdstip. Lesmoment 1 werd wel ingepland.";
                        return RedirectToAction("Index", "Planner");
                    }

                    CourseMoment moment2 = new CourseMoment { CourseId = vakid, ScheduleId = schedule.Id, RoomId = lokaal2Id, ApplicationUserId = user.Id, LessenLijst = lessenlijst};

                    //hier code schrijven zodat er geen tweede dezelfde coursemoment aangemaakt kan worden

                    db.CourseMoment.Add(moment2);
                    db.SaveChanges();
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
                    Planner planner = new Planner { rId = qry.Id, Gebouw = qry.Gebouw, Verdiep = qry.Verdiep, Nummer = qry.Nummer, Capaciteit = qry.Capaciteit};
                    planners.Add(planner);
                }
                foreach (var qry in vakQry) {
                    Planner planner = new Planner { cId = qry.Id, Vakcode = qry.Vakcode, Titel = qry.Titel};
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
        public ActionResult Delete(int cmId) {
            CourseMoment moment = db.CourseMoment.Find(cmId);
            if (moment == null) {
                TempData["error"] = "deleteError" + "/" + "Er is geen overeenkomend moment gevonden.";
                return RedirectToAction("Index", "Planner");
            }
            try
            {
                db.CourseMoment.Remove(moment);

                var cmuL = db.CourseMomentUsers.Where(e => e.CoursMomentId == moment.Id);

                if (cmuL.Any())
                {
                    foreach (var cmu in cmuL)
                    {
                        db.CourseMomentUsers.Remove(cmu);
                    }
                }
                db.SaveChanges();
            }
            catch(Exception e)
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
                CourseMoment oldMoment = db.CourseMoment.Find(cmId);
                if (oldMoment == null)
                {
                    TempData["error"] = "deleteError" + "/" + "Er is geen overeenkomend moment gevonden in de databank.";
                    return RedirectToAction("Index", "Planner");
                }

                if (oldMoment.RoomId != newLokaalid) 
                {
                    oldMoment.RoomId = db.Room.Find(newLokaalid).Id;
                    db.SaveChanges();
                }

                double _duratie = Convert.ToDouble(newDuratie);

                DateTime datum = DateTime.ParseExact(newDatum, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime tijd = new DateTime(1800, 1, 1, int.Parse(newStartMoment.Split(":")[0]), int.Parse(newStartMoment.Split(":")[1]), 0);
                DateTime eindmoment = tijd.AddHours(_duratie);

                if (oldMoment.Schedule.Datum != datum || oldMoment.Schedule.Startmoment != tijd || oldMoment.Schedule.Eindmoment != eindmoment) 
                {
                    // Deze lijn moet nog vervangen worden zodat er niet altijd een nieuwe reeds bestaande schedule wordt aangemaakt: Schedule schedule = db.Schedule.Find(datum, tijd,eindmoment);

                    Schedule schedule = new Schedule { Datum = datum, Startmoment = tijd, Eindmoment = eindmoment};

                    db.Schedule.Add(schedule);
                    db.SaveChanges();

                    oldMoment.ScheduleId = schedule.Id;
                    db.SaveChanges();
                }

                if (oldMoment.CourseId != newVakcode) 
                {
                    oldMoment.CourseId = db.Course.Find(newVakcode).Id;
                    db.SaveChanges();
                }

                oldMoment.LessenLijst = newLessenlijst;

                var user = await userManager.GetUserAsync(User);

                oldMoment.ApplicationUserId = user.Id;
                db.SaveChanges();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                TempData["error"] = "editError" + "/" + e.Message + " " + e.InnerException.Message == null ? " " : e.InnerException.Message;
                return RedirectToAction("Index","Planner");
            }
            TempData["error"] = "editGood";
            db.SaveChanges();
            return RedirectToAction("Index", "Planner");
        }

        [HttpGet]
        [Route("planner/viewTopic")]
        public ActionResult ViewTopic(int cmId)
        {
            try {
                var qryCm = from cm in db.CourseMoment
                            join c in db.Course on cm.CourseId equals c.Id
                            join s in db.Schedule on cm.ScheduleId equals s.Id
                            join r in db.Room on cm.RoomId equals r.Id
                            join u in db.Users on cm.ApplicationUserId equals u.Id
                            where cm.Id == cmId
                            select new
                            {
                                cmId = cm.Id,
                                cId = c.Id,
                                Datum = s.Datum,
                                Startmoment = s.Startmoment,
                                Eindmoment = s.Eindmoment,
                                Gebouw = r.Gebouw,
                                Verdiep = r.Verdiep,
                                Nummer = r.Nummer,
                                Vakcode = c.Vakcode,
                                Titel = c.Titel,
                                LessenLijst = cm.LessenLijst,
                                LectorRNum = u.UserName
                            };

                if (qryCm.Any())
                {
                    List<ApplicationUser> userList = new List<ApplicationUser>();

                    if (User.IsInRole("Lector") && User.Identity.Name == qryCm.FirstOrDefault().LectorRNum) 
                    {
                        //lijst van alle users die toegevoegd zijn aan coursemoment
                        var qryCU = from cmu in db.CourseMomentUsers
                                    join u in db.Users on cmu.ApplicationUserId equals u.Id
                                    orderby u.UserName
                                    where qryCm.FirstOrDefault().cmId == cmu.CoursMomentId
                                    select u;

                        foreach (var us in qryCU)
                        {
                            userList.Add(us);
                        }
                    }

                    Planner planner = new Planner
                    {
                        cmId = qryCm.FirstOrDefault().cmId,
                        Datum = qryCm.FirstOrDefault().Datum,
                        Startmoment = qryCm.FirstOrDefault().Startmoment,
                        Eindmoment = qryCm.FirstOrDefault().Eindmoment,
                        Gebouw = qryCm.FirstOrDefault().Gebouw,
                        Verdiep = qryCm.FirstOrDefault().Verdiep,
                        Nummer = qryCm.FirstOrDefault().Nummer,
                        Vakcode = qryCm.FirstOrDefault().Vakcode,
                        Titel = qryCm.FirstOrDefault().Titel,
                        LessenLijst = qryCm.FirstOrDefault().LessenLijst, 
                        RNummer = qryCm.FirstOrDefault().LectorRNum,
                        users = userList
                    };

                    if (TempData["error"] != null)
                    {
                        ViewBag.error = TempData["error"].ToString();
                        TempData["error"] = null;
                    }

                    return View("../Planning/ViewTopi", planner);
                }
                else 
                {
                    TempData["error"] = "topicError" + "/" + "Databank fout.";
                    return RedirectToAction("Index", "Planner");
                }
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
                var _qry = from cm in db.CourseMoment
                           join c in db.Course on cm.CourseId equals c.Id
                           join s in db.Schedule on cm.ScheduleId equals s.Id
                           join r in db.Room on cm.RoomId equals r.Id
                           where cm.CourseId == vakcode
                           where s.Datum >= DateTime.Now
                           orderby s.Datum, s.Startmoment, s.Eindmoment, r.Gebouw, r.Verdiep, r.Nummer
                           select new
                           {
                               datum = s.Datum,
                               startmoment = s.Startmoment,
                               gebouw = r.Gebouw,
                               verdiep = r.Verdiep,
                               nummer = r.Nummer,
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

        [HttpGet]
        public IActionResult EditStudsInCm(int cmId)
        {
            ViewBag.cmId = cmId;
            var model = new List<EditStudInCmViewModel>();

            var cm = db.CourseMoment.Find(cmId);

            if (cm == null) 
            {
                TempData["error"] = "Oops, het coursemoment id dat werd meegegeven is incorrect.";
                return RedirectToAction("ViewTopic", new { cmId = cmId });
            }

            //lijst van alle studenten die geaccepteerd zijn voor dit vak
            var qryu = from cu in db.CourseUser
                       join u in db.Users on cu.ApplicationUserId equals u.Id
                       orderby u.UserName
                       where cu.GoedGekeurd
                       where cu.CourseId == cm.CourseId
                       select u;

            foreach (var u in qryu) 
            {
                //checken of student "u" reeds in het vak zit, indien ja: IsSelected == true
                var qryCMU = from cmu in db.CourseMomentUsers
                             join us in db.Users on cmu.ApplicationUserId equals us.Id
                             where cmu.ApplicationUserId == u.Id
                             where cmu.CoursMomentId == cmId
                             select us;

                var editStudInCmViewModel = new EditStudInCmViewModel { userId = u.Id,
                                                                        Naam = u.Naam,
                                                                        VoorNaam = u.VoorNaam,
                                                                        RNum = u.UserName};

                if (qryCMU.Any())
                {
                    editStudInCmViewModel.IsSelected = true;
                }
                else 
                {
                    editStudInCmViewModel.IsSelected = false;
                }

                model.Add(editStudInCmViewModel);
            }

            return View("../Planning/EditStudsInCm", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditStudsInCm(List<EditStudInCmViewModel> model, int cmId)
        {
            int maxAmountStudsInCm = 0;

            var cmForMaxAmount = await db.CourseMoment.Include("Room").FirstOrDefaultAsync(cm => cm.Id == cmId);

            if (cmForMaxAmount == null)
            {
                ViewBag.ErrorMessage = "Coursemoment met lokaal niet gevonden.";
                return View("../Shared/NotFound");
            }
            else 
            {
                maxAmountStudsInCm = cmForMaxAmount.Room.Capaciteit;
            }

            var qryCMUAmount = from cmu in db.CourseMomentUsers
                               where cmu.CoursMomentId == cmId
                               select cmu;

            int counter = qryCMUAmount.Count();

            for (int i = 0; i < model.Count; i++)
            {
                var qryCMU = from cmu in db.CourseMomentUsers
                                where cmu.ApplicationUserId == model[i].userId
                                where cmu.CoursMomentId == cmId
                                select cmu;

                if (qryCMU.Any() && !model[i].IsSelected)
                {
                    db.CourseMomentUsers.Remove(db.CourseMomentUsers.Find(qryCMU.FirstOrDefault().Id));

                    db.SaveChanges();
                }
                else if (!qryCMU.Any() && model[i].IsSelected)
                {
                    counter++;

                    if (counter > maxAmountStudsInCm)
                    {
                        break;
                    }

                    CourseMomentUsers cmu = new CourseMomentUsers { ApplicationUserId = model[i].userId, CoursMomentId = cmId };

                    db.CourseMomentUsers.Add(cmu);

                    db.SaveChanges();
                }
                else
                {
                    continue;
                }
            }

            if (counter > maxAmountStudsInCm) 
            {
                TempData["error"] = "U heeft het maximum aantal studenten voor dit lokaal bereikt, indien u meer studenten had aangeduid werden deze niet toegelaten.";
            }

            return RedirectToAction("ViewTopic", new { cmId = cmId});
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult ViewCourseUsers(int vakcode) 
        {
            try
            {
                Course course = db.Course.Find(vakcode);

                CourseUsersViewModel cuvw = new CourseUsersViewModel
                {
                    cId = course.Id,
                    Vakcode = course.Vakcode,
                    Titel = course.Titel
                };

                cuvw.users = GetUsersSubForCourse(vakcode);

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
            var model = new List<EditStudInCmViewModel>();

            //lijst van alle studenten die geaccepteerd zijn voor dit vak
            var qryu = from cu in db.CourseUser
                       join u in db.Users on cu.ApplicationUserId equals u.Id
                       orderby u.UserName
                       where cu.GoedGekeurd
                       where cu.CourseId == vakcode
                       select u;

            foreach (var u in qryu) 
            {
                var editStudInCmViewModel = new EditStudInCmViewModel
                {
                    userId = u.Id,
                    Naam = u.Naam,
                    VoorNaam = u.VoorNaam,
                    RNum = u.UserName
                };

                if (GetUsersSubForCourse(vakcode).Contains(u)) 
                {
                    editStudInCmViewModel.IsSelected = true;
                }
                else 
                {
                    editStudInCmViewModel.IsSelected = false;
                }

                model.Add(editStudInCmViewModel);
            }

            return View("../Planning/EditStudsToEachCm", model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Lector")]
        public async Task<ActionResult> AddStudsToEachCm(List<EditStudInCmViewModel> model, int vakcode) 
        {
            try {
                foreach (var editStudInC in model)
                {
                    ApplicationUser user = db.Users.Find(editStudInC.userId);
                    if (editStudInC.IsSelected) //wanneer user is aangeklikt
                    {
                        if (!GetUsersSubForCourse(vakcode).Contains(user))
                        //user moet toegevoegd worden aan alle lesmomenten (voor dit vak) waarvoor hij nog niet is ingeschreven. Zonder capaciteit te overschrijden.
                        //Als capa overschreden is => error tonen + verdergaan 
                        //VB: Deze studenten konden niet toegevoegd worden aan bepaalde lesmomenten
                        //    door overschrijden capaciteit: M0000001, M00..... 
                        {
                            //alle lesmomenten opvragen van dit vak.
                            var qryCm = from cm in db.CourseMoment
                                        where cm.CourseId == vakcode
                                        select cm;

                            //lesmomenten aflopen
                            foreach (var cm in qryCm)
                            {
                                //Kijken of de user reeds ingeschreven is voor dit bepaalde lesmoment
                                var qryCmU = from cmu in db.CourseMomentUsers
                                             where cmu.ApplicationUserId == user.Id
                                             where cmu.CoursMomentId == cm.Id
                                             select cmu;

                                //indien dit niet het geval is => kijken of capaciteit van lokaal overschreden wordt bij inschrijven user.
                                if (!qryCmU.Any())
                                {
                                    var qryCmUAmount = from cmu in db.CourseMomentUsers
                                                       where cmu.CoursMomentId == cm.Id
                                                       select cmu;
                                    int counter = qryCmUAmount.Count();
                                    var temp = await db.CourseMoment.Include("Room").FirstOrDefaultAsync(cmt => cmt.Id == cm.Id);
                                    int maxAmountStuds = temp.Room.Capaciteit;

                                    //indien het niet overschreden wordt, user toevoegen.
                                    if (counter + 1 <= maxAmountStuds)
                                    {
                                        db.CourseMomentUsers.Add(new CourseMomentUsers { CoursMomentId = cm.Id, ApplicationUserId = user.Id });
                                    }
                                    else //tonen dat de student voor dit bepaald lesmoment niet toegevoegd kon worden.
                                    {
                                        TempData["Error"] += " " + user.UserName;
                                    }
                                }
                            }
                            db.SaveChanges();
                        }

                        //else do nothing -> stud reeds ingeschreven voor alle lesmomenten
                    }
                    else //wanneer user niet is aangeklikt
                    {
                        if (GetUsersSubForCourse(vakcode).Contains(user)) // user verwijderen uit alle courseMomentUsers
                        {
                            var qCmU = from cmu in db.CourseMomentUsers
                                       join cm in db.CourseMoment on cmu.CoursMomentId equals cm.Id
                                       where cmu.ApplicationUserId == user.Id
                                       where cm.CourseId == vakcode
                                       select cmu;

                            if (qCmU.Any())
                            {
                                foreach (var cmu in qCmU)
                                {
                                    db.CourseMomentUsers.Remove(db.CourseMomentUsers.Find(cmu.Id));
                                }
                                db.SaveChanges();
                            }
                        }
                        //else do nothing
                    }
                }
                return RedirectToAction("ViewCourseUsers", "Planner", new {vakcode});
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

        private bool DubbeleBoeking(DateTime datum, DateTime startmoment, DateTime eindmoment, int lokaalId)
        {
            // checken of voor dat lokaal er op die datum reeds een lesmoment is waar startmoment of eindmoment tussen dit startmoment of eindmoment valt.
            // indien dit het geval is, return true

            var qrySched = from cm in db.CourseMoment
                        join s in db.Schedule on cm.ScheduleId equals s.Id
                        where cm.RoomId == lokaalId
                        where s.Datum >= DateTime.Today
                        select s;

            if (!qrySched.Any()) 
            {
                return false;
            }
            else 
            {
                foreach (var sched in qrySched) 
                {
                    if (sched.Datum == datum) 
                    {
                        //valt het startmoment ertussen? vb : (14<16 && 17 > 16)
                        if (sched.Startmoment < startmoment && sched.Eindmoment > startmoment)
                        {
                            return true;
                        }
                        // valt het eindmoment ertussen? vb : ()
                        else if (sched.Startmoment > startmoment && sched.Startmoment < eindmoment)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private List<ApplicationUser> GetUsersSubForCourse(int vakcode) 
        {
            List<ApplicationUser> userList = new List<ApplicationUser>();

            var cms = from CrsM in db.CourseMoment
                      where CrsM.CourseId == vakcode
                      select CrsM;

            var users = (from us in db.CourseUser
                         join uRol in db.UserRoles on us.ApplicationUserId equals uRol.UserId
                         join rol in db.Roles on uRol.RoleId equals rol.Id
                         join u in db.Users on us.ApplicationUserId equals u.Id
                         where rol.NormalizedName == "STUDENT"
                         select u).Distinct().OrderBy(user => user.UserName);

            if (cms.Any())
            {
                if (users.Any())
                {
                    foreach (var user in users)
                    {
                        try
                        {
                            foreach (var cm in cms)
                            {
                                var temp = from crsmus in db.CourseMomentUsers
                                           where crsmus.CoursMomentId == cm.Id
                                           where crsmus.ApplicationUserId == user.Id
                                           select crsmus;

                                if (!temp.Any())
                                {
                                    throw new Exception("Student zit niet in ��n van de lesmomenten");
                                }
                            }
                            userList.Add(user);
                        }
                        catch (Exception e)
                        { }
                    }
                }
            }
            return userList;
        }
    }
}