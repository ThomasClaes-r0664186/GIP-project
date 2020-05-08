using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    [Authorize(Roles = "Admin, Lector, Student")]
    public class FieldOfStudyController : Controller
    {
        private IFieldOfStudyService service;

        public FieldOfStudyController(IFieldOfStudyService service) 
        {
            this.service = service;
        }

        [HttpGet]
        [Route("fieldOfStudy")]
        public IActionResult Index()
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"].ToString();
                TempData["error"] = null;
            }
            return View(service.GetAllFieldOfStudy());
        }

        [HttpPost]
        [Route("fieldOfStudy/add")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Add(string code, string titel, string type,int studiepunten)
        {
            try
            {
                service.AddRichting(code, titel, type, studiepunten);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TempData["error"] = "addError" + "/" + e.Message;
                return RedirectToAction("Index", "FieldOfStudy");
            }
            TempData["error"] = "addGood";
            return RedirectToAction("Index", "FieldOfStudy");
        }
    }
}