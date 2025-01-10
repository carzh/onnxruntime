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

    [Test]
    public void FailingOutputTest()
    {
        throw new Exception("This is meant to fail.");
    }

    [Test]
    public void SuccessfulTest()
    {
        Assert.True(true);
    }


    [Test]
    public async Task ClickRunAllTest()
    {

        IReadOnlyCollection<AppiumElement> btnElements = driver.FindElements(By.XPath("//Button"));

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

        Assert.That(btn, Is.Not.Null, "Run All button was not found.");

        while (!btn.Enabled)
        {
            // whille the button is disabled, then wait half a second
            await Task.Delay(500);
        }

        IReadOnlyCollection<AppiumElement> labelElements = driver.FindElements(By.XPath("//Text"));
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
                break;
            }
        }

        // if either of these are -1, then we couldn't find the correct labels;
        Assert.True(numPassed >= 0, "Could not find number passed label.");
        Assert.True(numFailed >= 0, "Could not find number failed label.");

        if (numFailed == 0)
        {
            // all tests passed! wahoo!
            return;
        }

        AppiumElement filterSelector = driver.FindElement(By.XPath("//ComboBox"));

        filterSelector.Click();
        await Task.Delay(500);

        IReadOnlyCollection<AppiumElement> filterOptions = driver.FindElements(By.XPath("//ListItem"));

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

        IReadOnlyCollection<AppiumElement> textResults = driver.FindElements(By.XPath("//Text"));

        foreach (var element in textResults)
        {
            sb.AppendLine(element.Text);
        }

        Assert.True(numFailed == 0, sb.ToString());

    }
}
