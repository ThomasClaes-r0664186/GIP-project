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
    public class LectorServiceTests
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
        public void GetStudentRequestsTest()
        {
            // ARRANGE
            LectorService service = new LectorService(ctxDb);
            ApplicationUser user1 = new ApplicationUser { UserName = "r0664186", Email = "testemail@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Cleas", VoorNaam = "Thomas", EmailConfirmed = true };
            ApplicationUser user2 = new ApplicationUser { UserName = "r1234567", Email = "testemail@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Haesevoets", VoorNaam = "Jaimie", EmailConfirmed = true };
            ApplicationUser user3 = new ApplicationUser { UserName = "r2345678", Email = "testemail@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "VanBeal", VoorNaam = "Rik", EmailConfirmed = true };
            ctxDb.Users.Add(user1);
            ctxDb.Users.Add(user2);
            ctxDb.Users.Add(user3);
            ctxDb.SaveChanges();

            string userId1 = ctxDb.Users.Where(u => u.UserName == "r0664186").FirstOrDefault().Id;
            string userId2 = ctxDb.Users.Where(u => u.UserName == "r1234567").FirstOrDefault().Id;
            string userId3 = ctxDb.Users.Where(u => u.UserName == "r2345678").FirstOrDefault().Id;

            Course course = new Course { Vakcode = "MGP01A", Titel = "front end", Studiepunten = 6, FieldOfStudyId = 123 };
            ctxDb.Course.Add(course);
            ctxDb.SaveChanges();

            int courseId1 = ctxDb.Course.Where(c => c.Vakcode == "MGP01A").FirstOrDefault().Id;

            CourseUser cu1 = new CourseUser { ApplicationUserId = userId1, CourseId = courseId1, GoedGekeurd = false };
            CourseUser cu2 = new CourseUser { ApplicationUserId = userId2, CourseId = courseId1, GoedGekeurd = true };
            CourseUser cu3 = new CourseUser { ApplicationUserId = userId3, CourseId = courseId1, GoedGekeurd = false };

            ctxDb.CourseUser.Add(cu1);
            ctxDb.CourseUser.Add(cu2);
            ctxDb.CourseUser.Add(cu3);
            ctxDb.SaveChanges();

            // ACT
            var requests = service.GetStudentRequests();

            // ASSERT
            Assert.IsTrue(requests.Count == 2);

            for (int i = 1; i < requests.Count; i++)
            {
                Assert.IsTrue(requests[i].RNum == user1.UserName || requests[i].RNum == user3.UserName);
            }

        }

        [TestMethod]
        public void ApproveStudentTest()
        {
            // ARRANGE
            LectorService service = new LectorService(ctxDb);

            ApplicationUser user1 = new ApplicationUser { UserName = "r6660800", Email = "testemail@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Cleas", VoorNaam = "Thomas", EmailConfirmed = true };
            ApplicationUser user2 = new ApplicationUser { UserName = "r7770800", Email = "testemail@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Cleas", VoorNaam = "Thomas", EmailConfirmed = true };

            ctxDb.Users.Add(user1);
            ctxDb.Users.Add(user2);
            ctxDb.SaveChanges();

            string userId1 = ctxDb.Users.Where(u => u.UserName == "r6660800").FirstOrDefault().Id;
            string userId2 = ctxDb.Users.Where(u => u.UserName == "r7770800").FirstOrDefault().Id;

            Course course = new Course { Vakcode = "MGP20A", Titel = "test end", Studiepunten = 6, FieldOfStudyId = 124 };
            ctxDb.Course.Add(course);
            ctxDb.SaveChanges();

            int courseId1 = ctxDb.Course.Where(c => c.Vakcode == "MGP20A").FirstOrDefault().Id;


            CourseUser cu1 = new CourseUser { ApplicationUserId = userId1, CourseId = courseId1, GoedGekeurd = false };
            CourseUser cu2 = new CourseUser { ApplicationUserId = userId2, CourseId = courseId1, GoedGekeurd = true };

            ctxDb.CourseUser.Add(cu1);
            ctxDb.CourseUser.Add(cu2);
            ctxDb.SaveChanges();

            // ACT
            service.ApproveStudent(cu1.Id);
            service.ApproveStudent(cu2.Id);

            // ASSERT
            Assert.IsTrue(cu1.GoedGekeurd == true);
            Assert.IsTrue(cu2.GoedGekeurd == true);
        }


        [TestMethod]
        public void DenyStudentTest()
        {
            // ARRAGE
            LectorService service = new LectorService(ctxDb);

            ApplicationUser user1 = new ApplicationUser { UserName = "r6660800", Email = "testemail@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Cleas", VoorNaam = "Thomas", EmailConfirmed = true };
            ApplicationUser user2 = new ApplicationUser { UserName = "r7770800", Email = "testemail@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Cleas", VoorNaam = "Thomas", EmailConfirmed = true };

            ctxDb.Users.Add(user1);
            ctxDb.Users.Add(user2);
            ctxDb.SaveChanges();

            string userId1 = ctxDb.Users.Where(u => u.UserName == "r6660800").FirstOrDefault().Id;
            string userId2 = ctxDb.Users.Where(u => u.UserName == "r7770800").FirstOrDefault().Id;

            Course course = new Course { Vakcode = "MGP20A", Titel = "test end", Studiepunten = 6, FieldOfStudyId = 124 };
            ctxDb.Course.Add(course);
            ctxDb.SaveChanges();

            int courseId1 = ctxDb.Course.Where(c => c.Vakcode == "MGP20A").FirstOrDefault().Id;



            CourseUser cu1 = new CourseUser { ApplicationUserId = userId1, CourseId = courseId1, GoedGekeurd = false, AfwijzingBeschr ="mag niet" };
            CourseUser cu2 = new CourseUser { ApplicationUserId = userId2, CourseId = courseId1, GoedGekeurd = true, AfwijzingBeschr = "mag wel" };

            ctxDb.CourseUser.Add(cu1);
            ctxDb.CourseUser.Add(cu2);
            ctxDb.SaveChanges();

            // ACT
            service.DenyStudent(cu1.Id, cu1.AfwijzingBeschr);
            service.DenyStudent(cu2.Id, cu2.AfwijzingBeschr);

            // ASSERT
            Assert.IsTrue(cu1.GoedGekeurd == null && cu1.AfwijzingBeschr == "mag niet");
            Assert.IsFalse(cu2.GoedGekeurd == null && cu1.AfwijzingBeschr == "mag wel");
        }
    }
}
