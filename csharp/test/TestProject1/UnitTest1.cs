using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.DevTools.V119.DOMStorage;
using TestProject1;
using NUnit.Framework;

namespace TestProject1;
[TestFixture]
public class MainPageTests : AppiumSetup
{
    public MainPageTests() : base() { }

    /// <summary>
    /// According to https://www.browserstack.com/docs/app-automate/appium/set-up-tests/mark-tests-as-pass-fail
    /// BrowserStack does not know whether your test's assertions have passed or failed. This only seems to apply
    /// to functions with multiple assertions. 
    /// 
    /// This function handles communicating a pass / fail status to BrowserStack.
    /// </summary>
    /// <param name="condition"></param>
    //public void browserStackTest(bool condition, )
    //{

    //}

    [Test]
    public void FailingOutputTest()
    {
        browserStackLog("TESTING");
        //throw new Exception("This is meant to fail.");
        Assert.That(false);
    }

    [Test]
    public void SuccessfulTest()
    {
        ((IJavaScriptExecutor)driver).ExecuteScript("browserstack_executor: {\"action\": \"annotate\", \"arguments\": {\"data\":\"Scenario : Search in Wiki\", \"level\": \"info\"}}");
        Assert.That(true);
    }

    [Test]
    public void TestLessThan()
    {
        Assert.That(2, Is.LessThan(-5));
    }


    [Test]
    public async Task ClickRunAllTest()
    {

        IReadOnlyCollection<AppiumElement> btnElements = driver.FindElements(By.XPath("//android.widget.Button"));

        AppiumElement? btn = null;
        foreach (var element in btnElements)
        {
            if (element.Text.Contains("Run All"))
            {
                btn = element;
                element.Click();
                break;
            }
        }

        Assert.That(btn, Is.Not.Null, "Run All button was not found. Number of elements in btnElements: " + btnElements.Count);

        while (!btn.Enabled)
        {
            // whille the button is disabled, then wait half a second
            await Task.Delay(500);
        }

        IReadOnlyCollection<AppiumElement> labelElements = driver.FindElements(By.XPath("//android.widget.TextView"));
        int numPassed = -1;
        int numFailed = -1;

        for (int i = 0; i < labelElements.Count; i++)
        {
            AppiumElement element = labelElements.ElementAt(i);

            if (element.Text.Equals("✔"))
            {
                i++;
                numPassed = int.Parse(labelElements.ElementAt(i).Text);
            }

            if (element.Text.Equals("⛔"))
            {
                i++;
                numFailed = int.Parse(labelElements.ElementAt(i).Text);
                element.Click();
                await Task.Delay(1000);
                browserStackLog("CLICKED =================");
                break;
            }
        }

        browserStackLog("numPassed: " + numPassed);
        browserStackLog("numFailed: " + numFailed);
        // if either of these are -1, then we couldn't find the correct labels;

        //Assert.That(numPassed, Is.LessThan(-5), "Could not find number passed label." + numPassed);
        //Assert.That(numFailed, Is.LessThan(-5), "Could not find number failed label." + numFailed);
        Assert.That(numPassed, Is.GreaterThanOrEqualTo(0), "Could not find number passed label.");
        Assert.That(numFailed, Is.GreaterThanOrEqualTo(0), "Could not find number failed label.");

        browserStackLog("After the AssertMultiple block");

        if (numFailed == 0)
        {
            browserStackLog("In the num failed == 0 if block");
            // all tests passed! wahoo!
            return;
        }

        browserStackLog("Now past clicking into the test details screen and attempting to filter for failed tests");
        IReadOnlyCollection<AppiumElement> editTextOptions = driver.FindElements(By.XPath("//android.widget.EditText"));

        foreach (var editTextOption in editTextOptions)
        {
            browserStackLog("We are at the edit text option with the text: " + editTextOption.Text);
            if (editTextOption.Text.Equals("All"))
            {
                editTextOption.Click();
                await Task.Delay(500);
                break;
            }
        }

        IReadOnlyCollection<AppiumElement> filterOptions = driver.FindElements(By.XPath("//android.widget.TextView"));

        foreach (var filterOption in filterOptions)
        {
            if (filterOption.Text.Equals("Failed"))
            {
                filterOption.Click();
                await Task.Delay(500);
                break;
            }
        }

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("PASSED TESTS: " + numPassed + " | FAILED TESTS: " + numFailed);

        IReadOnlyCollection<AppiumElement> textResults = driver.FindElements(By.XPath("//android.widget.TextView"));

        foreach (var element in textResults)
        {
            sb.AppendLine(element.Text);
        }

        Assert.That(numFailed, Is.EqualTo(0), sb.ToString());

    }
}
