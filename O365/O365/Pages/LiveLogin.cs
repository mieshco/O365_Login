using O365.Helpers;
using OpenQA.Selenium;

namespace O365.Pages
{
    public class LiveLogin
    {
        private IWebDriver _driver;
        private readonly string _password;
        public LiveLogin(IWebDriver driver, string password)
        {
            _driver = driver;
            _password = password;
        }

        private readonly By _passwordDescription = By.Id("passwordDesc");
        private readonly By _passwordInput = By.Name("passwd");
        private readonly By _signInButton = By.XPath("//input[@type='submit']");

        public LiveLogin Login()
        {
            _driver.WaitForElementToBeVisible(this._passwordDescription);
            var passwordInput = _driver.FindElement(_passwordInput);
            passwordInput.Click();
            passwordInput.SendKeys(_password);
            var signInButton = _driver.FindElement(_signInButton);
            signInButton.Click();
            return this;
        }
    }
}