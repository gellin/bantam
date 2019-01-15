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
    public partial class PortScanner : Form
    {

        public string ShellUrl { get; set; }

        public enum PORTS_OPTIONS
        {
           ONE_TO_1024 = 1,
           COMMON_PORTS,
           ALL_PORTS
        }

        public PortScanner(string shellUrl)
        {
            InitializeComponent();

            ShellUrl = shellUrl;

            this.Text += " - (" + ShellUrl + ")";
        }

        private async void btnScan_Click(object sender, EventArgs e)
        {
            btnScan.Enabled = false;

            if (BantamMain.Shells.ContainsKey(ShellUrl)) {
                string scanType = string.Empty;
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
                BantamMain.executePHPCodeDisplayInRichTextBox(ShellUrl, phpCode, "Opened Ports - " + textBoxHost.Text, encryptResponse, responseEncryptionMode);
            }

            btnScan.Enabled = true;
        }

        private void comboBoxCommonPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnScan.Enabled = true;
            if (comboBoxCommonPorts.SelectedIndex != 0) {
                textBoxPorts.Text = "";
            }
        }

        private void textBoxPorts_TextChanged(object sender, EventArgs e)
        {
            comboBoxCommonPorts.SelectedIndex = 0;
            btnScan.Enabled = true;
        }

        private void PortScanner_Load(object sender, EventArgs e)
        {

        }

        private void textBoxPorts_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }
    }
}
