﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;

namespace MediaVF.Web.ArtistTrack.UI
{
    public partial class Main : UserControl
    {
        IUnityContainer Container { get; set; }

        public Main()
        {
            InitializeComponent();

            if (!DesignerProperties.IsInDesignTool)
            {
                Container = ServiceLocator.Current.GetInstance<IUnityContainer>();

                DataContext = Container.Resolve<MainViewModel>();

                Loaded += (sender, e) => ((MainViewModel)DataContext).Initialize();
            }
        }
    }
}