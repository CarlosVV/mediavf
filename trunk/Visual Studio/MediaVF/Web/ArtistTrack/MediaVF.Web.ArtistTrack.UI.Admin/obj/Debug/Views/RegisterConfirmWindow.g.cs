﻿#pragma checksum "C:\Users\Evan\Projects\Visual Studio\MediaVF\Web\BandedTogether\MediaVF.Web.BandedTogether.UserManagement\Views\RegisterConfirmWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EA918CC22E23496ECF9785F54F1C0CFD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace MediaVF.Web.BandedTogether.UserManagement.Views {
    
    
    public partial class RegisterConfirmWindow : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock EmailLabel;
        
        internal System.Windows.Controls.TextBox EmailTextBox;
        
        internal System.Windows.Controls.TextBlock PasswordLabel;
        
        internal System.Windows.Controls.PasswordBox PasswordBox;
        
        internal System.Windows.Controls.TextBlock ConfirmPasswordLabel;
        
        internal System.Windows.Controls.PasswordBox ConfirmPasswordBox;
        
        internal System.Windows.Controls.Button OKButton;
        
        internal System.Windows.Controls.Button CancelButton;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/MediaVF.Web.BandedTogether.UserManagement;component/Views/RegisterConfirmWindow." +
                        "xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.EmailLabel = ((System.Windows.Controls.TextBlock)(this.FindName("EmailLabel")));
            this.EmailTextBox = ((System.Windows.Controls.TextBox)(this.FindName("EmailTextBox")));
            this.PasswordLabel = ((System.Windows.Controls.TextBlock)(this.FindName("PasswordLabel")));
            this.PasswordBox = ((System.Windows.Controls.PasswordBox)(this.FindName("PasswordBox")));
            this.ConfirmPasswordLabel = ((System.Windows.Controls.TextBlock)(this.FindName("ConfirmPasswordLabel")));
            this.ConfirmPasswordBox = ((System.Windows.Controls.PasswordBox)(this.FindName("ConfirmPasswordBox")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
        }
    }
}
