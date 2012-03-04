using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MediaVF.Common.Media.Audio
{
    [Serializable]
    public class Album
    {
        public Album(string album)
        {
            Name = album;
        }

        public void AddFile(AudioFile mf)
        {
            mf.Parent = this;
            Files.Add(mf);
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Artist Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public List<AudioFile> Files
        {
            get
            {
                if (_files == null)
                    _files = new List<AudioFile>();

                return _files;
            }
            set { _files = value; }
        }

        private string _name = string.Empty;
        private Artist _parent = null;
        private List<AudioFile> _files = null;
    }
}
