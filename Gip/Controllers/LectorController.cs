using Gip.Models;
using Gip.Models.ViewModels;
using Gip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gip.Controllers
{
    [Authorize(Roles="Lector")]
    public class LectorController : Controller
    {
        private ILectorService service;

        public LectorController(ILectorService service) { this.service = service; }

        [HttpGet]
        [Route("Lector")]
        public ActionResult Index()
        {
            var studentRequests = service.GetStudentRequests();
            return View(studentRequests);
        }
        [Route("lector/approvestudent")]
        [HttpPost]
        public ActionResult ApproveStudent(int cuId) 
        {
            try
            {
                service.ApproveStudent(cuId);
            }
            catch (Exception e)
            {
                TempData["error"] = "error" + "/" + e.Message;
            }
            return RedirectToAction("Index");
        }
        [Route("lector/denystudent")]
        [HttpPost]
        public ActionResult DenyStudent(int cuId, string beschrijving)
        {
            try
            {
                service.DenyStudent(cuId, beschrijving);

                TempData["error"] = "denyGood";
            }
            catch (Exception e)
            {
                TempData["error"] = "error" + "/" + e.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
