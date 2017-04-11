using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;

namespace O365.Helpers
{
    public static class WebDriverWaits
    {
        private const int DefaultExplicitTimeout = 45;
        private const int TimeInterval = 250;
        
        public static void DeactivateDefaultImplicitTimeout(this IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }

        public static IWebElement WaitForElementToBeVisible(this IWebDriver driver, By locator, int timeout = DefaultExplicitTimeout, int interval = TimeInterval)
        {
            return driver.WaitFor(Condition.VisibleElement(locator), timeout, interval);
        }

        public static IWebElement WaitForElementToBeClickable(this IWebDriver driver, By locator, int timeout = DefaultExplicitTimeout)
        {
            return driver.WaitFor(Condition.ElementToBeClickable(locator), timeout);
        }

        public static T WaitFor<T>(this ISearchContext context, Func<IWebDriver, T> condition, int timeout, int timeInterval = TimeInterval)
        {
            var driver = context.GetDriver();
            try
            {
                driver.DeactivateDefaultImplicitTimeout();
            }
            catch (UnhandledAlertException)
            {
            }
            var wait = new WebDriverWait(new SystemClock(), driver, TimeSpan.FromSeconds(timeout), TimeSpan.FromMilliseconds(timeInterval));
            return wait.Until(condition);
        }

        public static IWebDriver GetDriver(this ISearchContext context)
        {
            if (context == null) throw new ArgumentException("Context can't be null");

            var webDriver = context as IWebDriver;
            if (webDriver != null)
            {
                return webDriver;
            }

            var webElement = context as IWebElement;
            if (webElement != null)
            {
                return ((IWrapsDriver)webElement).WrappedDriver;
            }

            throw new ArgumentException("Context isn't IWebDriver nor IWebElement");
        }
    }
}