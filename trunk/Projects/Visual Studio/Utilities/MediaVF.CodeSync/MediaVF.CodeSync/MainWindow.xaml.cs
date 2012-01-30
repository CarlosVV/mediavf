using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Forms = System.Windows.Forms;
using System.ComponentModel;

namespace MediaVF.CodeSync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Forms.NotifyIcon NotifyIcon { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel(((App)Application.Current).Container);

            // create notify icon
            NotifyIcon = new Forms.NotifyIcon();
            Uri iconUri = new Uri("pack://application:,,,/Icons/sync.ico");
            NotifyIcon.Icon = new Icon(Application.GetResourceStream(iconUri).Stream);
            NotifyIcon.Text = "MediaVF Code Sync";
            NotifyIcon.Visible = true;
            NotifyIcon.Click += new EventHandler(OnNotifyIconClick);
        }

        private void OnNotifyIconClick(object Sender, EventArgs e)
        {
            // show if minimized
            if (WindowState == WindowState.Minimized)
                WindowState = WindowState.Normal;

            // activate the form
            Activate();
        }
    }
}
