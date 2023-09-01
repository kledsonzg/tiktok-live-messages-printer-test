using KledsonZG.Tiktok;
using Webdriver;

namespace KledsonZG.Main
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("TikTok Live Messages Printer - Por KledsonZG");
            Console.WriteLine("Repositório: " + "https://github.com/kledsonzg/tiktok-live-messages-printer-test");
            Console.WriteLine("Use o comando 'help' para listar os comandos disponíveis.");
            Thread.Sleep(1000);

            Listener? httpServer = null;
            DriverManager? manager = null;

            Config.Setup();
            if(Config.Method == ConfigMethod.METHOD_MICROSOFT_POWER_AUTOMATE)
                httpServer = new Listener();
            else if(Config.Method == ConfigMethod.METHOD_SELENIUM_WEB_DRIVER)
            {
                if(Config.LiveURL != "")
                {
                    manager = new DriverManager();
                    manager.Start(Config.LiveURL);
                }
                else Console.WriteLine("Configure o URL utilizando o comando 'url [LINK]'");
            }
            
            if(GetUserCommandsFromConsole() == 1)
            {
                if(httpServer != null)
                    httpServer.Stop();
                if(manager != null)
                    manager.Reading = false;
                
                Main();
                return;
            }

            ClearConsole();

            Console.WriteLine("Salvando configuração...");
            Config.Save();

            Console.WriteLine("Parando o programa...");
            
            if(httpServer != null)
                httpServer.Stop();
            if(manager != null)
                manager.Reading = false;
        }

        private static int GetUserCommandsFromConsole()
        {
            while(true)
            {
                string cmd = "";
                int c = 0;
                int[] eos = new int[]{-1, 0, 10, 13};
                while( eos.Contains( (c = Console.Read() ) ) == false)
                {
                    cmd += (char) c;
                }

                if(cmd == "exit")
                    break;
                else if(cmd == "clear")
                    ClearConsole();
                else if(cmd == "help")
                    ListCommands();
                else if(cmd == "modo 1")
                    Config.Method = ConfigMethod.METHOD_SELENIUM_WEB_DRIVER;
                else if(cmd == "modo 2")
                    Config.Method = ConfigMethod.METHOD_MICROSOFT_POWER_AUTOMATE;
                else if(cmd.Contains("url") )
                {
                    var param = cmd.Replace("url ", "");
                    Config.LiveURL = param;
                    Console.WriteLine("URL alterado com sucesso! Recarregando configurações...");
                    Config.Save();
                    Thread.Sleep(1000);
                    return 1;
                }
            }
            return 0;  
        }

        private static void ClearConsole()
        {
            for(int i = 0; i < 100; i++)
                Console.WriteLine("");
            
            Console.WriteLine("Console limpo com sucesso.");
        }

        private static void ListCommands()
        {
            var cmds = "help -> Listar os comandos disponíveis\n\n" +
                "exit -> Fechar o console\n\n" +
                "clear -> Limpar o console\n\n" +
                "url [LINK] -> Altera o URL da live que o programa irá ler\n" +
                "\tFuncionando somente no MODO 1.\n\n" +
                "modo [1] / modo [2] -> Altera o método em que o programa irá ler o chat\n" +
                "\tMODO 1: Utiliza o Selenium Web Driver para gerenciar a instância do navegador.\n" +
                "\tMODO 2: Criar um servidor HTTP para \"ouvir\" as requisições feitas no Microsoft Power Automate.";
            
            Console.WriteLine(cmds);
        }
    }
}
