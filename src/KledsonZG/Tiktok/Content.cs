namespace KledsonZG.Tiktok
{
    internal class Content
    {
        private string Username = "";
        private string Message = "";
        private DateTime Time;

        internal string Sender { get { return Username; } }
        internal string Text { get { return Message; } }

        internal Content(string username, string message)
        {
            Time = DateTime.Now;

            Username = username;
            Message = message;
        }
    }
}
