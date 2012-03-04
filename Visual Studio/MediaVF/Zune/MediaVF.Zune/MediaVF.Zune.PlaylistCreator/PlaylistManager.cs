using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using MediaVF.Common.Utilities;
using System.Configuration;
using System.IO;

namespace MediaVF.Zune.PlaylistCreator
{
    public class PlaylistManager
    {
        #region Events

        public event Action<PlaylistGenerator> GeneratorAdded;

        private void RaiseGeneratorAdded(PlaylistGenerator generator)
        {
            if (GeneratorAdded != null)
                GeneratorAdded(generator);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of playlist creators
        /// </summary>
        Dictionary<string, PlaylistGenerator> PlaylistGenerators
        {
            get
            {
                if (_playlistGenerators == null)
                    _playlistGenerators = new Dictionary<string, PlaylistGenerator>();
                return _playlistGenerators;
            }
        }
        Dictionary<string, PlaylistGenerator> _playlistGenerators;

        #endregion

        #region Methods

        /// <summary>
        /// Loads playlist generators from xml
        /// </summary>
        public void Load(string xmlFile)
        {
            if (File.Exists(xmlFile))
            {
                // create doc and load
                XmlDocument playlistsXmlDoc = new XmlDocument();
                playlistsXmlDoc.Load(xmlFile);

                // get the parent node
                XmlNode parentNode = playlistsXmlDoc.ChildNodes.Cast<XmlNode>().FirstOrDefault();
                if (parentNode != null)
                {
                    // loop through playlist nodes in xml
                    foreach (XmlNode playlistNode in parentNode.ChildNodes)
                    {
                        // create playlist generator from node
                        PlaylistGenerator playlistGenerator =
                            PlaylistGenerator.CreateFromXml((XmlElement)playlistNode);

                        // add to list
                        PlaylistGenerators.Add(playlistGenerator.Name, playlistGenerator);
                    }
                }
            }
        }

        /// <summary>
        /// Saves playlist generators to xml
        /// </summary>
        public void Save(string xmlFile)
        {
            XmlDocument playlistsXmlDoc = new XmlDocument();

            // create root
            XmlElement root = playlistsXmlDoc.CreateElement("playlistGenerators");
            playlistsXmlDoc.AppendChild(root);

            // create generator elements
            foreach (PlaylistGenerator playlistGenerator in PlaylistGenerators.Values)
                root.AppendChild(playlistGenerator.CreateXmlElement(playlistsXmlDoc));

            // save the xml
            playlistsXmlDoc.Save(xmlFile);
        }

        /// <summary>
        /// Gets the generators in the manager
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PlaylistGenerator> GetPlaylistGenerators()
        {
            return PlaylistGenerators.Values;
        }

        /// <summary>
        /// Adds a new playlist creator to the list
        /// </summary>
        /// <param name="mediaFolder">The folder to watch for media files</param>
        /// <param name="outputFolder">The output folder for playlist files</param>
        /// <param name="scanInterval">The interval at which to scan the media folder</param>
        /// <param name="playlistCreationInterval">The interval at which to create new playlists</param>
        public PlaylistGenerator AddPlaylistGenerator(string name,
            string mediaFolder,
            string outputFolder,
            TimeSpan scanInterval,
            TimeSpan playlistCreationInterval)
        {
            // create the playlist generator
            PlaylistGenerator playlistGenerator = PlaylistGenerator.Create(name, mediaFolder, outputFolder, scanInterval, playlistCreationInterval);

            // add it to the list
            PlaylistGenerators.Add(name, playlistGenerator);

            // indicate that it's been added
            RaiseGeneratorAdded(playlistGenerator);

            return playlistGenerator;
        }

        /// <summary>
        /// Removes (and stops) a playlist creator
        /// </summary>
        /// <param name="name">The name of the playlist creator to remove</param>
        public void RemovePlaylistGenerator(string name)
        {
            if (!string.IsNullOrEmpty(name) && PlaylistGenerators.ContainsKey(name))
            {
                // stop playlist creation
                PlaylistGenerators[name].Stop();

                // remove from collection
                PlaylistGenerators.Remove(name);
            }
        }

        /// <summary>
        /// Starts playlist creation for all saved playlist creators
        /// </summary>
        public void Start()
        {
            // start all playlist creators
            foreach (PlaylistGenerator playlistCreator in PlaylistGenerators.Values)
                playlistCreator.Start();
        }

        /// <summary>
        /// Stops playlist creation for all saved playlist creators
        /// </summary>
        public void Stop()
        {
            // start all playlist creators
            foreach (PlaylistGenerator playlistCreator in PlaylistGenerators.Values)
                playlistCreator.Stop();
        }

        #endregion
    }
}
