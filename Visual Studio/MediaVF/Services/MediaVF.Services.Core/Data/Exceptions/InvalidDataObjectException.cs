using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Core.Data
{
    public class InvalidDataObjectException : Exception
    {
        Type _t;
        string _innerMessage;

        public override string Message
        {
            get
            {
                return string.Format("Type {0} is not a valid DataObject{1}{2}.",
                    _t.FullName,
                    !string.IsNullOrEmpty(_innerMessage) ? ": " : "",
                    _innerMessage);
            }
        }

        public InvalidDataObjectException(Type t)
        {
            _t = t;
        }

        public InvalidDataObjectException(Type t, string message)
            : this(t)
        {
            _innerMessage = message;
        }
    }
}
