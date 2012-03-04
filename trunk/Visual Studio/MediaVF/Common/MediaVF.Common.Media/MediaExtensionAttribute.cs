using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Common.Media
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MediaExtensionAttribute : Attribute
    {
        #region Properties

        public string FileExtension { get; private set; }

        #endregion

        #region Constructors

        public MediaExtensionAttribute(string fileExtension)
        {
            FileExtension = fileExtension;
        }

        #endregion
    }
}
