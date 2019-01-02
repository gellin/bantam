using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bantam_php
{
    public partial class ProxyOptions : Form
    {
        public static ProxyOptions instance = null;

        public enum PROXY_TYPE
        {
            socks = 0,
            http = 1
        }

        public ProxyOptions()
        {
            InitializeComponent();
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
        private void ProxyOptions_Load(object sender, EventArgs e)
        {
            comboBoxProxyType.SelectedIndex = 0;
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
        private async void button1_Click(object sender, EventArgs e)
        {
            //todo verify url
            buttonOk.Enabled = false;
            if (!string.IsNullOrEmpty(txtBoxProxyUrl.Text)) {
                if (int.TryParse(txtBoxProxyPort.Text, out int port)) {
                    if (comboBoxProxyType.SelectedIndex == (int)PROXY_TYPE.http) {
                        WebHelper.AddHttpProxy(txtBoxProxyUrl.Text, txtBoxProxyPort.Text);
                    } else if (comboBoxProxyType.SelectedIndex == (int)PROXY_TYPE.socks) {
                        WebHelper.AddSocksProxy(txtBoxProxyUrl.Text, port);
                    }
                           
                    try {
                        var task = WebHelper.GetRequest("http://ipv4.icanhazip.com/");

                        //Todo tie this timeout in as a configureable option
                        if (await Task.WhenAny(task, Task.Delay(10000)) == task) {
                            if (string.IsNullOrEmpty(task.Result)) {
                                MessageBox.Show("Unable to connect to proxy try again...", "Connection Failed");
                                WebHelper.ResetHttpClient();
                            } else {
                                MessageBox.Show("Your IP Is : " + task.Result, "Connection Success");
                                buttonOk.Enabled = true;
                                this.Close();
                            }
                        } else {
                            MessageBox.Show("Unable to connect to proxy try again...");
                            WebHelper.ResetHttpClient();
                        }
                    }
                    catch(Exception) {
                        MessageBox.Show("Unable to connect to proxy try again...");
                        WebHelper.ResetHttpClient();
                    }
                }
            }
            buttonOk.Enabled = true;
        }

    }
}
