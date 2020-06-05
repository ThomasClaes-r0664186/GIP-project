using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Xunit;

namespace GipGUITest
{
    //public class UnitTest1 : IDisposable
    //{
    //    private readonly IWebDriver _driver;
    //    private Uri urlHome = new Uri("https://localhost:44318/");
    //    private Uri urlLokaal = new Uri("https://localhost:44318/lokaal");
    //    private Uri urlVak = new Uri("https://localhost:44318/vak?");
    //    private Uri urlPlanner = new Uri("https://localhost:44318/planner/add");
    //    private Uri urlPlanning = new Uri("https://localhost:44318/planner");
    //    private string testUserName = "u0000001";
    //    private string testPasswordCorrect = "Xx*123";

    //    public UnitTest1() 
    //    {
    //        _driver = new ChromeDriver();
    //    }

    //    [Fact]
    //    public void runTest() 
    //    {
    //        _driver.Navigate().GoToUrl(urlHome);
    //        Assert.Equal("Home Page - Gip", _driver.Title);
    //    }

    //    [Fact]
    //    public void lokaal_Add_Test() 
    //    {
    //        login();
    //        _driver.FindElement(By.Id("lokalen")).Click();
    //        Assert.Equal("Lokaaloverzicht - Gip", _driver.Title);
    //    }

    //    [Fact]
    //    public void vak_Add_Test()
    //    {
    //        login();
    //        _driver.FindElement(By.Id("navbarDropdown")).Click();
    //        _driver.FindElement(By.Id("vakken")).Click();
    //        Assert.Equal("Vakken - Gip", _driver.Title);
    //    }

    //    [Fact]
    //    public void planner_Add_Test()
    //    {
    //        _driver.Navigate().GoToUrl(urlPlanner);
    //    }

    //    public void login() {
    //        //navigeren naar een bepaalde url
    //        _driver.Navigate().GoToUrl(urlHome);

    //        //een element zoeken adhv de id en dan erop klikken
    //        _driver.FindElement(By.Id("login")).Click();

    //        //controleren of de pagina de juist titel heeft, dus of je op de juiste pagina zit
    //        Assert.Equal("Login - Gip", _driver.Title);

    //        //een element zoeken adhv de id en dan daar een bepaalde tekst invullen
    //        _driver.FindElement(By.Id("rnum")).SendKeys(testUserName);
    //        _driver.FindElement(By.Id("pwd")).SendKeys(testPasswordCorrect);

    //        _driver.FindElement(By.Id("loginButton")).Click();

    //        _driver.Navigate().GoToUrl(urlHome);
            
    //        Assert.Equal("Henk Verelst", _driver.FindElement(By.Id("name")).Text);
    //    }

    //    public void Dispose()
    //    {
    //        //dit gebeurd na elke test, zorgt ervoor dat het venster gesloten wordt
    //        _driver.Quit();
    //        _driver.Dispose();
    //    }
    //}
}
