using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Polling.Matching
{
    public interface ITextMatcher
    {
    }

    public interface ITextMatcher<T> : ITextMatcher
    {
        void Initialize();

        List<T> GetMatches(string text);
    }
}
