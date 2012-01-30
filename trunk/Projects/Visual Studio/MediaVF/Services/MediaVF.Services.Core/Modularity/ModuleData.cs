using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Core.Modularity
{
    [Serializable]
    public class ModuleData
    {
        public int ModuleID { get; set; }

        public string Class { get; set; }

        public string ModuleName { get; set; }

        public bool IsActive { get; set; }
    }
}
