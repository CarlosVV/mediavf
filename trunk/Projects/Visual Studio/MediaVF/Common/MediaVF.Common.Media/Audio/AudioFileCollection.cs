using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Common.Media.Audio
{
    public class AudioFileCollection : IEnumerable<AudioFile>
    {
        #region Properties

        /// <summary>
        /// Gets the parent media folder to which this audio collection belongs
        /// </summary>
        public MediaFolder Parent { get; set; }

        /// <summary>
        /// Gets a list of the audio files in the collection
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
        /// Gets a collection of artist objects by artist name
        /// </summary>
        Dictionary<string, Artist> _artists;
        Dictionary<string, Artist> Artists
        {
            get
            {
                if (_artists == null)
                    _artists = new Dictionary<string, Artist>();
                return _artists;
            }
        }

        #endregion

        #region Methods

        public void Add(AudioFile audioFile)
        {
            AudioFiles.Add(audioFile);

            // add artist to folder if we don't already have it
            if (!Artists.ContainsKey(audioFile.Artist))
                Artists.Add(audioFile.Artist, new Artist(this, audioFile.Artist));

            // add album to artist if we don't already have it
            if (!Artists[audioFile.Artist].Albums.ContainsKey(audioFile.Album))
                Artists[audioFile.Artist].AddAlbum(audioFile.Album);

            // add file to album
            Artists[audioFile.Artist].Albums[audioFile.Album].AddFile(audioFile);
        }

        public AudioFileCollection Copy()
        {
            AudioFileCollection newFileCollection = new AudioFileCollection();

            AudioFiles.ForEach(audioFile =>
            {
                newFileCollection.Add(audioFile.Copy());
            });

            return newFileCollection;
        }

        #endregion

        #region IEnumerable Implementation

        public IEnumerator<AudioFile> GetEnumerator()
        {
            return AudioFiles.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return AudioFiles.GetEnumerator();
        }

        #endregion
    }
}
