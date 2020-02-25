using Gip.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web.Mvc;

namespace Gip.Controllers
{
    public class LokaalController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();
        // GET
        [HttpGet]
        [Route("lokaal")]
        public ActionResult Index()
        {
            var qry = from d in db.Room 
                      orderby d.Gebouw, d.Verdiep, d.Nummer 
                      select new {
                          /*d.Gebouw, 
                          d.Verdiep, 
                          d.Nummer,*/
                          lokaalId = d.Gebouw + d.Verdiep + d.Nummer,
                          d.Type,
                          d.Capaciteit,
                          d.Middelen
                      };

            return View(qry);
        }

        // POST /add/lokaal
        [HttpPost]
        [Route("lokaal/add")]
        public ActionResult Add(string gebouw, int verdiep, string nummer, string type, int capaciteit, string middelen )
        {
            Room room = new Room();
            room.Gebouw = gebouw;
            room.Verdiep = verdiep;
            room.Nummer = nummer;
            room.Type = type;
            room.Capaciteit = capaciteit;
            room.Middelen = middelen;
            db.Room.Add(room);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        [Route("lokaal/add")]
        public ActionResult Add()
        {
            return View();
        }
        
        [HttpPost]
        [Route("lokaal/delete")]
        public ActionResult Delete(string lokaalId)
        {
            //
        }

        [HttpGet]
        [Route("lokaal/delete")]
        public ActionResult Delete(string lokaalId)
        {
            if (lokaalId == null || lokaalId.Trim().Equals(""))
            {
                return "iets";
            }
            Room room = db.Room.Find(lokaalId);
            if (room == null)
            {
                return "iets anders";
            }
            return View(room);
        }

        [HttpGet]
        [Route("lokaal/edit")]
        public ActionResult Edit()
        {
            return View();
        }
    }
}