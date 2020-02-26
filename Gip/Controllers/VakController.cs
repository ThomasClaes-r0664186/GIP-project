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
            return RedirectToAction("Index", "Vak");
        }

        [HttpGet]
        [Route("vak/add")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Route("vak/delete")]
        public ActionResult Delete(string vakcode)
        {
            if (vakcode == null || vakcode.Trim().Equals("")) {
                ViewBag.error = true;
                return NotFound();
            }

            Course course = db.Course.Find(vakcode);

            if (course == null) {
                ViewBag.error = true;
                return RedirectToAction("Index", "Vak");
            }
            db.Course.Remove(course);
            db.SaveChanges();
            ViewBag.error = false;
            return RedirectToAction("Index", "Vak");
        }
        
        [HttpPost]
        [Route("vak/edit")]
        public ActionResult Edit(string vakcodeOld, string vakcodeNew, string titel, int studiepunten)
        {
            if (vakcodeOld == null || vakcodeOld.Trim().Equals(""))
            {
                ViewBag.error = true;
                return NotFound();
            }

            Course course = db.Course.Find(vakcodeOld);
            Delete(vakcodeOld);

            try
            {
            }
            catch (Exception) { 
            
            }

            return View();
        }
    }
    }