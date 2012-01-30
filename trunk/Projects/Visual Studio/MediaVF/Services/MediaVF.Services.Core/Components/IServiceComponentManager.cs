using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MediaVF.Common.Modularity;

namespace MediaVF.Services.Core.Components
{
    public interface IServiceComponentManager : IModuleLoadListener
    {
        List<IServiceComponent> ServiceComponents { get; }

        void RunComponents();
    }
}
