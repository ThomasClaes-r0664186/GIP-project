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
    class LectorServiceTests
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
            LectorService service = new LectorService(ctxDb);
            /*
            CourseUser user1 = new CourseUser { GoedGekeurd = false, Id =11,  ApplicationUserId = "190efbed - 8a40 - 425c - aac7 - 4e21261760bc", CourseId=13,AfwijzingBeschr=null };
            CourseUser user2 = new CourseUser { GoedGekeurd = false, Id = 13, ApplicationUserId = "cd81f7f8-fcfc-4b2e-9787-a9a3aacc798f", CourseId = 11, AfwijzingBeschr = null };
            CourseUser user3 = new CourseUser { GoedGekeurd = false, Id = 14, ApplicationUserId = "53f00b16-1758-4fd0-8bd2-6017e466ee7d", CourseId = 11, AfwijzingBeschr = null };
            CourseUser user4 = new CourseUser { GoedGekeurd = true, Id = 15, ApplicationUserId = "15f4dd33-d275-4c68-9617-7cdc0f29f437", CourseId = 11, AfwijzingBeschr = null };
            CourseUser user5 = new CourseUser { GoedGekeurd = true, Id = 16, ApplicationUserId = "ddc7020f-15ba-4e32-9514-f55624d74baa", CourseId = 11, AfwijzingBeschr = null };
           */
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

            for (int i = 0; i < requests.Count; i++)
            {
                Assert.IsTrue(requests[i].RNum == cu1.ApplicationUserId);
                Assert.IsTrue(requests[i].RNum == cu2.ApplicationUserId);

            }

        }
    }
}
