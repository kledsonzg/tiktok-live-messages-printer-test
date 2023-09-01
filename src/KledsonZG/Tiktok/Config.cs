
namespace KledsonZG.Tiktok
{
    internal class Config
    {
        internal static ConfigMethod Method = ConfigMethod.METHOD_SELENIUM_WEB_DRIVER;
        internal static string LiveURL = "";
        private static string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
            "\\KledsonZG\\Tik Tok Live Printer";
        internal static void Setup()
        {
            Directory.CreateDirectory(documentsFolder);
            string settingsFile = documentsFolder + "\\config.xml";

            if(File.Exists(settingsFile) == false)
            {
                File.Create(settingsFile).Close();
                CreateXMLFileStructure(settingsFile);
            }
                
            Method = GetConfigMethod(settingsFile);
            var lastUrl = GetLastStreamURL(settingsFile);
            if(lastUrl == null)
                return;
            
            LiveURL = lastUrl;
        }

        private static void CreateXMLFileStructure(string settingsFile)
        {
            var reader = new kDivReader();
            reader.ReadFile(settingsFile);

            var writer = reader.GetWriter();
            if(writer == null)
                throw new Exception("Houve um erro durante a criação da estrutura do arquivo de configuração. (writer é nulo.)");
            
            var parentElement = writer.CreatePrimaryElement("config");
            var content = writer.CreateContent("1");
            var methodElement = writer.CreateElement("method");
            writer.CreateElement("last-url").SetParent(parentElement);

            if(content != null)
                methodElement.SetContentAsChild(content);
            
            writer.Save();
        }

        internal static void Save()
        {
            string settingsFile = documentsFolder + "\\config.xml";
            if(File.Exists(settingsFile) == true)
                File.Delete(settingsFile);
            
            File.Create(settingsFile).Close();

            var reader = new kDivReader();
            reader.ReadFile(settingsFile);

            var writer = reader.GetWriter();
            if(writer == null)
                throw new Exception("Houve um erro durante a criação da estrutura do arquivo de configuração. (writer é nulo.)");
            
            var parentElement = writer.CreatePrimaryElement("config");
            var content = writer.CreateContent( ( (int) Method).ToString() );
            var methodElement = writer.CreateElement("method");
            var lastUrlElement = writer.CreateElement("last-url");
            
            lastUrlElement.SetParent(parentElement);

            if(content != null)
                methodElement.SetContentAsChild(content);
            
            content = writer.CreateContent(LiveURL, lastUrlElement);
            
            writer.Save();
        }

        private static ConfigMethod GetConfigMethod(string settingsFile)
        {
            var reader = new kDivReader();
            reader.ReadFile(settingsFile);

            var element = reader.GetElementByAddress("config/method");
            if(element == null)
                throw new Exception("Houve um erro durante a leitura da estrutura do arquivo de configuração. (estrutura do arquivo incorreta ou inexistente.)");
            var content = element.GetContent();
            if(content == null)
                return ConfigMethod.METHOD_SELENIUM_WEB_DRIVER;
            
            try
            {
                return (ConfigMethod) int.Parse(content.GetText() );
            }
            catch{}

            return ConfigMethod.METHOD_SELENIUM_WEB_DRIVER;
        }

        private static string? GetLastStreamURL(string settingsFile)
        {
            var reader = new kDivReader();
            reader.ReadFile(settingsFile);

            var element = reader.GetElementByAddress("config/last-url");
            if(element == null)
                throw new Exception("Houve um erro durante a leitura da estrutura do arquivo de configuração. (estrutura do arquivo incorreta ou inexistente.)");
            var content = element.GetContent();    
            if(content == null)
                return null;
            
            return content.GetText();
        }
    }
}