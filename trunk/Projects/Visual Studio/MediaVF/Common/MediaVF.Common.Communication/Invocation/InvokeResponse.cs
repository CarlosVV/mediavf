using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using MediaVF.Common.Communication.Utilities;

namespace MediaVF.Common.Communication.Invocation
{
    /// <summary>
    /// Represents a response to an invocation request to an IInvocableService
    /// </summary>
    [DataContract]
    public class InvokeResponse
    {
        #region Properties

        /// <summary>
        /// Gets or sets the serialized result of the operation
        /// </summary>
        [DataMember]
        public string OperationResult { get; set; }

        /// <summary>
        /// Gets or sets the error, if any, that occurred during the operation
        /// </summary>
        [DataMember]
        public Exception Error { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Deserializes the operation result to the given type
        /// </summary>
        /// <typeparam name="T">The type to which to deserialize the operation result</typeparam>
        /// <returns>The deserialized operation result</returns>
        public T GetValue<T>()
        {
            return GetValue<T>(null);
        }

        /// <summary>
        /// Deserializes the operation result to the given type, using the given encryption key
        /// </summary>
        /// <typeparam name="T">The type to which to deserialize the result</typeparam>
        /// <param name="encryptionKey">The key to use to decrypt the serialized object before deserializing</param>
        /// <returns>The deserialized operation result</returns>
        public T GetValue<T>(string encryptionKey)
        {
            // deserialize to the given type
            return DataContractUtility.Deserialize<T>(OperationResult, encryptionKey);
        }

        #endregion
    }
}
