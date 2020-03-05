using System;
using System.Collections.Generic;
using System.Text;
using Gip.Models;
using NUnit.Framework;
namespace GipUnitTest
{
    [TestFixture]
    public class CourseTest
    {
      

        [SetUp]
        public void TestRegexVakCode()
        {
            Course course = new Course("lll22z", "programmeren 3", 10);
        }

    }
}
