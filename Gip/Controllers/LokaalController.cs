using Gip.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;

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
            //var gebouwlst = new List<string>();
            var qry = from d in db.Room
                      orderby d.Gebouw, d.Verdiep, d.Nummer
                      select d;
    
            return View(qry);
        }

        // POST /add/lokaal
        [HttpPost]
        [Route("lokaal/add")]
        public ActionResult Add(string gebouw, int verdiep, string nummer, string type, int capaciteit, string middelen )
        {
            Room room = new Room();
            room.Gebouw = gebouw.ToUpper();
            room.Verdiep = verdiep;
            room.Nummer = nummer;
            room.Type = type;
            room.Capaciteit = capaciteit;
            room.Middelen = middelen;
            db.Room.Add(room);
            db.SaveChanges();
            return RedirectToAction("Index", "Lokaal");
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
            if (lokaalId == null || lokaalId.Trim().Equals(""))
            {   
                ViewBag.error = true; 
                return NotFound();
            }
            lokaalId = lokaalId.Trim() + " ";
            string gebouw = lokaalId.Substring(0, 1);
            int verdieping = int.Parse(lokaalId.Substring(1,1));
            string nummer = lokaalId.Substring(2, (lokaalId.Length-2));
            
            Room room = db.Room.Find(gebouw,verdieping,nummer);

            if (room == null)
            {
                ViewBag.error = true;
                return RedirectToAction("Index", "Lokaal");
            }

            db.Room.Remove(room);
            db.SaveChanges();
            ViewBag.error = false;
            return RedirectToAction("Index", "Lokaal");
        }

        [HttpPost]
        [Route("lokaal/edit")]
        public ActionResult Edit(string lokaalId, string gebouw, int verdiep, string nummer, string type, int capaciteit, string middelen)
        {
            gebouw = gebouw.ToUpper();
            if (lokaalId == null || lokaalId.Trim().Equals(""))
            {
                ViewBag.error = true;
                return NotFound();
            }

            lokaalId = lokaalId.Trim() + " ";
            string gebouwId = lokaalId.Substring(0, 1);
            int verdieping = int.Parse(lokaalId.Substring(1, 1));
            string nummerOld = lokaalId.Substring(2, (lokaalId.Length - 2));

            //omdat we de room sowieso deleten, kunnen we het eigenlijk zonder de changed bool doen, reminder Thomas
            Room room = db.Room.Find(gebouwId, verdieping, nummerOld);
            Delete(lokaalId);

            try
            {
                room.Gebouw = gebouw;
                room.Verdiep = verdiep;
                room.Nummer = nummer;
                room.Type = type;
                room.Capaciteit = capaciteit;
                room.Middelen = middelen;                    
            }
            catch (Exception)
            {
                ViewBag.error = true;
                return View();
            }
            db.Room.Add(room);
            db.SaveChanges();
            ViewBag.error = false;
            return RedirectToAction("Index", "Lokaal");
        }
    }
}