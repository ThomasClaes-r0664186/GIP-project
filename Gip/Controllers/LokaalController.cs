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
            try
            {
                //var gebouwlst = new List<string>();
                var _qry = from d in db.Room
                    orderby d.Gebouw, d.Verdiep, d.Nummer
                    select d;
                ViewBag.error = "none";
                return View(qry);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                ViewBag.error = "indexLokaal";
                return RedirectToAction("Index", "Home");
            }
            
        }

        // POST /add/lokaal
        [HttpPost]
        [Route("lokaal/add")]
        public ActionResult Add(string gebouw, int verdiep, string nummer, string type, int capaciteit, string middelen )
        {
            try
            {
                Room room = new Room();
                room.Gebouw = gebouw.ToUpper();
                room.Verdiep = verdiep;
                room.Nummer = nummer;
                room.Type = type;
                room.Capaciteit = capaciteit;
                room.Middelen = middelen;
                db.Room.Add(room);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                ViewBag.error = "add";
                return RedirectToAction("Index", "Lokaal");
            }
            db.SaveChanges();
            ViewBag.error = "none";
            return RedirectToAction("Index", "Lokaal");
        }
        
        [HttpGet]
        [Route("lokaal/add")]
        public ActionResult Add()
        {
            ViewBag.error = "none";
            return View();
        }

        [HttpPost]
        [Route("lokaal/delete")]
        public ActionResult Delete(string lokaalId)
        {
            if (lokaalId == null || lokaalId.Trim().Equals(""))
            {   
                ViewBag.error = "delete"; 
                return RedirectToAction("Index", "Lokaal");
            }
            lokaalId = lokaalId.Trim() + " ";
            string gebouw = lokaalId.Substring(0, 1);
            int verdieping = int.Parse(lokaalId.Substring(1,1));
            string nummer = lokaalId.Substring(2, (lokaalId.Length-2));
            
            Room room = db.Room.Find(gebouw,verdieping,nummer);

            if (room == null)
            {
                ViewBag.error = "delete";
                return RedirectToAction("Index", "Lokaal");
            }

            db.Room.Remove(room);
            db.SaveChanges();
            ViewBag.error = "none";
            return RedirectToAction("Index", "Lokaal");
        }

        [HttpPost]
        [Route("lokaal/edit")]
        public ActionResult Edit(string lokaalId, string gebouw, int verdiep, string nummer, string type, int capaciteit, string middelen)
        {
            gebouw = gebouw.ToUpper();
            if (lokaalId == null || lokaalId.Trim().Equals(""))
            {
                ViewBag.error = "edit";
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
                room.Gebouw = gebouw;
                room.Verdiep = verdiep;
                room.Nummer = nummer;
                room.Type = type;
                room.Capaciteit = capaciteit;
                room.Middelen = middelen;                    
            }
            catch (Exception)
            {
                ViewBag.error = "edit";
                return View();
            }
            db.Room.Add(room);
            db.SaveChanges();
            ViewBag.error = "none";
            return RedirectToAction("Index", "Lokaal");
        }
    }
}