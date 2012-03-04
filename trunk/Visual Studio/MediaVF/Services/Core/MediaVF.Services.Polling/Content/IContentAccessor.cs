using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Polling.Content
{
    public interface IContentAccessor
    {
        bool RawContentIsPlainText { get; }

        object RawContent { get; }

        string ContentType { get; }
    }

    public interface IContentAccessor<T> : IContentAccessor
    {
        bool HasValidContent(T item);

        void Load(T item);
    }
}
