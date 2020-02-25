using Gip.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class LokaalController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();
        // GET
        public IActionResult Index()
        {
            return View();
        }
        // POST /add/lokaal
        [HttpPost]
        [Route("lokaal/add")]
        public IActionResult Add(string gebouw, int verdiep, string nummer, string type, int capaciteit, string middelen )
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
        public IActionResult Add()
        {
            return View();
        }
        
        [HttpGet]
        [Route("lokaal/delete")]
        public IActionResult Delete()
        {
            return View();
        }
        
        [HttpGet]
        [Route("lokaal/edit")]
        public IActionResult Edit()
        {
            return View();
        }
    }
}