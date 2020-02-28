using Gip.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace Gip.Controllers
{
    public class PlannerController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();

        // GET /planner
        [HttpGet]
        [Route("planner")]
        public ActionResult Index(int week)
        {
            int weekToUse = ((DateTime.Now.DayOfYear / 7) + week);
            try
            {
                var _qry = from cm in db.CourseMoment
                           join c in db.Course on cm.Vakcode equals c.Vakcode
                           join s in db.Schedule
                                on new { cm.Datum, cm.Startmoment }
                                equals new { s.Datum, s.Startmoment }
                                //we hebben aan week 52+1 gedaan, maar vonden het niet en het was al laat op den dag
                           where (cm.Datum.DayOfYear / 7) == weekToUse
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
                ViewBag.nextWeek = week += 1;
                ViewBag.prevWeek = week -= 2;
                return View("../Planning/Index",planners);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ViewBag.error = "indexVakError";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Route("planner/add")]
        public ActionResult Add(string dat, string uur, string lokaalId, double duratie, string vakcode, string lessenlijst)
        {
            double _duratie = Convert.ToDouble(duratie);
            DateTime datum = DateTime.ParseExact(dat, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime tijd = new DateTime(1800, 1, 1, int.Parse(uur.Split(":")[0]), int.Parse(uur.Split(":")[1]), 0);
            lokaalId = lokaalId.Trim() + " ";
            string gebouw = lokaalId.Substring(0, 1);
            int verdieping = int.Parse(lokaalId.Substring(1, 1));
            string nummer = lokaalId.Substring(2, (lokaalId.Length - 2));

            try
            {
                Schedule schedule = db.Schedule.Find(datum, tijd);
                if (schedule == null) {
                    schedule = new Schedule();
                    schedule.Datum = datum;
                    schedule.Startmoment = tijd;
                    schedule.Eindmoment = tijd.AddHours(_duratie);

                    db.Schedule.Add(schedule);
                    db.SaveChanges();
                }

                CourseMoment moment = new CourseMoment();
                moment.Vakcode = vakcode;
                moment.Datum = datum;
                moment.Startmoment = schedule.Startmoment;
                moment.Gebouw = gebouw;
                moment.Verdiep = verdieping;
                moment.Nummer = nummer;
                moment.Userid = "r0664186";
                moment.LessenLijst = lessenlijst;

                db.CourseMoment.Add(moment);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ViewBag.error = "addError";
                return RedirectToAction("Index", "Planner");
            }
            ViewBag.error = "addGood";
            db.SaveChanges();
            return RedirectToAction("Index", "Planner");
        }

        [HttpGet]
        [Route("planner/add")]
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
                    Planner planner = new Planner(qry.Gebouw, qry.Verdiep, qry.Nummer);
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
                ViewBag.error = "indexVakError";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Route("planner/delete")]
        public ActionResult Delete(string vakcode, DateTime datum, DateTime startMoment, string gebouw, int verdiep, string nummer) {
            DateTime newStartMoment = new DateTime(1800, 1, 1, startMoment.Hour, startMoment.Minute, startMoment.Second);
            CourseMoment moment = db.CourseMoment.Find(vakcode, datum, newStartMoment, gebouw, verdiep, nummer, "r0664186");
            if (moment == null) {
                ViewBag.error = "deleteError";
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
                ViewBag.error = "coursemomentsDeleteError";
                return RedirectToAction("Index", "Planner");
            }
            ViewBag.error = "coursemomentDeletedCorrectly";
            return RedirectToAction("Index", "Planner");
        }

        [HttpPost]
        [Route("planner/edit")]
        public ActionResult Edit(string oldVakcode, 
            DateTime oldDatum, DateTime oldStartMoment, 
            string oldGebouw, int oldVerdiep, string oldNummer, 
            string newVakcode, 
            string newDatum, string newStartMoment, double duratie,
            string newGebouw, int newVerdiep, string newNummer, string newLessenlijst) {
            //new vakcode => dropdown met lokalen, datetime => checken of bestaat anders aanmaken.

            try
            {
                CourseMoment oldMoment = db.CourseMoment.Find(oldVakcode, oldDatum, oldStartMoment, oldGebouw, oldVerdiep, oldNummer, "r0664186");
                if (oldMoment == null)
                {
                    ViewBag.error = "deleteError";
                    return RedirectToAction("Index", "Planner");
                }
                else
                {
                    db.CourseMoment.Remove(oldMoment);
                }

                DateTime datum = DateTime.ParseExact(newDatum, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime tijd = new DateTime(1800, 1, 1, int.Parse(newStartMoment.Split(":")[0]), int.Parse(newStartMoment.Split(":")[1]), 0);
                Schedule schedule = db.Schedule.Find(datum, tijd);

                if (schedule == null)
                {
                    schedule = new Schedule();
                    schedule.Datum = datum;
                    schedule.Startmoment = tijd;
                    schedule.Eindmoment = tijd.AddHours(duratie);

                    db.Schedule.Add(schedule);
                    db.SaveChanges();
                }

                CourseMoment newMoment = new CourseMoment(newVakcode, datum, tijd, newGebouw, newVerdiep, newNummer, "r0664186", newLessenlijst);
                db.CourseMoment.Add(newMoment);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                ViewBag.error = "coursemomentEditError";
                return RedirectToAction("Index","Planner");
            }
            ViewBag.error = "addGood";
            db.SaveChanges();
            return RedirectToAction("Index", "Planner");
        }

        [HttpGet]
        [Route("planner/viewTopic")]
        public ActionResult ViewTopic(string vakcode, DateTime datum, DateTime startMoment, string gebouw, int verdiep, string nummer)
        {
            try {
                CourseMoment oldMoment = db.CourseMoment.Find(vakcode, datum, startMoment, gebouw, verdiep, nummer, "r0664186");
                return View("../Planning/ViewTopi", oldMoment);
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return RedirectToAction("Index", "Planner");
            }
        }

        [HttpGet]
        [Route("planner/viewCourseMoments")]
        public ActionResult ViewCourseMoments(string vakcode)
        {
            try
            {
                var qry = from cm in db.CourseMoment
                          where cm.Vakcode == vakcode
                          select cm;
                ViewBag.error = "coursemomentsGood";
                return View(qry);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ViewBag.error = "coursmomentsError";
                return RedirectToAction("Index", "Planner");
            }
        }

    }
}