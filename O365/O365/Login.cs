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
        private string BaseUrl { get; set; }
        private string LoginUser { get; set; }
        private string LoginPass { get; set; }

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
        public void GetLoginCookie(string target, string username, string password, string filePath)
        {
            BaseUrl = target;
            LoginUser = username;
            LoginPass = password;
            var gate1 = new O365Login(_driver,LoginUser);
            var gate2 = new LiveLogin(_driver, LoginPass);

            _driver.Navigate().GoToUrl(BaseUrl);
            gate1.Login();
            gate2.Login();
            _driver.WaitForElementToBeVisible(By.LinkText("Documents"));
            _driver.Title.Should().Contain("Documents");
            var listItem = _driver.FindElement(By.XPath("//a[contains(@href,'dummy64')]"));
            listItem.Text.Should().Contain("dummy64");

            var cookie1 = Browser.ReadCookie(_driver,"FedAuth");
            var cookie2 = Browser.ReadCookie(_driver, "rtFa");
            System.IO.File.WriteAllText(filePath, $"{cookie1};{cookie2}");
        }

        private static readonly object[] LoginCombinations =
        {
            new object[] {"https://x.sharepoint.com/AllItems.aspx",
                "username@email.com",
                "password",
                @"D:\jmeter\cookie.csv"}
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
