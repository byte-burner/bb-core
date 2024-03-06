namespace net_iot_core.Shared.ResultHandling
{
    /// <summary>
    /// Represents the result of a service operation with success status and error handling capabilities.
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess => !IsError;

        /// <summary>
        /// Gets a value indicating whether the operation resulted in an error.
        /// </summary>
        public bool IsError => Errors.Any();

        public List<Error> Errors { get; set; }

        private static Dictionary<string, string> ErrorDictionary => ErrorManager.ErrorDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResult"/> class.
        /// </summary>
        public ServiceResult()
        {
            Errors = new List<Error>();     
        }

        /// <summary>
        /// Adds an error with the specified error code.
        /// </summary>
        /// <param name="code">The error code to add.</param>
        /// <exception cref="KeyNotFoundException">Thrown when no error message is found for the specified error code.</exception>
        public void Add(string code)
        {
            string message;

            try
            {
                message = ErrorDictionary[code];
            }
            catch(KeyNotFoundException)
            {
                throw new KeyNotFoundException($"An error message for key {code} must be added to the error dictionary");
            }

            Errors.Add(new Error(code, message));
        }

        /// <summary>
        /// Adds an error with the specified error code and formats the message using provided parameters.
        /// </summary>
        /// <param name="code">The error code to add.</param>
        /// <param name="paramList">Additional parameters used to format the error message.</param>
        /// <exception cref="KeyNotFoundException">Thrown when no error message is found for the specified error code.</exception>
        public void Add(string code, params string[] paramList)
        {
            string message;

            try
            {
                message = ErrorDictionary[code];
            }
            catch(KeyNotFoundException)
            {
                throw new KeyNotFoundException($"An error message for key {code} must be added to the error dictionary");
            }

            Errors.Add(new Error(code , String.Format(message, paramList)));
        }
    }

    /// <summary>
    /// Represents the result of a service operation with success status, error handling capabilities, and payload of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of payload associated with the service result.</typeparam>
    public class ServiceResult<T> : ServiceResult
    {
        /// <summary>
        /// Gets or sets the payload associated with the service result.
        /// </summary>
        public T? Payload { get; set; }
    }
}