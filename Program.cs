using KledsonZG.Tiktok;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Starting program...");
        Console.WriteLine("Printing live messages!");

        var httpServer = new Listener();

        GetUserCommandsFromConsole();

        ClearConsole();
        Console.WriteLine("Stopping program...");
        httpServer.Stop();
    }

    private static void GetUserCommandsFromConsole()
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
        }   
    }

    private static void ClearConsole()
    {
        for(int i = 0; i < 100; i++)
            Console.WriteLine("");
        
        Console.WriteLine("Console limpo com sucesso.");
    }
}