using System;

namespace AutoTrade.Core.UI
{
    public interface IDialogService
    {
        /// <summary>
        /// Allows a user to select a folder
        /// </summary>
        /// <param name="description"></param>
        /// <param name="rootFolder"></param>
        /// <param name="allowNewFolders"></param>
        /// <returns></returns>
        string SelectFolder(string description,
            Environment.SpecialFolder rootFolder = Environment.SpecialFolder.Desktop,
            bool allowNewFolders = false);
    }
}
