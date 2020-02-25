using Gip.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class VakController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();
        // GET
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("vak/add")]
        public IActionResult Add(string vakcode, string titel, int studiepunten)
        { 
            Course course = new Course();
            course.Vakcode = vakcode;
            course.Titel = titel;
            course.Studiepunten = studiepunten;
            db.Course.Add(course);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        [Route("vak/delete")]
        public IActionResult Delete()
        {
            
            return View();
        }
        
        [HttpGet]
        [Route("vak/edit")]
        public ActionResult Edit()
        {
            return View();
        }
    }
    }