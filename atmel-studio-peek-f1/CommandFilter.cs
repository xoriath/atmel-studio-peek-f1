﻿using System;
using System.Diagnostics;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio;

namespace Atmel.Studio.Help.Peek
{
    internal class CommandFilter : IOleCommandTarget
    {
        private readonly ITextView _textView;
        private readonly IPeekBroker _peekBroker;

        internal IOleCommandTarget Next { get; set; }

        public CommandFilter(ITextView textView, IPeekBroker peekBroker)
        {
            if (textView == null)
            {
                throw new ArgumentNullException("textView");
            }

            if (peekBroker == null)
            {
                throw new ArgumentNullException("peekBroker");
            }
            
            _textView = textView;
            _peekBroker = peekBroker;
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (pguidCmdGroup == PeekF1Package.PackageCmdSetGuid && nCmdID == PeekF1Package.PeekHelpCmdId)
            {
                HandlePeekHelp();
                return VSConstants.S_OK;
            }

            Debug.Assert(this.Next != null);
            if (this.Next != null)
            {
                return this.Next.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
            }
            else
            {
                return VSConstants.S_OK;
            }
        }

        private void HandlePeekHelp()
        {
            PeekSessionCreationOptions options = new PeekSessionCreationOptions(_textView, PeekHelpRelationship.Instance.Name);
            if (_textView.Roles.Contains(PredefinedTextViewRoles.EmbeddedPeekTextView))
            {
                ITextView containingTextView;
                if (_textView.Properties.TryGetProperty("PeekContainingTextView", out containingTextView) && containingTextView != null)
                {
                    IPeekSession session = _peekBroker.GetPeekSession(containingTextView);
                    if (session != null)
                    {
                        _peekBroker.TriggerNestedPeekSession(options, session);
                    }
                }
            }
            else
            {
                _peekBroker.TriggerPeekSession(options);
            }
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            if (pguidCmdGroup == PeekF1Package.PackageCmdSetGuid && cCmds == 1)
            {
                if (prgCmds[0].cmdID == PeekF1Package.PeekHelpCmdId)
                {
                    prgCmds[0].cmdf = (uint)(OLECMDF.OLECMDF_ENABLED | OLECMDF.OLECMDF_SUPPORTED);
                    return VSConstants.S_OK;
                }
            }

            Debug.Assert(this.Next != null);
            if (this.Next != null)
            {
                return this.Next.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
            }
            else
            {
                return VSConstants.S_OK;
            }
        }
    }
}
