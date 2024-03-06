namespace net_iot_core.Services.DeviceProgramming.Exceptions
{
    /// <summary>
    /// Exception thrown when an I/O error occurs related to a bridge device.
    /// </summary>
    public class BridgeIOException : IOException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BridgeIOException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public BridgeIOException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BridgeIOException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public BridgeIOException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
