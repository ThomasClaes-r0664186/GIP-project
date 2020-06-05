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
using System.Linq;

namespace GipUnitTest.ControllerTests
{
    [TestClass]
    public class LokaalControllerTests
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
        public void AddTest()
        {
            // ARRANGE
            LokaalService service = new LokaalService(ctxDb);
            LokaalController controller = SetupLokaalController(service).Result;

            // ACT
            var res = controller.Add("A", 1, "01", "Lokaal", 20, "wifi");

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            Assert.IsNotNull(ctxDb.Room.Where(r => r.Gebouw == "A" && r.Verdiep == 1 && r.Gebouw == "01"));

        }

        [TestMethod]
        public void DeleteTest() 
        {
            // ARRANGE
            LokaalService service = new LokaalService(ctxDb);
            LokaalController controller = SetupLokaalController(service).Result;
            service.AddLokaal("A", 1, "01", "Lokaal", 20, "wifi");
            var roomId = ctxDb.Room.Where(r => r.Gebouw == "A" && r.Verdiep == 1 && r.Nummer == "01").FirstOrDefault().Id;

            // ACT
            var res = controller.Delete(roomId);

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            Assert.IsNull(ctxDb.Room.Where(r => r.Gebouw == "A" && r.Verdiep == 1 && r.Nummer == "01").FirstOrDefault());

        }

        [TestMethod]
        public void EditTest() 
        {
            // ARRANGE
            LokaalService service = new LokaalService(ctxDb);
            LokaalController controller = SetupLokaalController(service).Result;
            service.AddLokaal("A", 1, "01", "Lokaal", 20, "wifi");
            var roomId = ctxDb.Room.Where(r => r.Gebouw == "A" && r.Verdiep == 1 && r.Nummer == "01").FirstOrDefault().Id;

            // ACT
            var res = controller.Edit(roomId, "B", 1, "02", "Lokaal", 20, "wifi");

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            Assert.AreEqual("B", ctxDb.Room.Find(roomId).Gebouw);
            Assert.AreEqual("02", ctxDb.Room.Find(roomId).Nummer);

        }

        private async Task<LokaalController> SetupLokaalController(LokaalService service)
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

            return new LokaalController(service) { ControllerContext = context, TempData = new TempDataDictionary(httpContext.Object, Mock.Of<ITempDataProvider>()) };
        }
    }
}
