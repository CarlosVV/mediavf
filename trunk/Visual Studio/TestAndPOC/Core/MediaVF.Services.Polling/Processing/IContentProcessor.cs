using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MediaVF.Services.Polling.Content;

namespace MediaVF.Services.Polling.Processing
{
    public interface IContentProcessor
    {
        void Initialize();

        void Process(List<IContent> content);
    }
}
