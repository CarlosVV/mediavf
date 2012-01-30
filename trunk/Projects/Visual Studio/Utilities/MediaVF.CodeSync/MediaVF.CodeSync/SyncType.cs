using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.CodeSync
{
    public enum SyncType
    {
        NeverOverwrite,
        OverwriteIfNewer,
        AlwaysOverwrite
    }
}
