using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace SLTestDemo.Base
{
    public class TestHelper
    {
        protected RemoteWebDriver Driver { get; set; }
        protected WebDriverWait Wait { get; set; }

        public TestHelper(RemoteWebDriver driver, WebDriverWait wait)
        {
            Driver = driver;
            Wait = wait;
        }

        private void ExecuteJavaScript(string script, params object[] args)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript(script, args);
        }

        private IWebElement FindClickableElement(By locator)
        {
            return Wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        private IWebElement FindVisibleElement(By locator)
        {
            return Wait.Until(ExpectedConditions.ElementToBeVisible(locator));
        }
        private IWebElement FindExistingElement(By locator)
        {
            try
            {
                return Wait.Until(ExpectedConditions.ElementExists(locator));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void TryScrollElementIntoView(IWebElement element)
        {
            try
            {
                var scrollTo = @"arguments[0].scrollIntoView({
                                    behavior: 'auto',
                                    block: 'center',
                                    inline: 'nearest'
                                });";

                ExecuteJavaScript(scrollTo, element);
            }
            catch (Exception)
            {
                //do nothing
            }

        }


        private void WaitForPageLoaded()
        {
            Wait.Until(ExpectedConditions.PageLoaded());
        }

        private void RecoverableClick(By locator, IWebElement element)
        {
            try
            {
                element.Click();
            }
            catch (StaleElementReferenceException)
            {
                try
                {
                    element = FindClickableElement(locator);
                    element.Click();
                }
                catch (Exception ex)
                {
                    var errorMessage = $"Failed to recover. {ex.Message}";
                    throw new Exception(errorMessage);
                }
            }

        }
        public void CLickOnElement(By locator)
        {
            WaitForPageLoaded();
            //Check if element exists in DOM so we can scroll to it
            var element = FindExistingElement(locator);
            TryScrollElementIntoView(element);
            //After scroll is performed check element to be clickable
            element = FindClickableElement(locator);
            try
            {
                RecoverableClick(locator, element);
            }
            catch (AggregateException aex)
            {
                throw aex;
            }
        }

        public void FillTextInToElement(By locator, string text)
        {
            WaitForPageLoaded();
            var element = FindClickableElement(locator);
            TryScrollElementIntoView(element);
            element.SendKeys(text);
        }

        public void ClearTextInElement(By locator)
        {
            WaitForPageLoaded();
            var element = FindClickableElement(locator);
            TryScrollElementIntoView(element);
            element.Clear();
        }

        public bool IsVisible(By locator)
        {
            try
            {
                return FindVisibleElement(locator).Displayed;

            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public void OpenUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("Parameter URL cannot be empty");
            }

            Driver.Navigate().GoToUrl(url);
        } 
    }

}
