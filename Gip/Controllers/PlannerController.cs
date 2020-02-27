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
                            cm.Datum,
                            cm.Startmoment,
                            cm.Gebouw,
                            cm.Verdiep,
                            cm.Nummer,
                            c.Titel,
                            s.Eindmoment
                       };
            return View(_qry);
        }
    }
}

/* try
            {
                var _qry = from d in db.Room
                    orderby d.Gebouw, d.Verdiep, d.Nummer
                    select d;
                ViewBag.error = "indexLokaalGood";
                return View(qry);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                ViewBag.error = "indexLokaalError";
                return RedirectToAction("Index", "Home");
            }

    var klant = from c in db.Customers
                join o in db.Orders on c.CustomerId equals o.CustomerId
                join d in db.OrderDetails on o.OrderId equals d.OrderId
                where c.CustomerId == costumerInQuestion
                select new {
                    aantal = d.UnitPrice * d.Quantity, 
                    orderId = o.OrderId
                };

    var orders = from o in db.Orders
                join c in db.Customers on o.CustomerId equals c.CustomerId
                join e in db.Employees on o.EmployeeId equals e.EmployeeId
                select new
                {
                    o.OrderId,
                    o.CustomerId,
                    c.CompanyName,
                    e.FirstName
                };*/
