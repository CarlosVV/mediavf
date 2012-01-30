using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

using Microsoft.Practices.Prism.Logging;

namespace MediaVF.Services.Logging
{
    public interface IComboLog : ILog, ILoggerFacade
    {
    }
}
