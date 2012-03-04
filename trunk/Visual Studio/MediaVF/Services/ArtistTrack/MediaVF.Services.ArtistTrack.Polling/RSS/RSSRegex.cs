using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MediaVF.Entities.ArtistTrack;
using MediaVF.Services.Polling.Matching;

namespace MediaVF.Services.ArtistTrack.Polling.RSS
{
    public class RSSRegex : IRegex
    {
        Regex Regex { get; set; }

        public RSSRegex(Regex regex)
        {
            Regex = regex;
        }

        public Dictionary<string, int> Captures
        {
            get { return Regex != null ? Regex.Captures.ToDictionary(c => c.Name, c => c.CaptureIndex) : null; }
        }

        public string RegexPattern
        {
            get { return Regex != null ? Regex.RegexPattern : string.Empty; }
        }
    }
}
