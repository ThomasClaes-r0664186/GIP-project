using Gip.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GipUnitTest.ServiceTests
{
    class PlannerServiceTests
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

        //


        [TestMethod]
        public void GetPlanningLectAdminTest() 
        {
            /*
            // ARRANGE
            Course course = new Course { Vakcode = "", Titel = "", Studiepunten = 0};
            ctxDb.Course.Add(course);
            
            Room room = new Room { Gebouw = "", Verdiep = 0, Nummer = "", Type = "", Capaciteit = 20};
            ctxDb.Room.Add(room);

            Schedule schedule = new Schedule { Datum = new DateTime(2020, 06, 5), Startmoment = new DateTime(0, 0, 0, 11, 0, 0), Eindmoment = new DateTime(0, 0, 0, 13, 0, 0) };
            ctxDb.Schedule.Add(schedule);

            ApplicationUser user = new ApplicationUser { };
            ctxDb.Users.Add(user);

            ctxDb.SaveChanges();

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "").FirstOrDefault().Id;
            int roomId = ctxDb.Room.Where(r => r.Gebouw == "" & r.Verdiep == 0 & r.Nummer == "").FirstOrDefault().Id;
            int scheduleId = ctxDb.Schedule.Where(s => s.Datum == new DateTime(0, 0, 0)).FirstOrDefault().Id;
            string userId = ctxDb.Users.Where(u => u.UserName == "").FirstOrDefault().Id;
            
            CourseMoment cm = new CourseMoment { };
            
            // ACT
            
            // ASSERT
            */
        }

        [TestMethod]
        public void AddPlanningTest() 
        {
            // ARRANGE

            // ACT
            
            // ASSERT
            
        }

        [TestMethod]
        public void GetLokalenTest() 
        {
            // ARRANGE

            // ACT
            
            // ASSERT
            
        }

        [TestMethod]
        public void DeletePlanningTest() 
        {
            // ARRANGE

            // ACT
            
            // ASSERT
            
        }

        [TestMethod]
        public void EditPlanningTest() 
        {
            // ARRANGE

            // ACT
            
            // ASSERT
            
        }

        [TestMethod]
        public void GetTopicTest() 
        {
            // ARRANGE

            // ACT
            
            // ASSERT
            
        }

        [TestMethod]
        public void GetCourseMomentsTest() 
        {
            // ARRANGE

            // ACT
            
            // ASSERT
            
        }

        [TestMethod]
        public void GetStudsInCmTest() 
        {
            // ARRANGE

            // ACT
            
            // ASSERT
            
        }

        [TestMethod]
        public void EditStudsInCmTest() 
        {
            // ARRANGE

            // ACT
            
            // ASSERT
            
        }

        [TestMethod]
        public void GetCourseUsersTest() 
        {
            // ARRANGE

            // ACT
            
            // ASSERT
            
        }

        [TestMethod]
        public void GetStudsInEachCmTest() 
        {
            // ARRANGE

            // ACT
            
            // ASSERT
            
        }

        [TestMethod]
        public void AddStudsToEachCmTest() 
        {
            // ARRANGE

            // ACT
            
            // ASSERT
            
        }
    }
}
