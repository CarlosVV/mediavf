using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaVF.UI.Core;
using System.Configuration;

namespace MediaVF.CodeSync
{
    public class FileTypeViewModel : ViewModelBase
    {
        #region Static

        /// <summary>
        /// Gets the file types from configuration
        /// </summary>
        public static List<FileTypeViewModel> GetFileTypes()
        {
            List<FileTypeViewModel> fileTypes = new List<FileTypeViewModel>();

            // get config section
            FileTypeConfiguration config = (FileTypeConfiguration)ConfigurationManager.GetSection("fileTypeConfiguration");
            if (config != null && config.FileTypes != null)
            {
                foreach (FileTypeElement fileTypeElement in config.FileTypes)
                {
                    // create file type from element
                    FileTypeViewModel fileType = new FileTypeViewModel();
                    fileType.FileTypeName = fileTypeElement.Name;

                    // add extensions for type
                    if (fileTypeElement.FileExtensions != null)
                        foreach (FileExtensionElement fileExt in fileTypeElement.FileExtensions)
                            fileType.FileExtensions.Add(fileExt.Extension);

                    fileTypes.Add(fileType);
                }
            }
            
            return fileTypes;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets flag indicating if this file type should be synced
        /// </summary>
        public bool ShouldSync
        {
            get { return _shouldSync; }
            set
            {
                if (_shouldSync != value)
                {
                    _shouldSync = value;

                    RaisePropertyChanged("ShouldSync");
                }
            }
        }
        bool _shouldSync;

        /// <summary>
        /// Gets or sets name of the file type
        /// </summary>
        public string FileTypeName
        {
            get { return _fileTypeName; }
            set
            {
                if (_fileTypeName != value)
                {
                    _fileTypeName = value;

                    RaisePropertyChanged("FileTypeName");
                }
            }
        }
        string _fileTypeName;

        /// <summary>
        /// Gets a list of file extensions for the type
        /// </summary>
        public List<string> FileExtensions
        {
            get
            {
                if (_fileExtensions == null)
                    _fileExtensions = new List<string>();
                return _fileExtensions;
            }
        }
        List<string> _fileExtensions;

        /// <summary>
        /// Gets the file extensions text
        /// </summary>
        public string FileExtensionsText
        {
            get
            {
                StringBuilder fileExtensionsText = new StringBuilder();

                foreach (string ext in FileExtensions)
                    fileExtensionsText.Append(fileExtensionsText.Length > 0 ? ", " : "").Append(ext);

                return fileExtensionsText.ToString();
            }
        }

        #endregion
    }
}
