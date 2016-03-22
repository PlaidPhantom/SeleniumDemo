using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace BasicWebApp.Tests
{
    [TestClass]
    class TestStartup
    {
        private static int _port = 8080;
        private static Process _process;

        public static IWebDriver Browser { get; private set; }

        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)));
            var applicationPath = Path.Combine(solutionFolder, "BasicWebApp");

            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            _process = new Process();
            _process.StartInfo.FileName = programFiles + @"\IIS Express\iisexpress.exe";
            _process.StartInfo.Arguments = $"/path:\"{applicationPath}\" /port:{_port}";
            _process.Start();

            Browser = new FirefoxDriver();
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            Browser.Quit();

            if (_process.HasExited == false)
            {
                _process.Kill();
            }
        }

        public static IWebDriver GetBrowserFor(string path)
        {
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }
            var url = $"http://localhost:{_port}{path}";

            Browser.Navigate().GoToUrl(url);

            return Browser;
        }
    }
}
