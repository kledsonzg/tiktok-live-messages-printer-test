namespace KledsonZG.Util
{
    internal class Format
    {
        internal static string GetStreamNameByStreamURL(string url)
        {
            url = url.Replace("https://www.tiktok.com/", "");
            return url.Remove(url.IndexOf("/live") );
        }
    }
}