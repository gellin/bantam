using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Forms;

using bantam.Classes;

namespace bantam.Forms
{
    public partial class ProxyOptions : Form
    {
        private static ProxyOptions instance;

        /// <summary>
        /// Proxy Types to be populated in the combo box
        /// </summary>
        private static readonly ReadOnlyCollection<string> proxyTypes = new List<string> {
            "socks",
            "http"
        }.AsReadOnly();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProxyOptions()
        {
            InitializeComponent();

            foreach (var proxyType in proxyTypes) {
                comboBoxProxyType.Items.Add(proxyType);
            }

            comboBoxProxyType.SelectedIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ProxyOptions getInstance()
        {
            if (instance == null) {
                instance = new ProxyOptions();
            }
            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProxyOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        /// <summary>
        /// Main routine for testing and connecting to a proxy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonConnect_Click(object sender, EventArgs e)
        {
            buttonConnect.Enabled = false;
            if (!string.IsNullOrEmpty(txtBoxProxyUrl.Text)) {
                if (int.TryParse(txtBoxProxyPort.Text, out int port)) {
                    if (comboBoxProxyType.Text == "http") {
                        WebHelper.AddHttpProxy(txtBoxProxyUrl.Text, txtBoxProxyPort.Text);
                    } else if (comboBoxProxyType.Text == "socks") {
                        WebHelper.AddSocksProxy(txtBoxProxyUrl.Text, port);
                    }
                           
                    try {
                        var task = WebHelper.GetRequest("http://ipv4.icanhazip.com/");

                        if (await Task.WhenAny(task, Task.Delay(Config.TimeoutMS)) == task) {
                            if (string.IsNullOrEmpty(task.Result)) {
                                MessageBox.Show("Unable to connect to proxy try again...", "Connection Failed");
                                WebHelper.ResetHttpClient();
                            } else {
                                MessageBox.Show("Your IP Is : " + task.Result, "Connection Success");
                                buttonConnect.Enabled = true;
                                buttonResetProxy.Enabled = true;
                                this.Close();
                            }
                        } else {
                            MessageBox.Show("Unable to connect to proxy try again...");
                            WebHelper.ResetHttpClient();
                            buttonResetProxy.Enabled = false;
                        }
                    }
                    catch(Exception) {
                        MessageBox.Show("Unable to connect to proxy try again...");
                        WebHelper.ResetHttpClient();
                        buttonResetProxy.Enabled = false;
                    }
                }
            }
            buttonConnect.Enabled = true;
        }

        /// <summary>
        /// Resets / Drops Proxy connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonResetProxy_Click(object sender, EventArgs e)
        {
            WebHelper.ResetHttpClient();
            buttonResetProxy.Enabled = false;
        }

        private void txtBoxProxyPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }
    }
}
