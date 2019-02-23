using System.Linq;

namespace Curtains.Helpers
{
    public static class StringExtensions
    {
        public static string EscapeSpecialCharacterGrep(this string grep)
            => new string(
                grep
                    .SelectMany(c => SpecialCharacter.Contains(c) ? new[] { '\\', c } : new[] { c })
                    .ToArray());
        
        static readonly char[] SpecialCharacter = new char[]
        {
            '*',
            '.',
            '[',
            '\\',
        };
    }
}
