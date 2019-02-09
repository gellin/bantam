namespace bantam.Classes
{
    public class ResponseObject
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly string Result;

        /// <summary>
        /// 
        /// </summary>
        public readonly string EncryptionKey;

        /// <summary>
        /// 
        /// </summary>
        public readonly string EncryptionIV;

        /// <summary>
        /// 
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