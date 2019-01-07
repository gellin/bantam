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
            if (string.IsNullOrEmpty(textBoxPorts.Text)) {
                //todo fuck off message
                return;
            }

            btnScan.Enabled = false;

            if (BantamMain.Shells.ContainsKey(ShellUrl)) {

                string scanType = string.Empty;
                string portsCode = string.Empty;

                bool encryptResponse = BantamMain.Shells[ShellUrl].responseEncryption;
                int responseEncryptionMode = BantamMain.Shells[ShellUrl].responseEncryptionMode;

                if (comboBoxCommonPorts.SelectedIndex == (int)PORTS_OPTIONS.ONE_TO_1024) {
                    portsCode = PhpHelper.PortsScannerPorts1To1024();
                } else if(comboBoxCommonPorts.SelectedIndex == (int)PORTS_OPTIONS.COMMON_PORTS) {
                    portsCode = PhpHelper.PortScannerPortsCommon();
                } else if (comboBoxCommonPorts.SelectedIndex == (int)PORTS_OPTIONS.ALL_PORTS) {
                    portsCode = PhpHelper.PortScannerPortsAll();
                } else {
                    //todo fuck off
                }

                string phpCode = PhpHelper.PortScanner(textBoxHost.Text, portsCode, encryptResponse);
                BantamMain.executePHPCodeDisplayInRichTextBox(ShellUrl, phpCode, "Port Scan Results ("+ comboBoxCommonPorts.Text + ")" + textBoxHost.Text, encryptResponse, responseEncryptionMode);
            }

            btnScan.Enabled = true;
        }

        private void comboBoxCommonPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnScan.Enabled = true;
            if (comboBoxCommonPorts.SelectedIndex == (int)PORTS_OPTIONS.ONE_TO_1024) {
                textBoxPorts.Text = "1-1024";
            } else if (comboBoxCommonPorts.SelectedIndex == (int)PORTS_OPTIONS.COMMON_PORTS) {
                textBoxPorts.Text = "common ports";
            } else if (comboBoxCommonPorts.SelectedIndex == (int)PORTS_OPTIONS.ALL_PORTS) {
                textBoxPorts.Text = "1-65535";
            } else {
                //todo fuck off
            }
        }

        private void textBoxPorts_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
