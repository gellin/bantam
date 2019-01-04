using System;
using System.Windows.Forms;

namespace bantam_php
{
    public partial class BrowserView : Form
    {
        public BrowserView(string data, int width, int height)
        {
            InitializeComponent();
            this.Height = height;
            this.Width = width;

            webBrowser1.DocumentText = data;
            webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void BrowserView_FormClosing(object sender, FormClosingEventArgs e)
        {
            webBrowser1.DocumentText = string.Empty;
        }
    }
}
