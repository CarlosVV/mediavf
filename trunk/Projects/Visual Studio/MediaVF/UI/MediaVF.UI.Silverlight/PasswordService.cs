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

namespace MediaVF.UI.Core
{
    public class PasswordService
    {
        public static readonly DependencyProperty AttachedProperty = DependencyProperty.RegisterAttached("Attached",
            typeof(bool),
            typeof(PasswordBox),
            new PropertyMetadata(false, new PropertyChangedCallback(OnAttachedChanged)));

        public static bool GetAttached(PasswordBox passwordBox)
        {
            return (bool)passwordBox.GetValue(AttachedProperty);
        }

        public static void SetAttached(PasswordBox passwordBox, bool attached)
        {
            passwordBox.SetValue(AttachedProperty, attached);
        }

        private static void OnAttachedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PasswordBox passwordBox = (PasswordBox)obj;

            passwordBox.PasswordChanged -= new RoutedEventHandler(OnPasswordChanged);
            passwordBox.PasswordChanged += new RoutedEventHandler(OnPasswordChanged);
        }

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password",
            typeof(string),
            typeof(PasswordBox),
            new PropertyMetadata(null));

        public static string GetPassword(PasswordBox passwordBox)
        {
            return (string)passwordBox.GetValue(PasswordProperty);
        }

        public static void SetPassword(PasswordBox passwordBox, string password)
        {
            passwordBox.SetValue(PasswordProperty, password);
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs args)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            SetPassword(passwordBox, passwordBox.Password);
        }
    }
}
