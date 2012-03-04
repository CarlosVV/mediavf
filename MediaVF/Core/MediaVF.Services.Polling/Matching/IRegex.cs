using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Polling.Matching
{
    /// <summary>
    /// Represents an interface for an object that holds a regex pattern
    /// </summary>
    public interface IRegex
    {
        /// <summary>
        /// Implemented to get a regex pattern for matching from a regex object
        /// </summary>
        string RegexPattern { get; }

        /// <summary>
        /// Implemented to get a list of captures in the regex pattern
        /// </summary>
        Dictionary<string, int> Captures { get; }
    }
}
