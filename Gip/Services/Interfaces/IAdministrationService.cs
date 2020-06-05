using Gip.Models;
using Gip.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Services.Interfaces
{
    public interface IAdministrationService
    {
        Task<IdentityResult> CreateRole(CreateRoleViewModel model);
        IQueryable<IdentityRole> GetRoles();
        Task<EditRoleViewModel> GetEditRole(string id);
        IOrderedQueryable<ApplicationUser> GetUsers();
        Task<EditUserViewModel> GetEditUser(string id);
        Task<IdentityResult> EditUser(EditUserViewModel model);
        Task<IdentityResult> DeleteUser(string id);
        Task<List<UserRoleViewModel>> GetEditUsersInRole(string roleId);
        void EditUsersInRole(List<UserRoleViewModel> model, string roleId);
        Task<List<UserRolesViewModel>> GetUserRoles(string userId);
        Task<IdentityResult> UpdateUserRoles(List<UserRolesViewModel> model, string userId);
        IQueryable<ApplicationUser> GetRegisterRequests();
        Task<IdentityResult> AcceptUserRequest(string id, char rol);
        void DeleteDbHistory();
    }
}
