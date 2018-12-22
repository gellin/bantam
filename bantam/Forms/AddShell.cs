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
        public AddHost()
        {
            InitializeComponent();
            
            comboBoxVarType.SelectedIndex = 0;
        }

        public AddHost(string shellUrl)
        {

        }

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
        }

        private void AddHost_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.g_BantamMain.addClientForm = null;
        }

        private void btnTestShell_Click(object sender, EventArgs e)
        {

        }
    }
}
