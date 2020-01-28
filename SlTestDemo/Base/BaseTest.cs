using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace SLTestDemo.Base
{
    [TestClass]
    public class BaseTest
    {
        protected WebDriverWait Wait { get; set; }
        protected RemoteWebDriver Driver { get; set; }
        protected TestHelper Base { get; set; }

        [AssemblyInitialize]
        public static void BaseInit(TestContext testContext)
        {
        }

        [TestInitialize]
        public void Initialize()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability(ChromeOptions.Capability,
                new Dictionary<string, object>
                {
                    {
                        "args",
                        new[]
                        {
                            "--disable-infobars", "--no-first-run", "--disable-features=TranslateUI",
                            "--disable-notifications", "--no-default-browser-check", "--deny-permission-prompts"
                        }
                    }
                });
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.BrowserName, "Chrome");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.NewCommandTimeout,
                TimeSpan.FromSeconds(90).TotalSeconds);
            appiumOptions.AddAdditionalCapability("unicodeKeyboard", true);
            appiumOptions.AddAdditionalCapability("resetKeyboard", true);
            appiumOptions.AddAdditionalCapability("automationName", "UiAutomator2");
            appiumOptions.AddAdditionalCapability("name", "SL test");

            Driver = new AndroidDriver<AndroidElement>(new Uri("https://eu1.appium.testobject.com/wd/hub"),
                appiumOptions, TimeSpan.FromSeconds(90));
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
            Base = new TestHelper(Driver, Wait);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (Driver == null) return;
            Driver.Close();
            Driver.Dispose();
        }


    }
}