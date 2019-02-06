using System;
using System.Collections.Generic;
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
        public Options()
        {
            InitializeComponent();
        }

        private void LoadConfig()
        {
            checkBoxEnableLogging.Checked = Config.EnableLogging;
            checkBoxGlobalLogs.Checked = Config.EnableGlobalMessageBoxes;
            trackBarLoggingLevel.Value = Config.LogLevel;

            checkBoxMaxExecutionTime.Checked = Config.MaxExecutionTime;
            checkBoxDisableErrorLogs.Checked = Config.DisableErrorLogs;
            textBoxMaxCookieSize.Text = Config.MaxCookieSizeB.ToString();
            textBoxMaxPostSize.Text = Config.MaxPostSizeKib.ToString();

            checkBoxRandomComments.Checked = Config.InjectRandomComments;
            trackBarCommentFrequency.Value = Config.CommentFrequency;
            textBoxMaxCommentLength.Text = Config.CommentMaxLength.ToString();

            checkBoxRandomPhpVarNames.Checked = Config.RandomizePhpVariableNames;
            textBoxPhpVarNameMaxLen.Text = Config.PhpVariableNameMaxLength.ToString();
        }

        private void Options_Load(object sender, EventArgs e)
        {
           LoadConfig();
        }

        private void trackBarCommentFrequency_Scroll(object sender, EventArgs e)
        {
            //todo
        }

        private void checkBoxRandomPhpVarNames_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxRandomPhpVarNames.Checked) {
                textBoxPhpVarNameMaxLen.Enabled = true;
            } else {
                textBoxPhpVarNameMaxLen.Enabled = false;
            }
        }

        private void checkBoxRandomComments_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxRandomComments.Checked) {
                trackBarCommentFrequency.Enabled = true;
                textBoxMaxCommentLength.Enabled = true;
            } else {
                trackBarCommentFrequency.Enabled = false;
                textBoxMaxCommentLength.Enabled = false;
            }
        }

        private void textBoxMaxCommentLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void textBoxPhpVarNameMaxLen_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void checkBoxDisableErrorLogs_CheckedChanged(object sender, EventArgs e)
        {
            //todo
        }
    }
}
