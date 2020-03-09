using System;
using System.Collections.Generic;
using System.Text;
using Gip.Models;
using Gip.Models.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace GipUnitTest
{
    [TestFixture]
    class CourseMomentTest
    {
        [SetUp]
        public void SetUp() 
        {
        }

        [Test]
        public void RegexVakCodeIsOke()
        {
            CourseMoment coursemoment = new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 1, 7, 7, 00, 00), "A", 0, "01", "r0749748", "aaaaa", new DateTime(1800, 1, 1, 17, 00, 00));

            Assert.True(coursemoment.Vakcode.Equals("lll22a"));
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexVakCodeIsFoutGeeftException()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll222ffua",new DateTime(2020,03,09), new DateTime(2020,1,7,1,00,00),"A",0,"01","r0749748","aaaaa",new DateTime(1800,1,1,17,00,00)));
            Assert.AreEqual("je hebt een foutief formaat van vakcode of een ongeldig character ingegeven. Gelieve een vakcode van het formaat AAA11A in te geven", ex.Message);

        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexVakCodeIsLeegGeeftException()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("", new DateTime(2020, 03, 09), new DateTime(2020, 1, 7, 1, 00, 00), "A", 0, "01", "r0749748", "aaaaa", new DateTime(1800, 1, 1, 17, 00, 00)));
            Assert.AreEqual("U heeft een lege vakcode meegegeven.", ex.Message);

        }
        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexDatumIsTeVerGeeftException()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2022, 03, 09), new DateTime(2020, 1, 7, 1, 00, 00), "A", 0, "01", "r0749748", "aaaaa", new DateTime(1800, 1, 1, 17, 00, 00)));
            Assert.AreEqual("De gekozen datum is te ver in de toekomst.", ex.Message);

        }
        [Test]
        public void RegexDatumIsOke()
        {
            CourseMoment coursemoment = new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 1, 9, 1, 00, 00), "A", 0, "01", "r0749748", "aaaaa", new DateTime(1800, 1, 1, 17, 00, 00));

            Assert.True(coursemoment.Datum.ToString("yyyy/MM/dd").Equals("2020/03/09"));
        }
        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexDatumGeslotenGeeftException()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 07), new DateTime(2020, 1, 7, 7, 00, 00), "A", 0, "01", "r0749748", "aaaaa", new DateTime(1800, 1, 1, 17, 00, 00)));
            Assert.AreEqual("De school is gesloten in het weekend.", ex.Message);

        }
        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexStartMomentGeslotenGeeftException()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 1, 7, 1, 00, 00), "A", 0, "01", "r0749748", "aaaaa", new DateTime(1800, 1, 1, 17, 00, 00)));
            Assert.AreEqual("De school is enkel open tussen 6:00 en 22:00", ex.Message);

        }
        [Test]
        public void RegexStartMomentIsOke()
        {
            CourseMoment coursemoment = new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 1, 9, 7, 00, 00), "A", 0, "01", "r0749748", "aaaaa", new DateTime(1800, 1, 1, 17, 00, 00));

            Assert.True(coursemoment.Startmoment.ToString("yyyy/MM/dd HH:mm").Equals("2020/1/9 07:00:00"));
        }
        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexGebouwIsLeegGeeftException()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 1, 7, 07, 00, 00), "", 0, "01", "r0749748", "aaaaa", new DateTime(1800, 1, 1, 17, 00, 00)));
            Assert.AreEqual("U heeft niets meegegeven als gebouwcharacter.", ex.Message);

        }
        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexGebouwBestaatNietGeeftException()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 1, 7, 07, 00, 00), "1", 0, "01", "r0749748", "aaaaa", new DateTime(1800, 1, 1, 17, 00, 00)));
            Assert.AreEqual("Dit gebouw bestaat niet of u heeft een verboden character ingegeven.", ex.Message);

        }
        [Test]
        public void RegexGebouwIsOke()
        {
            CourseMoment coursemoment = new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 1, 7, 7, 00, 00), "A", 0, "01", "r0749748", "aaaaa", new DateTime(1800, 1, 1, 17, 00, 00));

            Assert.True(coursemoment.Gebouw.Equals("A"));
        }




    }
}
