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
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

namespace MediaVF.UI.Silverlight.Behaviors
{
    public static class ExtendedCommands
    {
        private class ClickData
        {
            public bool Clicked { get; set; }

            public object ClickedObject { get; set; }
        }

        #region DoubleClickCommand

        static Dictionary<Control, ClickData> _clickedOnceMapping;
        static Dictionary<Control, ClickData> ClickedOnceMapping
        {
            get
            {
                if (_clickedOnceMapping == null)
                    _clickedOnceMapping = new Dictionary<Control, ClickData>();
                return _clickedOnceMapping;
            }
        }

        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached("DoubleClickCommand",
            typeof(ICommand),
            typeof(Control),
            new PropertyMetadata(null, new PropertyChangedCallback(OnDoubleClickCommandChanged)));

        public static ICommand GetDoubleClickCommand(Control control)
        {
            return (ICommand)control.GetValue(DoubleClickCommandProperty);
        }

        public static void SetDoubleClickCommand(Control control, ICommand command)
        {
            control.SetValue(DoubleClickCommandProperty, command);
        }

        private static void OnDoubleClickCommandChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            Control control = (Control)sender;
            ICommand command = (ICommand)args.NewValue;

            if (!ClickedOnceMapping.ContainsKey(control))
                ClickedOnceMapping.Add(control, new ClickData());

            control.MouseLeftButtonUp -= new MouseButtonEventHandler(OnMouseLeftButtonUp);
            control.MouseLeftButtonUp += new MouseButtonEventHandler(OnMouseLeftButtonUp);
        }

        private static void OnMouseLeftButtonUp(object sender, MouseEventArgs args)
        {
            Control control = (Control)sender;
            ICommand command = GetDoubleClickCommand(control);

            if (ClickedOnceMapping.ContainsKey(control))
            {
                if (!ClickedOnceMapping[control].Clicked)
                {
                    ClickedOnceMapping[control].Clicked = true;
                    ClickedOnceMapping[control].ClickedObject = GetDoubleClickCommandParameter(control);

                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                    timer.Tick += (tickSender, e) => ClickedOnceMapping[control].Clicked = false;
                }
                else if (GetDoubleClickCommandParameter(control) == ClickedOnceMapping[control].ClickedObject)
                {
                    ClickedOnceMapping[control].Clicked = false;

                    object parameter = GetDoubleClickCommandParameter(control);

                    if (command != null && command.CanExecute(parameter))
                        command.Execute(parameter);
                }
            }
        }

        #endregion

        #region DoubleClickCommandParameter

        public static readonly DependencyProperty DoubleClickCommandParameterProperty =
            DependencyProperty.RegisterAttached("DoubleClickCommandParameter",
                typeof(object),
                typeof(Control),
                new PropertyMetadata(null));

        public static object GetDoubleClickCommandParameter(Control control)
        {
            return (object)control.GetValue(DoubleClickCommandParameterProperty);
        }

        public static void SetDoubleClickCommandParameter(Control control, object command)
        {
            control.SetValue(DoubleClickCommandParameterProperty, command);
        }

        #endregion
    }
}
