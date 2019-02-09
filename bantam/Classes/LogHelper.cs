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
        /// The different levels for logging
        /// </summary>
        public enum LOG_LEVEL
        {
            REQUESTED = 0, //this log was requested to be shown and should bypass any level checks
            ERROR,
            WARNING,
            INFO 
        }

        /// <summary>
        /// Add's a log to the main logs tab, richtextbox
        /// </summary>
        /// <param name="url">The url of the shell</param>
        /// <param name="logMessage">The log message</param>
        /// <param name="logLevel">The level/intensity of the issue being logged, checks the current Config Loglevel to see if the message should be displayed</param>
        public static void AddShellLog(string url, string logMessage, LOG_LEVEL logLevel)
        {
            if (!Config.EnableLogging) {
                return;
            }

            if (!BantamMain.ValidTarget()) {
                return;
            }

            if (logLevel > Config.LogLevel) {
                return;
            }

            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            BantamMain.Instance.AppendToRichTextBoxLogs("[" + timestamp + "] - [" + url + "] - " + logMessage + "\r\n\r\n");
        }

        /// <summary>
        /// Creates a messagebox with a global log on it
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="windowTitle">The title of the window in the messagebox</param>
        /// <param name="logLevel">The level/intensity of the issue being logged, checks the current Config Loglevel to see if the message should be displayed</param>
        public static void AddGlobalLog(string logMessage, string windowTitle, LOG_LEVEL logLevel)
        {
            if (!Config.EnableGlobalMessageBoxes) {
                return;
            }

            if (logLevel > Config.LogLevel) {
                return;
            }

            MessageBox.Show(logMessage, windowTitle);
        }
    }
}
