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
        private readonly MailHandler mailHandler;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            mailHandler = new MailHandler();
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
                try
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
                                //voor zowel u als voor x moet er door een admin bevestigt worden dat het account aangemaakt mag worden.

                                //hier kan bijvoorbeeld nog een melding gezet worden dat deze geaccepteerd moet worden door admin.

                                //var result5 = await userManager.AddToRoleAsync(user, "Lector");
                                //foreach (var error5 in result5.Errors)
                                //{
                                //    ModelState.AddModelError("", error5.Description);
                                //}
                                break;
                            case 'x':
                                //var result6 = await userManager.AddToRoleAsync(user, "Admin");
                                //foreach (var error6 in result6.Errors)
                                //{
                                //    ModelState.AddModelError("", error6.Description);
                                //}
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
                                //Welcome page
                                //return RedirectToAction("LoggedIn", "Account");
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                }
                catch (Exception e) 
                {
                    ModelState.AddModelError("", e.Message);
                    return View("../Home/Register");
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
                        //Welcome page
                        //return RedirectToAction("LoggedIn", "Account");
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View("../Home/ForgotPassword");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    //maak password reset token
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    //bouw password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    mailHandler.SendMail(user, passwordResetLink, "Password recovery");

                    return View("../Home/ForgotPasswordConfirmation");
                }
                return View("../Home/ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token==null || email==null)
            {
                ModelState.AddModelError("", "Ongeldige wachtwoord reset token");
            }
            return View("../Home/ResetPassword"); // niet zeker
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // reset the user password
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("../Home/ResetPasswordConfirmation");
                    }
                    // Display validation errors. For example, password reset token already
                    // used to change the password or password complexity rules not met
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist
                return View("../Home/ResetPasswordConfirmation");
            }
            // Display validation errors if model state is not valid
            return View(model);
        }

        //Welcome page
        //[HttpGet]
        //public async Task<ActionResult> LoggedIn() 
        //{
        //    var user = await userManager.FindByNameAsync(User.Identity.Name);

        //    if (user == null) 
        //    {
        //        return View("Index", "Home");
        //    }
        //    return View("../Home/LoggedIn", user);
        //}
    }
}
