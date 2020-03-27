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
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
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
                var user = new IdentityUser
                {
                    UserName = model.RNum,
                    Email = model.Email
                };
                if (email != null)
                {
                    ModelState.AddModelError("", "Email " + model.Email + " is already in use.");
                }
                else
                {
                    var result = await userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded && email == null)
                    {
                        if (signInManager.IsSignedIn(User) && User.IsInRole("Admin")) 
                        {
                            return RedirectToAction("ListUsers", "Administration");
                        }
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
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

        //Werkt niet omdat de Json dit wilt returnen naar view: account/register, deze bestaat niet, moet op een manier gereturned worden naar Home/register
        //[AcceptVerbs("Get", "Post")]
        //[AllowAnonymous]
        //public async Task<IActionResult> IsEmailInUse(string email)
        //{
        //    var user = await userManager.FindByEmailAsync(email);

        //    if (user == null)
        //    {
        //        return Json(true);
        //    }
        //    else
        //    {
        //        return Json($"Email {email} is already in use.");
        //    }
        //}

        //[AcceptVerbs("Get", "Post")]
        //[AllowAnonymous]
        //public async Task<IActionResult> RNumInUse(string RNum)
        //{
        //    var user = await userManager.FindByNameAsync(RNum);

        //    if (user == null)
        //    {
        //        return Json(true);
        //    }
        //    else
        //    {
        //        return Json($"{RNum} is already in use.");
        //    }
        //}
    }
}
