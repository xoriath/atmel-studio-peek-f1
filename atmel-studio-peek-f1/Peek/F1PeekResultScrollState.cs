using Microsoft.VisualStudio.Language.Intellisense;
using System;

namespace Atmel.Studio.Help.Peek
{
    internal class F1PeekResultScrollState : IPeekResultScrollState
    {
        private F1PeekResultPresentation _presentation;

        public F1PeekResultScrollState(F1PeekResultPresentation presentation)
        {
            if (presentation == null)
            {
                throw new ArgumentNullException("presentation");
            }
            _presentation = presentation;
        }

        public Uri BrowserUrl
        {
            get
            {
                if (_presentation.Browser != null)
                {
                    return _presentation.Browser.Url;
                }
                return null;
            }
        }

        public void RestoreScrollState(IPeekResultPresentation presentation)
        {
        }

        public void Dispose()
        {
        }
    }
}
