using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using MediaVF.Common.Media.Audio;

namespace MediaVF.Common.Media
{
    [Serializable]
    public class MediaFolder
    {
        #region Constructors

        public MediaFolder(string path)
        {
            Path = path;
        }

        #endregion Constructors

        #region Create

        /// <summary>
        /// Create the folder by calling the recursive AddFolder
        /// and storing the results as the Root
        /// </summary>
        /// <param name="bw"></param>
        public void Create()
        {
            Root = AddFolder(Path);
        }

        /// <summary>
        /// Recurse through folders, storing data read from files
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="strPath"></param>
        /// <returns></returns>
        private Folder AddFolder(string path)
        {
            Folder f = new Folder(path);

            // do files
            string[] filenames = Directory.GetFiles(path);
            int fileCount = 0;
            foreach (string file in filenames)
            {
                // TODO: Check for wrapper type for extension
                MediaFile mediaFile = MediaFile.GetMediaFileByFilename(file);

                if (mediaFile != null)
                {
                    fileCount++;

                    f.Files.Add(mediaFile);

                    if (mediaFile is AudioFile)
                        AudioFiles.Add((AudioFile)mediaFile);
                    // HANDLE OTHER TYPES

                    //// report number of files found
                    //bw.ReportProgress(0, iMediaFileCount);
                }
            }

            // recurse through child folders
            string[] directories = Directory.GetDirectories(path);
            foreach (string directory in directories)
                f.Folders.Add(AddFolder(directory));

            return f;
        }

        #endregion Create

        #region AddArtist

        #endregion AddArtist

        #region Properties

        /// <summary>
        /// Gets or sets the path for the media folder
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the parent media collection
        /// </summary>
        public MediaCollection Parent { get; set; }

        /// <summary>
        /// Gets or sets the root folder
        /// </summary>
        public Folder Root { get; set; }

        public List<MediaFile> AllFiles
        {
            get
            {
                List<MediaFile> allFiles = new List<MediaFile>();

                // add audio files
                allFiles.AddRange(AudioFiles);

                // add additional types

                return allFiles;
            }
        }

        /// <summary>
        /// Gets or sets a collection of audio files contained
        /// </summary>
        AudioFileCollection _audioFiles;
        public AudioFileCollection AudioFiles
        {
            get
            {
                if (_audioFiles == null)
                    _audioFiles = new AudioFileCollection();
                return _audioFiles;
            }
            set { _audioFiles = value; }
        }

        #endregion
    }
}
