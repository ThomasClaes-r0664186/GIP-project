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
    public class CourseTest
    {
      

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RegexVakCodeIsOke()
        {
            Course course = new Course("lll22a", "programmeren 3", 10);

            Assert.True(course.Vakcode.Equals("lll22a"));
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexVakCodeIsFoutGeeftException()
        {
            Exception ex =  Assert.Throws<DatabaseException>(() => new Course("lll222hfz", "programmeren 3", 10));
            Assert.AreEqual("je hebt een foutief formaat van vakcode of een ongeldig character ingegeven. Gelieve een vakcode van het formaat AAA11A in te geven", ex.Message);
            
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexVakCodeIsLeegGeeftException()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new Course("", "programmeren 3", 10));
            Assert.AreEqual("U heeft een lege vakcode meegegeven.", ex.Message);

        }

        [Test]
        public void RegexTitelIsOke()
        {
            Course course = new Course("lll22a", "programmeren 3", 10);

            Assert.True(course.Titel.Equals("programmeren 3"));
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexTitelFoutGeeftException()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new Course("lll22a", "programmeren<><// ;>3", 10));
            Assert.AreEqual("U heeft een verboden character ingegeven, gelieve dit niet te doen.", ex.Message);
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexTitelIsLeegGeeftException()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new Course("lll22a", "", 10));
            Assert.AreEqual("De titel mag niet leeg zijn.", ex.Message);
        }

        [Test]
        public void RegexStudiepuntenIsOke()
        {
            Course course = new Course("lll22a", "programmeren 3", 10);

            Assert.True(course.Studiepunten.Equals(10));
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexStudiepuntenTeLaagGeeftException()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new Course("lll22a", "programmeren 3", -10));
            Assert.AreEqual("Het aantal studiepunten mag niet negatief zijn.", ex.Message);
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexStudiepuntenTeHoogGeeftException()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new Course("lll22a", "programmeren 3", 100));
            Assert.AreEqual("Het aantal studiepunten mag niet hoger zijn dan 60.", ex.Message);
        }

       
        
    }   



}
