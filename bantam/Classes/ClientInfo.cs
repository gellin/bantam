using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace bantam_php
{
    public class ClientInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public ClientInfo()
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

            CWD = data[(int)PHP_Helper.INIT_DATA_VARS.CWD];
            FreeHDDSpace = data?[(int)PHP_Helper.INIT_DATA_VARS.FREE_SPACE];
            TotalHDDSpace = data[(int)PHP_Helper.INIT_DATA_VARS.TOTAL_SPACE];
            UnameRelease = data[(int)PHP_Helper.INIT_DATA_VARS.RELEASE];
            UnameKernel = data[(int)PHP_Helper.INIT_DATA_VARS.KERNEL];
            IP = data[(int)PHP_Helper.INIT_DATA_VARS.SERVER_IP];
            ServerSoftware = data[(int)PHP_Helper.INIT_DATA_VARS.SERVER_SOFTWARE];
            User = data[(int)PHP_Helper.INIT_DATA_VARS.USER];
            UID = data[(int)PHP_Helper.INIT_DATA_VARS.UID];
            GID = data[(int)PHP_Helper.INIT_DATA_VARS.GID];
            Group = data[(int)PHP_Helper.INIT_DATA_VARS.GROUP];
            PHP_Version = data[(int)PHP_Helper.INIT_DATA_VARS.PHP_VERSION];
            Windows = (data[(int)PHP_Helper.INIT_DATA_VARS.OS] == "win") ? true : false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ping"></param>
        /// <param name="windows"></param>
        public ClientInfo(long ping, bool windows)
        {
            Ping = ping;
            Windows = windows;

            Files = new TreeView();
        }

        /// <summary>
        /// 
        /// </summary>
        public long Ping { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Windows { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CWD { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FreeHDDSpace { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TotalHDDSpace { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UnameRelease { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UnameKernel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServerSoftware { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PHP_Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TreeView Files { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Stopwatch PingStopwatch { get; set; }

        /// <summary>
        /// If true the client target/host is DOWN
        /// </summary>
        public bool Down { get; set; } = false;

        /// <summary>
        /// If TRUE request's to this target/client will be sent via [GET] using a [COOKIE] to communicate data, if FALSE it will use a [POST] request
        /// </summary>
        public bool SendDataViaCookie { get; set; } = false;

        /// <summary>
        /// Name of the cookie or post argument used to send data to target/hosts
        /// </summary>
        public string RequestArgName { get; set; } = "command";
    }
}
