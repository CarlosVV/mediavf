using Microsoft.Practices.Prism.Logging;
using log4net;

namespace AutoTrade.Core.Logging
{
    class Log4NetWrapper : ILoggerFacade
    {
        #region Fields

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILog _logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a <see cref="Log4NetWrapper"/>
        /// </summary>
        /// <param name="logger"></param>
        public Log4NetWrapper(ILog logger)
        {
            _logger = logger;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Logs a message using log4net
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="priority"></param>
        public void Log(string message, Category category, Priority priority)
        {
            switch (category)
            {
                case Category.Debug:
                    _logger.Debug(message);
                    break;
                case Category.Info:
                    _logger.Info(message);
                    break;
                case Category.Warn:
                    _logger.Warn(message);
                    break;
                case Category.Exception:
                    _logger.Error(message);
                    break;
            }
        }

        #endregion
    }
}
