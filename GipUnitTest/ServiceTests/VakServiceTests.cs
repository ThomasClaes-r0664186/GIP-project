using Gip.Models;
using Gip.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace GipUnitTest.ServiceTests
{
    [TestClass]
    public class VakServiceTests
    {
        private gipDatabaseContext ctxDb;

        // TestInit en TestCleanup worden voor en na elke test gedaan. Dit om ervoor te zorgen dat je geen gekoppelde testen hebt. (Geen waardes hergebruikt)

        [TestInitialize]
        public void InitializeTestZone() {
            var builder = new DbContextOptionsBuilder<gipDatabaseContext>();
            builder.UseInMemoryDatabase("gipDatabase");
            this.ctxDb = new gipDatabaseContext(builder.Options);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.ctxDb.Dispose();
        }

        //

        [TestMethod]
        public void TestEmptyDbReturnsZeroCourses(){
            // ARRANGE
            VakService service = new VakService(ctxDb);

            // ACT
            var vakList = service.GetVakkenLectAdm();

            // ASSERT
            Assert.IsTrue(vakList.Count == 0);

        }
    }
}
