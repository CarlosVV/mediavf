using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.UI.Core
{
    public interface IDialogService
    {
        /// <summary>
        /// Shows a folder dialog
        /// </summary>
        /// <param name="description"></param>
        /// <param name="rootFolder"></param>
        /// <param name="allowNewFolders"></param>
        /// <returns></returns>
        string ShowFolderDialog(string description, Environment.SpecialFolder rootFolder, bool allowNewFolders);
    }
}
