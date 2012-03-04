using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using TagLib;

namespace MediaVF.Common.Media.Audio
{
    [Serializable]
    [MediaExtension(".mp3")]
    public class MP3File : AudioFile
    {
        #region Constructor

        public MP3File()
            : base(string.Empty) { }

        public MP3File(string path)
            : base(path) { }

        #endregion

        #region Populate

        protected override AudioFile Create()
        {
            return new MP3File();
        }

        /// <summary>
        /// Populates the object with metadata from the taglib file object
        /// </summary>
        protected override void PopulateMediaMetadata()
        {
            if (!string.IsNullOrEmpty(Path))
            {
                // try to create the file object
                TagLib.File file = null;
                try
                {
                    file = TagLib.File.Create(Path);
                }
                catch { }

                // if the file obj was successfully created, get data from it
                if (file != null)
                {
                    Artists.AddRange(file.Tag.AlbumArtists);
                    Album = file.Tag.Album;
                    Genres.AddRange(file.Tag.Genres);
                    Composers.AddRange(file.Tag.Composers);
                    
                    Title = file.Tag.Title;
                    Year = file.Tag.Year;
                    Track = file.Tag.Track;
                    Disc = file.Tag.Disc;
                    Duration = file.Properties.Duration;
                    Lyrics = file.Tag.Lyrics;
                    Comment = file.Tag.Comment;
                }
            }
        }

        #endregion Constructors
    }
}
