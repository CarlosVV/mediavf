using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MediaVF.Common.Media.Audio
{
    [Serializable]
    public class Artist
    {
        #region Constructors

        /// <summary>
        /// Instantiates an artist with the given parent collection and name
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="artist"></param>
        public Artist(AudioFileCollection parent, string artist)
        {
            Parent = parent;
            Name = artist;
        }

        #endregion

        #region AddAlbum

        public void AddAlbum(string strAlbum)
        {
            Album album = new Album(strAlbum);
            album.Parent = this;
            Albums.Add(album.Name,
                album);
        }

        #endregion AddAlbum

        #region Properties

        /// <summary>
        /// Gets or sets the parent collection for this artist
        /// </summary>
        public AudioFileCollection Parent { get; private set; }

        /// <summary>
        /// Gets or sets the name of the artist
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets a collection of albums by name
        /// </summary>
        Dictionary<string, Album> _albums;
        public Dictionary<string, Album> Albums
        {
            get
            {
                if (_albums == null)
                    _albums = new Dictionary<string, Album>();
                return _albums;
            }
            set { _albums = value; }
        }

        #endregion
    }
}
