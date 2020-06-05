using Gip.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using Gip.Utils;
using Gip.Services.Interfaces;

namespace Gip.Controllers
{
    [Authorize(Roles="Admin,Lector")]
    public class LokaalController : Controller
    {
        private ILokaalService service;
        public LokaalController(ILokaalService service) { this.service = service; }
        // GET
        [HttpGet]
        [Route("lokaal")]
        public ActionResult Index()
        {
            try
            {
                if (TempData["error"] != null)
                {
                    ViewBag.error = TempData["error"].ToString();
                    TempData["error"] = null;
                }
                return View();
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
                service.AddLokaal(gebouw, verdiep, nummer, type, capaciteit, middelen);
            }
            catch (Exception e)
            {
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

        //fixed, verwijder bij gebruik lesmoment?
        [HttpPost]
        [Route("lokaal/delete")]
        public ActionResult Delete(int lokaalId)
        {
            try
            {
                service.DeleteLokaal(lokaalId);
            }
            catch (Exception e)
            {
                TempData["error"] = "deleteError" + "/" + e.Message;
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
        public ActionResult Edit(int lokaalId, string gebouw, int verdiep, string nummer, string type, int capaciteit, string middelen)
        {
            TempData["error"] = "";
            try
            {
                service.EditLokaal(lokaalId, gebouw, verdiep, nummer, type, capaciteit, middelen);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["error"] = "editError" + "/" + e.Message;
                return RedirectToAction("Index", "Lokaal");
            }
            TempData["error"] = "editGood";
            return RedirectToAction("Index", "Lokaal");
        }
    }
}
 