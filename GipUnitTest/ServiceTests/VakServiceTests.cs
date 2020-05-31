using Gip.Models;
using Gip.Models.ViewModels;
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
    public class VakServiceTests
    {
        private gipDatabaseContext ctxDb;

        // TestInit en TestCleanup worden voor en na elke test gedaan. Dit om ervoor te zorgen dat je geen gekoppelde testen hebt. (Geen waardes hergebruikt)

        [TestInitialize]
        public void TestInit() {
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
        public void EmptyDbReturnsZeroCoursesTest(){
            // ARRANGE
            VakService service = new VakService(ctxDb);

            // ACT
            var vakList = service.GetVakkenLectAdm();

            // ASSERT
            Assert.IsTrue(vakList.Count == 0);
        }

        [TestMethod]
        public void AddVakTest() {
            // ARRANGE
            VakService service = new VakService(ctxDb);

            // ACT
            service.AddVak("MGP01A", "Programmeren met C#: basis", 6);

            // ASSERT
            Course course = ctxDb.Course.Where(c => c.Vakcode.Equals("MGP01A")).FirstOrDefault();
            Assert.IsNotNull(course);
            Assert.IsTrue(course.Vakcode == "MGP01A");
            Assert.IsTrue(course.Titel == "Programmeren met C#: basis");
            Assert.IsTrue(course.Studiepunten == 6);
        }

        [TestMethod]
        public void GetVakkenReturnsCorrectCoursesTest() {
            // ARRANGE
            VakService service = new VakService(ctxDb);

            Course course1 = new Course { Vakcode = "MGP01A", Titel = "Programmeren met C#: basis", Studiepunten = 4 };
            Course course2 = new Course { Vakcode = "MGP02A", Titel = "Programmeren met C#: gevorderd", Studiepunten = 6 };
            Course course3 = new Course { Vakcode = "MGP03A", Titel = "Front-end: basis", Studiepunten = 4 };
            Course course4 = new Course { Vakcode = "MGP04A", Titel = "Front-end: gevorderd", Studiepunten = 6 };

            List<Course> courseList = new List<Course>();

            courseList.Add(course1);
            courseList.Add(course2);
            courseList.Add(course3);
            courseList.Add(course4);

            // ACT
            service.AddVak(course1.Vakcode, course1.Titel, course1.Studiepunten);
            service.AddVak(course2.Vakcode, course2.Titel, course2.Studiepunten);
            service.AddVak(course3.Vakcode, course3.Titel, course3.Studiepunten);
            service.AddVak(course4.Vakcode, course4.Titel, course4.Studiepunten);

            var VakkenList = service.GetVakkenLectAdm();

            // ASSERT
            Assert.IsTrue(VakkenList.Count == 4);

            for (int i = 0; i < VakkenList.Count; i++) {
                Assert.IsTrue(VakkenList[i].Vakcode == courseList[i].Vakcode);
                Assert.IsTrue(VakkenList[i].Titel == courseList[i].Titel);
                Assert.IsTrue(VakkenList[i].Studiepunten == courseList[i].Studiepunten);
            }
        }

        [TestMethod]
        public void DeleteVakTest()
        {
            // ARRANGE
            VakService service = new VakService(ctxDb);
            service.AddVak("MGP01A", "Programmeren met C#: basis", 4);

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "MGP01A").FirstOrDefault().Id;

            // ACT
            service.DeleteVak(courseId);

            // ASSERT
            Assert.IsTrue(service.GetVakkenLectAdm().Count == 0);
        }

        [TestMethod]
        public void EditVakTest() 
        {
            // ARRANGE
            VakService service = new VakService(ctxDb);
            service.AddVak("MGP01A", "Programmeren met C#: basis", 4);

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "MGP01A").FirstOrDefault().Id;

            String ChangedTitle = "Programmeren met C#: gevorderd";
            String ChangedVakcode = "MGP01B";
            //we gaan hetzelfde aantal studiepunten behouden

            // ACT
            service.EditVak(courseId, ChangedVakcode, ChangedTitle, 4);

            // ASSERT
            Course course = ctxDb.Course.Find(courseId);
            Assert.AreEqual(course.Vakcode, ChangedVakcode);
            Assert.AreEqual(course.Titel, ChangedTitle);
            Assert.AreEqual(course.Studiepunten, 4);
        }

        [TestMethod]
        public void SubscribeTest() 
        {
            // ARRANGE
            VakService service = new VakService(ctxDb);
            service.AddVak("MGP01A", "Programmeren met C#: basis", 4);

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "MGP01A").FirstOrDefault().Id;
            ApplicationUser user = new ApplicationUser { UserName = "r0664186", Email = "testemail@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true};
            ctxDb.Users.Add(user);

            // ACT
            service.Subscribe(courseId, user);

            // ASSERT
            CourseUser cu = ctxDb.CourseUser.Where(cu => cu.CourseId == courseId).FirstOrDefault();

            Assert.IsNotNull(cu);
            Assert.AreEqual(courseId, cu.CourseId);
            Assert.AreEqual(ctxDb.Users.Where(u => u.UserName == "r0664186").FirstOrDefault().Id, cu.ApplicationUserId);
        }

        [TestMethod]
        public void UnSubscribeTest() 
        {
            // ARRANGE
            VakService service = new VakService(ctxDb);

            service.AddVak("MGP01A", "Programmeren met C#: basis", 4);
            int courseId = ctxDb.Course.Where(c => c.Vakcode == "MGP01A").FirstOrDefault().Id;

            ApplicationUser user = new ApplicationUser { UserName = "r0664186", Email = "testemail@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true };
            ctxDb.Users.Add(user);

            service.Subscribe(courseId, user);

            // ACT
            service.UnSubscribe(courseId, user);

            // ASSERT
            Assert.IsTrue(ctxDb.CourseUser.Count() == 0);
        }
    }
}
