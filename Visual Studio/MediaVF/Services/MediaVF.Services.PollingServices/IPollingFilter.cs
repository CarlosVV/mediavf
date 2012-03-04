using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Polling
{
    public interface IPollingFilter<T>
    {
        bool PassesFilter(T item);
    }
}
