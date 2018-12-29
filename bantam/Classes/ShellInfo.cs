using System.Diagnostics;
using System.Windows.Forms;

namespace bantam_php
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
        public void update(long ping, string[] data)
        {
            Ping = ping;
            Files = new TreeView();

            CWD = PWD = data[(int)PhpHelper.INIT_DATA_VARS.CWD];
            FreeHDDSpace = data[(int)PhpHelper.INIT_DATA_VARS.FREE_SPACE];
            TotalHDDSpace = data[(int)PhpHelper.INIT_DATA_VARS.TOTAL_SPACE];
            UnameRelease = data[(int)PhpHelper.INIT_DATA_VARS.RELEASE];
            UnameKernel = data[(int)PhpHelper.INIT_DATA_VARS.KERNEL];
            IP = data[(int)PhpHelper.INIT_DATA_VARS.SERVER_IP];
            ServerSoftware = data[(int)PhpHelper.INIT_DATA_VARS.SERVER_SOFTWARE];
            User = data[(int)PhpHelper.INIT_DATA_VARS.USER];
            UID = data[(int)PhpHelper.INIT_DATA_VARS.UID];
            GID = data[(int)PhpHelper.INIT_DATA_VARS.GID];
            Group = data[(int)PhpHelper.INIT_DATA_VARS.GROUP];
            PHP_Version = data[(int)PhpHelper.INIT_DATA_VARS.PHP_VERSION];
            isWindows = (data[(int)PhpHelper.INIT_DATA_VARS.OS] == "win") ? true : false;
        }

        /// <summary>
        /// 
        /// </summary>
        public long Ping;

        /// <summary>
        /// 
        /// </summary>
        public bool isWindows;

        /// <summary>
        /// 
        /// </summary>
        public string CWD;

        /// <summary>
        /// 
        /// </summary>
        public string PWD;

        /// <summary>
        /// 
        /// </summary>
        public string FreeHDDSpace;

        /// <summary>
        /// 
        /// </summary>
        public string TotalHDDSpace;

        /// <summary>
        /// 
        /// </summary>
        public string UnameRelease;

        /// <summary>
        /// 
        /// </summary>
        public string UnameKernel;

        /// <summary>
        /// 
        /// </summary>
        public string IP;

        /// <summary>
        /// 
        /// </summary>
        public string ServerSoftware;

        /// <summary>
        /// 
        /// </summary>
        public string User;

        /// <summary>
        /// 
        /// </summary>
        public string UID;

        /// <summary>
        /// 
        /// </summary>
        public string GID;

        /// <summary>
        /// 
        /// </summary>
        public string Group;

        /// <summary>
        /// 
        /// </summary>
        public string PHP_Version;

        /// <summary>
        /// 
        /// </summary>
        public TreeView Files;

        /// <summary>
        /// 
        /// </summary>
        public Stopwatch PingStopwatch;

        /// <summary>
        /// If true the client target/host is DOWN
        /// </summary>
        public bool Down = false;

        /// <summary>
        /// If TRUE request's to this target/client will be sent via [GET] using a [COOKIE] to communicate data, if FALSE it will use a [POST] request
        /// </summary>
        public bool SendDataViaCookie = false;

        /// <summary>
        /// Name of the cookie or post argument used to send data to target/hostTargets
        /// </summary>
        public string RequestArgName = "command";
    }
}
