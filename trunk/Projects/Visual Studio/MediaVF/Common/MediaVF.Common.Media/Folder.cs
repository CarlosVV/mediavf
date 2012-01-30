using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Common.Media
{
    [Serializable]
    public class Folder
    {
        /// <summary>
        /// Gets or sets the path of the folder
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the child folders
        /// </summary>
        List<Folder> _folders;
        public List<Folder> Folders
        {
            get
            {
                if (_folders == null)
                    _folders = new List<Folder>();
                return _folders;
            }
            set { _folders = value; }
        }

        /// <summary>
        /// Gets or sets the list of media files in the folder
        /// </summary>
        List<MediaFile> _files;
        public List<MediaFile> Files
        {
            get
            {
                if (_files == null)
                    _files = new List<MediaFile>();
                return _files;
            }
            set { _files = value; }
        }

        /// <summary>
        /// Instantiates the folder with the given path
        /// </summary>
        /// <param name="path"></param>
        public Folder(string path)
        {
            Path = path;
        }
    }
}
