namespace net_iot_core.Shared
{
    /// <summary>
    /// Provides functionality for managing errors and error messages.
    /// </summary>
    public static class ErrorManager
    {
        public static Dictionary<string, string> ErrorDictionary = new Dictionary<string, string>();

        /// <summary>
        /// Loads the error dictionary with provided error codes and messages.
        /// </summary>
        /// <param name="errorDictionary">The dictionary containing error codes and messages.</param>
        public static void Load(Dictionary<string, string> errorDictionary)
        {
            ErrorDictionary = errorDictionary;
        }
    }
}