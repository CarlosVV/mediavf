using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Practices.Prism.Modularity;

using MediaVF.Common.Modularity;
using Microsoft.Practices.Unity;

namespace MediaVF.UI.Core
{
    public class BrowserManager : IBrowserMessageManager
    {
        IUnityContainer Container { get; set; }

        public BrowserManager(IUnityContainer container)
        {
            Container = container;

            HtmlPage.RegisterScriptableObject("browserManager", this);
        }

        [ScriptableMember]
        public void HandleBrowserMessage(string popupID, string callbackID, string args)
        {
            if (Container.IsRegistered<IBrowserMessageManager>(popupID))
                Container.Resolve<IBrowserMessageManager>(popupID).HandleBrowserMessage(popupID, callbackID, args);
        }
    }
}
