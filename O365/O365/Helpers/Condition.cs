using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace O365.Helpers
{
    public class Condition
    {
        public static Func<IWebDriver, IWebElement> VisibleElement(By locator)
        {
            return d =>
            {
                try
                {
                    IList<IWebElement> elements = d.FindElements(locator);
                    return AnyElementDisplayed(elements) ? elements.First(el => el.Displayed) : null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            };
        }

        public static Func<IWebDriver, IWebElement> ElementToBeClickable(By locator)
        {
            return d =>
            {
                try
                {
                    var element = d.FindElement(locator);
                    return (IsElementDisplayed(element) && element.Enabled) ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return d.FindElement(locator);
                }
            };
        }

        public static Func<IWebDriver, bool> UrlChange(string oldUrl)
        {
            return d => !String.Equals(oldUrl, d.Url, StringComparison.CurrentCultureIgnoreCase);
        }


        private static bool AnyElementDisplayed(IEnumerable<IWebElement> elements)
        {
            return elements.Any(el => el.Displayed);
        }

        private static bool IsElementDisplayed(IWebElement element)
        {
            return element.Displayed;
        }

    }
}
