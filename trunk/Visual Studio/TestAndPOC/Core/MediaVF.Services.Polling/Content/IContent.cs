using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Polling.Content
{
    public interface IContent
    {
        bool IsLoaded { get; set; }

        void Load(object content);
    }

    public interface IContent<T> : IContent
    {
        T Content { get; }
    }
}
