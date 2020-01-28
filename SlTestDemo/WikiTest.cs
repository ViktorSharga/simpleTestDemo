using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SLTestDemo.Base;

namespace SLTestDemo
{
    [TestClass]
    public class WikiTest : BaseTest
    {
        private static readonly By SearchField = By.Id("searchInput");
        private static readonly By SearchButton = By.CssSelector("[class*='button'][type*='submit']");
        private static readonly By HeaderLogo = By.CssSelector("[class*='brand'][class*='box']");
        private static readonly By SearchResult = By.CssSelector("[class*='parser'][class*='output']1");

        [TestMethod]
        public void SearchForText()
        {
            Base.OpenUrl("https://www.wikipedia.org/");

            Base.CLickOnElement(SearchField);
            Base.ClearTextInElement(SearchField);
            Base.FillTextInToElement(SearchField, "google");
            Base.CLickOnElement(SearchButton);
            Assert.IsTrue(Base.IsVisible(HeaderLogo), nameof(HeaderLogo) + " element is not visible");
            Assert.IsTrue(Base.IsVisible(SearchResult),
                nameof(HeaderLogo) + " element is not visible");
        }
    }
}
