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
                //var gebouwlst = new List<string>();
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
                Room room = new Room();
                room.Gebouw = gebouw.ToUpper();
                room.Verdiep = verdiep;
                room.Nummer = nummer;
                room.Type = type;
                room.Capaciteit = capaciteit;
                room.Middelen = middelen;
                db.Room.Add(room);
                
                db.SaveChanges();
            }
            catch (Exception e)
            {
                if (e.InnerException != null && e.InnerException.Message.ToLower().Contains("primary"))
                {
                    TempData["error"] = "addError" + "/" + "De combinatie van gebouw, verdiep en nummer die u heeft ingegeven, is reeds in gebruik. Gelieve een andere combinatie te gebruiken.";
                    return RedirectToAction("Index", "Lokaal");
                }
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

        [HttpPost]
        [Route("lokaal/delete")]
        public ActionResult Delete(string lokaalId)
        {
            if (lokaalId == null || lokaalId.Trim().Equals(""))
            {
                TempData["error"] = "deleteError" + "/" + "LokaalId werd leeg meegegeven.";
                return RedirectToAction("Index", "Lokaal");
            }
            lokaalId = lokaalId.Trim() + " ";
            string gebouw = lokaalId.Substring(0, 1);
            int verdieping = int.Parse(lokaalId.Substring(1,1));
            string nummer = lokaalId.Substring(2, (lokaalId.Length-2));
            
            Room room = db.Room.Find(gebouw,verdieping,nummer);

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
        public ActionResult Edit(string lokaalId, string gebouw, int verdiep, string nummer, string type, int capaciteit, string middelen)
        {
            TempData["error"] = "";
            try
            {
                gebouw = gebouw.ToUpper();
                if (lokaalId == null || lokaalId.Trim().Equals(""))
                {
                    TempData["error"] = "editError" + "/" + "De oude lokaal id is niet goed doorgegeven want deze is leeg.";
                    return RedirectToAction("Index", "Lokaal");
                }

                lokaalId = lokaalId.Trim() + " ";
                string gebouwId = lokaalId.Substring(0, 1);
                int verdieping = int.Parse(lokaalId.Substring(1, 1));
                string nummerOld = lokaalId.Substring(2, (lokaalId.Length - 2));

                Room room = db.Room.Find(gebouwId, verdieping, nummerOld);
                Room newRoom = new Room();

                newRoom.Gebouw = gebouw.Trim();
                newRoom.Verdiep = verdiep;
                newRoom.Nummer = nummer;
                newRoom.Type = type;
                newRoom.Capaciteit = capaciteit;
                newRoom.Middelen = middelen;

                if (room.Gebouw.Trim().Equals(newRoom.Gebouw) && room.Verdiep.Equals(newRoom.Verdiep) && room.Nummer.Equals(newRoom.Nummer))
                {
                    db.Room.Find(room.Gebouw, room.Verdiep, room.Nummer).Type = newRoom.Type;
                    db.Room.Find(room.Gebouw, room.Verdiep, room.Nummer).Capaciteit = newRoom.Capaciteit;
                    db.Room.Find(room.Gebouw, room.Verdiep, room.Nummer).Middelen = newRoom.Middelen;
                    db.SaveChanges();
                }
                else 
                {
                    db.Room.Add(newRoom);
                    db.SaveChanges();
                    Delete(lokaalId);
                }
                if (TempData["error"].ToString().Contains("deleteError"))
                {
                    TempData["error"] = "editError" + "/" + "Dit lokaal werd gebruikt in een lesmoment. Er werd een nieuw lokaal aangemaakt met de nieuwe waarden, gelieve dit aan te passen in de planning.";
                    return RedirectToAction("Index", "Lokaal");
                }
            }
            catch (Exception e)
            {
                if (e.InnerException != null && e.InnerException.Message.ToLower().Contains("primary"))
                {
                    TempData["error"] = "addError" + "/" + "De combinatie van gebouw, verdiep en nummer die u heeft ingegeven, is reeds in gebruik. Gelieve een andere combinatie te gebruiken.";
                    return RedirectToAction("Index", "Lokaal");
                }
                Console.WriteLine(e.Message);
                TempData["error"] = "editError" + "/" + e.Message;
                return RedirectToAction("Index", "Lokaal");
            }
            TempData["error"] = "editGood";
            return RedirectToAction("Index", "Lokaal");
        }
    }
}
 