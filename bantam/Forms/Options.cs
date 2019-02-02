using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bantam.Forms
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }

        private void DefaultOptions()
        {

        }

        private void Options_Load(object sender, EventArgs e)
        {

        }

        private void trackBarCommentFrequency_Scroll(object sender, EventArgs e)
        {

        }

        private void checkBoxRandomPhpVarNames_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxRandomComments_CheckedChanged(object sender, EventArgs e)
        {

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

        }
    }
}
