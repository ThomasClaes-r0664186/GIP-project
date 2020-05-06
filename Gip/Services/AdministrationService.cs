using Gip.Models;
using Gip.Models.ViewModels;
using Gip.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gip.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private gipDatabaseContext db;

        public AdministrationService(gipDatabaseContext db,
                                        RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task<IdentityResult> CreateRole(CreateRoleViewModel model)
        {
            IdentityRole role = new IdentityRole { Name = model.RoleName};

            IdentityResult result = await roleManager.CreateAsync(role);

            return result;
        }

        public IQueryable<IdentityRole> GetRoles()
        {
            return roleManager.Roles;
        }

        public async Task<EditRoleViewModel> GetEditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null) {
                throw new Exception("De rol kon niet gevonden worden.");
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

            return model;
        }

        public IOrderedQueryable<ApplicationUser> GetUsers()
        {
            return userManager.Users.OrderBy(u => u.UserName);
        }

        public async Task<EditUserViewModel> GetEditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null) 
            {
                throw new Exception("De user met id: " + id +" kon niet gevonden worden");
            }

            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Name = user.VoorNaam,
                SurName = user.Naam,
                RNum = user.UserName,
                GeboorteDatum = user.GeboorteDatum,
                Email = user.Email,
                Roles = userRoles
            };

            return model;
        }

        public async Task<IdentityResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                throw new Exception("De user met id: " + model.Id + " kon niet gevonden worden.");
            }
            else 
            {
                IdentityResult result;
                var emailUser = await userManager.FindByEmailAsync(model.Email);
                var usernameUser = await userManager.FindByNameAsync(model.RNum);

                if (emailUser != null && user != emailUser)
                {
                    throw new ArgumentException("Email " + model.Email + " is already in use.");
                }
                if (usernameUser != null && user != usernameUser)
                {
                    throw new ArgumentException("Student number: " + model.RNum + " is already in use.");
                }
                else
                {
                    user.UserName = model.RNum;
                    user.Email = model.Email;
                    user.Naam = model.SurName;
                    user.VoorNaam = model.Name;
                    user.GeboorteDatum = model.GeboorteDatum;
                    result = await userManager.UpdateAsync(user);
                }
                return result;
            }
        }

        public async Task<IdentityResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new Exception("De user met id: " + id + " kon niet gevonden worden.");
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

                return await userManager.DeleteAsync(user);
            }
        }

        public async Task<List<UserRoleViewModel>> GetEditUsersInRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null) 
            {
                throw new ArgumentException("De rol met id: " + roleId + " bestaat niet.");
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

            return model;
        }

        public async void EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null) 
            {
                throw new ArgumentException("De rol met id: " + roleId + " bestaat niet.");
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
                        throw new Exception("Er is iets foutgelopen: " + result.Errors.ToString());
                }
            }
        }

        public async Task<List<UserRolesViewModel>> GetUserRoles(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("De user met id: " + userId + " kon niet gevonden worden.");
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

            return model;
        }

        public async Task<IdentityResult> UpdateUserRoles(List<UserRolesViewModel> model, string userId)
        {
            IdentityResult result;

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("De user met id: " + userId + " kon niet gevonden worden.");
            }

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                result = await userManager.RemoveFromRoleAsync(user, role);

                if (!result.Succeeded)
                {
                    return result;
                }
            }

            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));

            return result;
        }

        public IQueryable<ApplicationUser> GetRegisterRequests()
        {
            return db.Users.Where(x => !db.UserRoles.Any(y => y.UserId == x.Id) && (x.UserName.ToLower().StartsWith("u") || x.UserName.ToLower().StartsWith("x")));
        }

        public async Task<IdentityResult> AcceptUserRequest(string id, char rol)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null) 
            {
                throw new Exception("De user met id: " + id + " kon niet gevonden worden.");
            }

            IdentityResult result = new IdentityResult();
            
            switch (rol)
            {
                case 'r':
                    result = await userManager.AddToRoleAsync(user, "Student");
                    break;
                case 'u':
                    result = await userManager.AddToRoleAsync(user, "Lector");
                    break;
                case 'x':
                    result = await userManager.AddToRoleAsync(user, "Admin");
                    break;
                default:
                    result.Errors.Append(new IdentityError { Description = "Het is niet gelukt om de user aan een rol toe te voegen." });
                    break;
            }

            return result;
        }

        public void DeleteDbHistory()
        {
            var historDate = DateTime.Now.AddMonths(-1);

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
        }
    }
}
