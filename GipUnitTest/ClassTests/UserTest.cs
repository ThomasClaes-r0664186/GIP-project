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
    class UserTest
    {
      

        [Test]
        public void UserOK() // kijkt setters van: naam, email, en userID na.
        {
            //User user = new User("Haesevoets", "jaimie", "jaimiehaesevoets@hotmail.com", "r0800449");
            //Assert.True(user.Naam.Equals("jaimie"));
            //Assert.True(user.Mail.Equals("jaimiehaesevoets@hotmail.com"));
            //Assert.True(user.Userid.Equals("r0800449"));
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void UserNaamFout()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new User("Haesevoets", "jaimie123", "jaimiehaesevoets@hotmail.com", "r0800449"));
            //Assert.AreEqual("U heeft verboden characters ingegeven voor de naam, gelieve dit niet te doen.", ex.Message);
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void UserNaamLeeg()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new User("Haesevoets", "", "jaimiehaesevoets@hotmail.com", "r0800449"));
            //Assert.AreEqual("The chosen name is empty!" + Environment.NewLine + "Please try again.", ex.Message);
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void UserMailLeeg()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new User("Haesevoets", "jaimie", "", "r0800449"));
            //Assert.AreEqual("Het email-adres is leeg.", ex.Message);
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void UserMailFout()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new User("Haesevoets", "jaimie", "jaimiehae<>;sevoets@hotmail.com", "r0800449"));
            //Assert.AreEqual("Het email-adres heeft een verkeerd formaat of een verkeerd character. Gelieve een deftig email-adres in te geven.", ex.Message);
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void UserIdFout()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new User("Haesevoets", "jaimie", "jaimiehaesevoets@hotmail.com", "r080011449"));
            //Assert.AreEqual("Deze user id heeft verbode characters, of is te lang! Probeer opnieuw.", ex.Message);
        }

        [Test]
        [ExpectedException(typeof(DatabaseException))]
        public void UserIdLeeg()
        {
            //Exception ex = Assert.Throws<DatabaseException>(() => new User("Haesevoets", "jaimie", "jaimiehaesevoets@hotmail.com", ""));
            //Assert.AreEqual("Deze user id is leeg.", ex.Message);
        }
    }
}
