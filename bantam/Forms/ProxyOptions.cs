using System;
using System.Threading.Tasks;
using System.Windows.Forms;

using bantam.Classes;

namespace bantam.Forms
{
    public partial class ProxyOptions : Form
    {
        private static ProxyOptions instance;

        private enum PROXY_TYPE
        {
            SOCKS = 0,
            HTTP = 1
        }

        public ProxyOptions()
        {
            InitializeComponent();

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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonConnect_Click(object sender, EventArgs e)
        {
            buttonConnect.Enabled = false;
            if (!string.IsNullOrEmpty(txtBoxProxyUrl.Text)) {
                if (int.TryParse(txtBoxProxyPort.Text, out int port)) {
                    if (comboBoxProxyType.SelectedIndex == (int)PROXY_TYPE.HTTP) {
                        WebHelper.AddHttpProxy(txtBoxProxyUrl.Text, txtBoxProxyPort.Text);
                    } else if (comboBoxProxyType.SelectedIndex == (int)PROXY_TYPE.SOCKS) {
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

        private void txtBoxProxyPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void buttonResetProxy_Click(object sender, EventArgs e)
        {
            WebHelper.ResetHttpClient();
            buttonResetProxy.Enabled = false;
        }
    }
}
