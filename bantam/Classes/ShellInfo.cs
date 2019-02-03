using System.Diagnostics;
using System.Windows.Forms;

namespace bantam.Classes
{
    public class ShellInfo
    {
        /// <summary>
        /// 
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
        public void Update(long ping, string[] data)
        {
            this.ping = ping;
            files = new TreeView();

            cwd = pwd = data[(int)INIT_DATA_VARS.CWD];
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
        }

        /// <summary>
        /// 
        /// </summary>
        private long ping { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private bool isWindows;

        public bool IsWindows
        {
            get { return isWindows; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string cwd;

        public string Cwd
        {
            get { return cwd; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string pwd;

        public string Pwd
        {
            get{ return pwd; }
            set { pwd = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string freeHDDSpace;

        public string FreeHDDSpace
        {
            get { return freeHDDSpace; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string totalHDDSpace;

        public string TotalHDDSpace
        {
            get { return totalHDDSpace; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string unameRelease;

        public string UnameRelease
        {
            get { return unameRelease; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string unameKernel;

        public string UnameKernel
        {
            get { return unameKernel; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string ip;

        public string Ip
        {
            get { return ip; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string serverSoftware;

        public string ServerSoftware
        {
            get { return serverSoftware; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string user;

        public string User
        {
            get { return user; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string uid;

        public string Uid
        {
            get { return uid; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string gid;

        public string Gid
        {
            get { return gid; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string group;

        public string Group
        {
            get { return group; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string PHP_Version;

        public string PHP_VERSION
        {
            get { return PHP_Version;  }
        }

        /// <summary>
        /// Stores the console richtextbox string
        /// </summary>
        public string consoleText;

        /// <summary>
        /// Stores the logs richtextbox string
        /// </summary>
        public string logText;

        /// <summary>
        /// 
        /// </summary>
        public TreeView files;

        /// <summary>
        /// 
        /// </summary>
        public Stopwatch pingStopwatch;

        /// <summary>
        /// If true the client target/host is DOWN
        /// </summary>
        public bool down = false;

        /// <summary>
        /// If TRUE request's to this target/client will be sent via [GET] using a [COOKIE] to communicate data, if FALSE it will use a [POST] request
        /// </summary>
        public bool sendDataViaCookie = false;

        /// <summary>
        /// 
        /// </summary>
        public bool responseEncryption = true;

        /// <summary>
        /// 
        /// </summary>
        public int responseEncryptionMode = 0;


        /// <summary>
        /// 
        /// </summary>
        public bool requestEncryption = false;

        /// <summary>
        /// 
        /// </summary>
        public string requestEncryptionKey;

        /// <summary>
        /// 
        /// </summary>
        public string requestEncryptionIV;

        /// <summary>
        /// 
        /// </summary>
        public bool sendRequestEncryptionIV = false;

        /// <summary>
        /// 
        /// </summary>
        public string requestEncryptionIVRequestVarName;

        /// <summary>
        /// 
        /// </summary>
        public bool gzipRequestData = false;

        /// <summary>
        /// Name of the cookie or post argument used to send data to target/hostTargets
        /// </summary>
        public string requestArgName = "command";
    }
}
