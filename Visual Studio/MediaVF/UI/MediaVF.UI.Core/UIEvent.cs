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

using Microsoft.Practices.Prism.Events;

namespace MediaVF.UI.Core
{
    public class UIEventArgs<T>
    {
        public string EventID { get; private set; }

        public T EventData { get; private set; }

        public UIEventArgs(string eventID, T eventData)
        {
            EventID = eventID;
            EventData = eventData;
        }
    }
}
