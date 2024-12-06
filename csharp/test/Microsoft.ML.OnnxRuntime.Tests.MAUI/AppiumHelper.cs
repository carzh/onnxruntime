using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Service.Options;

namespace Microsoft.ML.OnnxRuntime.Tests.MAUI;

/// <summary>
/// Helper to start/stop Appium. The platform specific code provides the createFunc to create the AppiumDriver.
/// </summary>
internal partial class AppiumHelper : IDisposable
{
    private AppiumLocalServer? server;
    private static AppiumDriver? driver;
    private bool disposedValue;

    public const string DefaultHostAddress = "127.0.0.1";
    public const int DefaultHostPort = 4723;

    public static AppiumDriver App => driver ?? throw new NullReferenceException("AppiumDriver is null");

    public AppiumHelper()
    {
        server = new AppiumLocalServer(DefaultHostAddress, DefaultHostPort);
        driver = CreateDriver(); // this function is implemented in the platform specific code
    }

    public void Stop()
    {

        // this seems to kill the app if everything works correctly but isn't as consistent if it fails due to a
        // zombie process. trying using autoterminate in MauiProgram.cs for a more graceful way, but not sure if
        // BrowserStack will require Quit to be explicitly called. this needs more debugging
        driver?.Quit(); 
        driver = null;

        server?.Dispose();
        server = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Stop();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

class AppiumLocalServer : IDisposable
{
    private AppiumLocalService? service;

    internal AppiumLocalServer(string host , int port)
    {
        if (service is not null)
        {
            return;
        }

        // this is needed on windows
        var options = new OptionCollector();
        options.AddArguments(new KeyValuePair<string, string>("--base-path", "/wd/hub"));
        options.AddArguments(new KeyValuePair<string, string>("--log-level", "debug"));

        var builder = new AppiumServiceBuilder()
            .WithIPAddress(host)
            .WithArguments(options)
            .WithLogFile(new FileInfo(@"C:\Users\carolinezhu\Documents\cowboy-coding\appium-server-logs\appium-server.log"))
            .UsingPort(port);

        // TODO: User should make sure node.exe is in the path, or NODE_BINARY_PATH
        // Environment.SetEnvironmentVariable("NODE_BINARY_PATH", @"C:\path\to\node.exe");

        // Start the server with the builder
        service = builder.Build();
        service.Start();
    }

    protected virtual void Dispose(bool disposing)
    {
        service?.Dispose();
        service = null;
    }

    ~AppiumLocalServer()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
