using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MediaVF.Common.Media
{
    [Serializable]
    public class MediaCollection
    {
        #region Properties

        /// <summary>
        /// User name of the user who owns this collection
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Flag indicating if this collection is on a remote machine
        /// </summary>
        public bool Remote { get; set; }

        /// <summary>
        /// Hash of folders in this collection
        /// </summary>
        private Dictionary<string, MediaFolder> _folders = null;
        public Dictionary<string, MediaFolder> Folders
        {
            get
            {
                if (_folders == null)
                    _folders = new Dictionary<string, MediaFolder>();

                return _folders;
            }
            set { _folders = value; }
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Serialize a MediaCollection to a byte array
        /// </summary>
        /// <param name="mc"></param>
        /// <returns></returns>
        public static byte[] Serialize(MediaCollection mc)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, mc);

            ms.Position = 0;
            byte[] abyteRet = new byte[ms.Length];
            ms.Read(abyteRet, 0, abyteRet.Length);
            ms.Flush();
            ms.Close();

            return abyteRet;
        }

        /// <summary>
        /// Deserialize a byte array to a MediaCollection
        /// </summary>
        /// <param name="abyte"></param>
        /// <returns></returns>
        public static MediaCollection Deserialize(byte[] abyte)
        {
            MemoryStream ms = new MemoryStream(abyte);
            ms.Position = 0;

            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(ms);
            ms.Flush();
            ms.Close();

            return obj as MediaCollection;
        }

        #endregion Serialization

        #region Creating and Adding Folders

        /// <summary>
        /// Create media folder by recursing through the folders,
        /// catalogue files based on Artist, Album, etc, and add
        /// folder to the collection
        /// </summary>
        /// <param name="bw"></param>
        /// <param name="strRootFolder"></param>
        public void CreateNewMediaFolder(string directoryPath)
        {
            // add new object to collection
            AddMediaFolder(directoryPath);

            // create new object
            if (Folders.ContainsKey(directoryPath))
                Folders[directoryPath].Create();
        }

        /// <summary>
        /// Create an empty MediaFolder object and add it to the collection
        /// </summary>
        /// <param name="strFolderName"></param>
        private void AddMediaFolder(string folderName)
        {
            MediaFolder mediaFolder = new MediaFolder(folderName) { Parent = this };
            Folders.Add(mediaFolder.Path, mediaFolder);
        }

        #endregion

        #region Copy

        /// <summary>
        /// Create and return a copy of this MediaCollection
        /// </summary>
        /// <returns></returns>
        public MediaCollection Copy()
        {
            MediaCollection newCollection = new MediaCollection();
            newCollection.UserName = UserName;
            foreach (string mediaFolder in Folders.Keys)
            {
                newCollection.AddMediaFolder(mediaFolder);
                newCollection.Folders[mediaFolder].AudioFiles = Folders[mediaFolder].AudioFiles.Copy();
            }

            return newCollection;
        }

        #endregion Copy

        #region Get

        public MediaFile GetFileByPath(string path)
        {
            foreach (MediaFolder folder in Folders.Values)
                foreach (MediaFile mediaFile in folder.AllFiles)
                    if (mediaFile.Path == path)
                        return mediaFile;

            return null;
        }

        #endregion Get
    }
}
