using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Timers;
using MediaVF.UI.Core;
using System.Xml;
using System.Reflection;
using MediaVF.Common.Utilities;

namespace MediaVF.Zune
{
    public class PlaylistGenerator
    {
        #region Properties

        /// <summary>
        /// Gets the name of the playlist creator
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets flag indicating if the generator is running
        /// </summary>
        public bool IsRunning { get; private set; }

        #region Playlist Creation

        /// <summary>
        /// Gets or sets the directory to which to write playlists
        /// </summary>
        public string OutputFolder { get; private set; }

        /// <summary>
        /// Gets or sets the interval at which to reset the playlist
        /// </summary>
        public TimeSpan PlaylistCreationInterval { get; private set; }

        /// <summary>
        /// Gets or sets the time the playlist was created
        /// </summary>
        public DateTime PlaylistCreated { get; private set; }

        /// <summary>
        /// Gets or sets the current playlist
        /// </summary>
        public ZunePlaylist CurrentPlaylist { get; private set; }

        #endregion

        #region Scanning

        /// <summary>
        /// Gets or sets the directory to scan for new files
        /// </summary>
        public string MediaFolder { get; private set; }

        /// <summary>
        /// Gets or sets the interval at which to check the folder for new files
        /// </summary>
        public TimeSpan ScanInterval { get; private set; }

        /// <summary>
        /// Gets or sets the last time a scan was run
        /// </summary>
        public DateTime LastScan { get; private set; }

        /// <summary>
        /// Gets the timer that runs scan at regular intervals
        /// </summary>
        Timer _scanTimer;
        Timer ScanTimer
        {
            get
            {
                if (_scanTimer == null)
                {
                    // create time and set to scan at regular intervals
                    _scanTimer = new Timer(ScanInterval.TotalMilliseconds);
                    _scanTimer.Elapsed += (sender, e) => Scan();
                }

                return _scanTimer;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Create

        /// <summary>
        /// Creates an PlaylistGenerator with the given media and output folders, and scan and playlist creation intervals
        /// </summary>
        /// <param name="mediaFolder"></param>
        /// <param name="outputFolder"></param>
        /// <param name="scanInterval"></param>
        /// <param name="playlistCreationInterval"></param>
        /// <returns></returns>
        public static PlaylistGenerator Create(string name,
            string mediaFolder,
            string outputFolder,
            TimeSpan scanInterval,
            TimeSpan playlistCreationInterval)
        {
            // create and populate dictionary of property values
            Dictionary<string, object> propertyValues = new Dictionary<string,object>();
            propertyValues.Add("Name", name);
            propertyValues.Add("MediaFolder", mediaFolder);
            propertyValues.Add("OutputFolder", outputFolder);
            propertyValues.Add("ScanInterval", scanInterval);
            propertyValues.Add("PlaylistCreationInterval", playlistCreationInterval);

            return Create(propertyValues, false);
        }

        /// <summary>
        /// Creates an PlaylistGenerator from a list property values
        /// </summary>
        /// <param name="propValues">The list of property values</param>
        /// <param name="valuesAsText">Flag indicating if the values have been provided as text and must be parsed</param>
        /// <returns>An PlaylistGenerator</returns>
        private static PlaylistGenerator Create(Dictionary<string, object> propValues, bool valuesAsText)
        {
            // create variables to hold data from xml
            PlaylistGenerator playlistCreator = new PlaylistGenerator();

            foreach (string propName in propValues.Keys)
            {
                PropertyInfo prop = typeof(PlaylistGenerator).GetProperty(propName);
                if (prop != null)
                {
                    object propValue = propValues[propName];
                    bool setValue = true;
                    if (valuesAsText)
                        setValue = StringParser.TryParse(prop.PropertyType, (string)propValue, out propValue);

                    if (setValue)
                        prop.SetValue(playlistCreator, propValue, null);
                }
            }

            // check that directory exists
            if (!Directory.Exists(playlistCreator.MediaFolder))
                throw new DirectoryNotFoundException(playlistCreator.MediaFolder);

            // set output folder
            if (!Directory.Exists(playlistCreator.OutputFolder))
                Directory.CreateDirectory(playlistCreator.OutputFolder);

            return playlistCreator;
        }

        #endregion

        #region Start/Stop

        /// <summary>
        /// Starts the playlist creator
        /// </summary>
        public void Start()
        {
            // set flag
            IsRunning = true;

            // start new playlist
            StartNewPlaylist();

            // start scanning for changes
            ScanTimer.Start();
        }

        /// <summary>
        /// Stops playlist creation
        /// </summary>
        public void Stop()
        {
            // stop scanning
            ScanTimer.Stop();

            // save current playlist
            if (CurrentPlaylist != null)
                CurrentPlaylist.SavePlaylist(OutputFolder);

            IsRunning = false;
        }

        #endregion

        #region Playlist Management

        /// <summary>
        /// Starts a new playlist by saving the current and creating new
        /// </summary>
        public void StartNewPlaylist()
        {
            // save playlist
            if (CurrentPlaylist != null && !string.IsNullOrEmpty(OutputFolder))
                CurrentPlaylist.SavePlaylist(OutputFolder);
            
            // create new
            CurrentPlaylist = new ZunePlaylist(DateTime.Now.ToString());

            // reset created time
            PlaylistCreated = DateTime.Now;
            LastScan = DateTime.Now;
        }

        #endregion

        #region Scanning

        /// <summary>
        /// Scans the scan directory for new files and adds them to the playlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scan()
        {
            // reset playlist
            if (DateTime.Now.Subtract(PlaylistCreated) >= PlaylistCreationInterval)
                StartNewPlaylist();

            // get new files
            List<string> newFiles = GetNewFiles();

            // add new files to playlist
            if (newFiles != null && newFiles.Count > 0)
                newFiles.ForEach(nf => CurrentPlaylist.AddFileToPlaylist(nf));

            // set current scan time
            LastScan = DateTime.Now;
        }

        /// <summary>
        /// Gets list of new files
        /// </summary>
        /// <returns></returns>
        private List<string> GetNewFiles()
        {
            return GetNewFiles(MediaFolder);
        }

        /// <summary>
        /// Gets list of new files in a folder
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private List<string> GetNewFiles(string directory)
        {
            List<string> newFiles = new List<string>();

            // add files created since last scan
            foreach (string file in Directory.GetFiles(directory))
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.CreationTime > LastScan)
                    newFiles.Add(file);
            }

            // scan subdirectories
            foreach (string subdirectory in Directory.GetDirectories(MediaFolder))
                newFiles.AddRange(GetNewFiles(subdirectory));

            return newFiles;
        }

        #endregion

        #region Xml

        /// <summary>
        /// Creates PlaylistGenerator from XML
        /// </summary>
        /// <param name="playlistNode"></param>
        /// <returns></returns>
        public static PlaylistGenerator CreateFromXml(XmlElement playlistNode)
        {
            Dictionary<string, object> propertyValues = new Dictionary<string, object>();
            foreach (XmlAttribute attribute in ((XmlElement)playlistNode).Attributes)
                propertyValues.Add(attribute.Name, attribute.Value);

            return Create(propertyValues, true);
        }

        /// <summary>
        /// Creates a playlist generator node
        /// </summary>
        /// <param name="xmlDoc">The XmlDocument used to create the element</param>
        /// <returns>An xml element representing this playlist generator</returns>
        public XmlElement CreateXmlElement(XmlDocument xmlDoc)
        {
            XmlElement playlistElement = xmlDoc.CreateElement("playlistGenerator");

            // add attributes
            playlistElement.Attributes.Append(CreateAttribute(xmlDoc, "Name", Name));
            playlistElement.Attributes.Append(CreateAttribute(xmlDoc, "MediaFolder", MediaFolder));
            playlistElement.Attributes.Append(CreateAttribute(xmlDoc, "OutputFolder", OutputFolder));
            playlistElement.Attributes.Append(CreateAttribute(xmlDoc, "ScanInterval", ScanInterval.ToString()));
            playlistElement.Attributes.Append(CreateAttribute(xmlDoc, "PlaylistCreationInterval", PlaylistCreationInterval.ToString()));

            return playlistElement;
        }

        /// <summary>
        /// Creates an attribute with the given name and value
        /// </summary>
        /// <param name="xmlDoc">The XmlDocument used to create the attribute</param>
        /// <param name="name">The name of the attribute</param>
        /// <param name="value">The value of the attribute</param>
        /// <returns>A new attribute with name and value set</returns>
        private XmlAttribute CreateAttribute(XmlDocument xmlDoc, string name, string value)
        {
            XmlAttribute attribute = xmlDoc.CreateAttribute(name);
            attribute.Value = value;
            return attribute;
        }

        #endregion

        #endregion
    }
}
