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
        
        [TestMethod]
        public void AddLokaalTest()
        {
            // ARRANGE
            LokaalService service = new LokaalService(ctxDb);
            
            // ACT
            service.AddLokaal("A", 2, "01", "Vergaderlokaal", 5, "Geen middelen");
            Room room = ctxDb.Room.Where(c => c.Gebouw.Equals("A")).FirstOrDefault();

            // ASSERT
            Assert.IsNotNull(room);
            Assert.IsTrue(room.Gebouw == "A");
            Assert.IsTrue( room.Verdiep == 2);
            Assert.IsTrue(room.Nummer == "01");
        }

        [TestMethod]
        public void DeleteLokaalTest()
        {
            // ARRANGE
            LokaalService service = new LokaalService(ctxDb);
            service.AddLokaal("A", 2, "01", "Vergaderlokaal", 5, "Geen middelen");
            int lId = ctxDb.Room.Where(i => i.Verdiep.Equals(2)).FirstOrDefault().Id;
            
            // ACT
            service.DeleteLokaal(lId);

            // ASSERT
            Assert.IsTrue(!ctxDb.Room.Any());
        }

        [TestMethod]
        public void EditLokaalTest()
        {
            // ARRANGE
            LokaalService service = new LokaalService(ctxDb);
            service.AddLokaal("A", 2, "01", "Vergaderlokaal", 5, "Geen middelen");
            int lId = ctxDb.Room.Where(i => i.Verdiep.Equals(2)).FirstOrDefault().Id;
            string gebouw = "B";
            //verdiep blijft hetzelfde
            string nummer = "02";
            string type = "Aula";
            int capac = 10;
            string middelen = "Scherm";

            // ACT 
            service.EditLokaal(lId, gebouw, 2, nummer, type, capac, middelen);

            // ASSERT
            Room room = ctxDb.Room.Find(lId);
            Assert.IsTrue(room.Gebouw.Equals("B"));
            Assert.IsTrue(room.Verdiep ==2);
            Assert.IsTrue(room.Nummer.Equals("02"));
            Assert.IsTrue(room.Type.Equals("Aula"));
            Assert.IsTrue(room.Capaciteit == 10);
            Assert.IsTrue(room.Middelen.Equals("Scherm"));
           

        }
    }
}
