using Gip.Models;
using Gip.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GipUnitTest.ServiceTests
{
    [TestClass]
    public class PlannerServiceTests
    {
        private gipDatabaseContext ctxDb;

        // TestInit en TestCleanup worden voor en na elke test gedaan. Dit om ervoor te zorgen dat je geen gekoppelde testen hebt. (Geen waardes hergebruikt)

        [TestInitialize]
        public void TestInit()
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
            // ARRANGE
            PlannerService service = new PlannerService(ctxDb);
            Course course = new Course { Vakcode = "MGP01A", Titel = "front end", Studiepunten = 6 };
            ctxDb.Course.Add(course);
            
            Room room = new Room { Gebouw = "A", Verdiep = 0, Nummer = "01", Type = "Lokaal", Capaciteit = 20};
            ctxDb.Room.Add(room);

            Schedule schedule = new Schedule { Datum = new DateTime(DateTime.Now.Year, DateTime.Now.Month, (DateTime.Now.Day + 1)), Startmoment = new DateTime(1800, 01, 10, 11, 0, 0), Eindmoment = new DateTime(1800, 01, 01, 13, 0, 0) };
            ctxDb.Schedule.Add(schedule);

            ApplicationUser user = new ApplicationUser { UserName = "r0664186", Email = "testemail@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true };
            ctxDb.Users.Add(user);

            ctxDb.SaveChanges();

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "MGP01A").FirstOrDefault().Id;
            int roomId = ctxDb.Room.Where(r => r.Gebouw == "A" & r.Verdiep == 0 & r.Nummer == "01").FirstOrDefault().Id;
            int scheduleId = ctxDb.Schedule.Where(s => s.Datum == new DateTime(DateTime.Now.Year, DateTime.Now.Month, (DateTime.Now.Day + 1))).FirstOrDefault().Id;
            string userId = ctxDb.Users.Where(u => u.UserName == "r0664186").FirstOrDefault().Id;
            
            CourseMoment cm = new CourseMoment { CourseId = courseId, RoomId = roomId, ScheduleId = scheduleId, ApplicationUserId = userId, LessenLijst = "Dit is een lesmoment om mee te testen"};
            ctxDb.CourseMoment.Add(cm);
            ctxDb.SaveChanges();

            // ACT
            var plannerList = service.GetPlanningLectAdmin(GetIso8601WeekOfYear(DateTime.Now));

            // ASSERT
            Assert.IsTrue(plannerList.Count() > 0);
            Assert.AreEqual(courseId, plannerList[0].cId);
            Assert.AreEqual(room.Gebouw, plannerList[0].Gebouw);
            Assert.AreEqual(room.Verdiep, plannerList[0].Verdiep);
            Assert.AreEqual(room.Nummer, plannerList[0].Nummer);
            Assert.AreEqual(schedule.Datum, plannerList[0].Datum);
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

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return (time.DayOfYear / 7);
        }
    }
}
