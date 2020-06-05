using System;
using System.Threading.Tasks;
using Gip.Models;
using Gip.Models.ViewModels;
using Gip.Services.Interfaces;
using Gip.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gip.Controllers
{
    public class AccountController : Controller
    {
        private IAccountService service;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly MailHandler mailHandler;

        public AccountController(IAccountService service, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, MailHandler mailHandler)
        {
            this.service = service;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mailHandler = mailHandler;
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
                    ApplicationUser user = await service.RegisterUser(model);

                    var remoteIp = Request.HttpContext.Connection.RemoteIpAddress;
                    utils.log("Er is een user aangemaakt van de ip: " + remoteIp.ToString(), new string[] { "Properties", user.UserName + ";" + user.VoorNaam + ";" + user.Naam + ";" + user.GeboorteDatum +";" + user.Email });

                    // email confirmation, zet dit in comment om geen mail te sturen bij registratie
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    
                    var confirmationLink = "Om uw email te bevestigen, klikt u op volgende link: " + Environment.NewLine + Url.Action("ConfirmEmail", "Account",
                            new { userId = user.Id, token = token }, Request.Scheme);
                    mailHandler.SendMail(user, confirmationLink, "Activeer account");
                    /**/

                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    else
                    {
                        // zet dit in comment en uncomment het andere gedeelte om bij registratie meteen aangemeld te worden.
                        ViewBag.ErrorTitle = "Registration successful";
                        ViewBag.ErrorMessage = "Alvorens u kan inloggen, bevestig uw " +
                                "email, door te klikken op de bevestigings link die u kreeg toegestuurd";
                        return View("Error");

                        /*
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                        */
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

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return View("NotFound");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }

            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("Error");
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
                var result = await service.Login(model);

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

                var result = await service.ChangePassword(user, model);

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
                    var passwordResetLink = "Om uw wachtwoord te herstellen, klikt u op volgende link: "+ Environment.NewLine + Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);
                    //verzenden met mailhandler
                    mailHandler.SendMail(user, passwordResetLink, "Password recovery");
                    var remoteIp = Request.HttpContext.Connection.RemoteIpAddress;
                    utils.log("Er werd een password recovery aangevraagd door: " + user.UserName + " van de ip: " + remoteIp.ToString(), new string[] { "Properties", user.UserName + ";" + remoteIp.ToString()});

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
    }
}
