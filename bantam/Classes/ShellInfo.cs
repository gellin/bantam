using System.Diagnostics;
using System.Windows.Forms;

namespace bantam.Classes
{
    public class ShellInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public ShellInfo()
        {

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

            cwd = pwd = data[(int)PhpHelper.INIT_DATA_VARS.CWD];
            freeHDDSpace = data[(int)PhpHelper.INIT_DATA_VARS.FREE_SPACE];
            totalHDDSpace = data[(int)PhpHelper.INIT_DATA_VARS.TOTAL_SPACE];
            unameRelease = data[(int)PhpHelper.INIT_DATA_VARS.RELEASE];
            unameKernel = data[(int)PhpHelper.INIT_DATA_VARS.KERNEL];
            ip = data[(int)PhpHelper.INIT_DATA_VARS.SERVER_IP];
            serverSoftware = data[(int)PhpHelper.INIT_DATA_VARS.SERVER_SOFTWARE];
            user = data[(int)PhpHelper.INIT_DATA_VARS.USER];
            uid = data[(int)PhpHelper.INIT_DATA_VARS.UID];
            gid = data[(int)PhpHelper.INIT_DATA_VARS.GID];
            group = data[(int)PhpHelper.INIT_DATA_VARS.GROUP];
            PHP_Version = data[(int)PhpHelper.INIT_DATA_VARS.PHP_VERSION];
            isWindows = (data[(int)PhpHelper.INIT_DATA_VARS.OS] == "win") ? true : false;
        }

        /// <summary>
        /// 
        /// </summary>
        private long ping { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool isWindows { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string cwd;

        /// <summary>
        /// 
        /// </summary>
        public string pwd;

        /// <summary>
        /// 
        /// </summary>
        public string freeHDDSpace;

        /// <summary>
        /// 
        /// </summary>
        public string totalHDDSpace;

        /// <summary>
        /// 
        /// </summary>
        public string unameRelease;

        /// <summary>
        /// 
        /// </summary>
        public string unameKernel;

        /// <summary>
        /// 
        /// </summary>
        public string ip;

        /// <summary>
        /// 
        /// </summary>
        public string serverSoftware;

        /// <summary>
        /// 
        /// </summary>
        public string user;

        /// <summary>
        /// 
        /// </summary>
        public string uid;

        /// <summary>
        /// 
        /// </summary>
        public string gid;

        /// <summary>
        /// 
        /// </summary>
        public string group;

        /// <summary>
        /// 
        /// </summary>
        public string PHP_Version;

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
