using System;
using System.Windows.Forms;

namespace AutoTrade.Core.UI
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
        public string SelectFolder(string description,
            Environment.SpecialFolder rootFolder = Environment.SpecialFolder.Desktop,
            bool allowNewFolders = false)
        {
            // create folder browser and set properties
            var folderBrowser = new FolderBrowserDialog
                {
                    Description = description,
                    RootFolder = rootFolder,
                    ShowNewFolderButton = allowNewFolders
                };

            // show the folder browser and return the result
            var result = folderBrowser.ShowDialog();
            
            return result == DialogResult.OK || result == DialogResult.Yes ? folderBrowser.SelectedPath : string.Empty;
        }
    }
}
