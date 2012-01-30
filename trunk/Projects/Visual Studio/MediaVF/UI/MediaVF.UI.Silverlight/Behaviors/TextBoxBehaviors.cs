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

namespace MediaVF.UI.Silverlight.Behaviors
{
    public static class TextBoxBehaviors
    {
        public static readonly DependencyProperty TextChangedCommandProperty = DependencyProperty.RegisterAttached("TextChangedCommand",
            typeof(ICommand),
            typeof(TextBox),
            new PropertyMetadata(null, new PropertyChangedCallback(OnTextChangedCommandChanged)));

        public static ICommand GetTextChangedCommand(TextBox textBox)
        {
            return (ICommand)textBox.GetValue(TextChangedCommandProperty);
        }

        public static void SetTextChangedCommand(TextBox textBox, ICommand command)
        {
            textBox.SetValue(TextChangedCommandProperty, command);
        }

        private static void OnTextChangedCommandChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            TextBox textBox = sender as TextBox;
            ICommand command = args.NewValue as ICommand;

            if (textBox != null)
            {
                // unattach (if already attached), then reattach
                textBox.TextChanged -= new TextChangedEventHandler(OnTextChanged);
                textBox.TextChanged += new TextChangedEventHandler(OnTextChanged);
            }
        }

        private static void OnTextChanged(object sender, TextChangedEventArgs args)
        {
            TextBox textBox = (TextBox)sender;
            ICommand command = GetTextChangedCommand(textBox);
            
            // if command is registered for textbox, check if it can execute, and if so, execute it
            if (command != null && command.CanExecute(textBox.Text))
                command.Execute(textBox.Text);
        }
    }
}
