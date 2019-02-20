using System.IO;

namespace Curtains.Helpers
{
    public static class StringStream
    {
        public static MemoryStream GenerateStreamFromString(this string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
