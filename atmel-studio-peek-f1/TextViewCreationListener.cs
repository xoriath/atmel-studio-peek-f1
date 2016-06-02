using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Editor;

namespace Atmel.Studio.Help.Peek
{
    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    internal class TextViewCreationListener : IVsTextViewCreationListener
    {
        [Import(typeof(IVsEditorAdaptersFactoryService))]
        private IVsEditorAdaptersFactoryService _editorFactory = null;

        [Import]
        private IPeekBroker _peekBroker = null;

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            IWpfTextView textView = _editorFactory.GetWpfTextView(textViewAdapter);
            if (textView == null)
            {
                return;
            }
            
            IOleCommandTarget next;
            var commandFilter = new CommandFilter(textView, _peekBroker);
            int hr = textViewAdapter.AddCommandFilter(commandFilter, out next);
            if (next != null)
            {
                commandFilter.Next = next;
            }
        }
    }
}
