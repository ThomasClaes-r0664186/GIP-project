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

        // POST /add /vak
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
    }
}