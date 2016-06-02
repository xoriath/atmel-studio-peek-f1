
using System.IO;
using System.Threading.Tasks;

namespace Atmel.Studio.Help.Peek.Protocol
{
    public interface IProtocol
    {
        string Name { get; }
        Task<Stream> GetStreamAsync(string url);
    }
}
