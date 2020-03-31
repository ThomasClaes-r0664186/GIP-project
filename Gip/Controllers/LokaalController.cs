using Gip.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Gip.Controllers
{
    [Authorize(Roles="Admin,Lector")]
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
                var qry = from d in db.Room
                    orderby d.Gebouw, d.Verdiep, d.Nummer
                    select d;

                if (TempData["error"] != null) {
                    ViewBag.error = TempData["error"].ToString();
                    TempData["error"] = null;
                }
                if (ViewBag.error == null || !ViewBag.error.Contains("addError") && !ViewBag.error.Contains("addGood") && !ViewBag.error.Contains("deleteError") && !ViewBag.error.Contains("deleteGood") && !ViewBag.error.Contains("editError") && !ViewBag.error.Contains("editGood"))
                {
                    ViewBag.error = "indexLokaalGood";
                }
                return View(qry);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ViewBag.error = "indexLokaalError" + "/" + "Er is een fout opgetreden bij het opvragen van het overzicht.";
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
                var rInUse = from d in db.Room
                             where d.Gebouw == gebouw
                             where d.Verdiep == verdiep
                             where d.Nummer == nummer
                             select d;

                if (rInUse.Any())
                {
                    TempData["error"] = "addError" + "/" + "Dit lokaal bestaat reeds.";
                    return RedirectToAction("Index", "Lokaal");
                }

                Room room = new Room { Gebouw = gebouw.ToUpper() , Verdiep = verdiep, Nummer = nummer, Type = type, Capaciteit = capaciteit, Middelen = middelen};
                db.Room.Add(room);
                
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TempData["error"] = "addError" + "/" + e.Message;
                return RedirectToAction("Index", "Lokaal");
            }
            TempData["error"] = "addGood";
            return RedirectToAction("Index", "Lokaal");
        }
        
        [HttpGet]
        [Route("lokaal/add")]
        public ActionResult Add()
        {
            return View();
        }

        //fixed, verwijder bij gebruik lesmoment?
        [HttpPost]
        [Route("lokaal/delete")]
        public ActionResult Delete(int lokaalId)
        {
            if (lokaalId < 0)
            {
                TempData["error"] = "deleteError" + "/" + "LokaalId werd verkeerd meegegeven.";
                return RedirectToAction("Index", "Lokaal");
            }

            Room room = db.Room.Find(lokaalId);

            if (room == null)
            {
                TempData["error"] = "deleteError" + "/" + "Dit lokaal werd niet in de databank gevonden.";
                return RedirectToAction("Index", "Lokaal");
            }

            try
            {
                db.Room.Remove(room);
                db.SaveChanges();
            }
            catch (Exception)
            {
                TempData["error"] = "deleteError" + "/" + "Dit lokaal is gebruikt in een lesmoment, pas dat eerst aan voordat je dit kan verwijderen.";
                return RedirectToAction("Index", "Lokaal");
            }
            TempData["error"] = "deleteGood";
            return RedirectToAction("Index", "Lokaal");
        }

        [HttpGet]
        [Route("lokaal/edit")]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [Route("lokaal/edit")]
        public ActionResult Edit(int lokaalId, string gebouw, int verdiep, string nummer, string type, int capaciteit, string middelen)
        {
            TempData["error"] = "";
            try
            {
                if (lokaalId < 0)
                {
                    TempData["error"] = "editError" + "/" + "LokaalId werd verkeerd meegegeven.";
                    return RedirectToAction("Index", "Lokaal");
                }

                var rInUse = from d in db.Room
                             where d.Gebouw == gebouw
                             where d.Verdiep == verdiep
                             where d.Nummer == nummer
                             select d;

                if (rInUse.Any()) 
                {
                    TempData["error"] = "editError" + "/" + "Dit lokaal bestaat reeds.";
                    return RedirectToAction("Index", "Lokaal");
                }

                Room room = db.Room.Find(lokaalId);
                Room newRoom = new Room { Gebouw = gebouw.Trim(), Verdiep = verdiep , Nummer = nummer, Type = type, Capaciteit = capaciteit, Middelen = middelen};

                room.Gebouw = newRoom.Gebouw;
                room.Verdiep = newRoom.Verdiep;
                room.Nummer = newRoom.Nummer;
                room.Type = newRoom.Type;
                room.Capaciteit = newRoom.Capaciteit;
                room.Middelen = newRoom.Middelen;

                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["error"] = "editError" + "/" + e.Message;
                return RedirectToAction("Index", "Lokaal");
            }
            TempData["error"] = "editGood";
            return RedirectToAction("Index", "Lokaal");
        }
    }
}
 