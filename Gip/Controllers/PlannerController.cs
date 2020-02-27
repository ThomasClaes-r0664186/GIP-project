using Gip.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

namespace Gip.Controllers
{
    public class PlannerController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();

        // GET /planner
        [HttpGet]
        [Route("planner")]
        public ActionResult Index()
        {
            try
            {
                var _qry = from cm in db.CourseMoment
                           join c in db.Course on cm.Vakcode equals c.Vakcode
                           join s in db.Schedule
                                on new { cm.Datum, cm.Startmoment }
                                equals new { s.Datum, s.Startmoment }

                           where (cm.Datum.DayOfYear / 7) == (DateTime.Now.DayOfYear / 7)
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
                return View(planners);
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
            DateTime datum = DateTime.ParseExact(dat, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime tijd = new DateTime(0001, 1, 1, int.Parse(uur.Split(":")[0]), int.Parse(uur.Split(":")[1]), 0);
            lokaalId = lokaalId.Trim() + " ";
            string gebouw = lokaalId.Substring(0, 1);
            int verdieping = int.Parse(lokaalId.Substring(1, 1));
            string nummer = lokaalId.Substring(2, (lokaalId.Length - 2));

            try
            {
                CourseMoment moment = new CourseMoment();
                moment.Vakcode = vakcode;
                moment.Datum = datum;
                moment.Startmoment = tijd;
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
            return View();
        }
    }
}