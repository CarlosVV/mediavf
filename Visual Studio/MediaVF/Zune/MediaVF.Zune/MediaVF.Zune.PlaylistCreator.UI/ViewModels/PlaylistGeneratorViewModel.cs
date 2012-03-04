using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaVF.UI.Core;
using Microsoft.Practices.Unity;

namespace MediaVF.Zune.PlaylistCreator.UI.ViewModels
{
    public class PlaylistGeneratorViewModel : ViewModelBase, ISelectable
    {
        #region Enums

        public enum StatusType
        {
            New,
            Running,
            Stopped
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the playlist generator
        /// </summary>
        public PlaylistGenerator PlaylistGenerator { get; protected set; }

        /// <summary>
        /// Gets or sets flag indicating if the item is selected
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;

                    RaisePropertyChanged("IsSelected");
                }
            }
        }
        bool _isSelected;

        /// <summary>
        /// Gets the current status of the underlying playlist generator
        /// </summary>
        public StatusType Status
        {
            get
            {
                if (PlaylistGenerator == null)
                    return StatusType.New;
                else if (PlaylistGenerator.IsRunning)
                    return StatusType.Running;
                else
                    return StatusType.Stopped;
            }
        }

        #region Generator Values

        /// <summary>
        /// Gets or sets the name of the playlist generator
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;

                    RaisePropertyChanged("Name");
                }
            }
        }
        string _name;

        /// <summary>
        /// Gets or sets the media folder
        /// </summary>
        public string MediaFolder
        {
            get { return _mediaFolder; }
            set
            {
                if (_mediaFolder != value)
                {
                    _mediaFolder = value;

                    RaisePropertyChanged("MediaFolder");
                }
            }
        }
        string _mediaFolder;

        /// <summary>
        /// Gets or sets the output folder
        /// </summary>
        public string OutputFolder
        {
            get { return _outputFolder; }
            set
            {
                if (_outputFolder != value)
                {
                    _outputFolder = value;

                    RaisePropertyChanged("OutputFolder");
                }
            }
        }
        string _outputFolder;

        /// <summary>
        /// Gets or sets the scan interval
        /// </summary>
        public double ScanInterval
        {
            get { return _scanInterval; }
            set
            {
                if (_scanInterval != value)
                {
                    _scanInterval = value;

                    RaisePropertyChanged("ScanInterval");
                }
            }
        }
        double _scanInterval;

        /// <summary>
        /// Gets or sets the playlist creation interval
        /// </summary>
        public double PlaylistCreationInterval
        {
            get { return _playlistCreationInterval; }
            set
            {
                if (_playlistCreationInterval != value)
                {
                    _playlistCreationInterval = value;

                    RaisePropertyChanged("PlaylistCreationInterval");
                }
            }
        }
        double _playlistCreationInterval;

        #endregion

        #region Commands

        /// <summary>
        /// Gets the command for running the generator
        /// </summary>
        public DelegateCommand RunCommand
        {
            get
            {
                if (_runCommand == null)
                    _runCommand = new DelegateCommand(obj => StartGenerator(),
                        obj => Status == StatusType.Stopped,
                        this);
                return _runCommand;
            }
        }
        DelegateCommand _runCommand;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates the view model with an existing playlist generator
        /// </summary>
        /// <param name="container"></param>
        /// <param name="playlistGenerator"></param>
        public PlaylistGeneratorViewModel(IUnityContainer container, PlaylistGenerator playlistGenerator)
            : base(container)
        {
            PlaylistGenerator = playlistGenerator;

            if (PlaylistGenerator != null)
                SetValuesFromGenerator();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets values on the view model from the generator
        /// </summary>
        protected void SetValuesFromGenerator()
        {
            // if generator is not null, get property values from the generator
            if (PlaylistGenerator != null)
            {
                Name = PlaylistGenerator.Name;
                MediaFolder = PlaylistGenerator.MediaFolder;
                OutputFolder = PlaylistGenerator.OutputFolder;
                ScanInterval = (int)PlaylistGenerator.ScanInterval.TotalSeconds;
                PlaylistCreationInterval = (int)PlaylistGenerator.PlaylistCreationInterval.TotalSeconds;

                RaiseStatusChanged();
            }
        }

        /// <summary>
        /// Raises status change
        /// </summary>
        protected void RaiseStatusChanged()
        {
            // raise status change
            RaisePropertyChanged("Status");
        }

        /// <summary>
        /// Runs the generator and updates its status
        /// </summary>
        public void StartGenerator()
        {
            if (PlaylistGenerator != null)
            {
                // start the playlist generator
                PlaylistGenerator.Start();

                RaiseStatusChanged();
            }
        }

        /// <summary>
        /// Stops the generator and updates its status
        /// </summary>
        public void StopGenerator()
        {
            if (PlaylistGenerator != null)
            {
                PlaylistGenerator.Stop();

                RaiseStatusChanged();
            }
        }

        #endregion
    }
}
