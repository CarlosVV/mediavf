using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MediaVF.Services.Polling.Content;

namespace MediaVF.Services.Polling
{
    public interface IPollingEndpoint<T>
    {
        bool ProcessAsPlainText { get; set; }

        List<T> Poll();
    }
}
