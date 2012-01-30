using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using MediaVF.UI.Core;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Practices.Unity;
using System.Reflection;

namespace MediaVF.CodeSync
{
    public class FolderSyncViewModel : WindowViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the original copy
        /// </summary>
        FolderSyncViewModel Original { get; set; }

        /// <summary>
        /// Gets or sets the current sync status
        /// </summary>
        public SyncStatus SyncStatus
        {
            get { return _syncStatus; }
            set
            {
                if (_syncStatus != value)
                {
                    _syncStatus = value;

                    RaisePropertyChanged("SyncStatus");
                }
            }
        }
        SyncStatus _syncStatus;

        /// <summary>
        /// Gets or sets the time last synced
        /// </summary>
        public DateTime? LastSynced
        {
            get { return _lastSynced; }
            set
            {
                if (_lastSynced != value)
                {
                    _lastSynced = value;

                    RaisePropertyChanged("LastSynced");
                }
            }
        }
        DateTime? _lastSynced;

        /// <summary>
        /// Gets or sets the source folder path
        /// </summary>
        public string SourceFolder
        {
            get { return _sourceFolder; }
            set
            {
                if (_sourceFolder != value)
                {
                    _sourceFolder = value;

                    RaisePropertyChanged("SourceFolder");
                    RaisePropertyChanged("SourceFolderShortName");
                }
            }
        }
        string _sourceFolder;

        /// <summary>
        /// Gets the short name of the source folder
        /// </summary>
        public string SourceFolderShortName
        {
            get { return !string.IsNullOrEmpty(SourceFolder) ? SourceFolder.TrimEnd('\\').Substring(SourceFolder.LastIndexOf('\\') + 1) : string.Empty; }
        }
        
        /// <summary>
        /// Gets or sets the destination folder path
        /// </summary>
        public string DestinationFolder
        {
            get { return _destinationFolder; }
            set
            {
                if (_destinationFolder != value)
                {
                    _destinationFolder = value;

                    RaisePropertyChanged("DestinationFolder");
                    RaisePropertyChanged("DestinationFolderShortName");
                }
            }
        }
        string _destinationFolder;

        /// <summary>
        /// Gets the short name of the destination folder
        /// </summary>
        public string DestinationFolderShortName
        {
            get { return !string.IsNullOrEmpty(DestinationFolder) ? DestinationFolder.TrimEnd('\\').Substring(DestinationFolder.LastIndexOf('\\') + 1) : string.Empty; }
        }

        /// <summary>
        /// Gets collection of sync types
        /// </summary>
        public ObservableCollection<SyncType> SyncTypes
        {
            get
            {
                if (_syncTypes == null)
                    _syncTypes = new ObservableCollection<SyncType>(Enum.GetValues(typeof(SyncType)).Cast<SyncType>());
                return _syncTypes;
            }
        }
        ObservableCollection<SyncType> _syncTypes;

        /// <summary>
        /// Gets or sets the sync type
        /// </summary>
        public SyncType SyncType
        {
            get { return _syncType; }
            set
            {
                if (_syncType != value)
                {
                    _syncType = value;

                    RaisePropertyChanged("SyncType");
                }
            }
        }
        SyncType _syncType;

        /// <summary>
        /// Gets or sets the file types to sync
        /// </summary>
        public ObservableCollection<FileTypeViewModel> FileTypes
        {
            get
            {
                if (_fileTypes == null)
                    _fileTypes = new ObservableCollection<FileTypeViewModel>(FileTypeViewModel.GetFileTypes());
                return _fileTypes;
            }
        }
        ObservableCollection<FileTypeViewModel> _fileTypes;

        /// <summary>
        /// Gets selected file types
        /// </summary>
        public IEnumerable<FileTypeViewModel> SelectedFileTypes
        {
            get { return FileTypes.Where(ft => ft.ShouldSync); }
        }

        #endregion

        #region Commands

        public DelegateCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new DelegateCommand(obj =>
                        {
                            Result = true;

                            // copy new values to original
                            if (Original != null)
                                CopyTo(Original);

                            RaiseClose();
                        });
                return _saveCommand;
            }
        }
        DelegateCommand _saveCommand;

        /// <summary>
        /// Gets the command to execute when cancelling
        /// </summary>
        public DelegateCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new DelegateCommand(obj =>
                        {
                            Result = false;

                            RaiseClose();
                        });
                return _cancelCommand;
            }
        }
        DelegateCommand _cancelCommand;

        public DelegateCommand BrowseFolderCommand
        {
            get
            {
                if (_browseFolderCommand == null)
                    _browseFolderCommand = new DelegateCommand(obj =>
                        {
                            // get folder
                            string folder = Container.Resolve<IDialogService>().ShowFolderDialog("Select the folder to sync.", Environment.SpecialFolder.Desktop, true);

                            // get property and set value
                            PropertyInfo propInfo = GetType().GetProperty((string)obj);
                            if (propInfo != null)
                                propInfo.SetValue(this, folder, null);
                        });
                return _browseFolderCommand;
            }
        }
        DelegateCommand _browseFolderCommand;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates the folder sync view model
        /// </summary>
        /// <param name="container"></param>
        public FolderSyncViewModel(IUnityContainer container)
            : base(container) { }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the original copy to the current state
        /// </summary>
        public FolderSyncViewModel CreateCopy()
        {
            // create copy
            FolderSyncViewModel copy = Container.Resolve<FolderSyncViewModel>();

            // set this instance as the original version
            copy.Original = this;

            // copy current values
            CopyTo(copy);

            return copy;
        }

        /// <summary>
        /// Copies values from this instance to another
        /// </summary>
        /// <param name="copy"></param>
        private void CopyTo(FolderSyncViewModel copy)
        {
            // set basic static values
            copy.SourceFolder = SourceFolder;
            copy.DestinationFolder = DestinationFolder;
            copy.SyncType = SyncType;
            copy.Modal = Modal;
            copy.Result = Result;

            // set file types
            foreach (FileTypeViewModel fileTypeViewModel in SelectedFileTypes)
                copy.FileTypes.First(ft => ft.FileTypeName == fileTypeViewModel.FileTypeName).ShouldSync = true;
        }

        #endregion
    }
}
