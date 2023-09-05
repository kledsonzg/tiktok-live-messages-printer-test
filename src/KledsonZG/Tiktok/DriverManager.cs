using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;
using Webdriver;

namespace KledsonZG.Tiktok
{
    internal class DriverManager
    {
        private Driver driver;
        private IWebElement? chatPanel;
        private IJavaScriptExecutor javaScript;
        private bool reading = false;
        private bool canStop = false;

        internal DriverManager()
        {
            driver = new Driver();
            javaScript = (IJavaScriptExecutor) driver.Controller;
        }

        internal bool Reading { get { return reading; } set{ canStop = !value; } }
        internal Driver GetDriver() { return driver; }

        internal void Start(string url)
        {          
            SetStreamURL(url);
        }

        internal void SetStreamURL(string url)
        {
            Console.WriteLine("Abrindo/Alterando URL para: " + url);
            while(Reading)
                Thread.Sleep(1000);
            
            driver.SetURL(url);
            reading = true;

            new Thread(new ThreadStart(
                delegate { InializeChatReader(); } 
            ) ).Start();
        }

        private IWebElement? GetChatPanelElement()
        {
            try
            {
                var controller = driver.Controller;

                var elements = controller.FindElements(By.TagName("div") );
                foreach(var element in elements)
                {
                    var className = element.GetAttribute("class");
                    if(className.Contains("DivChatMessageList") == false)
                        continue;
                    
                    return element;
                }
            }
            catch{ return null; }

            return null;
        }

        private void CheckAndClickForNewMessages()
        {
            if(chatPanel == null)
                return;
            
            try
            {
                var parent = (WebElement) javaScript.ExecuteScript("return arguments[0].parentNode;", chatPanel);
                var buttonContainer = parent.FindElements(By.TagName("div") ).Where(p => p.GetAttribute("class").Contains("DivUnreadTips") ).First();
                var button = (WebElement) javaScript.ExecuteScript("return arguments[0].childNodes[0];", buttonContainer);

                button.Click();
            }
            catch{ return; }
        }
        
        private void InializeChatReader()
        {
            while(chatPanel == null)
            {
                if(canStop)
                {
                    reading = false;
                    break;
                }

                chatPanel = GetChatPanelElement();
            }
                
            
            while(Reading)
            {
                if(canStop)
                {
                    reading = false;
                    break;
                }
                             
                var contents = new List<Content>();
                List<IWebElement> chatElements;

                CheckAndClickForNewMessages();
      
                try
                {
                    if(chatPanel == null)
                        throw new NullReferenceException("'chatPanel' possui um valor nulo.");
                    
                    chatElements = chatPanel.FindElements(By.TagName("div") ).ToList();
                }
                catch
                {
                    Thread.Sleep(1000);

                    chatPanel = GetChatPanelElement();
                    while(chatPanel == null)
                    {
                        if(canStop)
                        {
                            reading = false;
                            break;
                        }

                        chatPanel = GetChatPanelElement();
                    }                    
         
                    continue;
                }           

                for(int i = 0; i < chatElements.Count; i++)
                {                  
                    try
                    {
                        var element = chatElements[i];
                        
                        string className = element.GetAttribute("class");
                        if(className.Contains(" read_completed") || className.Contains("DivChatMessage") == false)
                        {
                            chatElements.RemoveAt(i);
                            i--;
                            continue;
                        }

                        var content = ExtractContentFromElement(element);
                        if(content == null)
                            continue;
                        
                        javaScript.ExecuteScript("arguments[0].className += \" read_completed\"", element);
                        contents.Add(content);
                    }
                    catch { continue; }                
                }

                Printer.PrintChatMessages(contents);
                Thread.Sleep(1000);
            }
        }

        private Content? ExtractContentFromElement(IWebElement element)
        {
            var validNameElementClassNames = new string[]{"message-owner-name", "SpanEllipsisName"};
            var validMessageElementClassNames = new string[]{"DivComment", "SpanChatRoomComment"};                

            string userName = "", message = "";
            try
            {
                foreach(var content in element.FindElements(By.TagName("span") ) )
                {
                    string spanClassName = content.GetAttribute("class");

                    if(userName.Length < 1)
                        for(int j = 0; j < validNameElementClassNames.Length; j++)
                            if(spanClassName.Contains(validNameElementClassNames[j] ) ){
                                userName = content.GetAttribute("innerHTML");
                                break;
                            }
                                
                    if(message.Length < 1)
                        for(int j = 0; j < validMessageElementClassNames.Length; j++)
                            if(spanClassName.Contains(validMessageElementClassNames[j] ) ){
                                message = content.GetAttribute("innerHTML");
                                break;
                            }
                    if((userName.Length == 0 || message.Length == 0) == false)
                        break;
                }
                if(message.Length == 0)
                    foreach(var content in element.FindElements(By.TagName("div") ) )
                    {
                        string spanClassName = content.GetAttribute("class");
                        
                        for(int j = 0; j < validMessageElementClassNames.Length; j++)
                            if(spanClassName.Contains(validMessageElementClassNames[j] ) ){
                                message = content.GetAttribute("innerHTML");
                                break;
                            }
                        if(message.Length > 0)
                            break;
                    }
            }
            catch{ return null; }

            if(userName.Length == 0 || message.Length == 0)
                return null;
            
            return new Content(userName, message);
        }
    }
}
    