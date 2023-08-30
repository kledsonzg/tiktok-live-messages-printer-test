namespace KledsonZG.Util
{
    internal class Format
    {
        internal static string GetStreamNameByStreamURL(string url)
        {
            return url.Replace("https://www.tiktok.com/", "").Replace("/live", "");
        }
    }
}