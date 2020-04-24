using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using bantam.Classes;

namespace bantam.Forms
{
    public partial class Options : Form
    {
        /// <summary>
        /// Php Shell Code execution vectors
        /// </summary>
        private static readonly ReadOnlyCollection<string> phpShellCodeExecutionVectors = new List<string> {
             "system",
             "exec",
             "shell_exec",
             "passthru",
             "popen",
             "backticks"
        }.AsReadOnly();

        /// <summary>
        /// Php Shell Code execution vectors, must represent what is within "phpShellCodeExecutionVectors"
        /// </summary>
        public enum PHP_SHELL_CODE_VECTORS
        {
            SYSTEM = 0,
            EXEC,
            SHELL_EXEC,
            PASSTHRU,
            POPEN,
            BACKTICKS
        }

        /// <summary>
        /// 
        /// </summary>
        public Options()
        {
            InitializeComponent();

            foreach (var shellCodeExecVec in phpShellCodeExecutionVectors) {
                comboBoxShellCodeExVectors.Items.Add(shellCodeExecVec);
            }

            LoadConfig();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        private void LoadConfig()
        {
            checkBoxEnableLogging.Checked = Config.EnableLogging;
            checkBoxGlobalLogs.Checked = Config.EnableGlobalMessageBoxes;
            trackBarLoggingLevel.Value = (int)Config.LogLevel;

            checkBoxMaxExecutionTime.Checked = Config.MaxExecutionTime;
            checkBoxDisableErrorLogs.Checked = Config.DisableErrorLogs;
            textBoxMaxCookieSize.Text = Config.MaxCookieSizeB.ToString();
            textBoxMaxPostSize.Text = Config.MaxPostSizeKib.ToString();

            checkBoxRandomComments.Checked = Config.InjectRandomComments;
            trackBarCommentFrequency.Value = Config.CommentFrequency;
            textBoxMaxCommentLength.Text = Config.CommentMaxLength.ToString();

            checkBoxRandomPhpVarNames.Checked = Config.RandomizePhpVariableNames;
            textBoxPhpVarNameMaxLen.Text = Config.PhpVariableNameMaxLength.ToString();

            comboBoxShellCodeExVectors.SelectedIndex = Config.PhpShellCodeExectionVectorValue;

            textBoxTimeout.Text = Config.TimeoutMS.ToString();
        }

        /// <summary>
        /// Enable random commends toggle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxRandomComments_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxRandomComments.Checked) {
                trackBarCommentFrequency.Enabled = true;
                textBoxMaxCommentLength.Enabled = true;
            } else {
                trackBarCommentFrequency.Enabled = false;
                textBoxMaxCommentLength.Enabled = false;
            }
            Config.InjectRandomComments = checkBoxRandomComments.Checked;
        }

        /// <summary>
        /// Prevent anything other than digits from being entered into the "textBoxMaxCommentLength"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxMaxCommentLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Prevent anything other than digits from being entered into the "textBoxPhpVarNameMaxLen"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxPhpVarNameMaxLen_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Prevent anything other than digits from being entered into the "textBoxMaxPostSize"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxMaxPostSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Prevent anything other than digits from being entered into the "textBoxTimeout"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxTimeout_keyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void checkBoxDisableErrorLogs_CheckedChanged(object sender, EventArgs e)
        {
            Config.DisableErrorLogs = checkBoxDisableErrorLogs.Checked;
        }

        private void trackBarLoggingLevel_ValueChanged(object sender, EventArgs e)
        {
            Config.LogLevel = (LogHelper.LOG_LEVEL)trackBarLoggingLevel.Value;
        }

        private void trackBarCommentFrequency_ValueChanged(object sender, EventArgs e)
        {
            Config.CommentFrequency = trackBarCommentFrequency.Value;
        }

        private void textBoxMaxCommentLength_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxMaxCommentLength.Text, out int commentMaxLen)) {
                Config.CommentMaxLength = commentMaxLen;
            }
        }

        private void textBoxPhpVarNameMaxLen_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxPhpVarNameMaxLen.Text, out int phpVarNameMaxLen)) {
                Config.PhpVariableNameMaxLength = phpVarNameMaxLen;
            }
        }

        private void textBoxMaxPostSize_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxMaxPostSize.Text, out int postSize)) {
                Config.MaxPostSizeKib = postSize;
            }
        }

        private void textBoxTimeout_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxTimeout.Text, out int timeoutMS)) {
                Config.TimeoutMS = timeoutMS;
            }
        }

        private void checkBoxEnableLogging_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableLogging = checkBoxEnableLogging.Checked;
        }

        private void checkBoxMaxExecutionTime_CheckedChanged(object sender, EventArgs e)
        {
            Config.MaxExecutionTime = checkBoxMaxExecutionTime.Checked;
        }

        private void checkBoxGlobalLogs_CheckedChanged(object sender, EventArgs e)
        {
            Config.EnableGlobalMessageBoxes = checkBoxGlobalLogs.Checked;
        }

        private void comboBoxShellCodeExVectors_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.PhpShellCodeExectionVectorValue = comboBoxShellCodeExVectors.SelectedIndex;
        }
    }
}
