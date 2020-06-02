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
        public void RegexCourseMomentIsOke()
        {
            //CourseMoment coursemoment = new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 01, 07, 15, 00, 00), "A", 0, "1", "r0749748", "aaaaa", new DateTime(2020, 01, 07, 17, 00, 00));

            //Assert.True(coursemoment.Vakcode.Equals("lll22a"));
            //Assert.True(coursemoment.Datum.ToString("yyyy/MM/dd").Equals("2020/03/09"));
            //Assert.True(coursemoment.Startmoment.ToString("yyyy/MM/dd HH:mm").Equals("2020/01/07 15:00"));
            //Assert.True(coursemoment.Gebouw.Equals("A"));
            //Assert.True(coursemoment.Verdiep.ToString().Equals("0"));
            //Assert.True(coursemoment.Nummer.Equals("1"));
            //Assert.True(coursemoment.Userid.Equals("r0749748"));
            //Assert.True(coursemoment.LessenLijst.Equals("aaaaa"));
            //Assert.True(coursemoment.Eindmoment.ToString("yyyy/MM/dd HH:mm").Equals("2020/01/07 17:00"));
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexVakCodeIsLeegGeeftException()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("", new DateTime(2020, 03, 09), new DateTime(2020, 01, 07, 15, 00, 00), "A", 0, "1", "r0749748", "aaaaa", new DateTime(2020, 01, 07, 17, 00, 00)));
            //Assert.AreEqual("U heeft een lege vakcode meegegeven.", ex.Message);

        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexVakCodeIsFoutGeeftException()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll222ffua", new DateTime(2020, 03, 09), new DateTime(2020, 01, 07, 15, 00, 00), "A", 0, "1", "r0749748", "aaaaa", new DateTime(2020, 01, 07, 17, 00, 00)));
            //Assert.AreEqual("je hebt een foutief formaat van vakcode of een ongeldig character ingegeven. Gelieve een vakcode van het formaat AAA11A in te geven", ex.Message);

        }


        /* [Test]
         [ExpectedException(typeof(DatabaseException))]
         public void RegexDatumGeslotenGeeftException()
         {
             Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 08), new DateTime(2020, 01, 07, 15, 00, 00), "A", 0, "1", "r0749748", "aaaaa", new DateTime(2020, 01, 07, 17, 00, 00)));
             Assert.AreEqual("De school is gesloten in het weekend.", ex.Message);

         }*/

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexDatumIsTeverGeeftException()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2022, 03, 09), new DateTime(2020, 01, 07, 15, 00, 00), "A", 0, "1", "r0749748", "aaaaa", new DateTime(2020, 01, 07, 17, 00, 00)));
            //Assert.AreEqual("De gekozen datum is te ver in de toekomst.", ex.Message);

        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexStartMomentGeslotenGeeftException()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 01, 07, 1, 00, 00), "A", 0, "1", "r0749748", "aaaaa", new DateTime(2020, 01, 07, 17, 00, 00)));
            //Assert.AreEqual("De school is enkel open tussen 6:00 en 22:00", ex.Message);

        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexGebouwIsLeegGeeftException()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 01, 07, 15, 00, 00), "", 0, "1", "r0749748", "aaaaa", new DateTime(2020, 01, 07, 17, 00, 00)));
            //Assert.AreEqual("U heeft niets meegegeven als gebouwcharacter.", ex.Message);

        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexGebouwBestaatNietGeeftException()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 01, 07, 15, 00, 00), "2", 0, "1", "r0749748", "aaaaa", new DateTime(2020, 01, 07, 17, 00, 00)));
            //Assert.AreEqual("Dit gebouw bestaat niet of u heeft een verboden character ingegeven.", ex.Message);

        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexVerdiepNietNegatiefGeeftException()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 01, 07, 15, 00, 00), "A", 10, "1", "r0749748", "aaaaa", new DateTime(2020, 01, 07, 17, 00, 00)));
            //Assert.AreEqual("Het verdiep mag niet negatief zijn noch boven 9.", ex.Message);

        }

        /* [Test]
         [ExpectedException(typeof(DatabaseException))]              kan niet getest worden
         public void RegexVerdiepFoutKarakterGeeftException()
         {
             Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 01, 07, 15, 00, 00), "A", 0, "1", "r0749748", "aaaaa", new DateTime(2020, 01, 07, 17, 00, 00)));
             Assert.AreEqual("U heeft een verboden character ingegeven, gelieve dit niet te doen.", ex.Message);

         }*/

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexNummerNietTeLangGeeftException()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 01, 07, 15, 00, 00), "A", 0, "11111", "r0749748", "aaaaa", new DateTime(2020, 01, 07, 17, 00, 00)));
            //Assert.AreEqual("Het nummer mag niet langer zijn dan 3 characters of u heeft een leeg nummer meegegeven.", ex.Message);

        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexNummerBestaatNietGeeftException()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 01, 07, 15, 00, 00), "A", 0, "ù", "r0749748", "aaaaa", new DateTime(2020, 01, 07, 17, 00, 00)));
            //Assert.AreEqual("U heeft een verboden character ingegeven, gelieve dit niet te doen.", ex.Message);

        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexUserIdIsLeegGeeftException()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 01, 07, 15, 00, 00), "A", 0, "1", "", "aaaaa", new DateTime(2020, 01, 07, 17, 00, 00)));
            //Assert.AreEqual("De userid mag niet leeg zijn.", ex.Message);

        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexUserIdIsFoutGeeftException()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 01, 07, 15, 00, 00), "A", 0, "1", "x0749748", "aaaaa", new DateTime(2020, 01, 07, 17, 00, 00)));
            //Assert.AreEqual("U heeft een verboden character ingegeven, gelieve dit niet te doen.", ex.Message);

        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexLessenLijstIsLeegGeeftException()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 01, 07, 15, 00, 00), "A", 0, "1", "r0749748", "", new DateTime(2020, 01, 07, 17, 00, 00)));
            //Assert.AreEqual("De lessenlijst mag niet leeg zijn.", ex.Message);

        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexLessenLijstIsFoutGeeftException()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new CourseMoment("lll22a", new DateTime(2020, 03, 09), new DateTime(2020, 01, 07, 15, 00, 00), "A", 0, "1", "r0749748", "ù", new DateTime(2020, 01, 07, 17, 00, 00)));
            //Assert.AreEqual("De lessenlijst bevat een verboden character, gelieve dit niet te doen.", ex.Message);

        }




    }
}
