namespace bantam.Classes
{
    public class ResponseObject
    {
        //I would use readonly properties here, as it IMO provides the same level of protection cleaner, but SONAR is currently complaining about that, todo

        /// <summary>
        /// 
        /// </summary>
        public string Result { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string EncryptionKey { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string EncryptionIV { get; private set; }

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