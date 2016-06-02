using Microsoft.VisualStudio.Help.Runtime;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Atmel.Studio.Help.Peek.Documenation;

namespace Atmel.Studio.Help.Peek.Protocol
{
    class MsXhelpProtocol : IProtocol
    {
        private readonly HelpCatalogManager helpCatalogManager;

        public MsXhelpProtocol(HelpCatalogManager helpCatalogManager)
        {
            this.helpCatalogManager = helpCatalogManager;
        }
        public string Name
        {
            get
            {
                return "ms-xhelp";
            }
        }

        public Task<Stream> GetStreamAsync(string url)
        {
            var request = UriExtensions.ParseQueryString(new Uri(url));

            var id = request["Id"];

            return Task.FromResult(helpCatalogManager.GetTopicWithId(id).AsStream());

        }
    }
}
