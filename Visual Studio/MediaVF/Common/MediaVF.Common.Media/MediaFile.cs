using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MediaVF.Common.Media
{
    public abstract class MediaFile
    {
        public class Tags
        {
            public const string NONE = "(none)";
            public const string ALL = "[ALL]";
        }

        #region Properties

        /// <summary>
        /// Gets or sets the path of the media file
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets or sets file info regarding the file
        /// </summary>
        public FileInfo FileInfo { get; private set; }

        /// <summary>
        /// Gets the file size from the file info
        /// </summary>
        public long FileSize
        {
            get { return FileInfo != null ? FileInfo.Length : 0; }
        }

        /// <summary>
        /// Gets the file name from the path
        /// </summary>
        public string FileName
        {
            get { return !string.IsNullOrEmpty(Path) ? Path.Substring(Path.LastIndexOf('\\') + 1) : string.Empty; }
        }

        /// <summary>
        /// Gets or sets the title of the media
        /// </summary>
        public string Title { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiates a media file wrapper for the given path
        /// </summary>
        /// <param name="path"></param>
        public MediaFile(string path)
        {
            Path = path;
            FileInfo = new FileInfo(path);

            PopulateMediaMetadata();
        }

        #endregion

        #region Static

        /// <summary>
        /// Gets a new media file object based on the path (and extension) of the file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static MediaFile GetMediaFileByFilename(string path)
        {
            // get the extension
            string extension = path.Substring(path.LastIndexOf('.') + 1);

            // get the type for the extension
            Type mediaFileType =
                Assembly.GetExecutingAssembly()
                        .GetTypes()
                        .FirstOrDefault(t => typeof(MediaFile).IsAssignableFrom(t) &&
                            t.GetCustomAttributes(typeof(MediaExtensionAttribute), true).Cast<MediaExtensionAttribute>().Any(mea => mea.FileExtension == extension));

            // return a new media file object of the given type, if found
            if (mediaFileType != null)
                return Activator.CreateInstance(mediaFileType, path) as MediaFile;
            else
                return null;
        }

        #endregion

        #region Abstract
        
        /// <summary>
        /// Allow derived classes to populate metadata from the file
        /// </summary>
        protected abstract void PopulateMediaMetadata();

        #endregion
    }
}
