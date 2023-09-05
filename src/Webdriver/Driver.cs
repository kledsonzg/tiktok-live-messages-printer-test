using System.Diagnostics;
using System.Reflection;
using KledsonZG.Main;
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
            var executablePath = Environment.ProcessPath;
            if(executablePath == null)
                throw new Exception("Não foi possível obter o diretório raiz do processo atual deste programa.");
            
            var folderInfo = Directory.GetParent(executablePath);
            if(folderInfo == null)
                throw new Exception("Não foi possível obter o diretório raiz do processo atual deste programa.");
            
            var folder = folderInfo.FullName.Replace(Path.GetFileName(folderInfo.FullName), "");
            var driverPath = "edge driver\\msedgedriver.exe";

            var options = new EdgeOptions();

            options.DebuggerAddress = "127.0.0.1:40020";
            options.PageLoadStrategy = PageLoadStrategy.Eager;

            Environment.CurrentDirectory = folder;
            webDriver = new EdgeDriver(driverPath, options);
        }

        internal void SetURL(string url)
        {
            webDriver.Navigate().GoToUrl(url);
        }
        
    }
}
