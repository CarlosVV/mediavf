using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Polling.Processing
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ContentProcessorAttribute : Attribute
    {
        public Type ContentType { get; private set; }

        public ContentProcessorAttribute(Type contentType)
        {
            ContentType = contentType;
        }
    }
}
