namespace bantam.Classes
{
    /// <summary>
    /// An object that represents 
    /// </summary>
    public sealed class ResponseObject
    {
        /// <summary>
        /// The result of the response, empty if none
        /// </summary>
        public readonly string Result;

        /// <summary>
        /// The encryption Key used in the response, empty if none
        /// </summary>
        public readonly string EncryptionKey;

        /// <summary>
        /// The encryption IV used in the response, empty if none
        /// </summary>
        public readonly string EncryptionIV;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="result"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        public ResponseObject(string result, string encryptionKey, string encryptionIV)
        {
            Result = result;
            EncryptionKey = encryptionKey;
            EncryptionIV = encryptionIV;
        }
    }
}