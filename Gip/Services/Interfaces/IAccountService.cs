using Gip.Models;
using Gip.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ApplicationUser> RegisterUser(RegisterViewModel model);
        //void LogOutUser();
        Task<SignInResult> Login(LoginViewModel model);
        Task<IdentityResult> ChangePassword(ApplicationUser user, ChangePasswordViewModel model);
    }
}
