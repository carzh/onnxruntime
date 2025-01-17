﻿using Newtonsoft.Json;
using NUnit.Framework.Interfaces;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ML.OnnxRuntime.Tests.BrowserStack.Android
{
    public class BrowserStackTest
    {
        public AndroidDriver driver;
        public BrowserStackTest()
        {}

        [SetUp]
        public void Init()
        {
            var androidOptions = new AppiumOptions {
                AutomationName = "UIAutomator2",
                PlatformName = "Android",
            };

            driver = new AndroidDriver(new Uri("http://127.0.0.1:4723/wd/hub"), androidOptions);
        }

        /// <summary>
        /// Sends a log to BrowserStack that is visible in the text logs and labeled as ANNOTATION.
        /// </summary>
        /// <param name="text">Log text to send.</param>
        /// <param name="logLevel">Log level -- choose between info, debug, warning, and error</param>
        public void browserStackLog(String text, String logLevel = "info")
        {
            String jsonToSend = String.Format(
                "browserstack_executor: {\"action\": \"annotate\", \"arguments\": {\"data\": {0}, \"level\": {1}}}",
                JsonConvert.ToString(text), JsonConvert.ToString(logLevel));

            ((IJavaScriptExecutor)driver).ExecuteScript(jsonToSend);
        }

        /// <summary>
        /// Passes the correct test status to BrowserStack and ensures the driver quits.
        /// </summary>
        [TearDown]
        public void Dispose()
        {
            try
            {
                // According to
                // https://www.browserstack.com/docs/app-automate/appium/set-up-tests/mark-tests-as-pass-fail
                // BrowserStack doesn't know whether test assertions have passed or failed. Below handles
                // passing the test status to BrowserStack along with any relevant information.
                if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                {
                    String failureMessage = TestContext.CurrentContext.Result.Message;
                    String jsonToSendFailure =
                        String.Format("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": " +
                                      "{\"status\":\"failed\", \"reason\": {0}}}",
                                      JsonConvert.ToString(failureMessage));

                    ((IJavaScriptExecutor)driver).ExecuteScript(jsonToSendFailure);
                }
                else
                {
                    ((IJavaScriptExecutor)driver)
                        .ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": " +
                                       "{\"status\":\"passed\", \"reason\": \"\"}}");
                }
            }
            finally
            {
                // will run even if exception is thrown by previous block
                ((AndroidDriver)driver).Quit();
            }
        }
    }
}