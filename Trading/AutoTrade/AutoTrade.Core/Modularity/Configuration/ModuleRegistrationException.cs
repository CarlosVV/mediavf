﻿using System;
using AutoTrade.Core.Properties;

namespace AutoTrade.Core.Modularity.Configuration
{
    class ModuleRegistrationException : Exception
    {
        /// <summary>
        /// The details of the module registration error
        /// </summary>
        private readonly string _details;

        /// <summary>
        /// Instantiates a <see cref="ModuleRegistrationException"/>
        /// </summary>
        /// <param name="details"></param>
        public ModuleRegistrationException(string details, params object[] args)
        {
            _details = string.Format(details, args);
        }

        /// <summary>
        /// Gets the message for the module registration error
        /// </summary>
        public override string Message
        {
            get { return string.Format(Resources.ModuleRegistrationsExceptionMessage, _details); }
        }
    }
}
