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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq;

namespace GipUnitTest.ControllerTests
{
    [TestClass]
    public class FieldOfStudyControllerTests
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
            FieldOfStudyService service = new FieldOfStudyService(ctxDb);
            FieldOfStudyController controller = SetupFosController(service).Result;

            service.AddRichting("MBP", "model bachelor programmeren", "bachelor");
            service.AddRichting("MGP", "model graduaat programmeren", "graduaat");
            service.AddRichting("MBF", "model bachelor financien", "bachelor");

            // ACT
            var res = controller.Index();

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            var vRes = (ViewResult)res;
            Assert.IsNotNull(vRes.Model);
            var model = (IQueryable<FieldOfStudy>)vRes.Model;

            Assert.AreEqual(3, model.Count());
        }

        [TestMethod]
        public void AddTest() 
        {
            // ARRANGE
            FieldOfStudyService service = new FieldOfStudyService(ctxDb);
            FieldOfStudyController controller = SetupFosController(service).Result;

            // ACT
            var res = controller.Add("MBP", "model bachelor programmeren", "bachelor");

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            var fos = ctxDb.FieldOfStudy.Where(fos => fos.RichtingCode == "MBP").FirstOrDefault(); 
            Assert.IsNotNull(fos);
            Assert.AreEqual(fos.RichtingCode, "MBP");
        }

        [TestMethod]
        public void DeleteTest() 
        {
            // ARRANGE
            FieldOfStudyService service = new FieldOfStudyService(ctxDb);
            FieldOfStudyController controller = SetupFosController(service).Result;

            service.AddRichting("MBP", "model bachelor programmeren", "bachelor");
            var fosId = ctxDb.FieldOfStudy.Where(fos => fos.RichtingCode == "MBP").FirstOrDefault().Id;

            // ACT
            var res = controller.Delete(fosId);

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            Assert.IsNull(ctxDb.FieldOfStudy.Where(fos => fos.RichtingCode == "MBP").FirstOrDefault());

        }

        [TestMethod]
        public void Edit_GetTheFieldOfStudy_Test() 
        {
            // ARRANGE
            FieldOfStudyService service = new FieldOfStudyService(ctxDb);
            FieldOfStudyController controller = SetupFosController(service).Result;

            service.AddRichting("MBP", "model bachelor programmeren", "bachelor");
            var fosId = ctxDb.FieldOfStudy.Where(fos => fos.RichtingCode == "MBP").FirstOrDefault().Id;

            // ACT
            var res = controller.Edit(fosId);

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            var vRes = (ViewResult)res;
            Assert.IsNotNull(vRes.Model);
            var model = (FieldOfStudy)vRes.Model;
            Assert.AreEqual("MBP", model.RichtingCode);
        }

        [TestMethod]
        public void Edit_WithParameters_Test() 
        {
            // ARRANGE
            FieldOfStudyService service = new FieldOfStudyService(ctxDb);
            FieldOfStudyController controller = SetupFosController(service).Result;

            service.AddRichting("MBP", "model bachelor programmeren", "bachelor");
            var fosId = ctxDb.FieldOfStudy.Where(fos => fos.RichtingCode == "MBP").FirstOrDefault().Id;

            // ACT
            var res = controller.Edit(fosId, "MGP", "model graduaat programmeren", "graduaat");

            // ACCEPT
            Assert.IsTrue(res is ActionResult);
            var fos = ctxDb.FieldOfStudy.Find(fosId);
            Assert.AreEqual("MGP", fos.RichtingCode);
            Assert.AreEqual("model graduaat programmeren", fos.RichtingTitel);
            Assert.AreEqual("graduaat", fos.Type);

        }

        [TestMethod]
        public async Task SubscribeTest()
        {
            // ARRANGE 
            FieldOfStudyService service = new FieldOfStudyService(ctxDb);
            FieldOfStudyController controller = SetupFosController(service).Result;

            service.AddRichting("MBP", "model bachelor programmeren", "bachelor");
            var fosId = ctxDb.FieldOfStudy.Where(fos => fos.RichtingCode == "MBP").FirstOrDefault().Id;

            VakService vakService = new VakService(ctxDb);
            vakService.AddVak("MBP01A", "testvak 1", 4);
            vakService.AddVak("MBP02A", "testvak 2", 4);
            vakService.AddVak("MBP03A", "testvak 3", 4);

            var vak1Id = ctxDb.Course.Where(c => c.Vakcode == "MBP01A").FirstOrDefault().Id;
            var vak2Id = ctxDb.Course.Where(c => c.Vakcode == "MBP02A").FirstOrDefault().Id;
            var vak3Id = ctxDb.Course.Where(c => c.Vakcode == "MBP03A").FirstOrDefault().Id;

            // ACT 
            var res = await controller.Subscribe(fosId);

            // ASSERT
            Assert.IsTrue(res is ActionResult);
            Assert.IsTrue(ctxDb.CourseUser.Any());
            Assert.IsNotNull(ctxDb.CourseUser.Where(cu => cu.CourseId == vak1Id).FirstOrDefault());
            Assert.IsNotNull(ctxDb.CourseUser.Where(cu => cu.CourseId == vak2Id).FirstOrDefault());
            Assert.IsNotNull(ctxDb.CourseUser.Where(cu => cu.CourseId == vak3Id).FirstOrDefault());

        }

        private async Task<FieldOfStudyController> SetupFosController(FieldOfStudyService service)
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

            return new FieldOfStudyController(service, userManager) { ControllerContext = context, TempData = new TempDataDictionary(httpContext.Object, Mock.Of<ITempDataProvider>()) };
        }
    }
}
