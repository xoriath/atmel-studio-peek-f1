using Microsoft.VisualStudio.Help.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Atmel.Studio.Help.Peek.Documenation
{
    class HelpSearcher
    {
        private static HelpSearcher instance;
        public static HelpSearcher Instance
        {
            get { return instance ?? ( instance = new HelpSearcher() ); }
            private set { instance = value; }
        }

        private HelpCatalogManager catalogManager;
        private HelpSearcher()
        {
            catalogManager = new HelpCatalogManager();
        }

        internal string Build(Dictionary<string, string[]> attributes)
        {
            if (!attributes.ContainsKey("keyword"))
                return string.Empty;

            var keywords = attributes["keyword"];

            var t = catalogManager.GetKeywordMatches(keywords);
            var to = catalogManager.GetTopicWithId("GUID-0EC909F9-8FB7-46B2-BF4B-05290662B5C3-GUID-62F4F8E9-00D2-4D03-9772-1E3CDDF1C639");

            //return CollateAssets(t);
            return to.AsHtmlString();
            
        }

        private string CollateAssets(ITopic t)
        {
            var doc = XDocument.Load(t.AsStream());

            foreach(var style in doc.XPathSelectElements(@".//*[local-name(.) = 'link']"))
            {
                var href = style.Attribute("href").Value;
                style.Attribute("href").Remove();

                //var queryString = href.Substring(href.IndexOf('?'));
                //var parts = UriExtensions.ParseQueryString(new Uri(queryString));

                //var method = parts["method"];
                //var id = parts["id"];
                //var package = parts["package"];
                //var locale = parts["topicLocale"];

                var package = t.Package;
                var path = href;
                var locale = t.TopicLocale;

                var linkContent = new StreamReader(catalogManager.GetResource(package, path, locale)).ReadToEnd();

                style.Value = $"<[CDATA[ { linkContent } ]]>";

            }

            return doc.ToString();
        }
    }
}
