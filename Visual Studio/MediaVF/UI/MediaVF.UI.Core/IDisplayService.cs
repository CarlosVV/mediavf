using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Unity;

namespace MediaVF.UI.Core
{
    public interface IDisplayService
    {
        IUnityContainer Container { get; }

        void Display(ViewModelBase viewModelBase);

        void Display<T>() where T : ViewModelBase;
    }
}
