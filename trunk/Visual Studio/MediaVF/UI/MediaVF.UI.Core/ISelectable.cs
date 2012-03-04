using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.UI.Core
{
    public interface ISelectable
    {
        /// <summary>
        /// Gets or sets flag indicating if the object is selected
        /// </summary>
        bool IsSelected { get; set; }
    }
}
