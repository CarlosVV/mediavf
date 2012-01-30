using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MediaVF.Common.Media.Audio;
using MediaVF.Common.Media;
using System.IO;

namespace MediaVF.Zune
{
    public class ZunePlaylist
    {
        #region Properties

        /// <summary>
        /// Gets or sets the playlist's unique id
        /// </summary>
        Guid _guid;
        Guid Guid
        {
            get
            {
                if (_guid == Guid.Empty)
                    _guid = Guid.NewGuid();
                return _guid;
            }
        }

        /// <summary>
        /// Gets or sets title of playlist
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets list of audio files in the playlist
        /// </summary>
        List<AudioFile> _audioFiles;
        List<AudioFile> AudioFiles
        {
            get
            {
                if (_audioFiles == null)
                    _audioFiles = new List<AudioFile>();
                return _audioFiles;
            }
        }

        /// <summary>
        /// Gets flag indicating if the playlist has any files
        /// </summary>
        public bool HasFiles { get { return AudioFiles.Any(); } }

        /// <summary>
        /// Gets the total duration of the playlist
        /// </summary>
        TimeSpan TotalDuration
        {
            get
            {
                // create new time span of 0
                TimeSpan totalDuration = new TimeSpan();

                // add all durations
                AudioFiles.ForEach(af => totalDuration = totalDuration.Add(af.Duration.HasValue ? af.Duration.Value : new TimeSpan()));

                // retun total
                return totalDuration;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a playlist with a title
        /// </summary>
        /// <param name="title"></param>
        public ZunePlaylist(string title)
        {
            // set title
            Title = title;
        }

        #endregion

        #region Methods

        #region Add
        
        /// <summary>
        /// Adds a file to the playlist
        /// </summary>
        /// <param name="filePath"></param>
        public void AddFileToPlaylist(string filePath)
        {
            // add file to end of playlist
            AddFileToPlaylist(filePath, AudioFiles.Count);
        }

        /// <summary>
        /// Adds a file to the playlist
        /// </summary>
        /// <param name="filePath"></param>
        public void AddFileToPlaylist(string filePath, int index)
        {
            // create audio file object
            AudioFile audioFile = MediaFile.GetMediaFileByFilename(filePath) as AudioFile;

            // if successful, add to collection
            if (audioFile != null)
            {
                // check if the file should be added to the end of the playlist or somewhere in the middle
                if (index < AudioFiles.Count)
                    AudioFiles.Insert(index, audioFile);
                else
                    AudioFiles.Add(audioFile);
            }
        }

        #endregion

        #region Save

        /// <summary>
        /// Adds a node to a parent node
        /// </summary>
        /// <param name="doc">The document used to create the attribute</param>
        /// <param name="parent">The parent node to which to add the element</param>
        /// <param name="name">The name of the element</param>
        /// <param name="value">The value of the element</param>
        /// <returns></returns>
        private XmlNode AddNode(XmlDocument doc, XmlNodeType nodeType, XmlNode parent, string name, string value)
        {
            // create the element
            XmlNode node = doc.CreateNode(nodeType, name, string.Empty);

            // set value
            if (!string.IsNullOrEmpty(value))
                node.Value = value;

            // add to parent node
            parent.AppendChild(node);

            // return the new node
            return node;
        }

        /// <summary>
        /// Adds a meta tag to the head tag of the playlist
        /// </summary>
        /// <param name="doc">The doc for the playlist xml</param>
        /// <param name="head">The head node</param>
        /// <param name="name">The name of the meta tag</param>
        /// <param name="content">The content of the meta tag</param>
        private void AddMetaTag(XmlDocument doc, XmlNode head, string name, string content)
        {
            // create meta tag
            XmlNode meta = AddNode(doc, XmlNodeType.Element, head, "meta", null);

            // set name and content attributes
            AddNode(doc, XmlNodeType.Element, meta, "name", name);
            AddNode(doc, XmlNodeType.Element, meta, "content", content);
        }

        /// <summary>
        /// Saves the playlist xml to a file
        /// </summary>
        /// <param name="filePath"></param>
        public void SavePlaylist(string filePath)
        {
            if (HasFiles)
            {
                XmlDocument xmlDoc = new XmlDocument();

                // add zpil processing instruction
                xmlDoc.AppendChild(xmlDoc.CreateProcessingInstruction("zpl", "version=\"2.0\""));

                // create root
                XmlNode smil = AddNode(xmlDoc, XmlNodeType.Element, xmlDoc, "smil", null);

                #region <head>

                // create head
                XmlNode head = AddNode(xmlDoc, XmlNodeType.Element, smil, "head", null);

                // add guid and title to head
                AddNode(xmlDoc, XmlNodeType.Element, head, "guid", string.Format("{{0}}", Guid.ToString()));
                AddNode(xmlDoc, XmlNodeType.Element, head, "title", Title);

                // add meta tags
                AddMetaTag(xmlDoc, head, "generator", "MediaVF Zune Playlist Creator");
                AddMetaTag(xmlDoc, head, "itemCount", AudioFiles.Count.ToString());
                AddMetaTag(xmlDoc, head, "totalDuration", TotalDuration.TotalMilliseconds.ToString());

                #endregion

                #region <body>

                // create body
                XmlNode body = AddNode(xmlDoc, XmlNodeType.Element, smil, "body", null);
                XmlNode seq = AddNode(xmlDoc, XmlNodeType.Element, body, "seq", null);

                // add audio files
                AudioFiles.ForEach(audioFile =>
                {
                    // create media node
                    XmlNode media = AddNode(xmlDoc, XmlNodeType.Element, seq, "media", null);

                    // add attributes
                    AddNode(xmlDoc, XmlNodeType.Attribute, media, "src", audioFile.Path);
                    AddNode(xmlDoc, XmlNodeType.Attribute, media, "albumTitle", audioFile.Album);
                    AddNode(xmlDoc, XmlNodeType.Attribute, media, "albumArtist", audioFile.Artist);
                    AddNode(xmlDoc, XmlNodeType.Attribute, media, "trackTitle", audioFile.Title);
                    AddNode(xmlDoc, XmlNodeType.Attribute, media, "trackArtist", audioFile.Artist);
                    AddNode(xmlDoc, XmlNodeType.Attribute, media, "duration", audioFile.Duration.HasValue ? audioFile.Duration.Value.TotalMilliseconds.ToString() : "0");
                });

                #endregion

                // save doc to path
                xmlDoc.Save(Path.Combine(filePath, Title, ".zpl"));
            }
        }

        #endregion

        #endregion
    }
}
