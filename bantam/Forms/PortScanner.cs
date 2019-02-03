using System;
using System.Windows.Forms;

using bantam.Classes;

namespace bantam.Forms
{
    public partial class PortScanner : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public string ShellUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public enum PORTS_OPTIONS
        {
           ONE_TO_1024 = 1,
           COMMON_PORTS,
           ALL_PORTS
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shellUrl"></param>
        public PortScanner(string shellUrl)
        {
            InitializeComponent();

            ShellUrl = shellUrl;

            this.Text += " - (" + ShellUrl + ")";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnScan_Click(object sender, EventArgs e)
        {
            btnScan.Enabled = false;

            if (BantamMain.Shells.ContainsKey(ShellUrl)) {
                string portsCode = string.Empty;

                bool encryptResponse = BantamMain.Shells[ShellUrl].responseEncryption;
                int responseEncryptionMode = BantamMain.Shells[ShellUrl].responseEncryptionMode;

                //todo validate this port
                if (!string.IsNullOrEmpty(textBoxPorts.Text)) {
                    portsCode = "$ports = array('" + textBoxPorts.Text + "');";
                    labelDynStatus.Text = "";
                } else {
                    if (comboBoxCommonPorts.SelectedIndex != 0) {
                        if (comboBoxCommonPorts.SelectedIndex == (int)PORTS_OPTIONS.ONE_TO_1024) {
                            portsCode = PhpHelper.PortsScannerPorts1To1024();
                            labelDynStatus.Text = "** May fail unless on local IP";
                        } else if (comboBoxCommonPorts.SelectedIndex == (int)PORTS_OPTIONS.COMMON_PORTS) {
                            labelDynStatus.Text = "** May fail unless on local IP";
                            portsCode = PhpHelper.PortScannerPortsCommon();
                        } else if (comboBoxCommonPorts.SelectedIndex == (int)PORTS_OPTIONS.ALL_PORTS) {
                            portsCode = PhpHelper.PortScannerPortsAll();
                            labelDynStatus.Text = "** May fail unless on local IP";
                        }
                    }
                }

                string phpCode = PhpHelper.PortScanner(textBoxHost.Text, portsCode, encryptResponse);
                BantamMain.ExecutePHPCodeDisplayInRichTextBox(ShellUrl, phpCode, "Opened Ports - " + textBoxHost.Text, encryptResponse, responseEncryptionMode);
            }

            btnScan.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxCommonPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnScan.Enabled = true;
            if (comboBoxCommonPorts.SelectedIndex != 0) {
                textBoxPorts.Text = "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxPorts_TextChanged(object sender, EventArgs e)
        {
            comboBoxCommonPorts.SelectedIndex = 0;
            btnScan.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxPorts_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }
    }
}
