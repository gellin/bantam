using System.Diagnostics;
using System.Windows.Forms;

namespace bantam.Classes
{
    public class ShellInfo
    {
        /// <summary>
        /// Default data vars that are sent on connection success in the order they are parsed
        /// </summary>
        public enum INIT_DATA_VARS
        {
            OS = 0,
            CWD,
            FREE_SPACE,
            TOTAL_SPACE,
            RELEASE,
            KERNEL,
            SERVER_IP,
            SERVER_SOFTWARE,
            USER,
            UID,
            GID,
            GROUP,
            PHP_VERSION
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ping"></param>
        /// <param name="data"></param>
        public void InitializeShellData(long ping, string[] data)
        {
            Ping = ping;
            files = new TreeView();
            pwd = data[(int)INIT_DATA_VARS.CWD];
            freeHDDSpace = data[(int)INIT_DATA_VARS.FREE_SPACE];
            totalHDDSpace = data[(int)INIT_DATA_VARS.TOTAL_SPACE];
            unameRelease = data[(int)INIT_DATA_VARS.RELEASE];
            unameKernel = data[(int)INIT_DATA_VARS.KERNEL];
            ip = data[(int)INIT_DATA_VARS.SERVER_IP];
            serverSoftware = data[(int)INIT_DATA_VARS.SERVER_SOFTWARE];
            user = data[(int)INIT_DATA_VARS.USER];
            uid = data[(int)INIT_DATA_VARS.UID];
            gid = data[(int)INIT_DATA_VARS.GID];
            group = data[(int)INIT_DATA_VARS.GROUP];
            PHP_Version = data[(int)INIT_DATA_VARS.PHP_VERSION];
            isWindows = (data[(int)INIT_DATA_VARS.OS] == "win") ? true : false;
            cwd = pwd;
        }

        /// <summary>
        /// 
        /// </summary>
        private long Ping {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        private bool isWindows;

        public bool IsWindows {
            get {
                return isWindows;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string cwd;

        public string Cwd {
            get {
                return cwd;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string pwd;

        public string Pwd {
            get {
                return pwd;
            }
            set {
                pwd = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string freeHDDSpace;

        public string FreeHDDSpace {
            get {
                return freeHDDSpace;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string totalHDDSpace;

        public string TotalHDDSpace {
            get {
                return totalHDDSpace;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string unameRelease;

        public string UnameRelease {
            get {
                return unameRelease;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string unameKernel;

        public string UnameKernel {
            get {
                return unameKernel;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string ip;

        public string Ip {
            get {
                return ip;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string serverSoftware;

        public string ServerSoftware {
            get {
                return serverSoftware;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string user;

        public string User {
            get {
                return user;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string uid;

        public string Uid {
            get {
                return uid;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string gid;

        public string Gid {
            get {
                return gid;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string group;

        public string Group {
            get {
                return group;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string PHP_Version;

        public string PHP_VERSION {
            get {
                return PHP_Version;
            }
        }

        /// <summary>
        /// Stores the console richtextbox string
        /// </summary>
        private string consoleText;

        public string ConsoleText {
            get {
                return consoleText;
            }
            set {
                consoleText = value;
            }
        }

        /// <summary>
        /// Stores the logs richtextbox string
        /// </summary>
        private string logText;

        public string LogText {
            get {
                return logText;
            }
            set {
                logText = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private TreeView files;

        public TreeView Files {
            get {
                return files;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Stopwatch pingStopwatch;

        public Stopwatch PingStopwatch {
            set {
                pingStopwatch = value;
            }
            get {
                return pingStopwatch;
            }
        }

        /// <summary>
        /// If true the client target/host is DOWN
        /// </summary>
        private bool down;

        public bool Down {
            get {
                return down;
            }
            set {
                down = value;
            }
        }

        /// <summary>
        /// If TRUE request's to this target/client will be sent via [GET] using a [COOKIE] to communicate data, if FALSE it will use a [POST] request
        /// </summary>
        private bool sendDataViaCookie;

        public bool SendDataViaCookie {
            get {
                return sendDataViaCookie;
            }
            set {
                sendDataViaCookie = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool responseEncryption = true;

        public bool ResponseEncryption {
            get {
                return responseEncryption;
            }
            set {
                responseEncryption = value;
            }
        }

        public int ResponseEncryptionMode {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        private bool requestEncryption;

        public bool RequestEncryption {
            get {
                return requestEncryption;
            }
            set {
                requestEncryption = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string requestEncryptionKey;

        public string RequestEncryptionKey {
            get {
                return requestEncryptionKey;
            }
            set {
                requestEncryptionKey = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string requestEncryptionIV;

        public string RequestEncryptionIV {
            get {
                return requestEncryptionIV;
            }
            set {
                requestEncryptionIV = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool sendRequestEncryptionIV;

        public bool SendRequestEncryptionIV {
            get {
                return sendRequestEncryptionIV;
            }
            set {
                sendRequestEncryptionIV = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string requestEncryptionIVRequestVarName;

        public string RequestEncryptionIVRequestVarName {
            get {
                return requestEncryptionIVRequestVarName;
            }
            set {
                requestEncryptionIVRequestVarName = value;
            }
        }

        /// <summary>
        /// If true the request data is gzcompressed, the bantam shell it is communicating with must decompress the data
        /// </summary>
        private bool gzipRequestData;

        public bool GzipRequestData {
            get {
                return gzipRequestData;
            }
            set {
                gzipRequestData = value;
            }
        }

        /// <summary>
        /// Name of the cookie or post argument used to send data to target/hostTargets
        /// </summary>
        private string requestArgName = "command";

        public string RequestArgName {
            get {
                return requestArgName;
            }
            set {
                requestArgName = value;
            }
        }
    }
}
