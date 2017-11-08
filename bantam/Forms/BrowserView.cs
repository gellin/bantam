using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        }
    }
}
