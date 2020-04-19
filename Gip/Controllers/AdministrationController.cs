using Gip.Models;
using Gip.Models.ViewModels;
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
        private gipDatabaseContext db = new gipDatabaseContext();

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager)
        {
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
                IdentityRole role = new IdentityRole { Name = model.RoleName };

                IdentityResult result = await roleManager.CreateAsync(role);

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
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ListUsers()
        {
            var users = userManager.Users.OrderBy(u => u.UserName);
            
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var userRoles = await userManager.GetRolesAsync(user);

            try
            {
                var model = new EditUserViewModel
                {
                    Id = user.Id,
                    Name = user.VoorNaam,
                    SurName = user.Naam,
                    RNum = user.UserName,
                    Email = user.Email,
                    Roles = userRoles
                };

                return View(model);
            }
            catch (Exception e) 
            {
                ModelState.AddModelError("", e.Message + " " + e.InnerException.Message == null ? " " : e.InnerException.Message);
                return View("../Home/Register");
            }
        }

        //fixed, nakijken wat er gebeurd wanneer je een user verwijderd gebruikt in coursemoment.
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var emailUser = await userManager.FindByEmailAsync(model.Email);
                var usernameUser = await userManager.FindByNameAsync(model.RNum);

                if (emailUser != null && user != emailUser)
                {
                    ModelState.AddModelError("", "Email " + model.Email + " is already in use.");
                }
                if (usernameUser != null && user != usernameUser) 
                {
                    ModelState.AddModelError("", "Student number: " + model.RNum + " is already in use.");
                }
                else
                {
                    user.UserName = model.RNum;
                    user.Email = model.Email;
                    user.Naam = model.SurName;
                    user.VoorNaam = model.Name;

                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListUsers");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                return View(model);
            }
        }

        //fixed, nakijken wat er gebeurd wanneer je een user verwijderd gebruikt in coursemoment.
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var qryDelUCMU = from cmu in db.CourseMomentUsers
                              where cmu.ApplicationUserId == user.Id
                              select cmu;

                if (qryDelUCMU.Any())
                {
                    foreach (var CoUs in qryDelUCMU)
                    {
                        db.CourseMomentUsers.Remove(CoUs);
                    }
                    db.SaveChanges();
                }

                var qryDelUCU = from cu in db.CourseUser
                              where cu.ApplicationUserId == user.Id
                              select cu;

                if (qryDelUCU.Any())
                {
                    foreach (var CoUs in qryDelUCU)
                    {
                        db.CourseUser.Remove(CoUs);
                    }
                    db.SaveChanges();
                }

                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            bool failed = false;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles) 
            {
                var result = await userManager.RemoveFromRoleAsync(user, role);

                if (!result.Succeeded) 
                {
                    failed = true;
                    ModelState.AddModelError("", "Cannot remove user existing roles");
                }
            }
            if (failed) 
            {
                return View(model);
            }

            var result1 = await userManager.AddToRolesAsync(user, model.Where(x=>x.IsSelected).Select(y=>y.RoleName));

            if (!result1.Succeeded) 
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { id = userId});
        }

        //opgepast bij verwijderen schedule dat gebruikt is in een lesmoment => error doordat cascade on delete nog niet werkt.
        public ActionResult DeleteDbHistory()
        {
            try
            {
                var historDate = DateTime.Now.AddMonths(-1);
                //var historDate = DateTime.Now.AddDays(-10);

                var schedToDel = from sched in db.Schedule
                                 where sched.Datum < historDate
                                 select sched;

                foreach (var sched in schedToDel)
                {
                    db.Schedule.Remove(sched);

                    var cmL = db.CourseMoment.Where(e => e.ScheduleId == sched.Id);
                    if (cmL.Any()) 
                    {
                        foreach (var cm in cmL) 
                        {
                            var cmuL = db.CourseMomentUsers.Where(e => e.CoursMomentId == cm.Id);

                            if (cmuL.Any()) 
                            {
                                foreach (var cmu in cmuL) 
                                {
                                    db.CourseMomentUsers.Remove(cmu);
                                }
                            }
                        }
                    }
                }

                db.SaveChanges();

                CookieOptions cookies = new CookieOptions();
                cookies.Expires = DateTime.Now.AddDays(1);

                Response.Cookies.Append("deleteDb", "true", cookies);

                TempData["error"] = "deleteGood";
            }
            catch (Exception e)
            {
                TempData["error"] = "dbDeleteFail";

                Console.WriteLine(e);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
