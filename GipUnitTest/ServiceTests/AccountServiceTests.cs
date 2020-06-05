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
using System.Linq;

namespace GipUnitTest.ServiceTests
{
    [TestClass]
    public class AccountServiceTests
    {
        private SqliteConnection sqliteConnection;
        private gipDatabaseContext ctxDb;

        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private SignInManager<ApplicationUser> signInManager;


        //Door complexiteit en minimale testbreedte, is voorlopig besloten dat we accountservice niet testen
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

        //

        [TestMethod]
        public void CreateUserTest() 
        {
            // ARRANGE
            AccountService service = new AccountService(userManager, signInManager);
            RegisterViewModel model = new RegisterViewModel
            {
                RNum = "R0664186",
                Name = "Thomas",
                SurName = "Claes",
                Email = "thomas.claes@student.ucll.be",
                GeboorteDatum = new DateTime(1998, 9, 21),
                Password = "Xx*123",
                ConfirmPassword = "Xx*123"
            };
            roleManager.CreateAsync(new IdentityRole { Name = "Student"});
            roleManager.CreateAsync(new IdentityRole { Name = "Lector"});
            roleManager.CreateAsync(new IdentityRole { Name = "Admin"});

            // ACT
            ApplicationUser user = service.RegisterUser(model).Result;

            // ASSERT
            Assert.IsNotNull(user);
            Assert.AreEqual(model.Email, user.Email);
        }
    }
}
