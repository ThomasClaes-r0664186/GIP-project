using Gip.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Gip.Models.ViewModels;
using System.Collections.Generic;
using Gip.Utils;
using Gip.Services.Interfaces;

namespace Gip.Controllers
{
    [Authorize(Roles = "Admin, Lector, Student")]
    public class VakController : Controller
    {
        private IVakService service;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        //private Logger logger;

        public VakController(IVakService service,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.service = service;
            this.userManager = userManager;
            this.signInManager = signInManager;
            //this.logger = LogManager.GetCurrentClassLogger();
        }

        // GET
        [HttpGet]
        [Route("vak")]
        public async Task<ActionResult> Index()
        {
            try{
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                List<VakViewModel> vakViewModels;
                // wanneer je een student bent, moet je het inschrijfgedeelte kunnen zien.
                if (User.IsInRole("Student"))
                {
                    vakViewModels = service.GetVakkenStudent(user);
                }
                //als je geen student bent, maar wel admin of lector, krijg je gewoon een overzicht van alle vakken. Die kan je dan bewerken of verwijderen.
                else 
                {
                    vakViewModels = service.GetVakkenLectAdm();
                }

                if (TempData["error"] != null)
                {
                    ViewBag.error = TempData["error"].ToString();
                    TempData["error"] = null;
                }
                if (ViewBag.error == null || !ViewBag.error.Contains("addError") && !ViewBag.error.Contains("addGood") && !ViewBag.error.Contains("deleteError") && !ViewBag.error.Contains("deleteGood") && !ViewBag.error.Contains("editError") && !ViewBag.error.Contains("editGood"))
                {
                    ViewBag.error = "indexVakGood";
                }
                
                return View(vakViewModels);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ViewBag.error = "indexVakError" + "/" + e.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Route("vak/add")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Add(string vakcode, string titel, int studiepunten)
        {
            try{
                service.AddVak(vakcode, titel, studiepunten);
                utils.log("Er is een vak aangemaakt door: " + User.Identity.Name, new string[] { "Properties", vakcode + ";" + titel + ";" + studiepunten });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TempData["error"] = "addError" + "/" + e.Message;
                return RedirectToAction("Index", "Vak");
            }
            TempData["error"] = "addGood";
            return RedirectToAction("Index", "Vak");
        }

        [HttpGet]
        [Route("vak/add")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Add()
        {
            return View();
        }

        // verwijder terwijl gebruikt in coursemoment?
        [HttpPost]
        [Route("vak/delete")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Delete(int vakcode)
        {
            if (vakcode < 0)
            {
                TempData["error"] = "deleteError" + "/" + "Vakcode is foutief.";
                return  RedirectToAction("Index", "Vak");
            }

            try
            {
                service.DeleteVak(vakcode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TempData["error"] = "deleteError" + "/" + "Dit vak is gebruikt in een lesmoment, pas dat eerst aan voordat je dit kan verwijderen.";
                return RedirectToAction("Index", "Vak");
            }

            TempData["error"] = "deleteGood";
            return RedirectToAction("Index", "Vak");
        }

        [HttpPost]
        [Route("vak/edit")]
        [Authorize(Roles = "Admin, Lector")]
        public ActionResult Edit(int vakcodeOld, string vakcodeNew, string titel, int studiepunten)
        {
            TempData["error"] = "";
            if (vakcodeOld < 0)
            {
                TempData["error"] = "editError" + "/" + "De oude vakcode is niet goed doorgegeven want deze is leeg.";
                return RedirectToAction("Index", "Vak");
            }
            try{
                service.EditVak(vakcodeOld, vakcodeNew, titel, studiepunten);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                TempData["error"] = "editError" + "/" + e.Message;
                return RedirectToAction("Index", "Vak");
            }
            TempData["error"] = "editGood";
            return RedirectToAction("Index", "Vak");
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> Subscribe(int vakCode) 
        {
            try
            {
                var user = await userManager.GetUserAsync(User);

                service.Subscribe(vakCode, user);
            }
            catch (Exception e) {
                ViewBag.error = e.Message + " " + e.InnerException.Message==null?" ": e.InnerException.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> UnSubscribe(int vakCode)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);

                service.UnSubscribe(vakCode, user);
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message + " " + e.InnerException.Message == null ? " " : e.InnerException.Message;
            }
            return RedirectToAction("Index");
        }
    }
}