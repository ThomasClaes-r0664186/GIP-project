using System;
using System.Threading.Tasks;
using Gip.Models;
using Gip.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class AccountController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("../Home/Register");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = await userManager.FindByEmailAsync(model.Email);
                var username = await userManager.FindByNameAsync(model.RNum);
                var user = new ApplicationUser
                {
                    UserName = model.RNum,
                    Email = model.Email,
                    Naam = model.SurName,
                    VoorNaam = model.Name
                };
                if (email != null)
                {
                    ModelState.AddModelError("", "Email " + model.Email + " is already in use.");
                }
                else if (username != null)
                {
                    ModelState.AddModelError("", "Student number: " + model.RNum + " is already in use.");
                }
                else
                {
                    var result = await userManager.CreateAsync(user, model.Password);

                    switch (user.UserName.ToLower().ToCharArray()[0])
                    {
                        case 'c':
                            var result1 = await userManager.AddToRoleAsync(user, "Student");
                            foreach (var error1 in result1.Errors)
                            {
                                ModelState.AddModelError("", error1.Description);
                            }
                            break;
                        case 'r':
                            var result2 = await userManager.AddToRoleAsync(user, "Student");
                            foreach (var error2 in result2.Errors)
                            {
                                ModelState.AddModelError("", error2.Description);
                            }
                            break;
                        case 's':
                            var result3 = await userManager.AddToRoleAsync(user, "Student");
                            foreach (var error3 in result3.Errors)
                            {
                                ModelState.AddModelError("", error3.Description);
                            }
                            break;
                        case 'm':
                            var result4 = await userManager.AddToRoleAsync(user, "Student");
                            foreach (var error4 in result4.Errors)
                            {
                                ModelState.AddModelError("", error4.Description);
                            }
                            break;
                        case 'u':
                            var result5 = await userManager.AddToRoleAsync(user, "Lector");
                            foreach (var error5 in result5.Errors)
                            {
                                ModelState.AddModelError("", error5.Description);
                            }
                            break;
                        case 'x':
                            var result6 = await userManager.AddToRoleAsync(user, "Admin");
                            foreach (var error6 in result6.Errors)
                            {
                                ModelState.AddModelError("", error6.Description);
                            }
                            break;
                        default: break;
                    }
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View("../Home/Register");
                    }
                    else
                    {
                        if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                        {
                            return RedirectToAction("ListUsers", "Administration");
                        }
                        else
                        {
                            await signInManager.SignInAsync(user, isPersistent: false);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            return View("../Home/Register");
        }

        [HttpPost]
        public async Task<IActionResult> Logout() {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View("../Home/Login");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.RNum, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }
            return View("../Home/Login", model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied(string ReturnUrl) 
        {
            return View("../Shared/AccessDenied");
        }

        [HttpGet]
        public IActionResult ChangePassword() 
        { return View("../Home/ChangePassword"); }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                var result = await userManager.ChangePasswordAsync(user,
                    model.CurrentPassword, model.NewPassword);

                // The new password did not meet the complexity rules or
                // the current password is incorrect. Add these errors to
                // the ModelState and rerender ChangePassword view
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("../Home/ChangePassword");
                }

                // Upon successfully changing the password refresh sign-in cookie
                await signInManager.RefreshSignInAsync(user);
                return View("../Home/ChangePasswordConfirmation");
            }

            return View("../Home/ChangePassword", model);
        }
    }
}
