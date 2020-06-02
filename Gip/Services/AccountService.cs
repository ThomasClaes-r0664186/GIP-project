using Gip.Controllers;
using Gip.Models;
using Gip.Models.ViewModels;
using Gip.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<ApplicationUser> RegisterUser(RegisterViewModel model)
        {
            var email = await userManager.FindByEmailAsync(model.Email);
            var username = await userManager.FindByNameAsync(model.RNum);

            IdentityResult result = IdentityResult.Success;

            if (email != null)
            {
                throw new ArgumentException("Email " + model.Email + " is already in use.");
            }
            else if (username != null)
            {
                throw new ArgumentException("Student number: " + model.RNum + " is already in use.");
            }
            else 
            {
                var user = new ApplicationUser
                {
                    UserName = model.RNum.ToLower(),
                    Email = model.Email,
                    Naam = model.SurName,
                    VoorNaam = model.Name,
                    GeboorteDatum = model.GeboorteDatum
                };

                result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var role = user.UserName.ToLower().ToCharArray()[0];
                    if (role == 'c' || role == 'r' || role == 's' || role == 'm')
                    {
                        result = await userManager.AddToRoleAsync(user, "Student");
                        if (!result.Succeeded) 
                        { throw new Exception(result.Errors.ToString()); }
                    }
                }
                else 
                {
                    throw new Exception(result.Errors.ToString());
                }

                return user;
            }
        }

        //public async void LogOutUser()
        //{
        //    await signInManager.SignOutAsync();
        //}

        public async Task<SignInResult> Login(LoginViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.RNum, model.Password, model.RememberMe, false);

            return result;
        }

        public async Task<IdentityResult> ChangePassword(ApplicationUser user, ChangePasswordViewModel model)
        {
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            return result;
        }
    }
}
