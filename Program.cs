using KledsonZG.Tiktok;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Starting program...");
        Console.WriteLine("Printing live messages!");

        var httpServer = new Listener();

        GetUserCommandsFromConsole();
        Console.WriteLine("Stoping program...");
        httpServer.httpListener.Stop();
    }

    private static void GetUserCommandsFromConsole()
    {
        while(true)
        {
            string cmd = "";
            int c = 0;

            while( (c = Console.Read() ) != 10)
            {
                cmd += (char) c;
            }
            cmd = cmd.Substring(0);
            Console.WriteLine(cmd + " == exit?" + " " + cmd == "exit");
            if(cmd == "exit")
                break;
        }   
    }

    private static void ClearConsole()
    {
        for(int i = 0; i < 100; i++)
            Console.WriteLine("");
        
        Console.WriteLine("Console limpo com sucesso.");
    }
}