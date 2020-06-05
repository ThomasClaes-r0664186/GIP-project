using Gip.Models;
using Gip.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            AddCm();

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "MGP01A").FirstOrDefault().Id;
            int roomId = ctxDb.Room.Where(r => r.Gebouw == "A" & r.Verdiep == 0 & r.Nummer == "01").FirstOrDefault().Id;
            var room = ctxDb.Room.Where(r => r.Gebouw == "A" & r.Verdiep == 0 & r.Nummer == "01").FirstOrDefault();
            int scheduleId = ctxDb.Schedule.Where(s => s.Datum == new DateTime(DateTime.Now.Year, DateTime.Now.Month, (DateTime.Now.Day + 1))).FirstOrDefault().Id;
            var schedule = ctxDb.Schedule.Where(s => s.Datum == new DateTime(DateTime.Now.Year, DateTime.Now.Month, (DateTime.Now.Day + 1))).FirstOrDefault();
            string userId = ctxDb.Users.Where(u => u.UserName == "r0664186").FirstOrDefault().Id;

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
            PlannerService service = new PlannerService(ctxDb);
            ApplicationUser user = new ApplicationUser { UserName = "r0664186", Email = "testemail@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true };
            ctxDb.Users.Add(user);

            Room room = new Room { Gebouw = "A", Verdiep = 1, Nummer = "01", Type = "Lokaal" };
            ctxDb.Room.Add(room);

            Course course = new Course { Vakcode ="AAA01A", Titel ="testvak", Studiepunten = 4};
            ctxDb.Course.Add(course);
            ctxDb.SaveChanges();

            var userFull = ctxDb.Users.Where(u => u.UserName == "r0664186").FirstOrDefault();
            int roomId = ctxDb.Room.Where(r => r.Type == "Lokaal").FirstOrDefault().Id;
            int courseId = ctxDb.Course.Where(c => c.Vakcode == "AAA01A").FirstOrDefault().Id;

            DateTime datum = new DateTime(DateTime.Now.Year, DateTime.Now.Month, (DateTime.Now.Day + 1));
            DateTime start = new DateTime(1800, 1, 1, 11, 0, 0);
            double duratie = 2.0;

            // ACT
            service.AddPlanning(userFull, datum, start, duratie, roomId, courseId, null, null, 3);

            // ASSERT
            Assert.IsTrue(ctxDb.CourseMoment.Any());
        }

        [TestMethod]
        public void GetLokalenTest() 
        {
            // ARRANGE
            PlannerService service = new PlannerService(ctxDb);

            ctxDb.Room.Add(new Room { Gebouw = "A", Verdiep = 1, Nummer = "01A", Type = "Aula", Capaciteit = 50});
            ctxDb.Course.Add(new Course { Vakcode = "AAA01A", Titel = "testvak", Studiepunten = 4});
            ctxDb.SaveChanges();

            // ACT
            var list = service.GetLokalen();

            // ASSERT
            Assert.IsNotNull(list);

            Assert.IsTrue(list.Find(p => p.Capaciteit == 50) != null);
            Assert.IsTrue(list.Find(p => p.Vakcode == "AAA01A") != null);
        }

        [TestMethod]
        public void DeletePlanningTest() 
        {
            // ARRANGE
            PlannerService service = new PlannerService(ctxDb);
            int cmId = AddCm();

            int scheduleId = ctxDb.Schedule.Where(s => s.Datum == new DateTime(DateTime.Now.Year, DateTime.Now.Month, (DateTime.Now.Day + 1))).FirstOrDefault().Id;
            int roomId = ctxDb.Room.Where(r => r.Gebouw == "A" & r.Verdiep == 0 & r.Nummer == "01").FirstOrDefault().Id;

            // ACT
            service.DeletePlanning(cmId);

            // ASSERT
            Assert.IsNull(ctxDb.CourseMoment.Where(cm => cm.ScheduleId == scheduleId && cm.RoomId == roomId).FirstOrDefault());
            Assert.IsTrue(!ctxDb.CourseMoment.Any());
        }

        [TestMethod]
        public void EditPlanningTest() 
        {
            // ARRANGE
            PlannerService service = new PlannerService(ctxDb);
            Course course1 = new Course { Vakcode = "BBB01B", Titel = "testvak", Studiepunten = 4 };
            ctxDb.Course.Add(course1);

            Room room1 = new Room { Gebouw = "B", Verdiep = 0, Nummer = "01", Type = "Lokaal", Capaciteit = 10 };
            ctxDb.Room.Add(room1);
            
            int cmId = AddCm();

            string userId = ctxDb.Users.Where(u => u.UserName == "r0664186").FirstOrDefault().Id;

            int newCourseId = ctxDb.Course.Where(c => c.Vakcode == "BBB01B").FirstOrDefault().Id;
            int newRoomId = ctxDb.Room.Where(r => r.Gebouw == "B" & r.Verdiep == 0 & r.Nummer == "01").FirstOrDefault().Id;
            var newUser = ctxDb.Users.Find(userId);
            DateTime newDatum = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string newDatumString = newDatum.ToString("yyyy-MM-dd");
            string newStart = "13:00";
            double newDuratie = 1.0;
            string newLessenlijst = "Editplanning test lesmoment";
            
            // ACT
            service.EditPlanning(cmId, newCourseId, newDatumString, newStart, newDuratie, newRoomId, newLessenlijst, newUser);

            // ASSERT
            var newCm = ctxDb.CourseMoment.Find(cmId);
            Assert.AreEqual(newCourseId, newCm.CourseId);
            Assert.AreEqual(newRoomId, newCm.RoomId);
            Assert.AreEqual(newUser.Id, newCm.ApplicationUserId);
            Assert.AreEqual(ctxDb.Schedule.Where(s => s.Datum == newDatum).FirstOrDefault().Id, newCm.ScheduleId);
            Assert.AreEqual(newLessenlijst, newCm.LessenLijst);
        }

        [TestMethod]
        public void GetTopicTest() 
        {
            // ARRANGE
            PlannerService service = new PlannerService(ctxDb);
            int cmId = AddCm();

            // ACT
            var planner = service.GetTopic(cmId, false, null);

            // ASSERT
            Assert.AreEqual(cmId, planner.cmId);
        }

        [TestMethod]
        public void GetCourseMomentsTest() 
        {
            // ARRANGE
            PlannerService service = new PlannerService(ctxDb);
            int cmId = AddCm();
            int vakcodeId = ctxDb.Course.Where(c => c.Vakcode == "MGP01A").FirstOrDefault().Id;
            string vakcode = ctxDb.Course.Find(vakcodeId).Vakcode;

            // ACT
            var plannerList = service.GetCourseMoments(vakcodeId);

            // ASSERT
            Assert.AreEqual(vakcode, plannerList.FirstOrDefault().Vakcode);
        }

        [TestMethod]
        public void GetStudsInCmTest() 
        {
            // ARRANGE
            PlannerService service = new PlannerService(ctxDb);

            int cmId = AddCm();

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "MGP01A").FirstOrDefault().Id;

            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000001", Email = "r0000001@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000002", Email = "r0000002@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000003", Email = "r0000003@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.SaveChanges();

            string user1Id = ctxDb.Users.Where(u => u.UserName == "r0000001").FirstOrDefault().Id;
            string user2Id = ctxDb.Users.Where(u => u.UserName == "r0000002").FirstOrDefault().Id;
            string user3Id = ctxDb.Users.Where(u => u.UserName == "r0000003").FirstOrDefault().Id;

            ctxDb.CourseUser.Add(new CourseUser { CourseId = courseId, ApplicationUserId = user1Id, GoedGekeurd = true, AfwijzingBeschr = null});
            ctxDb.CourseUser.Add(new CourseUser { CourseId = courseId, ApplicationUserId = user2Id, GoedGekeurd = true, AfwijzingBeschr = null });
            ctxDb.CourseUser.Add(new CourseUser { CourseId = courseId, ApplicationUserId = user3Id, GoedGekeurd = true, AfwijzingBeschr = null });
            ctxDb.SaveChanges();

            ctxDb.CourseMomentUsers.Add(new CourseMomentUsers { ApplicationUserId = user1Id, CoursMomentId = cmId});
            ctxDb.CourseMomentUsers.Add(new CourseMomentUsers { ApplicationUserId = user2Id, CoursMomentId = cmId });
            ctxDb.SaveChanges();

            // ACT
            var UList = service.GetStudsInCm(cmId);

            // ASSERT
            Assert.IsTrue(UList[0].IsSelected);
            Assert.AreEqual(user1Id, UList[0].userId);

            Assert.IsTrue(UList[1].IsSelected);
            Assert.AreEqual(user2Id, UList[1].userId);

            Assert.IsFalse(UList[2].IsSelected);
            Assert.AreEqual(user3Id, UList[2].userId);
        }

        [TestMethod]
        public void EditStudsInCmTest() 
        {
            // ARRANGE
            PlannerService service = new PlannerService(ctxDb);

            int cmId = AddCm();

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "MGP01A").FirstOrDefault().Id;

            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000001", Email = "r0000001@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000002", Email = "r0000002@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000003", Email = "r0000003@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.SaveChanges();

            string user1Id = ctxDb.Users.Where(u => u.UserName == "r0000001").FirstOrDefault().Id;
            string user2Id = ctxDb.Users.Where(u => u.UserName == "r0000002").FirstOrDefault().Id;
            string user3Id = ctxDb.Users.Where(u => u.UserName == "r0000003").FirstOrDefault().Id;

            ctxDb.CourseUser.Add(new CourseUser { CourseId = courseId, ApplicationUserId = user1Id, GoedGekeurd = true, AfwijzingBeschr = null });
            ctxDb.CourseUser.Add(new CourseUser { CourseId = courseId, ApplicationUserId = user2Id, GoedGekeurd = true, AfwijzingBeschr = null });
            ctxDb.CourseUser.Add(new CourseUser { CourseId = courseId, ApplicationUserId = user3Id, GoedGekeurd = true, AfwijzingBeschr = null });
            ctxDb.SaveChanges();

            ctxDb.CourseMomentUsers.Add(new CourseMomentUsers { ApplicationUserId = user1Id, CoursMomentId = cmId });
            ctxDb.CourseMomentUsers.Add(new CourseMomentUsers { ApplicationUserId = user2Id, CoursMomentId = cmId });
            ctxDb.SaveChanges();

            var UList = service.GetStudsInCm(cmId);
            UList.ForEach(u => u.IsSelected = !u.IsSelected);

            // ACT
            service.EditStudsInCm(UList, cmId);

            // ASSERT
            Assert.AreEqual(1, ctxDb.CourseMomentUsers.Count());
        }

        [TestMethod]
        public void GetCourseUsersTest() 
        {
            // ARRANGE
            PlannerService service = new PlannerService(ctxDb);

            int cmId = AddCm();

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "MGP01A").FirstOrDefault().Id;

            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000001", Email = "r0000001@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000002", Email = "r0000002@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000003", Email = "r0000003@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.SaveChanges();

            string user1Id = ctxDb.Users.Where(u => u.UserName == "r0000001").FirstOrDefault().Id;
            string user2Id = ctxDb.Users.Where(u => u.UserName == "r0000002").FirstOrDefault().Id;
            string user3Id = ctxDb.Users.Where(u => u.UserName == "r0000003").FirstOrDefault().Id;

            ctxDb.CourseUser.Add(new CourseUser { CourseId = courseId, ApplicationUserId = user1Id, GoedGekeurd = true, AfwijzingBeschr = null });
            ctxDb.CourseUser.Add(new CourseUser { CourseId = courseId, ApplicationUserId = user2Id, GoedGekeurd = true, AfwijzingBeschr = null });
            ctxDb.CourseUser.Add(new CourseUser { CourseId = courseId, ApplicationUserId = user3Id, GoedGekeurd = false, AfwijzingBeschr = null });
            ctxDb.SaveChanges();

            ctxDb.Roles.Add(new IdentityRole { Name = "Student", NormalizedName = "STUDENT"});
            ctxDb.SaveChanges();

            var roleId = ctxDb.Roles.Where(r => r.Name == "Student").FirstOrDefault().Id;

            ctxDb.UserRoles.Add(new IdentityUserRole<string> { UserId = user1Id, RoleId = roleId});
            ctxDb.UserRoles.Add(new IdentityUserRole<string> { UserId = user2Id, RoleId = roleId});
            ctxDb.UserRoles.Add(new IdentityUserRole<string> { UserId = user3Id, RoleId = roleId});
            ctxDb.SaveChanges();

            ctxDb.CourseMomentUsers.Add(new CourseMomentUsers { ApplicationUserId = user2Id, CoursMomentId = cmId});
            ctxDb.SaveChanges();

            // ACT
            var list = service.GetCourseUsers(courseId);

            // ASSERT
            Assert.AreEqual(1, list.users.Count());
            Assert.AreEqual(user2Id, list.users[0].Id);
        }

        [TestMethod]
        public void GetStudsInEachCmTest() 
        {
            // ARRANGE
            PlannerService service = new PlannerService(ctxDb);

            int cmId = AddCm();

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "MGP01A").FirstOrDefault().Id;

            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000001", Email = "r0000001@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000002", Email = "r0000002@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000003", Email = "r0000003@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.SaveChanges();

            string user1Id = ctxDb.Users.Where(u => u.UserName == "r0000001").FirstOrDefault().Id;
            string user2Id = ctxDb.Users.Where(u => u.UserName == "r0000002").FirstOrDefault().Id;
            string user3Id = ctxDb.Users.Where(u => u.UserName == "r0000003").FirstOrDefault().Id;

            ctxDb.CourseUser.Add(new CourseUser { CourseId = courseId, ApplicationUserId = user1Id, GoedGekeurd = true, AfwijzingBeschr = null });
            ctxDb.CourseUser.Add(new CourseUser { CourseId = courseId, ApplicationUserId = user2Id, GoedGekeurd = true, AfwijzingBeschr = null });
            ctxDb.CourseUser.Add(new CourseUser { CourseId = courseId, ApplicationUserId = user3Id, GoedGekeurd = false, AfwijzingBeschr = null });
            ctxDb.SaveChanges();

            // ACT
            var list = service.GetStudsInEachCm(courseId);

            // ASSERT
            Assert.IsFalse(list[0].IsSelected);
            Assert.IsFalse(list[1].IsSelected);
        }

        [TestMethod]
        public async Task AddStudsToEachCmTest() 
        {
            // ARRANGE
            PlannerService service = new PlannerService(ctxDb);

            int cmId = AddCm();

            int courseId = ctxDb.Course.Where(c => c.Vakcode == "MGP01A").FirstOrDefault().Id;

            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000001", Email = "r0000001@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000002", Email = "r0000002@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.Users.Add(new ApplicationUser { UserName = "r0000003", Email = "r0000003@hotmail.com", GeboorteDatum = new DateTime(1998, 09, 21), Naam = "Claes", VoorNaam = "Thomas", EmailConfirmed = true });
            ctxDb.SaveChanges();

            string user1Id = ctxDb.Users.Where(u => u.UserName == "r0000001").FirstOrDefault().Id;
            string user2Id = ctxDb.Users.Where(u => u.UserName == "r0000002").FirstOrDefault().Id;
            string user3Id = ctxDb.Users.Where(u => u.UserName == "r0000003").FirstOrDefault().Id;

            ctxDb.CourseUser.Add(new CourseUser { CourseId = courseId, ApplicationUserId = user1Id, GoedGekeurd = true, AfwijzingBeschr = null });
            ctxDb.CourseUser.Add(new CourseUser { CourseId = courseId, ApplicationUserId = user2Id, GoedGekeurd = true, AfwijzingBeschr = null });
            ctxDb.CourseUser.Add(new CourseUser { CourseId = courseId, ApplicationUserId = user3Id, GoedGekeurd = false, AfwijzingBeschr = null });
            ctxDb.SaveChanges();

            var list = service.GetStudsInEachCm(courseId);
            list[1].IsSelected = true;

            // ACT
            var result = await service.AddStudsToEachCm(list, courseId);

            // ASSERT
            Assert.IsTrue(ctxDb.CourseMomentUsers.Any());
            Assert.AreEqual(user2Id, ctxDb.CourseMomentUsers.FirstOrDefault().ApplicationUserId);
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

        public int AddCm() 
        {
            Course course = new Course { Vakcode = "MGP01A", Titel = "front end", Studiepunten = 6 };
            ctxDb.Course.Add(course);

            Room room = new Room { Gebouw = "A", Verdiep = 0, Nummer = "01", Type = "Lokaal", Capaciteit = 20 };
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

            CourseMoment cm = new CourseMoment { CourseId = courseId, RoomId = roomId, ScheduleId = scheduleId, ApplicationUserId = userId, LessenLijst = "Dit is een lesmoment om mee te testen" };
            ctxDb.CourseMoment.Add(cm);
            ctxDb.SaveChanges();

            return ctxDb.CourseMoment.Where(cm => cm.ScheduleId == scheduleId && cm.RoomId == roomId).FirstOrDefault().Id;
        }
    }
}
