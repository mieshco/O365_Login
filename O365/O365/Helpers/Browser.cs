using System.Diagnostics;
using OpenQA.Selenium;

namespace O365.Helpers
{
    public class Browser
    {
        public static void KillBrowserDriver(string driverProcessName)
        {
            var targetProcessName = driverProcessName;
            var processes = Process.GetProcessesByName(targetProcessName);
                foreach (var target in processes)
                {
                    target.Kill();
                }
        }
        
        public static string ReadCookie(IWebDriver driver, string cookieName)
        {
            var cookieValue= driver.Manage().Cookies.GetCookieNamed(cookieName).Value;
            return $"{cookieName}={cookieValue}";
        }

    }
}
