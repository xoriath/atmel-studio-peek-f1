
using Microsoft.VisualStudio.Help.Runtime;
using System.IO;
using System.Threading.Tasks;

namespace Atmel.Studio.Help.Peek.Documenation
{
    public static class TopicExtensions
    {

        public static Stream AsStream(this ITopic t)
        {
            return t.FetchContent() as Stream;
        }
        public static Task<string> AsHtmlStringAsync(this ITopic t)
        {
            var stream = t.AsStream();
            if (stream == null)
                return new Task<string>( () => string.Empty );

            var reader = new StreamReader(stream);
            return reader.ReadToEndAsync();
        }

        public static string AsHtmlString(this ITopic t)
        {
            var stream = t.AsStream();
            if (stream == null)
                return string.Empty;

            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

    }
}
