using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using bantam.Forms;

namespace bantam.Classes
{
    static class LogHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="log"></param>
        /// <param name="logLevel"></param>
        public static void AddShellLog(string url, string log, int logLevel)
        {
            if (!Config.EnableLogging) {
                return;
            }

            if (!BantamMain.ValidTarget()) {
                return;
            }

            //if (url != BantamMain.g_SelectedShellUrl) {
            //    return;
            //}

            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            BantamMain.Instance.AppendToRichTextBoxLogs("[" + timestamp + "] - " + log + "\r\n\r\n");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="windowTitle"></param>
        /// <param name="logLevel"></param>
        public static void AddGlobalLog(string log, string windowTitle, int logLevel)
        {
            if (!Config.EnableGlobalMessageBoxes) {
                return;
            }

            MessageBox.Show(log, windowTitle);
        }
    }
}
