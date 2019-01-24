using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

using bantam.Classes;

namespace bantam.Forms
{
    public partial class ModifyShell : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public static string g_CallingShellUrl = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public static ReadOnlyCollection<string> requestTypes = new List<string> {
            "cookie",
            "post",
        }.AsReadOnly();

        /// <summary>
        /// 
        /// </summary>
        public static ReadOnlyCollection<string> responseEncryptionModes = new List<string>() {
             "openssl",
             "mcrypt",
        }.AsReadOnly();

        /// <summary>
        /// Add shell constructor
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
        /// Update / Edit Shell Constructor
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

                if (checkBoxEncryptRequest.Checked = BantamMain.Shells[shellUrl].requestEncryption) {
                    checkBoxSendIVInRequest.Checked = (string.IsNullOrEmpty(BantamMain.Shells[shellUrl].requestEncryptionIV)) ? true : false;
                    textBoxEncrpytionIV.Text = BantamMain.Shells[shellUrl].requestEncryptionIV;
                    textBoxEncrpytionKey.Text = BantamMain.Shells[shellUrl].requestEncryptionKey;
                    textBoxIVVarName.Text = BantamMain.Shells[shellUrl].requestEncryptionIVRequestVarName;
                }
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

            if (checkBoxEncryptRequest.Checked) {
                string encryptionKey = textBoxEncrpytionKey.Text;

                //todo set as var?
                if (encryptionKey.Length != 32) {
                    labelDynAddHostsStatus.Text = "Encryption key length must be 32 charectors... Try again.";
                    return;
                }

                if (!checkBoxSendIVInRequest.Checked) {
                    string encryptionIV = textBoxEncrpytionIV.Text;

                    if (string.IsNullOrEmpty(encryptionIV) || encryptionIV.Length != 16) {
                        labelDynAddHostsStatus.Text = "Encryption IV length must be 16 charectors... Try again.";
                        return;
                    }
                }
            }

            //Remove Shell
            if (BantamMain.Shells.ContainsKey(shellURL)) {
                Program.g_BantamMain.guiCallbackRemoveShellURL(shellURL);
                BantamMain.Shells.TryRemove(shellURL, out ShellInfo shellInfoOut);
            }

            //Add Shell
            BantamMain.Shells.TryAdd(shellURL, new ShellInfo());

            BantamMain.Shells[shellURL].requestArgName = txtBoxArgName.Text;

            if (comboBoxVarType.Text == "cookie") {
                BantamMain.Shells[shellURL].sendDataViaCookie = true;
            }

            if (checkBoxResponseEncryption.Checked == false) {
                BantamMain.Shells[shellURL].responseEncryption = false;
            } else {
                BantamMain.Shells[shellURL].responseEncryption = true;
                BantamMain.Shells[shellURL].responseEncryptionMode = comboBoxEncryptionMode.SelectedIndex;
            }

            if (checkBoxGZipRequest.Checked) {
                BantamMain.Shells[shellURL].gzipRequestData = true;
            } else {
                BantamMain.Shells[shellURL].gzipRequestData = false;
            }

            bool encryptResponse = BantamMain.Shells[shellURL].responseEncryption;
            int responseEncryptionMode = BantamMain.Shells[shellURL].responseEncryptionMode;


            if (checkBoxEncryptRequest.Checked) {
                string encryptionKey = textBoxEncrpytionKey.Text;

                BantamMain.Shells[shellURL].requestEncryption = true;
                BantamMain.Shells[shellURL].requestEncryptionKey = textBoxEncrpytionKey.Text;

                if (checkBoxSendIVInRequest.Checked) {
                    BantamMain.Shells[shellURL].sendRequestEncryptionIV = true;
                    BantamMain.Shells[shellURL].requestEncryptionIV = string.Empty;
                    BantamMain.Shells[shellURL].requestEncryptionIVRequestVarName = textBoxIVVarName.Text;
                } else {
                    BantamMain.Shells[shellURL].requestEncryptionIV = textBoxEncrpytionIV.Text;
                    BantamMain.Shells[shellURL].requestEncryptionIVRequestVarName = string.Empty;
                }
            } else {
                BantamMain.Shells[shellURL].requestEncryption = false;
                BantamMain.Shells[shellURL].requestEncryptionIVRequestVarName = string.Empty;
                BantamMain.Shells[shellURL].requestEncryptionIV = string.Empty;
                BantamMain.Shells[shellURL].requestEncryptionKey = string.Empty;
            }

            string phpCode = PhpHelper.PhpTestExecutionWithEcho1(encryptResponse);
            ResponseObject response = await WebHelper.ExecuteRemotePHP(shellURL, phpCode);

            if (string.IsNullOrEmpty(response.Result)) {
                labelDynAddHostsStatus.Text = "Unable to connect, check your settings and try again.";
                return;
            }

            string result = response.Result;

            if (encryptResponse) {
                result = EncryptionHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, responseEncryptionMode);
            }

            if (string.IsNullOrEmpty(result)) {
                labelDynAddHostsStatus.Text = "Unable to connect, check your settings and try again.";
                return;
            }

            Program.g_BantamMain.InitializeShellData(shellURL);

            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnUpdateShell_Click(object sender, EventArgs e)
        {
            string newShellURL = txtBoxShellUrl.Text;

            if (string.IsNullOrEmpty(newShellURL)) {
                return;
            }

            if (checkBoxEncryptRequest.Checked) {
                string encryptionKey = textBoxEncrpytionKey.Text;

                //todo set as var?
                if (encryptionKey.Length != 32) {
                    labelDynAddHostsStatus.Text = "Encryption key length must be 32 charectors... Try again.";
                    return;
                }

                if (!checkBoxSendIVInRequest.Checked) {
                    string encryptionIV = textBoxEncrpytionIV.Text;

                    if (string.IsNullOrEmpty(encryptionIV) || encryptionIV.Length != 16) {
                        labelDynAddHostsStatus.Text = "Encryption IV length must be 16 charectors... Try again.";
                        return;
                    }
                }
            }

            if (BantamMain.Shells.ContainsKey(g_CallingShellUrl)) {
                Program.g_BantamMain.guiCallbackRemoveShellURL(g_CallingShellUrl);

                BantamMain.Shells.TryRemove(g_CallingShellUrl, out ShellInfo outShellInfo);
            }

            if (BantamMain.Shells.ContainsKey(newShellURL)) {
                Program.g_BantamMain.guiCallbackRemoveShellURL(newShellURL);

                BantamMain.Shells.TryRemove(g_CallingShellUrl, out ShellInfo outShellInfo);
            }

            BantamMain.Shells.TryAdd(newShellURL, new ShellInfo());
            BantamMain.Shells[newShellURL].requestArgName = txtBoxArgName.Text;

            if (comboBoxVarType.Text == "cookie") {
                BantamMain.Shells[newShellURL].sendDataViaCookie = true;
            }

            if (checkBoxResponseEncryption.Checked == false) {
                BantamMain.Shells[newShellURL].responseEncryption = false;
            } else {
                BantamMain.Shells[newShellURL].responseEncryption = true;
                BantamMain.Shells[newShellURL].responseEncryptionMode = comboBoxEncryptionMode.SelectedIndex;
            }

            if (checkBoxGZipRequest.Checked) {
                BantamMain.Shells[newShellURL].gzipRequestData = true;
            } else {
                BantamMain.Shells[newShellURL].gzipRequestData = false;
            }

            bool encryptResponse = BantamMain.Shells[newShellURL].responseEncryption;
            int responseEncryptionMode = BantamMain.Shells[newShellURL].responseEncryptionMode;

            if (checkBoxEncryptRequest.Checked) {
                string encryptionKey = textBoxEncrpytionKey.Text;

                BantamMain.Shells[newShellURL].requestEncryption = true;
                BantamMain.Shells[newShellURL].requestEncryptionKey = textBoxEncrpytionKey.Text;

                if (checkBoxSendIVInRequest.Checked) {
                    BantamMain.Shells[newShellURL].sendRequestEncryptionIV = true;
                    BantamMain.Shells[newShellURL].requestEncryptionIV = string.Empty;
                    BantamMain.Shells[newShellURL].requestEncryptionIVRequestVarName = textBoxIVVarName.Text;
                } else {
                    BantamMain.Shells[newShellURL].requestEncryptionIV = textBoxEncrpytionIV.Text;
                    BantamMain.Shells[newShellURL].requestEncryptionIVRequestVarName = string.Empty;
                }
            } else {
                BantamMain.Shells[newShellURL].requestEncryption = false;
                BantamMain.Shells[newShellURL].requestEncryptionIVRequestVarName = string.Empty;
                BantamMain.Shells[newShellURL].requestEncryptionIV = string.Empty;
                BantamMain.Shells[newShellURL].requestEncryptionKey = string.Empty;
            }

            string phpCode = PhpHelper.PhpTestExecutionWithEcho1(encryptResponse);
            ResponseObject response = await WebHelper.ExecuteRemotePHP(newShellURL, phpCode);

            if (string.IsNullOrEmpty(response.Result)) {
                labelDynAddHostsStatus.Text = "Unable to connect, check your settings and try again.";
                return;
            }

            string result = response.Result;

            if (encryptResponse) {
                result = EncryptionHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, responseEncryptionMode);
            }

            if (string.IsNullOrEmpty(result)) {
                labelDynAddHostsStatus.Text = "Unable to connect, check your settings and try again.";
                return;
            }

            Program.g_BantamMain.InitializeShellData(newShellURL);

            this.Close();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRandomIV_Click(object sender, EventArgs e)
        {
            textBoxEncrpytionIV.Text = EncryptionHelper.GetRandomEncryptionIV();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRandomKey_Click(object sender, EventArgs e)
        {
            textBoxEncrpytionKey.Text = EncryptionHelper.GetRandomEncryptionKey();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxSendIVInRequest_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSendIVInRequest.Checked) {
                textBoxEncrpytionIV.Enabled = false;
                textBoxIVVarName.Enabled = true;
            } else {
                textBoxEncrpytionIV.Enabled = true;
                textBoxIVVarName.Enabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxEncryptRequest_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEncryptRequest.Checked) {
                textBoxEncrpytionIV.Enabled = true;
                textBoxEncrpytionKey.Enabled = true;
                checkBoxSendIVInRequest.Enabled = true;
                buttonRandomIV.Enabled = true;
                buttonRandomKey.Enabled = true;
            } else {
                textBoxEncrpytionIV.Enabled = false;
                textBoxEncrpytionKey.Enabled = false;
                textBoxIVVarName.Enabled = false;
                buttonRandomIV.Enabled = false;
                checkBoxSendIVInRequest.Enabled = false;
                buttonRandomKey.Enabled = false;
            }
        }
    }
}
