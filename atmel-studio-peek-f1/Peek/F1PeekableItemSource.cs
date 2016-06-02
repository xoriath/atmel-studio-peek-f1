using EnvDTE;
using Atmel.Studio.Help.Peek.Documenation;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Atmel.Studio.Help.Peek
{
    internal class F1PeekableItemSource : IPeekableItemSource
    {
        public void AugmentPeekSession(IPeekSession session, IList<IPeekableItem> peekableItems)
        {
            if (session.RelationshipName == PeekHelpRelationship.Instance.Name)
            {
                DTE dte = ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE;
                var attributes = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
                
                ExtractAttributes(dte.ActiveWindow.ContextAttributes, attributes);
                ExtractAttributes(dte.ContextAttributes, attributes);
                attributes["keyword"] = new[] { "atmel;device:atmega328pb;register:portb" };
                //#region F1 functionality
                //// Setup help F1 functionality
                //object userContextObject;
                //((IVsWindowFrame)Frame).GetProperty((int)__VSFPROPID.VSFPROPID_UserContext, out userContextObject);
                //if (userContextObject != null && userContextObject is IVsUserContext)
                //{
                //    var userContext = (IVsUserContext)userContextObject;
                //    userContext.AddAttribute(VSUSERCONTEXTATTRIBUTEUSAGE.VSUC_Usage_LookupF1, "keyword", Resources.HelpF1LookupKey);
                //    userContext.SetDirty(1);
                //}

                string helpUrl = HelpSearcher.Instance.Build(attributes);
                if (!string.IsNullOrWhiteSpace(helpUrl))
                {
                    peekableItems.Add(new F1PeekableItem(helpUrl));
                }
            }
        }

        private static void ExtractAttributes(ContextAttributes contextAttributes, Dictionary<string, string[]> attributes)
        {
            try
            {
                ExtractAttributes(contextAttributes.HighPriorityAttributes, attributes);
                if (contextAttributes != null)
                {
                    contextAttributes.Refresh();
                    foreach (ContextAttribute attr in contextAttributes)
                    {
                        var attrCollection = attr.Values as ICollection;
                        if (attrCollection != null)
                        {
                            string[] values = new string[attrCollection.Count];
                            int i = 0;
                            foreach (string value in attrCollection)
                            {
                                values[i++] = value;
                            }
                            attributes.Add(attr.Name, values);
                        }
                    }
                }
            }
            catch { }
        }

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}
