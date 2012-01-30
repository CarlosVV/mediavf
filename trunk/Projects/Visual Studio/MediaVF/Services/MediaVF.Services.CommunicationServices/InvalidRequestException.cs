using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.Services.Communication
{
    public class InvalidRequestException : Exception
    {
        public InvalidRequestException(string message)
            : base(message) { }

        public InvalidRequestException(string message, Exception ex)
            : base(message, ex) { }

        public override string Message
        {
            get { return string.Format("Invalid request: {0}", base.Message); }
        }
    }
}
