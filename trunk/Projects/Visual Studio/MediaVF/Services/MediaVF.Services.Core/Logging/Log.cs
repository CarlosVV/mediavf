using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Unity;

using log4net;
using log4net.Core;

using MediaVF.Services.Core.Properties;

namespace MediaVF.Services.Core.Logging
{
    public class NetLogger : IComboLog
    {
        #region Properties

        ILog _underlyingLog;
        ILog UnderlyingLog
        {
            get
            {
                if (_underlyingLog == null)
                    _underlyingLog = LogManager.GetLogger(Resources.LogName);
                return _underlyingLog;
            }
        }

        public ILogger Logger
        {
            get { return UnderlyingLog.Logger; }
        }

        #endregion Properties

        #region Fatal

        public bool IsFatalEnabled
        {
            get { return UnderlyingLog.IsFatalEnabled; }
        }

        public void Fatal(object message, Exception exception)
        {
            UnderlyingLog.Fatal(message, exception);
        }

        public void Fatal(object message)
        {
            UnderlyingLog.Fatal(message);
        }

        #region FatalFormat

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            UnderlyingLog.FatalFormat(provider, format, args);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            UnderlyingLog.FatalFormat(format, arg0, arg1, arg2);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            UnderlyingLog.FatalFormat(format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0)
        {
            UnderlyingLog.FatalFormat(format, arg0);
        }

        public void FatalFormat(string format, params object[] args)
        {
            UnderlyingLog.FatalFormat(format, args);
        }

        #endregion FatalFormat

        #endregion Fatal

        #region Error

        public bool IsErrorEnabled
        {
            get { return UnderlyingLog.IsErrorEnabled; }
        }

        public void Error(object message, Exception exception)
        {
            UnderlyingLog.Error(message, exception);
        }

        public void Error(object message)
        {
            UnderlyingLog.Error(message);
        }

        #region ErrorFormat

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            UnderlyingLog.ErrorFormat(provider, format, args);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            UnderlyingLog.ErrorFormat(format, arg0, arg1, arg2);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            UnderlyingLog.ErrorFormat(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0)
        {
            UnderlyingLog.ErrorFormat(format, arg0);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            UnderlyingLog.ErrorFormat(format, args);
        }

        #endregion ErrorFormat

        #endregion Error

        #region Warn

        public bool IsWarnEnabled
        {
            get { return UnderlyingLog.IsWarnEnabled; }
        }

        public void Warn(object message, Exception exception)
        {
            UnderlyingLog.Warn(message, exception);
        }

        public void Warn(object message)
        {
            UnderlyingLog.Warn(message);
        }

        #region WarnFormat

        public void WarnFormat(string format, object arg0)
        {
            UnderlyingLog.WarnFormat(format, arg0);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            UnderlyingLog.WarnFormat(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            UnderlyingLog.WarnFormat(format, arg0, arg1, arg2);
        }

        public void WarnFormat(string format, params object[] args)
        {
            UnderlyingLog.WarnFormat(format, args);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            UnderlyingLog.WarnFormat(provider, format, args);
        }

        #endregion WarnFormat

        #endregion Warn

        #region Info

        public bool IsInfoEnabled
        {
            get { return UnderlyingLog.IsInfoEnabled; }
        }

        public void Info(object message, Exception exception)
        {
            UnderlyingLog.Info(message, exception);
        }

        public void Info(object message)
        {
            UnderlyingLog.Info(message);
        }

        #region InfoFormat

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            UnderlyingLog.InfoFormat(provider, format, args);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            UnderlyingLog.InfoFormat(format, arg0, arg1, arg2);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            UnderlyingLog.InfoFormat(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0)
        {
            UnderlyingLog.InfoFormat(format, arg0);
        }

        public void InfoFormat(string format, params object[] args)
        {
            UnderlyingLog.InfoFormat(format, args);
        }

        #endregion InfoFormat

        #endregion Info

        #region Debug

        public bool IsDebugEnabled
        {
            get { return UnderlyingLog.IsDebugEnabled; }
        }

        public void Debug(object message, Exception exception)
        {
            UnderlyingLog.Debug(message, exception);
        }

        public void Debug(object message)
        {
            UnderlyingLog.Debug(message);
        }

        #region DebugFormat

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            UnderlyingLog.DebugFormat(provider, format, args);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            UnderlyingLog.DebugFormat(format, arg0, arg1, arg2);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            UnderlyingLog.DebugFormat(format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0)
        {
            UnderlyingLog.DebugFormat(format, arg0);
        }

        public void DebugFormat(string format, params object[] args)
        {
            UnderlyingLog.DebugFormat(format, args);
        }

        #endregion DebugFormat

        #endregion Debug

        #region ILoggerFacade

        /// <summary>
        /// Use underlying log4net log
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="priority"></param>
        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    UnderlyingLog.Debug(message);
                    break;

                case Category.Info:
                    UnderlyingLog.Info(message);
                    break;

                case Category.Warn:
                    UnderlyingLog.Warn(message);
                    break;

                case Category.Exception:
                    if (priority != Priority.High)
                        UnderlyingLog.Error(message);
                    else
                        UnderlyingLog.Fatal(message);
                    break;
            }
        }

        #endregion ILoggerFacade
    }
}
