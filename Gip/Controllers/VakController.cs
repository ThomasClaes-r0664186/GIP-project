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
            try{
                var qry = from d in db.Course
                          orderby d.Vakcode
                          select d;
                ViewBag.error = "indexVakGood";
                return View(qry);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ViewBag.error = "indexVakError";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Route("vak/add")]
        public ActionResult Add(string vakcode, string titel, int studiepunten)
        {
            try{
                Course course = new Course();
                course.Vakcode = vakcode;
                course.Titel = titel;
                course.Studiepunten = studiepunten;
                db.Course.Add(course);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ViewBag.error = "addError";
                return RedirectToAction("Index", "Vak");
            }
            ViewBag.error = "addGood";
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
            if (vakcode == null || vakcode.Trim().Equals(""))
            {
                ViewBag.error = "deleteError";
                return  RedirectToAction("Index", "Vak");
            }

            Course course = db.Course.Find(vakcode);

            if (course == null) {
                ViewBag.error = "deleteError";
                return RedirectToAction("Index", "Vak");
            }

            try
            {
                db.Course.Remove(course);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                ViewBag.error = "deleteError";
                return RedirectToAction("Index", "Vak");
            }

            ViewBag.error = "deleteGood";
            return RedirectToAction("Index", "Vak");
        }
        
        [HttpPost]
        [Route("vak/edit")]
        public ActionResult Edit(string vakcodeOld, string vakcodeNew, string titel, int studiepunten)
        {
            if (vakcodeOld == null || vakcodeOld.Trim().Equals(""))
            {
                ViewBag.error = "editError";
                return RedirectToAction("Index", "Vak");
            }
            try{
                
                Course course = db.Course.Find(vakcodeOld);
                Delete(vakcodeOld);
                course.Vakcode = vakcodeNew;
                course.Titel = titel;
                course.Studiepunten = studiepunten;
                db.Course.Add(course);
                db.SaveChanges();
            }catch (Exception) {
                ViewBag.error = "deleteError";
                return View();
            }
            ViewBag.error = "deleteGood";
            return RedirectToAction("Index", "Vak");
        }
    }
}