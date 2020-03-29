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
    [Authorize(Roles="Lector")]
    public class LectorController : Controller
    {
        private gipDatabaseContext db = new gipDatabaseContext();

        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        [HttpGet]
        [Route("Lector")]
        public ActionResult Index()
        {
            var vakL = from v in db.Course
                      orderby v.Titel
                      select v;

            var cuL = from cu in db.CourseUser
                      orderby cu.Userid
                      where !cu.GoedGekeurd
                      select cu;

            var uL = from us in db.User
                     orderby us.Naam
                     select us;

            List<StudentRequestsViewModel> studentRequests = new List<StudentRequestsViewModel>();

            foreach (var vak in vakL) 
            {
                StudentRequestsViewModel studReq = new StudentRequestsViewModel {Titel = vak.Titel, VakCode = vak.Vakcode };
                studentRequests.Add(studReq);

                var cuL2 = cuL.Where(c => c.Vakcode.Equals(vak.Vakcode));

                foreach (var u in cuL2) 
                {
                    var user = db.User.Find(u.Userid);

                    studReq = new StudentRequestsViewModel { VakCode = vak.Vakcode, Titel = vak.Titel, UserId = user.Userid, Naam = user.Naam, VoorNaam = user.VoorNaam};

                    studentRequests.Add(studReq);
                }
            }

            return View(studentRequests); 
        }
    }
}
