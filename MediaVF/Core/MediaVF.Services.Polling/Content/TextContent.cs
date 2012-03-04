using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Polling.Content
{
    [ContentType("text")]
    public class TextContent : IContent<string>
    {
        public string Content { get; private set; }

        public bool IsLoaded { get; set; }

        public void Load(object content)
        {
            Content = content as string;
        }
    }
}
