using Gip.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class AddController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();

        // GET /add/lokaal
        [HttpGet]
        public IActionResult Lokaal()
        {
            return View();
        }

        // POST /add/vak
        [HttpPost]
        public IActionResult Vak(string vakcode, string titel, int studiepunten)
        { 
            Course course = new Course();
            course.Vakcode = vakcode;
            course.Titel = titel;
            course.Studiepunten = studiepunten;
            db.Course.Add(course);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        
        // POST /add/lokaal
        [HttpPost]
        public IActionResult Lokaal(string gebouw, int verdiep, string nummer, string type, int capaciteit, string middelen )
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
        
    }
}