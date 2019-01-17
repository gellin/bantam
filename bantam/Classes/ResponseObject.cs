namespace bantam.Classes
{
    public class ResponseObject
    {
        public string Result { get; set; }
        public string EncryptionKey { get; set; }
        public string EncryptionIV { get; set; }

        public ResponseObject(string result, string encryptionKey, string encryptionIV)
        {
            Result = result;
            EncryptionKey = encryptionKey;
            EncryptionIV = encryptionIV;
        }
    }
}
