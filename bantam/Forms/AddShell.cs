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
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, int> requestTypes = new Dictionary<string, int>() {
            {"cookie", 0},
            {"post", 1},
            {"request", 2}
        };

        /// <summary>
        /// 
        /// </summary>
        public AddHost()
        {
            InitializeComponent();

            comboBoxVarType.SelectedIndex = 0;
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

            if(requestTypes.ContainsKey(varType))
            {
                comboBoxVarType.SelectedIndex = requestTypes[varType];
            } else {
                //todo: possible alert to user about default behaviour
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
        private void btnAddShell_Click(object sender, EventArgs e)
        {
            //todo, ugly static var thread safe?
            string shellURL = txtBoxShellUrl.Text;

            if (string.IsNullOrEmpty(shellURL))
            {
                return;
            }

            if (BantamMain.Hosts.ContainsKey(shellURL))
            {
                Program.g_BantamMain.guiCallbackRemoveShellURL(shellURL);
                BantamMain.Hosts.Remove(shellURL);
            }

            BantamMain.Hosts.Add(shellURL, new HostInfo());
            BantamMain.Hosts[shellURL].RequestArgName = txtBoxArgName.Text;

            if(comboBoxVarType.Text == "cookie")
            {
                BantamMain.Hosts[shellURL].SendDataViaCookie = true;
            }

            Thread t = new Thread(() => Program.g_BantamMain.getInitDataThread(shellURL));
            t.Start();

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
        private void btnUpdateShell_Click(object sender, EventArgs e)
        {
            string shellURL = txtBoxShellUrl.Text;

            if (string.IsNullOrEmpty(shellURL))
            {
                return;
            }

            if (BantamMain.Hosts.ContainsKey(shellURL))
            {
                Program.g_BantamMain.guiCallbackRemoveShellURL(shellURL);
                BantamMain.Hosts.Remove(shellURL);

                BantamMain.Hosts.Add(shellURL, new HostInfo());
                BantamMain.Hosts[shellURL].RequestArgName = txtBoxArgName.Text;

                if (comboBoxVarType.Text == "cookie")
                {
                    BantamMain.Hosts[shellURL].SendDataViaCookie = true;
                }

                Thread t = new Thread(() => Program.g_BantamMain.getInitDataThread(shellURL));
                t.Start();

                Program.g_BantamMain.editHostForm.Hide();
            }
        }
    }
}
