using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bantam_php
{
    public partial class AddHost : Form
    {
        public static AddHost instance = null;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, int> requestTypes = new Dictionary<string, int>() {
            { "cookie", 0 },
            { "post", 1 },
        };

        /// <summary>
        /// 
        /// </summary>
        public AddHost()
        {
            InitializeComponent();

            comboBoxVarType.SelectedIndex = 0;
        }


        public static AddHost getInstance()
        {
            if (instance == null) {
                instance = new AddHost();
            }
            return instance;
        }

        /// <summary>
        /// Update / Edit Shell Routine
        /// </summary>
        /// <param name="shellUrl"></param>
        /// <param name="varName"></param>
        /// <param name="varType"></param>
        public AddHost(string shellUrl = "", string varName = "", string varType = "")
        {
            InitializeComponent();

            Text = "Update Shell";

            txtBoxShellUrl.Text = shellUrl;
            txtBoxArgName.Text = varName;

            if (requestTypes.ContainsKey(varType)) {
                comboBoxVarType.SelectedIndex = requestTypes[varType];
            } else {
                comboBoxVarType.SelectedIndex = 0;
            }

            btnAddShell.Visible = false;
            btnUpdateShell.Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnAddShell_Click(object sender, EventArgs e)
        {
            string shellURL = txtBoxShellUrl.Text;

            if (string.IsNullOrEmpty(shellURL)) {
                return;
            }

            if (BantamMain.Hosts.ContainsKey(shellURL)) {
                Program.g_BantamMain.guiCallbackRemoveShellURL(shellURL);
                BantamMain.Hosts.Remove(shellURL);
            }

            BantamMain.Hosts.Add(shellURL, new ShellInfo());
            BantamMain.Hosts[shellURL].RequestArgName = txtBoxArgName.Text;

            if (comboBoxVarType.Text == "cookie") {
                BantamMain.Hosts[shellURL].SendDataViaCookie = true;
            }

            Program.g_BantamMain.getInitDataThread(shellURL);
            Program.g_BantamMain.addClientForm.Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddHost_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.g_BantamMain.addClientForm = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnUpdateShell_Click(object sender, EventArgs e)
        {
            string shellURL = txtBoxShellUrl.Text;

            if (string.IsNullOrEmpty(shellURL)) {
                return;
            }

            if (BantamMain.Hosts.ContainsKey(shellURL)) {
                Program.g_BantamMain.guiCallbackRemoveShellURL(shellURL);
                BantamMain.Hosts.Remove(shellURL);

                BantamMain.Hosts.Add(shellURL, new ShellInfo());
                BantamMain.Hosts[shellURL].RequestArgName = txtBoxArgName.Text;

                if (comboBoxVarType.Text == "cookie") {
                    BantamMain.Hosts[shellURL].SendDataViaCookie = true;
                }

                Program.g_BantamMain.getInitDataThread(shellURL);
                Program.g_BantamMain.updateHostForm.Hide();
            }
        }
    }
}
