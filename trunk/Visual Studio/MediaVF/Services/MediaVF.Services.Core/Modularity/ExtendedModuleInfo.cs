using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Prism.Modularity;

namespace MediaVF.Services.Core.Modularity
{
    [Serializable]
    public class ExtendedModuleInfo : ModuleInfo
    {
        public int ModuleID { get; set; }
    }
}
