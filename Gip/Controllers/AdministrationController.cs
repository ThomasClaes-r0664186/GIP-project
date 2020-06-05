using Gip.Models;
using Gip.Models.ViewModels;
using Gip.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private IAdministrationService service;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(IAdministrationService service, 
                                        RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager)
        {
            this.service = service;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await service.CreateRole(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ListRoles()
        {
            return View(service.GetRoles());
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            try {
                var model = await service.GetEditRole(id);
                return View(model);
            }
            catch (Exception e) 
            {
                ViewBag.ErrorMessage = e.Message;
                return View("NotFound");
            }
        }

        [HttpGet]
        public ActionResult ListUsers()
        {
            return View(service.GetUsers());
        }
        
        [Route("admin/user/edit")]
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            try
            {
                var model = await service.GetEditUser(id);

                return View(model);
            }
            catch (Exception e) 
            {
                ModelState.AddModelError("", e.Message + " " + (e.InnerException.Message == null ? " " : e.InnerException.Message));
                return View("../Home/Register");
            }
        }

        //fixed, nakijken wat er gebeurd wanneer je een user verwijderd gebruikt in coursemoment.
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            try
            {
                var result = await service.EditUser(model);

                if (!result.Succeeded) 
                {
                    throw new Exception(result.Errors.ToString());
                }

                return RedirectToAction("ListUsers");
            }
            catch (Exception e) 
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }
        }

        //fixed, nakijken wat er gebeurd wanneer je een user verwijderd gebruikt in coursemoment.
        [Route("admin/user/delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var result = await service.DeleteUser(id);

                if (!result.Succeeded) 
                {
                    ModelState.AddModelError("", result.Errors.ToString());
                    return View("ListUsers");
                }

                return RedirectToAction("ListUsers");
            }
            catch (Exception e) 
            {
                ModelState.AddModelError("", e.Message);
                return View("ListUsers");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            try
            {
                var model = await service.GetEditUsersInRole(roleId);

                return View(model);
            }
            catch (Exception e) 
            {
                ViewBag.ErrorMessage = e.Message;
                return View("NotFound");
            }
        }

        [HttpPost]
        public IActionResult EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            try
            {
                service.EditUsersInRole(model, roleId);
            }
            catch (Exception e) 
            {
                ViewBag.ErrorMessage = e.Message;
                return RedirectToAction("EditRole", new { Id = roleId });
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            try
            {
                var model = await service.GetUserRoles(userId);
                ViewBag.userId = userId;
                return View(model);
            }
            catch (Exception e) 
            {
                ViewBag.ErrorMessage = e.Message;
                return View("NotFound");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            try
            {
                var result = await service.UpdateUserRoles(model, userId);

                if (!result.Succeeded) 
                {
                    throw new Exception(result.Errors.ToString());
                }

                return RedirectToAction("EditUser", new { id = userId });
            }
            catch (Exception e) 
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult ListRegisterRequests() {
            if (TempData["error"] != null) 
            {
                ViewBag.error = TempData["error"];
            }
            return View(service.GetRegisterRequests());
        }

        [HttpPost]
        public async Task<ActionResult> AcceptUserRequest(string id, char rol) 
        {
            try
            {
                var result = await service.AcceptUserRequest(id, rol);

                if (!result.Succeeded) 
                {
                    throw new Exception(result.Errors.ToString());
                }
            }
            catch (Exception e) 
            {
                TempData["error"] = e.Message;
            }

            return RedirectToAction("ListRegisterRequests");
        }

        //opgepast bij verwijderen schedule dat gebruikt is in een lesmoment => error doordat cascade on delete nog niet werkt.
        public ActionResult DeleteDbHistory()
        {
            try
            {
                service.DeleteDbHistory();

                TempData["error"] = "deleteGood";
            }
            catch (Exception e)
            {
                TempData["error"] = "dbDeleteFail";

                Console.WriteLine(e);
            }

            CookieOptions cookies = new CookieOptions();
            cookies.Expires = DateTime.Now.AddDays(1);

            Response.Cookies.Append("deleteDb", "true", cookies);

            return RedirectToAction("Index", "Home");
        }
    }
}
