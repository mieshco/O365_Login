using System;
using FluentAssertions;
using NUnit.Framework;
using O365.Helpers;
using O365.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;

namespace O365
{
    [TestFixture]
    public class Login
    {
        private IWebDriver _driver;

        [SetUp]
        public void SetupTest()
        {
            var options = new InternetExplorerOptions
            {
                EnsureCleanSession = true,
                UsePerProcessProxy = true,
                EnablePersistentHover = true,
                UnexpectedAlertBehavior = InternetExplorerUnexpectedAlertBehavior.Ignore
            };
            _driver = new InternetExplorerDriver(options);
        }
        
        [Test, TestCaseSource(nameof(LoginCombinations))]
        public void GetLoginCookie(string target, string user, string pwd, string outputPath)
        {
            var gate1 = new O365Login(_driver, user);
            var gate2 = new LiveLogin(_driver, pwd);

            _driver.Navigate().GoToUrl(target);
            gate1.Login();
            gate2.Login();
            _driver.WaitForElementToBeVisible(By.LinkText("Documents"));
            _driver.Title.Should().Contain("Documents");
            var listItem = _driver.FindElement(By.XPath("//a[contains(@href,'D64MB')]"));
            listItem.Text.Should().Contain("D64MB");

            var cookie1 = Browser.ReadCookie(_driver,"FedAuth");
            var cookie2 = Browser.ReadCookie(_driver, "rtFa");
            System.IO.File.WriteAllText(outputPath, $"{cookie1};{cookie2}");
        }

        private static readonly object[] LoginCombinations =
        {
            new object[] {
                TestContext.Parameters["target"],
                TestContext.Parameters["user"],
                TestContext.Parameters["pwd"],
                @TestContext.Parameters["outputPath"]
            }
        };
        [TearDown]
        public void TeardownTest()
        {
            try
            {
                _driver.Quit();
            }
            catch (Exception)
            {
                Browser.KillBrowserDriver("IEDriverServer");
            }
            
        }
    }
}
