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
using Gip.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using Gip.Models.ViewModels;
using System.Linq;

namespace GipUnitTest.ControllerTests
{
    [TestClass]
    public class VakControllerTests
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
        public async Task IndexTest() 
        {
            // ARRANGE
            VakService service = new VakService(ctxDb);

            service.AddVak("AAA01A", "testvak 1", 2);
            service.AddVak("BBB01B", "testvak 2", 4);
            service.AddVak("CCC01C", "testvak 3", 6);

            VakController controller = SetupVakController(service).Result;

            // ACT
            IActionResult result = (ViewResult)controller.Index().Result;

            // ASSERT
            Assert.IsNotNull(result);
            Assert.IsTrue(result is ViewResult);
            ViewResult vRes = (ViewResult)result;
            Assert.IsTrue(vRes is ViewResult);
            Assert.IsTrue(vRes.Model is List<VakViewModel>);
            var model = (List<VakViewModel>)vRes.Model;
            Assert.AreEqual(3, model.Count);
        }

        [TestMethod]
        public async Task AddVakTest() 
        {
            // ARRANGE
            VakService service = new VakService(ctxDb);
            VakController controller = SetupVakController(service).Result;

            // ACT
            var res = controller.Add("AAA01A", "testvak 1", 4);

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            Assert.IsNotNull(ctxDb.Course.Where(c => c.Vakcode == "AAA01A").FirstOrDefault());
        }

        [TestMethod]
        public void DeleteTest() 
        {
            // ARRANGE
            VakService service = new VakService(ctxDb);
            VakController controller = SetupVakController(service).Result;
            service.AddVak("AAA01A", "testvak 1", 4);
            int vakId = ctxDb.Course.Where(c => c.Vakcode == "AAA01A").FirstOrDefault().Id;

            // ACT
            var res = controller.Delete(vakId);

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            Assert.IsNull(ctxDb.Course.Where(c => c.Vakcode == "AAA01A").FirstOrDefault());

        }

        [TestMethod]
        public void EditTest() 
        {
            // ARRANGE
            VakService service = new VakService(ctxDb);
            VakController controller = SetupVakController(service).Result;
            service.AddVak("AAA01A", "testvak 1", 4);
            int vakId = ctxDb.Course.Where(c => c.Vakcode == "AAA01A").FirstOrDefault().Id;

            // ACT
            var res = controller.Edit(vakId, "BBB01B", "testvak 2", 4);

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            Assert.AreEqual("BBB01B", ctxDb.Course.Find(vakId).Vakcode);

        }

        [TestMethod]
        public void SubscribeTest() 
        {
            // ARRANGE
            VakService service = new VakService(ctxDb);
            VakController controller = SetupVakController(service).Result;

            service.AddVak("AAA01A", "testvak 1", 4);
            int vakId = ctxDb.Course.Where(c => c.Vakcode == "AAA01A").FirstOrDefault().Id;

            // ACT
            var res = controller.Subscribe(vakId).Result;

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            var cu = ctxDb.CourseUser.Where(cu => cu.CourseId == vakId).Include("ApplicationUsers").FirstOrDefault();
            Assert.IsNotNull(cu);
            Assert.AreEqual(controller.User.Identity.Name, cu.ApplicationUsers.UserName);
        }

        [TestMethod]
        public void UnSubscribeTest() 
        {
            // ARRANGE
            VakService service = new VakService(ctxDb);
            VakController controller = SetupVakController(service).Result;

            service.AddVak("AAA01A", "testvak 1", 4);
            int vakId = ctxDb.Course.Where(c => c.Vakcode == "AAA01A").FirstOrDefault().Id;
            var user = ctxDb.Users.Where(u => u.UserName == "r0000001").FirstOrDefault();
            service.Subscribe(vakId, user);

            // ACT
            var res = controller.UnSubscribe(vakId).Result;

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            Assert.IsNull(ctxDb.CourseUser.Where(cu => cu.CourseId == vakId).Include("ApplicationUsers").FirstOrDefault());
        }

        private async Task<VakController> SetupVakController(VakService service) 
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

            return new VakController(service, userManager, signInManager) { ControllerContext = context, TempData = new TempDataDictionary(httpContext.Object, Mock.Of<ITempDataProvider>()) };
        }
    }
}
