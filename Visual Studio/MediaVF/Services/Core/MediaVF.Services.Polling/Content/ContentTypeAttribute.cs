using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Polling.Content
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContentTypeAttribute : Attribute
    {
        public string ContentType { get; private set; }

        public ContentTypeAttribute(string contentType)
        {
            ContentType = contentType;
        }
    }
}
