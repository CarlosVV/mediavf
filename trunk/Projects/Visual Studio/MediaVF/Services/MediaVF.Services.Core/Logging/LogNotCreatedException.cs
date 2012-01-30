using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Core.Logging
{
    public class LogNotCreatedException : Exception
    {
        public override string Message
        {
            get { return "An attempt was made to access the log before it was created."; }
        }
    }
}
