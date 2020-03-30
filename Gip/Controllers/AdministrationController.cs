using Gip.Models;
using Gip.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        private readonly UserManager<IdentityUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<IdentityUser> userManager)
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

            var users = userManager.Users;
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
                var userForNames = db.User.Find(user.UserName);

                var model = new EditUserViewModel
                {
                    Id = user.Id,
                    Name = userForNames.VoorNaam,
                    SurName = userForNames.Naam,
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

                if (emailUser != null && user != emailUser)
                {
                    ModelState.AddModelError("", "Email " + model.Email + " is already in use.");
                }
                else
                {
                    try
                    {
                        var OldDbUser = db.User.Find(user.UserName);
                        if (model.RNum.Equals(OldDbUser.Userid))
                        {
                            db.User.Find(user.UserName).VoorNaam = model.Name;
                            db.User.Find(user.UserName).Naam = model.SurName;
                            db.User.Find(user.UserName).Mail = model.Email;
                            db.SaveChanges();
                        }
                        else 
                        {
                            db.User.Add(new User { Userid = model.RNum, VoorNaam = model.Name, Naam = model.SurName, Mail = model.Email});
                            db.User.Remove(OldDbUser);
                            db.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Uw user is met zijn nummer aangeduid als lector in een lesmoment, gelieve dit lesmoment te verwijderen voordat u de user kan aanpassen. Fout: " + e.InnerException.Message == null ? " " : e.InnerException.Message);
                        return View(model);
                    }

                    user.UserName = model.RNum;
                    user.Email = model.Email;

                    var result = await userManager.UpdateAsync(user);

                    if(ModelState.ErrorCount >= 1) 
                    {
                        ModelState.AddModelError("", "Uw user is met zijn nummer aangeduid als lector in een lesmoment, gelieve dit lesmoment te verwijderen voordat u de user kan aanpassen.");
                    }

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
                try
                {
                    var OldDbUser = db.User.Find(user.UserName);
                    db.User.Remove(OldDbUser);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Uw user is aangeduid als lector in een lesmoment, gelieve dit lesmoment te verwijderen voordat u de user kan verwijderen. Fout: " + e.InnerException.Message == null ? " " : e.InnerException.Message);
                    return RedirectToAction("ListUsers");
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
    }
}
