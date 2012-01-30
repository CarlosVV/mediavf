using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaVF.UI.Core;
using Microsoft.Practices.Unity;

namespace MediaVF.CodeSync
{
    public class WindowViewModel : ContainerViewModel
    {
        /// <summary>
        /// Gets or sets flag indicating if window should be shown modally
        /// </summary>
        public bool Modal { get; set; }

        /// <summary>
        /// Gets or sets flag indicating the result of closing the view model
        /// </summary>
        public bool? Result { get; set; }

        public WindowViewModel(IUnityContainer container)
            : base(container) { }
    }
}
