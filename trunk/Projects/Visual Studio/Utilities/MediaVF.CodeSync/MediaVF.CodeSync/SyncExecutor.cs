using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace MediaVF.CodeSync
{
    public class SyncExecutor
    {
        #region Events

        /// <summary>
        /// Event raised when sync starts
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Raises the Started event
        /// </summary>
        private void RaiseStarted()
        {
            if (Started != null)
                Started(this, EventArgs.Empty);
        }

        /// <summary>
        /// Event raised when progress changes
        /// </summary>
        public event EventHandler ProgressChanged;

        /// <summary>
        /// Raises the ProgressChanged event
        /// </summary>
        private void RaiseProgressChanged()
        {
            if (ProgressChanged != null)
                ProgressChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Event raised when sync is completed
        /// </summary>
        public event EventHandler Completed;

        /// <summary>
        /// Raises the Completed event
        /// </summary>
        private void RaiseCompleted()
        {
            if (Completed != null)
                Completed(this, EventArgs.Empty);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the sync data
        /// </summary>
        FolderSyncViewModel FolderSyncData { get; set; }

        /// <summary>
        /// Gets the number of files synced so far
        /// </summary>
        public int NumberOfSyncedFolders { get; private set; }

        /// <summary>
        /// Gets the number of files to be synced
        /// </summary>
        public int NumberOfFoldersToSync { get; private set; }

        /// <summary>
        /// Gets the worker for performing sync in the background
        /// </summary>
        BackgroundWorker SyncWorker
        {
            get
            {
                if (_syncWorker == null)
                {
                    _syncWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };

                    _syncWorker.DoWork += new DoWorkEventHandler(OnStarted);
                    _syncWorker.ProgressChanged += new ProgressChangedEventHandler(OnProgressChanged);
                    _syncWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnCompleted);
                }

                return _syncWorker;
            }
        }
        BackgroundWorker _syncWorker;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a sync executor with sync data
        /// </summary>
        /// <param name="folderSyncData"></param>
        public SyncExecutor(FolderSyncViewModel folderSyncData)
        {
            FolderSyncData = folderSyncData;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Syncs folders
        /// </summary>
        public void Sync()
        {
            // validate data
            if (FolderSyncData == null)
                throw new Exception("Sync data is null.");
            if (string.IsNullOrEmpty(FolderSyncData.SourceFolder))
                throw new Exception("Source folder not provided.");
            if (string.IsNullOrEmpty(FolderSyncData.SourceFolder))
                throw new Exception("Source folder not provided.");
            if (!Directory.Exists(FolderSyncData.DestinationFolder))
                throw new Exception("Destination folder not found.");
            if (!Directory.Exists(FolderSyncData.DestinationFolder))
                throw new Exception("Destination folder not found.");
        }

        /// <summary>
        /// Syncs a source and destination folder
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destinationFolder"></param>
        private void SyncFolders(string sourceFolder, string destinationFolder)
        {
            List<string> sourceFiles = Directory.GetFiles(sourceFolder).ToList();
            List<string> destinationFiles = Directory.GetFiles(destinationFolder).ToList();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles starting the worker thread to sync
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStarted(object sender, DoWorkEventArgs e)
        {
            //GetSubFolderCount(FolderSyncData

            // raise started event
            RaiseStarted();
        }

        /// <summary>
        /// Handles progress changes from the sync worker thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // raise progress changed event
            RaiseProgressChanged();
        }

        /// <summary>
        /// Handles completion of the sync worker thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // raise completed event
            RaiseCompleted();
        }

        #endregion
    }
}
