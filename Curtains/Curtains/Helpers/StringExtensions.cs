namespace Curtains.Helpers
{
    internal static class StringExtensions
    {
        internal static string EscapeStarForGrep(this string grep)
            => string.Join("\\*", grep.Split('*'));
    }
}
