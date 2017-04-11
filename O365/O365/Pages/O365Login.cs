using O365.Helpers;
using OpenQA.Selenium;

namespace O365.Pages
{ 
    public class O365Login
    {
        private IWebDriver _driver;
        private readonly string _username;

        public O365Login(IWebDriver driver, string username)
        {
            _driver = driver;
            _username = username;
        }

        private readonly By _inputEmail = By.Id("cred_userid_inputtext");
        private By _inputPassword = By.Id("cred_password_inputtext");
        private readonly By _rememberCheckbox = By.Id("cred_keep_me_signed_in_checkbox");
        private By _signInButton = By.Id("cred_sign_in_button");

        public O365Login Login()
        {
            var email = _driver.WaitForElementToBeClickable(_inputEmail);
            var checkbox = _driver.FindElement(_rememberCheckbox);

            checkbox.Click();
            email.Click();
            email.SendKeys(_username);
            email.SendKeys(Keys.Tab);
            return this;
        }
            
    }
}