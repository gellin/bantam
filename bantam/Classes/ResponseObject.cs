namespace bantam.Classes
{
    public class ResponseObject
    {
        /// <summary>
        /// 
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EncryptionKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EncryptionIV { get; set; }

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
