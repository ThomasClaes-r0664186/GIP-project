using Gip.Models;
using Gip.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GipUnitTest.ServiceTests
{
    [TestClass]
    public class LokaalServiceTests
    {
        private gipDatabaseContext ctxDb;

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
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.ctxDb.Dispose();
        }

<<<<<<< HEAD
        
=======
        //

        [TestMethod]
        public void IsTrue()
        {
            Assert.IsTrue(true);
        }
>>>>>>> 3bf81fc91cfd720727f879c7822bd030af2ee64e
    }
}
