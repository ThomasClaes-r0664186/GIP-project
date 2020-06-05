using Gip.Models;
using Gip.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GipUnitTest.ServiceTests
{
    [TestClass]
    public class FieldOfStudyServiceTests
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
        public async Task GetAllFieldOfStudyTest() 
        {
            // ARRANGE
            FieldOfStudyService service = new FieldOfStudyService(ctxDb);
            FieldOfStudy FOS1 = new FieldOfStudy { RichtingCode = "MGP", RichtingTitel = "Model graduaat programmeren", Type = "Graduaat", RichtingStudiepunten = 60};
            FieldOfStudy FOS2 = new FieldOfStudy { RichtingCode = "MBI", RichtingTitel = "Model bachelor informatica", Type = "bachelor", RichtingStudiepunten = 90};
            FieldOfStudy FOS3 = new FieldOfStudy { RichtingCode = "MBC", RichtingTitel = "Model bachelor chemie", Type = "bachelor", RichtingStudiepunten = 120};

            ctxDb.FieldOfStudy.Add(FOS1);
            ctxDb.FieldOfStudy.Add(FOS2);
            ctxDb.FieldOfStudy.Add(FOS3);

            ctxDb.SaveChanges();

            // ACT
            var fosList = service.GetAllFieldOfStudy();

            // ASSERT
            Assert.IsTrue((await fosList.FirstOrDefaultAsync()).RichtingCode == "MBC");
        }

        [TestMethod]
        public void GetStudAlreadySubscribedTest() 
        {
            // ARRANGE

            /*the commented area is to check if functions returns -1 when there isn't a fos*/
            FieldOfStudyService service = new FieldOfStudyService(ctxDb);

            ApplicationUser user = new ApplicationUser { UserName = "r0664186", Email = "testemail@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true };
            ctxDb.Users.Add(user);
            ctxDb.SaveChanges();

            string userId = ctxDb.Users.Where(u => u.UserName == "r0664186").FirstOrDefault().Id;

            FieldOfStudy fos = new FieldOfStudy { RichtingCode = "MGP", RichtingTitel = "model graduaat programmeren", Type = "graduaat"};
            ctxDb.FieldOfStudy.Add(fos);
            ctxDb.SaveChanges();

            int fosId = ctxDb.FieldOfStudy.Where(fos => fos.RichtingCode == "MGP").FirstOrDefault().Id;

            Course course = new Course { Vakcode = "MGP01A", Titel = "front end", Studiepunten = 6, FieldOfStudyId = fosId };
            ctxDb.Course.Add(course);

            //Course course1 = new Course { Vakcode = "MBP01A", Titel = "front end", Studiepunten = 8};
            //ctxDb.Course.Add(course1);

            ctxDb.SaveChanges();

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "MGP01A").FirstOrDefault().Id;
            //int course1Id = ctxDb.Course.Where(c => c.Vakcode == "MBP01A").FirstOrDefault().Id;

            CourseUser cu = new CourseUser { ApplicationUserId = userId, CourseId = courseId };
            ctxDb.CourseUser.Add(cu);
            //CourseUser cu1 = new CourseUser { ApplicationUserId = userId, CourseId = course1Id};
            //ctxDb.CourseUser.Add(cu1);
            ctxDb.SaveChanges();

            // ACT
            int reqFosId = service.GetStudAlreadySubscribed(ctxDb.Users.Find(userId));

            // ASSERT
            Assert.AreEqual(fosId, reqFosId);

        }

        [TestMethod]
        public void AddRichtingTest()
        {
            // ARRANGE
            FieldOfStudyService service = new FieldOfStudyService(ctxDb);

            // ACT
            service.AddRichting("MGP", "Model graduaat programmeren", "Graduaat");

            // ASSERT
            Assert.IsTrue(ctxDb.FieldOfStudy.FirstOrDefault().RichtingCode == "MGP");
        }

        [TestMethod]
        public void DeleteRichtingTest()
        {
            // ARRANGE
            FieldOfStudyService service = new FieldOfStudyService(ctxDb);
            service.AddRichting("MGP", "Model graduaat programmeren", "Graduaat");
            int fosId = ctxDb.FieldOfStudy.Where(fos => fos.RichtingCode == "MGP").FirstOrDefault().Id;

            // ACT
            service.DeleteRichting(fosId);

            // ASSERT
            Assert.IsTrue(ctxDb.FieldOfStudy.FirstOrDefault() == null);
        }

        [TestMethod]
        public void GetRichtingTest()
        {
            // ARRANGE
            FieldOfStudyService service = new FieldOfStudyService(ctxDb);
            service.AddRichting("MGP", "Model graduaat programmeren", "Graduaat");
            int fosId = ctxDb.FieldOfStudy.Where(fos => fos.RichtingCode == "MGP").FirstOrDefault().Id;

            // ACT
            FieldOfStudy fos = service.GetRichting(fosId);

            // ASSERT
            Assert.AreEqual("MGP", fos.RichtingCode);
            Assert.AreEqual("model graduaat programmeren", fos.RichtingTitel);
            Assert.AreEqual("graduaat", fos.Type);
        }

        [TestMethod]
        public void EditRichtingTest()
        {
            // ARRANGE
            FieldOfStudyService service = new FieldOfStudyService(ctxDb);
            service.AddRichting("MGP", "Model graduaat programmeren", "Graduaat");
            int fosId = ctxDb.FieldOfStudy.Where(fos => fos.RichtingCode == "MGP").FirstOrDefault().Id;

            // ACT
            service.EditRichting(fosId, "MBP", "Model bachelor programmeren", "Bachelor");
            FieldOfStudy fos = service.GetRichting(fosId);

            // ASSERT
            Assert.AreEqual("MBP", fos.RichtingCode);
            Assert.AreEqual("Model bachelor programmeren", fos.RichtingTitel);
            Assert.AreEqual("Bachelor", fos.Type);
        }

        [TestMethod]
        public void SubscribeFOSTest()
        {
            // ARRANGE
            FieldOfStudyService service = new FieldOfStudyService(ctxDb);
            VakService vakService = new VakService(ctxDb);

            service.AddRichting("MGP", "Model graduaat programmeren", "Graduaat");
            int fosId = ctxDb.FieldOfStudy.Where(fos => fos.RichtingCode == "MGP").FirstOrDefault().Id;

            vakService.AddVak("MGP01A", "Programmeren met C#: basis", 6);
            vakService.AddVak("MGP01B", "Programmeren met C#: gevorderd", 4);
            vakService.AddVak("MGP01C", "Security", 8);

            ApplicationUser user = new ApplicationUser { UserName = "r0664186", Email = "testemail@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true };
            ctxDb.Users.Add(user);

            ctxDb.SaveChanges();

            // ACT
            service.SubscribeFos(fosId, user);

            // ASSERT
            Assert.AreEqual(3, ctxDb.Course.Where(c => c.FieldOfStudyId == fosId).Count());

        }
    }
}
