using Gip.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Data.Sqlite;
using System;
using Microsoft.Extensions.DependencyInjection;
using Gip.Services;
using Microsoft.Extensions.Logging;
using GipUnitTest.LoggerUtils;
using System.Threading.Tasks;
using Gip.Controllers;
using Microsoft.Extensions.Options;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Linq;
using Gip.Models.ViewModels;

namespace GipUnitTest.ControllerTests
{
    [TestClass]
    public class PlannerControllerTests
    {
        private SqliteConnection sqliteConnection;
        private gipDatabaseContext ctxDb;

        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private SignInManager<ApplicationUser> signInManager;

        // TestInit en TestCleanup worden voor en na elke test gedaan. Dit om ervoor te zorgen dat je geen gekoppelde testen hebt. (Geen waardes hergebruikt)

        [TestInitialize]
        public void InitializeTestZone()
        {
            IServiceCollection serviceCol = new ServiceCollection();

            sqliteConnection = new SqliteConnection("DataSource=:memory:");
            serviceCol.AddDbContext<gipDatabaseContext>(options => options.UseSqlite(sqliteConnection));

            ctxDb = serviceCol.BuildServiceProvider().GetService<gipDatabaseContext>();
            ctxDb.Database.OpenConnection();
            ctxDb.Database.EnsureCreated();

            serviceCol.AddIdentity<ApplicationUser, IdentityRole>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddEntityFrameworkStores<gipDatabaseContext>()
                .AddDefaultTokenProviders();


            serviceCol.AddSingleton<ILogger<UserManager<ApplicationUser>>>(new UserManagerLogger());
            serviceCol.AddSingleton<ILogger<DataProtectorTokenProvider<ApplicationUser>>>(new DataProtectorLogger());
            serviceCol.AddSingleton<ILogger<RoleManager<ApplicationUser>>>(new RoleManagerUserLogger());
            serviceCol.AddSingleton<ILogger<RoleManager<IdentityRole>>>(new RoleManagerRoleLogger());
            serviceCol.AddSingleton<ILogger<SignInManager<ApplicationUser>>>(new SignInManagerLogger());


            userManager = serviceCol.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
            roleManager = serviceCol.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();
            signInManager = serviceCol.BuildServiceProvider().GetService<SignInManager<ApplicationUser>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ctxDb.Database.EnsureDeleted();
            ctxDb.Dispose();
            sqliteConnection.Close();
        }

        [TestMethod]
        public void IndexTest() 
        {
            // ARRANGE 
            PlannerService service = new PlannerService(ctxDb);
            PlannerController controller = SetupPlannerController(service).Result;

            AddTestPlanning(service);
            
            // ACT
            var res = controller.Index(0).Result;

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            var vRes = (ViewResult)res;
            Assert.IsNotNull(vRes.Model);
            var model = (List<Planner>)vRes.Model;
            Assert.IsTrue(model.Any());
            Assert.IsNotNull(model.Where(p => p.Vakcode == "AAA01A" && p.Gebouw == "A" && p.Verdiep == 1 && p.Nummer == "01").FirstOrDefault());
        }

        [TestMethod]
        public async Task AddTest() 
        {
            // ARRANGE 
            PlannerService service = new PlannerService(ctxDb);
            PlannerController controller = SetupPlannerController(service).Result;

            Room room = new Room { Gebouw = "A", Verdiep = 1, Nummer = "01", Type = "Lokaal" };
            ctxDb.Room.Add(room);

            Course course = new Course { Vakcode = "AAA01A", Titel = "testvak", Studiepunten = 4 };
            ctxDb.Course.Add(course);
            ctxDb.SaveChanges();

            string dat = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
            string uur = "11:00";
            double duratie = 2.0;
            int lokaalId = ctxDb.Room.Where(r => r.Gebouw == "A" && r.Verdiep == 1 && r.Nummer == "01").FirstOrDefault().Id;
            int vakId = ctxDb.Course.Where(c => c.Vakcode == "AAA01A").FirstOrDefault().Id;

            // ACT
            var res = await controller.Add(dat, uur, lokaalId, duratie, vakId, null, null, 0);

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            Assert.IsNotNull(ctxDb.CourseMoment.Where(cm => cm.CourseId == vakId && cm.RoomId == lokaalId).FirstOrDefault());

        }

        [TestMethod]
        public void DeleteTest() 
        {
            // ARRANGE 
            PlannerService service = new PlannerService(ctxDb);
            PlannerController controller = SetupPlannerController(service).Result;

            var cmId = AddTestPlanning(service);

            // ACT
            var res = controller.Delete(cmId);

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            Assert.IsNull(ctxDb.CourseMoment.Where(cm => cm.Id == cmId).FirstOrDefault());

        }

        [TestMethod]
        public async Task EditTest() 
        {
            // ARRANGE 
            PlannerService service = new PlannerService(ctxDb);
            PlannerController controller = SetupPlannerController(service).Result;

            int cmId = AddTestPlanning(service);

            Course course1 = new Course { Vakcode = "BBB01B", Titel = "testvak", Studiepunten = 4 };
            ctxDb.Course.Add(course1);
            Room room1 = new Room { Gebouw = "B", Verdiep = 0, Nummer = "01", Type = "Lokaal", Capaciteit = 10 };
            ctxDb.Room.Add(room1);
            ctxDb.SaveChanges();

            int newCourseId = ctxDb.Course.Where(c => c.Vakcode == "BBB01B").FirstOrDefault().Id;
            int newRoomId = ctxDb.Room.Where(r => r.Gebouw == "B" & r.Verdiep == 0 & r.Nummer == "01").FirstOrDefault().Id;
            DateTime newDatum = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string newDatumString = newDatum.ToString("yyyy-MM-dd");
            string newStart = "13:00";
            double newDuratie = 1.0;
            string newLessenlijst = "Editplanning test lesmoment";

            // ACT
            var res = await controller.Edit(cmId, newCourseId, newDatumString, newStart, newDuratie, newRoomId, newLessenlijst);

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            Assert.AreEqual(newRoomId ,ctxDb.CourseMoment.Find(cmId).RoomId);
            Assert.AreEqual(newCourseId, ctxDb.CourseMoment.Find(cmId).CourseId);
            Assert.AreEqual(newLessenlijst, ctxDb.CourseMoment.Find(cmId).LessenLijst);

        }

        [TestMethod]
        public void ViewTopicTest() 
        {
            // ARRANGE 
            PlannerService service = new PlannerService(ctxDb);
            PlannerController controller = SetupPlannerController(service).Result;

            int cmId = AddTestPlanning(service);

            // ACT
            var res = controller.ViewTopic(cmId).Result;

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            var vRes = (ViewResult)res;
            Assert.IsNotNull(vRes.Model);
            var model = (Planner)vRes.Model;
            Assert.AreEqual(cmId, model.cmId);
        }

        [TestMethod]
        public void ViewCourseMomentsTest() 
        {
            // ARRANGE 
            PlannerService service = new PlannerService(ctxDb);
            PlannerController controller = SetupPlannerController(service).Result;

            int cmId = AddTestPlanning(service);

            var courseId = (int)ctxDb.CourseMoment.Find(cmId).CourseId;

            // ACT
            var res = controller.ViewCourseMoments(courseId);

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            var vRes = (ViewResult)res;
            Assert.IsNotNull(vRes.Model);
            var model = (List<Planner>)vRes.Model;
            Assert.IsNotNull(model.Where(p => p.Vakcode == "AAA01A" && p.Gebouw == "A" && p.Verdiep == 1 && p.Nummer == "01").FirstOrDefault());
        }

        [TestMethod]
        public void EditStudsInCm_get_Test() 
        {
            // ARRANGE 
            PlannerService service = new PlannerService(ctxDb);
            PlannerController controller = SetupPlannerController(service).Result;

            int cmId = AddTestPlanning(service);

            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000001", Email = "r0000001@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000002", Email = "r0000002@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000003", Email = "r0000003@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.SaveChanges();

            string user1Id = ctxDb.Users.Where(u => u.UserName == "r0000001").FirstOrDefault().Id;
            string user2Id = ctxDb.Users.Where(u => u.UserName == "r0000002").FirstOrDefault().Id;
            string user3Id = ctxDb.Users.Where(u => u.UserName == "r0000003").FirstOrDefault().Id;

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "AAA01A").FirstOrDefault().Id;

            ctxDb.CourseUser.Add(new CourseUser { ApplicationUserId = user1Id, CourseId = courseId, GoedGekeurd = true});
            ctxDb.CourseUser.Add(new CourseUser { ApplicationUserId = user2Id, CourseId = courseId, GoedGekeurd = true});
            ctxDb.CourseUser.Add(new CourseUser { ApplicationUserId = user3Id, CourseId = courseId, GoedGekeurd = true});
            ctxDb.SaveChanges();

            // ACT
            var res = controller.EditStudsInCm(cmId);

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            var vRes = (ViewResult)res;
            Assert.IsNotNull(vRes.Model);
            var model = (List<EditStudInCmViewModel>)vRes.Model;
            Assert.AreEqual(3 ,model.Count());

        }

        [TestMethod]
        public void EditStudsInCm_Set_Test() 
        {
            // ARRANGE 
            PlannerService service = new PlannerService(ctxDb);
            PlannerController controller = SetupPlannerController(service).Result;

            int cmId = AddTestPlanning(service);

            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000001", Email = "r0000001@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000002", Email = "r0000002@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000003", Email = "r0000003@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.SaveChanges();

            string user1Id = ctxDb.Users.Where(u => u.UserName == "r0000001").FirstOrDefault().Id;
            string user2Id = ctxDb.Users.Where(u => u.UserName == "r0000002").FirstOrDefault().Id;
            string user3Id = ctxDb.Users.Where(u => u.UserName == "r0000003").FirstOrDefault().Id;

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "AAA01A").FirstOrDefault().Id;

            ctxDb.CourseUser.Add(new CourseUser { ApplicationUserId = user1Id, CourseId = courseId, GoedGekeurd = true });
            ctxDb.CourseUser.Add(new CourseUser { ApplicationUserId = user2Id, CourseId = courseId, GoedGekeurd = true });
            ctxDb.CourseUser.Add(new CourseUser { ApplicationUserId = user3Id, CourseId = courseId, GoedGekeurd = true });
            ctxDb.SaveChanges();

            List<EditStudInCmViewModel> modelList = (List<EditStudInCmViewModel>)(((ViewResult)controller.EditStudsInCm(cmId)).Model);

            foreach (EditStudInCmViewModel stud in modelList) 
            {
                stud.IsSelected = true;
            }

            // ACT
            var res = controller.EditStudsInCm(modelList, cmId);

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            Assert.IsNotNull(ctxDb.CourseMomentUsers.Where(cmu => cmu.ApplicationUserId == user1Id).FirstOrDefault());
            Assert.IsNotNull(ctxDb.CourseMomentUsers.Where(cmu => cmu.ApplicationUserId == user2Id).FirstOrDefault());
            Assert.IsNotNull(ctxDb.CourseMomentUsers.Where(cmu => cmu.ApplicationUserId == user3Id).FirstOrDefault());
        }

        [TestMethod]
        public void ViewCourseUsersTest() 
        {
            // ARRANGE 
            PlannerService service = new PlannerService(ctxDb);
            PlannerController controller = SetupPlannerController(service).Result;

            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000001", Email = "r0000001@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000002", Email = "r0000002@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000003", Email = "r0000003@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Course.Add(new Course { Vakcode = "AAA01A", Titel = "testvak 1", Studiepunten = 20 });
            ctxDb.SaveChanges();

            string user1Id = ctxDb.Users.Where(u => u.UserName == "r0000001").FirstOrDefault().Id;
            string user2Id = ctxDb.Users.Where(u => u.UserName == "r0000002").FirstOrDefault().Id;
            string user3Id = ctxDb.Users.Where(u => u.UserName == "r0000003").FirstOrDefault().Id;

            var roleId = ctxDb.Roles.Where(r => r.Name == "Student").FirstOrDefault().Id;

            ctxDb.UserRoles.Add(new IdentityUserRole<string> { UserId = user2Id, RoleId = roleId });
            ctxDb.UserRoles.Add(new IdentityUserRole<string> { UserId = user3Id, RoleId = roleId });
            ctxDb.SaveChanges();

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "AAA01A").FirstOrDefault().Id;

            ctxDb.CourseUser.Add(new CourseUser { ApplicationUserId = user1Id, CourseId = courseId, GoedGekeurd = true });
            ctxDb.CourseUser.Add(new CourseUser { ApplicationUserId = user2Id, CourseId = courseId, GoedGekeurd = false });
            ctxDb.CourseUser.Add(new CourseUser { ApplicationUserId = user3Id, CourseId = courseId, GoedGekeurd = true });
            ctxDb.SaveChanges();
            
            int cuId = AddTestPlanning(service);

            ctxDb.CourseMomentUsers.Add(new CourseMomentUsers { ApplicationUserId = user1Id, CoursMomentId = cuId});
            ctxDb.CourseMomentUsers.Add(new CourseMomentUsers { ApplicationUserId = user3Id, CoursMomentId = cuId});
            ctxDb.SaveChanges();

            // ACT
            var res = controller.ViewCourseUsers(courseId);

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            var vRes = (ViewResult)res;
            Assert.IsNotNull(vRes.Model);
            var cuvm = (CourseUsersViewModel)vRes.Model;
            Assert.AreEqual(courseId, cuvm.cId);
            Assert.AreEqual(2, cuvm.users.Count);
        }

        [TestMethod]
        public async Task AddStudsToEachCmTest() 
        {
            // ARRANGE 
            PlannerService service = new PlannerService(ctxDb);
            PlannerController controller = SetupPlannerController(service).Result;

            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000001", Email = "r0000001@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000002", Email = "r0000002@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000003", Email = "r0000003@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Course.Add(new Course { Vakcode = "AAA01A", Titel = "testvak 1", Studiepunten = 20 });
            ctxDb.SaveChanges();

            string user1Id = ctxDb.Users.Where(u => u.UserName == "r0000001").FirstOrDefault().Id;
            string user2Id = ctxDb.Users.Where(u => u.UserName == "r0000002").FirstOrDefault().Id;
            string user3Id = ctxDb.Users.Where(u => u.UserName == "r0000003").FirstOrDefault().Id;

            var roleId = ctxDb.Roles.Where(r => r.Name == "Student").FirstOrDefault().Id;

            ctxDb.UserRoles.Add(new IdentityUserRole<string> { UserId = user2Id, RoleId = roleId });
            ctxDb.UserRoles.Add(new IdentityUserRole<string> { UserId = user3Id, RoleId = roleId });
            ctxDb.SaveChanges();

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "AAA01A").FirstOrDefault().Id;

            ctxDb.CourseUser.Add(new CourseUser { ApplicationUserId = user1Id, CourseId = courseId, GoedGekeurd = true });
            ctxDb.CourseUser.Add(new CourseUser { ApplicationUserId = user2Id, CourseId = courseId, GoedGekeurd = true });
            ctxDb.CourseUser.Add(new CourseUser { ApplicationUserId = user3Id, CourseId = courseId, GoedGekeurd = true });
            ctxDb.SaveChanges();

            int cuId = AddTestPlanning(service);

            ctxDb.CourseMomentUsers.Add(new CourseMomentUsers { ApplicationUserId = user1Id, CoursMomentId = cuId });
            ctxDb.CourseMomentUsers.Add(new CourseMomentUsers { ApplicationUserId = user3Id, CoursMomentId = cuId });
            ctxDb.SaveChanges();

            var model = service.GetStudsInEachCm(courseId);

            foreach (var user in model) 
            {
                user.IsSelected = !user.IsSelected;
            }

            // ACT
            var res = await controller.AddStudsToEachCm(model, courseId);

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            Assert.IsNotNull(ctxDb.CourseMomentUsers.Where(cmu => cmu.ApplicationUserId == user2Id).FirstOrDefault());
            Assert.IsNull(ctxDb.CourseMomentUsers.Where(cmu => cmu.ApplicationUserId == user1Id).FirstOrDefault());
        }

        private async Task<PlannerController> SetupPlannerController(PlannerService service)
        {
            ctxDb.Roles.Add(new IdentityRole { Name = "Student", NormalizedName = "STUDENT" });
            ctxDb.SaveChanges();

            AccountService accService = new AccountService(userManager, signInManager);
            var user = await accService.RegisterUser(new RegisterViewModel { RNum = "r0000001", Email = "r0000001@hotmail.com", Name = "Thomas", SurName = "Claes", Password = "Xx*123", ConfirmPassword = "Xx*123", GeboorteDatum = new DateTime(1998, 9, 21) });

            IOptions<IdentityOptions> options = Options.Create<IdentityOptions>(new IdentityOptions { });

            UserClaimsPrincipalFactory<ApplicationUser> userClaimFactory = new UserClaimsPrincipalFactory<ApplicationUser>(userManager, options);

            var claim = userClaimFactory.CreateAsync(user).Result;

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.User).Returns(claim);

            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));

            return new PlannerController(service, userManager, signInManager) { ControllerContext = context, TempData = new TempDataDictionary(httpContext.Object, Mock.Of<ITempDataProvider>()) };
        }

        private int AddTestPlanning(PlannerService service) 
        {
            ApplicationUser user = new ApplicationUser { UserName = "r0664186", Email = "testemail@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true };
            ctxDb.Users.Add(user);

            Room room = new Room { Gebouw = "A", Verdiep = 1, Nummer = "01", Type = "Lokaal", Capaciteit = 20 };
            ctxDb.Room.Add(room);

            Course course = new Course { Vakcode = "AAA01A", Titel = "testvak", Studiepunten = 4 };
            ctxDb.Course.Add(course);
            ctxDb.SaveChanges();

            var userFull = ctxDb.Users.Where(u => u.UserName == "r0664186").FirstOrDefault();
            int roomId = ctxDb.Room.Where(r => r.Type == "Lokaal").FirstOrDefault().Id;
            int courseId = ctxDb.Course.Where(c => c.Vakcode == "AAA01A").FirstOrDefault().Id;

            DateTime datum = new DateTime(DateTime.Now.Year, DateTime.Now.Month, (DateTime.Now.Day + 1));
            DateTime start = new DateTime(1800, 1, 1, 11, 0, 0);
            double duratie = 2.0;

            service.AddPlanning(userFull, datum, start, duratie, roomId, courseId, null, null, 3);

            return ctxDb.CourseMoment.Where(cm => cm.CourseId == courseId && cm.RoomId == roomId).FirstOrDefault().Id;
        }
    }
}
