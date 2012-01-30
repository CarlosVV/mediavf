using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MediaVF.UI.Core
{
    public class DialogService : IDialogService
    {
        /// <summary>
        /// Shows a folder dialog and returns the path to the selected folder
        /// </summary>
        /// <param name="description">The description to show on the dialog</param>
        /// <param name="rootFolder">The root folder to show in the dialog</param>
        /// <param name="allowNewFolders">Flag indicating if the dialog should allow adding new folders</param>
        /// <returns>The path to the selected folder</returns>
        public string ShowFolderDialog(string description, Environment.SpecialFolder rootFolder, bool allowNewFolders)
        {
            // create folder browser and set properties
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = description;
            folderBrowser.RootFolder = rootFolder;
            folderBrowser.ShowNewFolderButton = allowNewFolders;

            // show the folder browser and return the result
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
                return folderBrowser.SelectedPath;
            else
                return string.Empty;
        }
    }
}
