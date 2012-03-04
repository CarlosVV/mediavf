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
using System.Collections.Generic;
using System.Windows.Browser;

namespace MediaVF.UI.Core
{
    public class PopupManager : IBrowserMessageManager
    {
        public class PopupWindowInfo
        {
            #region Properties

            public string Page { get; private set; }

            public HtmlPopupWindowOptions Options { get; private set; }

            Dictionary<string, Action<string>> _callbacks;
            public Dictionary<string, Action<string>> Callbacks
            {
                get
                {
                    if (_callbacks == null)
                        _callbacks = new Dictionary<string, Action<string>>();
                    return _callbacks;
                }
            }

            #endregion

            #region Constructors

            public PopupWindowInfo(string page,
                bool resizeable,
                bool scrollbars,
                bool menubar,
                bool toolbar,
                bool statusbar,
                int width,
                int height)
            {
                Page = page;
                Options = new HtmlPopupWindowOptions()
                {
                    Resizeable = resizeable,
                    Scrollbars = scrollbars,
                    Menubar = menubar,
                    Toolbar = toolbar,
                    Status = statusbar,
                    Width = width,
                    Height = height
                };
            }

            #endregion
        }

        Dictionary<string, PopupWindowInfo> _popups;
        Dictionary<string, PopupWindowInfo> Popups
        {
            get
            {
                if (_popups == null)
                    _popups = new Dictionary<string, PopupWindowInfo>();
                return _popups;
            }
        }

        public void AddPopup(string key, string page, bool resizeable, bool scrollbars, bool menubar, bool toolbar, bool statusbar, int width, int height)
        {
            Popups.Add(key, new PopupWindowInfo(page, resizeable, scrollbars, menubar, toolbar, statusbar, width, height));
        }

        public bool AddCallback(string popupKey, string callbackKey, Action<string> callback)
        {
            if (Popups.ContainsKey(popupKey))
            {
                Popups[popupKey].Callbacks.Add(callbackKey, callback);

                return true;
            }
            else
                return false;
        }

        public bool ShowPopup(string popupKey)
        {
            if (Popups.ContainsKey(popupKey))
            {
                string root =
                    Application.Current.Host.Source.OriginalString.Substring(0,
                        Application.Current.Host.Source.OriginalString.IndexOf("ClientBin"));

                HtmlPage.PopupWindow(new Uri(root + Popups[popupKey].Page), popupKey, Popups[popupKey].Options);
            }
                
            return false;
        }

        public void HandleBrowserMessage(string popupID, string callbackID, string args)
        {
            if (Popups.ContainsKey(popupID) && Popups[popupID].Callbacks.ContainsKey(callbackID))
                Popups[popupID].Callbacks[callbackID].Invoke(args);
        }
    }
}