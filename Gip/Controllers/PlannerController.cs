using Gip.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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
                            titel = c.Titel,
                            eindmoment = s.Eindmoment
                       };
            return View(_qry);
        }
    }
}
