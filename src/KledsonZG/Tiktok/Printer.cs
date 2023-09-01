namespace KledsonZG.Tiktok
{
    internal class Printer
    {
        internal static void PrintChatMessages(List<Content> contents)
        {
            foreach(var content in contents)
            {
                Console.WriteLine(content.Sender + ": " + content.Text);
            }
        }
    }
}