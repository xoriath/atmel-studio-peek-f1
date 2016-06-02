using Microsoft.VisualStudio.Language.Intellisense;
using System;
using System.Threading;

namespace Atmel.Studio.Help.Peek
{
    internal class F1PeekResultSource : IPeekResultSource
    {
        private string _helpUrl;

        public F1PeekResultSource(string helpUrl)
        {
            if (string.IsNullOrWhiteSpace(helpUrl))
            {
                throw new ArgumentException("helpUrl");
            }
            _helpUrl = helpUrl;
        }

        public void FindResults(string relationshipName, IPeekResultCollection resultCollection,
            CancellationToken cancellationToken, IFindPeekResultsCallback callback)
        {
            resultCollection.Add(new F1PeekResult(_helpUrl));
        }
    }
}
