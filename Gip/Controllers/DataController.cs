﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gip.Models;
using Gip.Services;
using Gip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Gip.Models.ViewModels;
using Microsoft.AspNetCore.Identity;


namespace Gip.Controllers
{
    [Authorize(Roles = "Admin, Lector, Student")]
    [ApiController]
    public class DataController : Controller
    {
        private IDataService service;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public DataController(IDataService service,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            this.service = service;
            
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        [HttpPost]
        [Route("data/vakken")]
        [Produces("application/json")]
        public IActionResult Vakken()
        {
            try
            {
                int start = Convert.ToInt32(Request.Form["start"]);
                int length = Convert.ToInt32(Request.Form["length"]);
                string searchValue = Request.Form["search[value]"];
                string sortColumnName = Request.Form["columns["+ HttpContext.Request.Form["order[0][column]"] + "][name]"];
                string sortDirection = Request.Form["order[0][dir]"];
                var returned = service.GetVakken(start, length,searchValue,sortColumnName,sortDirection);
                var qry = returned.Item1;
                int draw;
                int.TryParse(Request.Form["draw"], out draw);
                int recordsFiltered = qry.Count();
                return Json(new
                {
                    data = qry.ToList<Course>(),
                    recordsTotal = returned.Item2,
                    recordsFiltered =recordsFiltered,
                    draw = draw,
                    iTotalDisplayRecords= returned.Item3
                });
            }    
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        
        [HttpGet]
        [Route("data/vakkenjson")]
        [Produces("application/json")]
        public async Task<IActionResult> VakkenJson()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (User.IsInRole("Student"))
            {
                return Json(new
                {
                    data = service.GetVakkenStudent(user),
                });
            }
            else
            {
                return Json(new
                {
                    data = service.GetVakkenAllLectAdm(),
                });
            }
            
        }
        
        [HttpPost]
        [Route("data/lokalen")]
        [Produces("application/json")]
        public IActionResult Lokalen()
        {
            try
            {
                int start = Convert.ToInt32(Request.Form["start"]);
                int length = Convert.ToInt32(Request.Form["length"]);
                string searchValue = Request.Form["search[value]"];
                string sortColumnName = Request.Form["columns["+ HttpContext.Request.Form["order[0][column]"] + "][name]"];
                string sortDirection = Request.Form["order[0][dir]"];
                var returned = service.GetLokalen(start, length,searchValue,sortColumnName,sortDirection);
                var qry = returned.Item1;
                int draw;
                int.TryParse(Request.Form["draw"], out draw);
                int recordsFiltered = qry.Count();
                return Json(new
                {
                    data = qry.ToList<LokaalViewModel>(),
                    recordsTotal = returned.Item2,
                    recordsFiltered =recordsFiltered,
                    draw = draw,
                    iTotalDisplayRecords= returned.Item3
                });
            }    
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        [HttpGet]
        [Route("data/lokalenjson")]
        [Produces("application/json")]
        public IActionResult LokalenJson()
        {
            return Json(new
            {
                data = service.GetLokalenAll().ToList<LokaalViewModel>(),
            });
        }
    }
}