using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gip.Models;
using Gip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    [Authorize(Roles = "Admin, Lector, Student")]
    public class FieldOfStudyController : Controller
    {
        private IFieldOfStudyService service;
        private readonly UserManager<ApplicationUser> userManager;
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

        [HttpPost]
        [Route("fieldOfStudy/delete")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Delete(int Id)
        {
            try
            {
                service.DeleteRichting(Id);
            }
            catch (Exception e)
            {
                TempData["error"] = "deleteError" + "/" + e.Message;
                return RedirectToAction("Index", "FieldOfStudy");
            }
            TempData["error"] = "deleteGood";
            return RedirectToAction("Index", "FieldOfStudy");
        }

        [HttpGet]
        [Route("fieldOfStudy/edit")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [Route("fieldOfStudy/edit")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Edit(int richtindId, string richtingCode, string richtingTitel, string type, int richtingStudiepunten)
        {
            TempData["error"] = "";
            try
            {
                service.EditRichting(richtindId, richtingCode, richtingTitel, type, richtingStudiepunten);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["error"] = "editError" + "/" + e.Message;
                
                return RedirectToAction("Index", "fieldOfStudy");

            }
            TempData["error"] = "editGood";
            return RedirectToAction("Index", "fieldOfStudy");
        }

        [HttpPost]
        [Route("fieldOfStudy/Subscribe")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> Subscribe(int fosId)
        {
            TempData["error"] = "";
            try
            {
                var user = await userManager.GetUserAsync(User);

                service.SubscribeFos(fosId, user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["error"] = "editError" + "/" + e.Message;

                return RedirectToAction("Index", "fieldOfStudy");
            }
            TempData["error"] = "editGood";
            return RedirectToAction("Index", "fieldOfStudy");
        }
    }
}