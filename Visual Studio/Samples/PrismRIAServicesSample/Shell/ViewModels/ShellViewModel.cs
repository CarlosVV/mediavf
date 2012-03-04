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
using System.ComponentModel;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Prism.Samples.Shell.ViewModels
{
    public class ShellViewModel : INotifyPropertyChanged
    {
        public ShellViewModel()
        {
            // Initialize this ViewModel's commands.
            ExitCommand = new DelegateCommand<object>( AppExit, CanAppExit );
        }

        #region ExitCommand
        public DelegateCommand<object> ExitCommand { get; private set; }

        private void AppExit( object commandArg )
        {
        }

        private bool CanAppExit( object commandArg )
        {
            return true;
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
