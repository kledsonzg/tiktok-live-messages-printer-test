using KledsonZG.Tiktok;
using KledsonZG.CMD;
using Webdriver;
using KledsonZG.Browser;

namespace KledsonZG.Main
{
    public class Program
    {
        public static void Main()
        {
            Listener? listener = null;
            DriverManager? manager = null;

            Config.Setup();

            Console.WriteLine("TikTok Live Messages Printer - Por KledsonZG");
            Console.WriteLine("Repositório: " + "https://github.com/kledsonzg/tiktok-live-messages-printer-test");
            Console.WriteLine("Use o comando 'help' para listar os comandos disponíveis.");
            Thread.Sleep(1000);

            HandleUserCommand(listener, manager);
        }

        internal static void ClearConsole()
        {
            for(int i = 0; i < 100; i++)
                Console.WriteLine("");
            
            Console.WriteLine("Console limpo com sucesso.");
        }

        private static void Start(Listener? listener, DriverManager? manager)
        {
            if(Config.LiveURL.Length < 1)
            {
                Console.WriteLine("Configuração de URL não encontrada.");
                Console.WriteLine("Configure o URL utilizando o comando 'url [LINK]'");

                HandleUserCommand(listener, manager);
                return;
            }

            if(Config.Method == ConfigMethod.METHOD_SELENIUM_WEB_DRIVER)
            {
                Console.WriteLine("Iniciando no 'MODO 1'...");
                ProcessInitializer.Start();
                manager = new DriverManager();
                manager.Start(Config.LiveURL);
            }
            else if(Config.Method == ConfigMethod.METHOD_MICROSOFT_POWER_AUTOMATE)
            {
                Console.WriteLine("Iniciando no 'MODO 2'...");
                listener = new Listener();
            }

            HandleUserCommand(listener, manager);
        }

        private static void Stop(Listener? listener, DriverManager? manager)
        {
            if(listener != null)
            {
                Console.WriteLine("Aguarde enquanto o Thread do servidor é fechado...");
                listener.Stop();
            }
            if(manager != null)
            {
                Console.WriteLine("Aguarde enquanto o Thread do leitor é fechado...");
                manager.Reading = false;
                
                while(manager.Reading)
                    Thread.Sleep(1000);
                
                manager.GetDriver().Controller.Quit();
                ProcessInitializer.Stop();
            }

            //Apenas para dar tempo de ver o output.
            Thread.Sleep(1000);
        }

        private static void HandleUserCommand(Listener? listener, DriverManager? manager)
        {
            UserInput.CMD cmd; 
            while( (cmd = UserInput.GetUserCommandsFromConsole() ) != UserInput.CMD.COMMAND_CLOSE)
            {
                if(cmd == UserInput.CMD.COMMAND_NO_CHANGES)
                    continue;
                if(cmd == UserInput.CMD.COMMAND_START)
                {
                    if(listener != null || manager != null)
                        Stop(listener, manager);
                    
                    Start(listener, manager);
                    return;
                }
                if(cmd == UserInput.CMD.COMMAND_RESTART_REQUIRED)
                {
                    ClearConsole();
                    Console.WriteLine("Iniciando/Reiniciando...");
                    Stop(listener, manager);         
                }

                Main();
                return;
            }
            Console.WriteLine("Salvando configuração...");
            Config.Save();

            Console.WriteLine("Parando o programa...");
            Stop(listener, manager);
        }
    }
}
