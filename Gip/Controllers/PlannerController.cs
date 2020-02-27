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
            var _qry = from cm in db.CourseMoment
                       join c in db.Course on cm.Vakcode equals c.Vakcode
                       join s in db.Schedule
                            on new { cm.Datum, cm.Startmoment }
                            equals new { s.Datum, s.Startmoment }

                       where (cm.Datum.DayOfYear/7) == (DateTime.Now.DayOfYear/7)
                       select new { 
                            datum =cm.Datum,
                            startmoment = cm.Startmoment,
                            gebouw = cm.Gebouw,
                            verdiep = cm.Verdiep,
                            nummer = cm.Nummer,
                            vakcode = c.Vakcode,
                            titel = c.Titel,
                            eindmoment = s.Eindmoment
                       };

            List<Planner> planners = new List<Planner>();
            foreach(var qry in _qry){
                Planner planner = new Planner(qry.datum, qry.startmoment, qry.gebouw, qry.verdiep, qry.nummer, qry.vakcode, qry.titel, qry.eindmoment);
                planners.Add(planner);
            }
            return View(planners);
        }

        [HttpPost]
        [Route("planner/add")]
        public ActionResult Add(string dat, string uur, string lokaalId, double duratie, string titel, string lessenlijst) {
            DateTime datum = DateTime.ParseExact(dat, "yyyy-MM-dd", CultureInfo.InvariantCulture);


            return View();
        }
    }
}

/**
 * [HttpPost]
   [Route("vak/add")]
 * public ActionResult Add(string vakcode, string titel, int studiepunten)
        { 
            try{
                Course course = new Course();
                course.Vakcode = vakcode;
                course.Titel = titel;
                course.Studiepunten = studiepunten;
                db.Course.Add(course);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                ViewBag.error = "addError";
                return RedirectToAction("Index", "Lokaal");
            }
            ViewBag.error = "addGood";
            db.SaveChanges();
            return RedirectToAction("Index", "Vak");
        }
 * 
 **/
