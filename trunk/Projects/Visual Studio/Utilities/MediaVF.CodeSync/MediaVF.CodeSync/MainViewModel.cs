using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaVF.UI.Core;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;

namespace MediaVF.CodeSync
{
    public class MainViewModel : ContainerViewModel
    {
        #region Properties

        /// <summary>
        /// Gets a collection of all the folders to be synced
        /// </summary>
        public ObservableCollection<FolderSyncViewModel> FolderSyncs
        {
            get
            {
                if (_folderSyncs == null)
                    _folderSyncs = new ObservableCollection<FolderSyncViewModel>();
                return _folderSyncs;
            }
        }
        ObservableCollection<FolderSyncViewModel> _folderSyncs;

        /// <summary>
        /// Gets the currently selected folder sync object
        /// </summary>
        public FolderSyncViewModel SelectedFolderSync
        {
            get { return _selectedFolderSync; }
            set
            {
                if (_selectedFolderSync != value)
                {
                    _selectedFolderSync = value;

                    RaisePropertyChanged("SelectedFolderSync");
                }
            }
        }
        FolderSyncViewModel _selectedFolderSync;

        #endregion

        #region Commands

        /// <summary>
        /// Gets a command for adding a folder sync
        /// </summary>
        public DelegateCommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                    _addCommand = new DelegateCommand(
                        obj => AddFolderSync(),
                        obj => SelectedFolderSync == null,
                        this);
                return _addCommand;
            }
        }
        DelegateCommand _addCommand;

        /// <summary>
        /// Gets a command for editing a folder sync
        /// </summary>
        public DelegateCommand EditCommand
        {
            get
            {
                if (_editCommand == null)
                    _editCommand = new DelegateCommand(
                        obj => EditFolderSync(),
                        obj => SelectedFolderSync != null,
                        this);
                return _editCommand;
            }
        }
        DelegateCommand _editCommand;

        /// <summary>
        /// Gets a command for removing a folder sync
        /// </summary>
        public DelegateCommand RemoveCommand
        {
            get
            {
                if (_removeCommand == null)
                    _removeCommand = new DelegateCommand(
                        obj => RemoveFolderSync(),
                        obj => SelectedFolderSync != null,
                        this);
                return _removeCommand;
            }
        }
        DelegateCommand _removeCommand;

        #endregion

        #region Constructors

        public MainViewModel(IUnityContainer container)
            : base(container) { }

        #endregion

        #region Methods

        private void AddFolderSync()
        {
            FolderSyncViewModel folderSyncViewModel = Container.Resolve<FolderSyncViewModel>();
            folderSyncViewModel.Modal = true;

            folderSyncViewModel.Close += () =>
            {
                if (!string.IsNullOrEmpty(folderSyncViewModel.SourceFolder) && !string.IsNullOrEmpty(folderSyncViewModel.SourceFolder))
                    FolderSyncs.Add(folderSyncViewModel);
            };

            Container.Resolve<IDisplayService>().Display(folderSyncViewModel.CreateCopy());
            if (folderSyncViewModel.Result.HasValue && folderSyncViewModel.Result.Value)
                FolderSyncs.Add(folderSyncViewModel);
        }

        private void EditFolderSync()
        {
            SelectedFolderSync.Modal = true;
            Container.Resolve<IDisplayService>().Display(SelectedFolderSync.CreateCopy());
        }

        private void RemoveFolderSync()
        {
            if (SelectedFolderSync != null)
                FolderSyncs.Remove(SelectedFolderSync);
        }

        #endregion
    }
}
