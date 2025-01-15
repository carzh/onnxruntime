using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using Newtonsoft.Json;

namespace TestProject1;
public class AppiumSetup
{
    //private static AppiumDriver? driver;

    //public static AppiumDriver App => driver ?? throw new NullReferenceException("AppiumDriver is null");

    public AndroidDriver driver;

    public AppiumSetup() { }


    [SetUp]
    public void Init()
    {
        // If you started an Appium server manually, make sure to comment out the next line
        // This line starts a local Appium server for you as part of the test run
        //AppiumServerHelper.StartAppiumLocalServer();

        var androidOptions = new AppiumOptions
        {
            // If running on local, uncomment the below line to use with the specified emulator
            //DeviceName = "emulator-5554",
            // Specify windows as the driver, typically don't need to change this
            AutomationName = "UIAutomator2",
            // Always Windows for Windows
            PlatformName = "Android",
            //PlatformVersion = "17.0",
            //// The identifier of the deployed application to test
            //App = "C:\\Users\\carolinezhu\\Documents\\cowboy-coding\\onnxruntime\\csharp\\test\\Microsoft.ML.OnnxRuntime.Tests.MAUI\\bin\\Debug\\net8.0-android\\ORT.CSharp.Tests.MAUI-Signed.apk",
        };

        // Note there are many more options that you can use to influence the app under test according to your needs

        driver = new AndroidDriver(new Uri("http://127.0.0.1:4723/wd/hub"), androidOptions);
    }

    public void browserStackLog(String text)
    {
        ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"annotate\", \"arguments\": {\"data\":\"" + text + "\", \"level\": \"info\"}}");
    }

    [TearDown]
    public void Dispose()
    {
        browserStackLog("Inside the Dispose function");
        // According to https://www.browserstack.com/docs/app-automate/appium/set-up-tests/mark-tests-as-pass-fail
        // BrowserStack doesn't know whether test assertions have passed or failed. Below handles
        // passing the test status to BrowserStack along with any relevant information.
        if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
        {
            // Get information about the failed test
            string testName = TestContext.CurrentContext.Test.Name;
            string failureMessage = TestContext.CurrentContext.Result.Message;

            ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": " + JsonConvert.ToString(failureMessage) + "}}");
        } else
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"\"}}");
        }

        driver?.Quit();
    }
}
