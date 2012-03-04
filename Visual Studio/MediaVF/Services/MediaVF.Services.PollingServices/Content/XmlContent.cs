using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

//using MediaVF.Services.Data.Utilities;

namespace MediaVF.Services.Polling.Content
{
    [ContentType("text/xml")]
    public class XmlContent : IContent<XmlDocument>
    {
        public XmlDocument Content { get; set; }

        public bool IsLoaded { get; set; }

        public void Load(object content)
        {
            Content = new XmlDocument();
            Content.LoadXml(content as string);
        }
    }
}
