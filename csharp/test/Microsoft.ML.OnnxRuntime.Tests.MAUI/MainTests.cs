using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ML.OnnxRuntime.Tests.MAUI;

[CollectionDefinition("Microsoft.ML.OnnxRuntime.Tests.MAUI")]
public sealed class UITestsCollectionDefinition : ICollectionFixture<AppiumHelper>
{

}

[Collection("Microsoft.ML.OnnxRuntime.Tests.MAUI")]
public class MainPageTests
{
    protected AppiumDriver App => AppiumHelper.App;

    [Fact]
    public void assertTrue()
    {
        Assert.True(true);
    }


}