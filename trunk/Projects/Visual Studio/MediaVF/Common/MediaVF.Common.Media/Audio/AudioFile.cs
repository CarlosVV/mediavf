using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Common.Media.Audio
{
    public abstract class AudioFile : MediaFile
    {
        #region Properties

        /// <summary>
        /// Gets or sets the parent album object
        /// </summary>
        public Album Parent { get; set; }

        /// <summary>
        /// Gets or sets the year the audio file was recorded
        /// </summary>
        public uint? Year { get; protected set; }

        /// <summary>
        /// Gets or sets the disc number of the audio file
        /// </summary>
        public uint? Disc { get; protected set; }

        /// <summary>
        /// Gets or sets the track number of the audio file
        /// </summary>
        public uint? Track { get; protected set; }

        /// <summary>
        /// Gets or sets the lyrics of the audio file
        /// </summary>
        public string Lyrics { get; protected set; }

        /// <summary>
        /// Gets or sets the comment on the audio file
        /// </summary>
        public string Comment { get; protected set; }

        /// <summary>
        /// Gets or sets the duration of the file
        /// </summary>
        public TimeSpan? Duration { get; protected set; }

        #region Collections

        #region Artists

        /// <summary>
        /// Gets a list of artists for the audio file
        /// </summary>
        List<string> _artists;
        public List<string> Artists
        {
            get
            {
                if (_artists == null)
                    _artists = new List<string>();
                return _artists;
            }
        }

        /// <summary>
        /// Gets the first artist in the list of artists
        /// </summary>
        public string Artist
        {
            get { return Artists.Count > 0 ? Artists[0] : MediaFile.Tags.NONE; }
            set
            {
                if (Artists.Count == 0)
                    Artists.Add(value);
                else
                    Artists[0] = value;
            }
        }

        #endregion

        #region Albums

        /// <summary>
        /// Gets the list of albums for this audio file
        /// </summary>
        List<string> _albums;
        public List<string> Albums
        {
            get
            {
                if (_albums == null)
                    _albums = new List<string>();
                return _albums;
            }
        }

        /// <summary>
        /// Gets or sets the primary album for this audio file
        /// </summary>
        public string Album
        {
            get { return Albums.Count > 0 ? Albums[0] : MediaFile.Tags.NONE; }
            set
            {
                if (Albums.Count == 0)
                    Albums.Add(value);
                else
                    Albums[0] = value;
            }
        }

        #endregion

        #region Genres

        /// <summary>
        /// Gets a list of genres for the audio file
        /// </summary>
        List<string> _genres;
        public List<string> Genres
        {
            get
            {
                if (_genres == null)
                    _genres = new List<string>();
                return _genres;
            }
        }

        /// <summary>
        /// Gets or sets the primary genre on the audio file
        /// </summary>
        public string Genre
        {
            get { return Genres.Count > 0 ? Genres[0] : MediaFile.Tags.NONE; }
            set
            {
                if (Genres.Count == 0)
                    Genres.Add(value);
                else
                    Genres[0] = value;
            }
        }

        #endregion

        #region Composers

        /// <summary>
        /// Gets the list of composers for this audio file
        /// </summary>
        List<string> _composers;
        public List<string> Composers
        {
            get
            {
                if (_composers == null)
                    _composers = new List<string>();
                return _composers;
            }
        }

        /// <summary>
        /// Gets or sets the primary composer on the audio file
        /// </summary>
        public string Composer
        {
            get { return Composers.Count > 0 ? Composers[0] : MediaFile.Tags.NONE; }
            set
            {
                if (Composers.Count == 0)
                    Composers.Add(value);
                else
                    Composers[0] = value;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new audio file wrapper for the given file path
        /// </summary>
        /// <param name="path"></param>
        public AudioFile(string path)
            : base(path) { }

        #endregion

        #region Methods

        protected abstract AudioFile Create();

        /// <summary>
        /// Copies data from one audio file wrapper to another
        /// </summary>
        /// <param name="mf"></param>
        public AudioFile Copy()
        {
            AudioFile audioFile = Create();

            audioFile.Artists.AddRange(Artists);

            audioFile.Albums.AddRange(Albums);

            audioFile.Genres.AddRange(Genres);

            audioFile.Composers.AddRange(Composers);

            audioFile.Title = Title;
            audioFile.Year = Year;
            audioFile.Track = Track;
            audioFile.Disc = Disc;
            audioFile.Duration = Duration;
            audioFile.Lyrics = Lyrics;
            audioFile.Comment = Comment;

            return audioFile;
        }

        #endregion
    }
}
