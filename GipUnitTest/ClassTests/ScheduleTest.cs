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
    class ScheduleTest
    {
        [SetUp]
        public void Setup()
        {
          
        }
        //[Test]
        //public void ScheduleOK()
        //{
        //    DateTime datum = new DateTime(2020, 03, 09);
        //    DateTime start = new DateTime(2020, 03, 09, 08, 00, 00);
        //    DateTime eind = new DateTime(2020, 03, 09, 16, 00, 00);
        //    Schedule schedule = new Schedule(datum, start, eind);
        //    Assert.True(schedule.Startmoment.Equals(start));
        //    Assert.True(schedule.Datum.Equals(datum));
        //    Assert.True(schedule.Eindmoment.Equals(eind));
        //}

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void DatumTeVerInToekomst()
        {
            DateTime datum = new DateTime(2022, 03, 09); //deze datum is te ver in de toekomst
            DateTime start = new DateTime(2020, 03, 09, 08, 00, 00);
            DateTime eind = new DateTime(2020, 03, 09, 16, 00, 00);
            Exception ex = Assert.Throws<DatabaseException>(() => new Schedule(datum, start, eind));
            Assert.AreEqual("De gekozen datum is te ver in de toekomst.", ex.Message);
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void DatumInWeekend()
        {
            DateTime datum = new DateTime(2020, 03, 14); 
            DateTime start = new DateTime(2020, 03, 09, 08, 00, 00);
            DateTime eind = new DateTime(2020, 03, 09, 16, 00, 00);
            Exception ex = Assert.Throws<DatabaseException>(() => new Schedule(datum, start, eind));
            Assert.AreEqual("De school is gesloten in het weekend.", ex.Message);
        }
        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void DatumAlGeweest()
        {
            DateTime datum = new DateTime(2020, 03, 05); // als je de datum van vorige week pakt, maar in een weekend
            // geeft hij eerst weekend exception, en niet dat datum al voorbij is
            DateTime start = new DateTime(2020, 03, 09, 08, 00, 00);
            DateTime eind = new DateTime(2020, 03, 09, 16, 00, 00);
            Exception ex = Assert.Throws<DatabaseException>(() => new Schedule(datum, start, eind));
            Assert.AreEqual("Je kan het moment niet vroeger dan vandaag plannen.", ex.Message);
        }
        
        //[Test]
        //[ExpectedException(typeof(DatabaseException))]
        //public void StartMomentTeVroeg()
        //{
        //    DateTime datum = new DateTime(2020, 03, 10); 
        //    DateTime start = new DateTime(2020, 03, 10, 05, 00, 00);
        //    DateTime eind = new DateTime(2020, 03, 10, 15, 00, 00);
        //    Exception ex = Assert.Throws<DatabaseException>(() => new Schedule(datum, start, eind));
        //    Assert.AreEqual("Uw eindmoment is te laat, de school is dan reeds gesloten.", ex.Message);
            
        //}
        
        //[Test]
        //[ExpectedException(typeof(DatabaseException))]
        //public void StartMomentTeLaat()
        //{
        //    DateTime datum = new DateTime(2020, 03, 10);
        //    DateTime start = new DateTime(2020, 03, 10, 23, 00, 00);
        //    DateTime eind = new DateTime(2020, 03, 09, 22, 00, 00);
        //    Exception ex = Assert.Throws<DatabaseException>(() => new Schedule(datum, start, eind));
        //    Assert.AreEqual("Uw eindmoment is te laat, de school is dan reeds gesloten.", ex.Message);
        //}
        /*
        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void StartMomenInWeekend()
        {
            DateTime datum = new DateTime(2020, 03, 10);
            DateTime start = new DateTime(2020, 03, 14, 07, 00, 00);
            DateTime eind = new DateTime(2020, 03, 14, 22, 00, 00);
            Exception ex = Assert.Throws<DatabaseException>(() => new Schedule(datum, start, eind));
            Assert.AreEqual("De school is enkel open tussen 6:00 en 22:00", ex.Message);
            // TEST WERKT NIET! setter aanpassen in schedule
        }
        */
        /*
        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void EindMomentTeVroeg()
        {
            DateTime datum = new DateTime(2020, 03, 10);
            DateTime start = new DateTime(2020, 03, 10, 07, 00, 00);
            DateTime eind = new DateTime(2020, 03, 09, 05, 00, 00); 
            Exception ex = Assert.Throws<DatabaseException>(() => new Schedule(datum, start, eind));
            Assert.AreEqual("Uw eindmoment is te laat, de school is dan reeds gesloten.", ex.Message);
        }
        */
        //[Test]
        //[ExpectedException(typeof(DatabaseException))]
        //public void EindMomentTeLaat()
        //{
        //    DateTime datum = new DateTime(2020, 03, 10);
        //    DateTime start = new DateTime(2020, 03, 10, 07, 00, 00);
        //    DateTime eind = new DateTime(2020, 03, 09, 23, 00, 00);
        //    Exception ex = Assert.Throws<DatabaseException>(() => new Schedule(datum, start, eind));
        //    Assert.AreEqual("Uw eindmoment is te laat, de school is dan reeds gesloten.", ex.Message);
        //    // veranderen naar "Uw eindmoment is te laat, de school is dan reeds gesloten." als setter is aangepast
            
        //}
        /*
        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void EindMomentInWeekend()
        {
            DateTime datum = new DateTime(2020, 03, 10);
            DateTime start = new DateTime(2020, 03, 13, 07, 00, 00);
            DateTime eind = new DateTime(2020, 03, 14, 10, 00, 00); 
            Exception ex = Assert.Throws<DatabaseException>(() => new Schedule(datum, start, eind));
            Assert.AreEqual("Uw eindmoment is te laat, de school is dan reeds gesloten.", ex.Message);
        }
        */

    }
}
