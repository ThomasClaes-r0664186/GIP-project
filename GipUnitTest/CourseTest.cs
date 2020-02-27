using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Gip.Models;
namespace GipUnitTest
{
    public class CourseTest
    {
        public CourseTest()
        {
            TestCourseRegex();
        }
        [Fact]
        public void TestCourseRegex()
        {
            
            Course course = new Course("mgp09b", "jaime", 10);
            Assert.Equal("mgp09b", course.Vakcode);
        }
        
    }
}
