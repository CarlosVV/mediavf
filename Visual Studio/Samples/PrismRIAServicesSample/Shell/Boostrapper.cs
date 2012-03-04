///////////////////////////////////////////////////////////////////////////////
//
// Copyright (C) 2008-2009 David Hill. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Windows;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.UnityExtensions;

using Prism.Samples.Shell.Views;

namespace Prism.Samples.Shell
{
    public partial class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
        }

        protected override IModuleCatalog GetModuleCatalog()
        {
            return ModuleCatalog.CreateFromXaml(
                        new Uri( "/Prism.Shell;component/ModuleCatalog.xaml",
                        UriKind.Relative ) );
        }

        protected override DependencyObject CreateShell()
        {
            // Use the container to create an instance of the shell.
            ShellView view = Container.Resolve<ShellView>();

            // Set it as the root visual for the application.
            Application.Current.RootVisual = view;

            return view;
        }
    }
}

