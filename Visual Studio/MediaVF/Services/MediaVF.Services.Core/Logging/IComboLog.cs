using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

using Microsoft.Practices.Prism.Logging;

namespace MediaVF.Services.Core.Logging
{
    public interface IComboLog : ILog, ILoggerFacade
    {
    }
}
