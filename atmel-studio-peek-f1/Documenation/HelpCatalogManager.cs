
using Atmel.VsIde.AvrStudio.Utils.Help;
using Microsoft.VisualStudio.Help.Runtime;
using System;
using System.IO;

namespace Atmel.Studio.Help.Peek.Documenation
{
    internal class HelpCatalogManager : IDisposable
    {
        private readonly string catalogPath;
        private readonly string locale = "en-US";

        private readonly CatalogRead catalogRead = new CatalogRead();
        private readonly Catalog catalog = new Catalog();

        public HelpCatalogManager(string catalogPath, string locale = "en-US")
        {
            this.catalogPath = catalogPath;
            this.locale = locale;

            OpenCatalog();
        }

        public HelpCatalogManager()
        {
            this.catalogPath = Path.Combine(HelpManagerConfiguration.HelpCatalogBase, HelpManagerConfiguration.CatalogName);
            this.locale = HelpManagerConfiguration.CatalogLocale;

            OpenCatalog();
            InitializeProtocolHandler();
        }

        private void InitializeProtocolHandler()
        {
            Protocol.ProtocolFactory.Register("ms-xhelp", () => new Protocol.MsXhelpProtocol(this));
        }

        private void OpenCatalog()
        {
            this.catalog.Open(this.catalogPath, new[] { this.locale });
        }

        public ITopic GetKeywordMatches(string[] keywords)
        {
            return this.catalogRead.GetTopicDetailsForF1Keyword(this.catalog, keywords, new HelpFilter());
        }
        public ITopic GetTopicWithId(string topicId)
        {
            return this.catalogRead.GetIndexedTopicDetails(this.catalog, topicId, new HelpFilter());
        }

        public Stream GetResource(string packageName, string path, string locale)
        {
            return this.catalogRead.GetLinkedAsset(this.catalog, packageName, path, locale) as Stream;
        }

        private bool isDisposed = false;
        public void Dispose()
        {
            if (this.isDisposed)
                return;

            if (this.catalog.IsOpen)
                this.catalog.Close();

            this.catalog.Dispose();
            this.isDisposed = true;
        }
    }
}
