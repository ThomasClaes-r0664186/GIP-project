using Gip.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
            var gebouwlst = new List<string>();
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
                return NotFound();
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
            bool changed = false;
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

            Room room = db.Room.Find(gebouwId, verdieping, nummerOld);
            Delete(lokaalId);

            try
            {
                if (!room.Gebouw.Equals(gebouw))
                {
                    room.Gebouw = gebouw;
                    changed = true;
                }
                if (!room.Verdiep.Equals(verdiep))
                {
                    room.Verdiep = verdiep;
                    changed = true;
                }
                if (!room.Nummer.Equals(nummer))
                {
                    room.Nummer = nummer;
                    changed = true;
                }
                if (!room.Type.Equals(type))
                {
                    room.Type = type;
                    changed = true;
                }
                if (!room.Capaciteit.Equals(capaciteit))
                {
                    room.Capaciteit = capaciteit;
                    changed = true;
                }
                if (!room.Middelen.Equals(middelen))
                {
                    room.Middelen = middelen;
                    changed = true;
                }
            }
            catch (Exception)
            {
                ViewBag.error = true;
                return View();
            }
            if (changed) {
                db.Room.Add(room);
                db.SaveChanges();
            }
            ViewBag.error = false;
            return View();
        }
    }
}