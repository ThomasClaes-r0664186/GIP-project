using Gip.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Gip.Controllers
{
    public class VakController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();
        
        // GET
        [HttpGet]
        [Route("vak")]
        public ActionResult Index()
        {
            var qry = from d in db.Course
                      orderby d.Vakcode
                      select d;

            return View(qry);
        }

        [HttpPost]
        [Route("vak/add")]
        public ActionResult Add(string vakcode, string titel, int studiepunten)
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
        public ActionResult Delete()
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