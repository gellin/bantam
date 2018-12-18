using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace bantam_php
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        static public BantamMain g_BantamMain = null;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            g_BantamMain = new BantamMain();

            Application.Run(g_BantamMain);
        }
    }
}
