using KledsonZG.Tiktok;

namespace KledsonZG.CMD
{
    internal class UserInput
    {
        internal enum CMD
        {
            COMMAND_NO_CHANGES = 1,
            COMMAND_CLOSE = 2,           
            COMMAND_START = 3,
            COMMAND_RESTART_REQUIRED = 4,
        }

        internal static CMD GetUserCommandsFromConsole()
        {
                string cmd = "";
                int c = 0;
                int[] eos = new int[]{-1, 0, 10, 13};
                while( eos.Contains( (c = Console.Read() ) ) == false)
                {
                    cmd += (char) c;
                }

                if(cmd == "exit")
                    return CMD.COMMAND_CLOSE;
                if(cmd == "start")
                    return CMD.COMMAND_START;
                else if(cmd == "clear")
                    Main.Program.ClearConsole();                 
                else if(cmd == "help")
                    ListCommands();
                
                else if(cmd == "modo 1")
                {
                    if(Config.Method == ConfigMethod.METHOD_SELENIUM_WEB_DRIVER)
                    {
                        Console.WriteLine("O 'MODO 1' já está ativo!");
                        return CMD.COMMAND_NO_CHANGES;
                    }

                    Config.Method = ConfigMethod.METHOD_SELENIUM_WEB_DRIVER;
                    return CMD.COMMAND_RESTART_REQUIRED;
                }
                    
                else if(cmd == "modo 2")
                {
                    if(Config.Method == ConfigMethod.METHOD_MICROSOFT_POWER_AUTOMATE)
                    {
                        Console.WriteLine("O 'MODO 2' já está ativo!");
                        return CMD.COMMAND_NO_CHANGES;
                    }

                    Config.Method = ConfigMethod.METHOD_MICROSOFT_POWER_AUTOMATE;
                    return CMD.COMMAND_RESTART_REQUIRED;
                }
                else if(cmd.IndexOf("url ") == 0)
                {
                    var param = cmd.Replace("url ", "");
                    Config.LiveURL = param;
                    Console.WriteLine("URL alterado com sucesso! Recarregando configurações...");
                    Config.Save();
                    Thread.Sleep(1000);
                    return CMD.COMMAND_RESTART_REQUIRED;
                }
            return CMD.COMMAND_NO_CHANGES;
        }

        private static void ListCommands()
        {
            var cmds = "help -> Listar os comandos disponíveis\n\n" +
                "start -> Iniciar a funções do programa\n\n" +
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