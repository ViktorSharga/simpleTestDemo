using System;
using OpenQA.Selenium;

namespace SLTestDemo.Base
{

    public static class ExpectedConditions
    {

        public static Func<IWebDriver, IWebElement> ElementExists(By locator)
        {
            return driver => driver.FindElement(locator);
        }

        public static Func<IWebDriver, IWebElement> ElementToBeClickable(By locator)
        {
            return driver =>
            {
                var element = ElementIfVisible(driver.FindElement(locator));

                try
                {
                    return element != null && element.Enabled ? element : null;
                }
                catch (NotFoundException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }

        public static Func<IWebDriver, IWebElement> ElementToBeVisible(By locator)
        {
            return driver =>
            {
                try
                {
                    return ElementIfVisible(driver.FindElement(locator));
                }
                catch (NotFoundException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }

        public static Func<IWebDriver, bool> PageLoaded()
        {
            return driver =>
            {
                try
                {
                    return ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState;").ToString().Equals("complete");
                }
                catch (Exception)
                {
                    return false;
                }
            };
        }

        private static IWebElement ElementIfVisible(IWebElement element)
        {
            return element.Displayed ? element : null;
        }

    }
}
