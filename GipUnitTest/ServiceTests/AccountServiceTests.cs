using Gip.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GipUnitTest.ServiceTests
{
    class AccountServiceTests
    {
        private gipDatabaseContext ctxDb;

        //Door complexiteit en minimale testbreedte, is voorlopig besloten dat we accountcontroller niet testen
        // TestInit en TestCleanup worden voor en na elke test gedaan. Dit om ervoor te zorgen dat je geen gekoppelde testen hebt. (Geen waardes hergebruikt)

        [TestInitialize]
        public void InitializeTestZone()
        {
            var builder = new DbContextOptionsBuilder<gipDatabaseContext>();
            builder.UseInMemoryDatabase("gipDatabase");
            this.ctxDb = new gipDatabaseContext(builder.Options);
            if (ctxDb != null)
            {
                ctxDb.Database.EnsureDeleted();
                ctxDb.Database.EnsureCreated();
            }
            //var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            //var userManager = new UserManager<ApplicationUser>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            //var mockAuthMgr = new Mock<AuthenticationManager>();
            //var signInManager = new Mock<SignInManager<ApplicationUser>>(userManager, mockAuthMgr.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.ctxDb.Dispose();
        }

        //
    }
}
