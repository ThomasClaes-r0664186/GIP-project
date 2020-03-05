using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Gip;
namespace GipUnitTest
{
    [TestFixture]
    public class SeleniumTest : IDisposable
    {
        public IWebDriver driver;
        string poortNummer = "5001";
    
        [SetUp]
        public void SetUp()
        {
            Gip.Program.Main(null);
            try
            {
                driver = new ChromeDriver();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }
        [TearDown]
        public void Dispose()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    
        [Test]
        public void TitleOfPageShouldBeCorrect()
        {
            driver.Navigate().GoToUrl("localhost:"+poortNummer+"/");
            Assert.Equals("Home Page - Gip", driver.Title);
        }
    }
}