using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace MediaVF.UI.Core
{
    public static class DisplayServiceExtensions
    {
        public static void ShowPopup(this IDisplayService displayService, string popupKey, string callbackKey, Action<string> callback)
        {
            Dictionary<string, Action<string>> callbacks = new Dictionary<string, Action<string>>();
            callbacks.Add(callbackKey, callback);

            ShowPopup(displayService, popupKey, callbacks);
        }

        public static void ShowPopup(this IDisplayService displayService, string popupKey, Dictionary<string, Action<string>> callbacks)
        {
            PopupManager popupManager = displayService.Container.Resolve<IBrowserMessageManager>(popupKey) as PopupManager;
            if (popupManager != null)
            {
                if (callbacks != null)
                    foreach (string callbackKey in callbacks.Keys)
                        popupManager.AddCallback(popupKey, callbackKey, callbacks[callbackKey]);

                popupManager.ShowPopup(popupKey);
            }
        }
    }
}
