using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support;

namespace Webdriver
{
    internal class Driver
    {
        private IWebDriver webDriver;
        
        internal IWebDriver Controller { get { return webDriver; } }

        internal Driver()
        {   
            var options = new EdgeOptions();
            options.DebuggerAddress = "127.0.0.1:40020";
            options.PageLoadStrategy = PageLoadStrategy.Eager;

            webDriver = new EdgeDriver(@"W:\csharp\tiktok live messages printer\edge driver\msedgedriver.exe", options);
        }

        internal void SetURL(string url)
        {
            webDriver.Navigate().GoToUrl(url);
            //webDriver.Manage().Window.Maximize();
        }
        
    }
}
