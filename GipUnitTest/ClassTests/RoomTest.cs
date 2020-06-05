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
    class RoomTest
    {
        [Test]
        public void RoomOK()
        {
            Room room = new Room("Wifi/projectorsetup", "A", 5, "10k", "Computerlokaal", 30);
            Assert.True(room.Middelen.Equals("Wifi/projectorsetup"));
            Assert.True(room.Gebouw.Equals("A"));
            Assert.True(room.Verdiep == 5 );
            Assert.True(room.Nummer.Equals("10k"));
            Assert.True(room.Type.Equals("Computerlokaal"));
            Assert.True(room.Capaciteit ==30);

        }

        //[Test]
        //[ExpectedException(typeof(DatabaseException))]
        //public void TeVeelMiddelen()
        //{
        //    Exception ex = Assert.Throws<DatabaseException>(() => new Room("Wifi/projectorsetup/scherm/", "A", 5, "10k", "Computerlokaal", 30));
        //    Assert.AreEqual("Het aantal middelen voor het lokaal is te hoog!" + Environment.NewLine + "Probeer opnieuw!", ex.Message);
        //}


        //[Test]
        //[ExpectedException(typeof(DatabaseException))]
        //public void FoutMiddel()
        //{
        //    Exception ex = Assert.Throws<DatabaseException>(() => new Room("Wifi/projectorsetup/papier", "A", 5, "10k", "Computerlokaal", 30));
        //    Assert.AreEqual("De middelen die u heeft aangeduid zijn niet beschikbaar" + Environment.NewLine + "Probeer opnieuw!", ex.Message);
        //}

        [Test]
        public void GeenMiddelen()
        {
            Room room = new Room("", "A", 5, "10k", "Computerlokaal", 30);
            Assert.True(room.Middelen.Equals("Geen middelen"));
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void GeenGebouwMeegegeven()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new Room("Wifi/projectorsetup", "", 5, "10k", "Computerlokaal", 30));
            Assert.AreEqual("U heeft niets meegegeven als gebouwcharacter.", ex.Message);
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void TeVeelCharactersGebouw()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new Room("Wifi/projectorsetup", "AAA", 5, "10k", "Computerlokaal", 30));
            Assert.AreEqual("U heeft meer als 1 character meegegeven voor gebouw, gelieve u te beperken to 1 letter.", ex.Message);
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void RegexFouteInputGebouw()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new Room("Wifi/projectorsetup", "1", 5, "10k", "Computerlokaal", 30));
            Assert.AreEqual("Dit gebouw bestaat niet of u heeft een verboden character ingegeven.", ex.Message);
        }


        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void VerdiepTeHoog()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new Room("Wifi/projectorsetup", "a", 10, "10k", "Computerlokaal", 30));
            Assert.AreEqual("Het verdiep mag niet negatief zijn noch boven 9.", ex.Message);
        }
        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void VerdiepTeLaag()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new Room("Wifi/projectorsetup", "a", -1, "10k", "Computerlokaal", 30));
            Assert.AreEqual("Het verdiep mag niet negatief zijn noch boven 9.", ex.Message);
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void NummerLeeg()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new Room("Wifi/projectorsetup", "a", 1, "", "Computerlokaal", 30));
            Assert.AreEqual("Het nummer mag niet langer zijn dan 3 characters of u heeft een leeg nummer meegegeven.", ex.Message);
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void NummerTeHoog()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new Room("Wifi/projectorsetup", "a", 1, "100k", "Computerlokaal", 30));
            Assert.AreEqual("Het nummer mag niet langer zijn dan 3 characters of u heeft een leeg nummer meegegeven.", ex.Message);
        }


        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void NummerFoutieveCharacters()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new Room("Wifi/projectorsetup", "a", 1, "1<k", "Computerlokaal", 30));
            Assert.AreEqual("U heeft een verboden character ingegeven, gelieve dit niet te doen.", ex.Message);
        }

        //[Test]
        //[ExpectedException(typeof(DatabaseException))]
        //public void TypeFout()
        //{
        //    Exception ex = Assert.Throws<DatabaseException>(() => new Room("Wifi/projectorsetup", "a", 1, "10k", "niks", 30));
        //    Assert.AreEqual("Het type lokaal bestaat niet!" + Environment.NewLine + "Probeer opnieuw", ex.Message);
        //}

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void CapaciteitTeHoog()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new Room("Wifi/projectorsetup", "a", 1, "10k", "Computerlokaal", 3000));
            Assert.AreEqual("De capaciteit mag niet hoger zijn dan 400.", ex.Message);
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void CapaciteitTeLaag()
        {
            Exception ex = Assert.Throws<DatabaseException>(() => new Room("Wifi/projectorsetup", "a", 1, "10k", "Computerlokaal", -1));
            Assert.AreEqual("De capaciteit mag niet negatief zijn.", ex.Message);
        }
    }
}
