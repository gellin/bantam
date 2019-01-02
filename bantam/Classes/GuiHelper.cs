using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace bantam_php
{
    class GuiHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public class TextBoxButton : TextBox
        {
            Button btnBack = new Button();

            public TextBoxButton()
            {
                //InitializeComponent();

            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="mouseClickFunction"></param>
            /// <param name="textboxHeight"></param>
            public void Initialize(MouseEventHandler mouseClickFunction, int textboxHeight)
            {
                btnBack.Size = new Size(25, textboxHeight + 2);
                btnBack.Location = new Point(1, -1);
                btnBack.Image = global::bantam.Properties.Resources.undo;
                // btnBack.ImageAlign = ContentAlignment.MiddleLeft;
                btnBack.MouseClick += mouseClickFunction;
                btnBack.TabStop = false;
                btnBack.FlatStyle = FlatStyle.Flat;
                btnBack.FlatAppearance.BorderSize = 0;
                btnBack.Cursor = System.Windows.Forms.Cursors.Hand;

                this.Controls.Add(btnBack);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="msg"></param>
            protected override void WndProc(ref Message msg)
            {
                base.WndProc(ref msg);

                switch (msg.Msg) {
                    case 0x30:
                        SendMessage(this.Handle, 0xd3, (IntPtr)1, (IntPtr)btnBack.Width);
                        //SendMessage(this.Handle, 0xd3, (IntPtr)2, (IntPtr)(btnBack.Width << 16));
                        break;
                    default:
                        break;
                }
            }

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUserAgent"></param>
        /// <param name="windowTitle"></param>
        /// <returns></returns>
        public static string UserAgentSwitcher(string currentUserAgent, string windowTitle)
        {
            Form prompt = new Form() {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = windowTitle,
                StartPosition = FormStartPosition.CenterScreen,
                MinimumSize = new System.Drawing.Size(500, 150)
            };

            Label textLabel = new Label() {
                Left = 17,
                Top = 20,
                Text = currentUserAgent,
                Width = 450
            };

            TextBox textBox = new TextBox() {
                Left = 17,
                Top = 50,
                Width = 450
            };

            Button cancel = new Button() {
                Text = "Cancel",
                Left = 200,
                Width = 100,
                Top = 80,
                DialogResult = DialogResult.Cancel
            };

            Button randomize = new Button() {
                Text = "Random",
                Left = 300,
                Width = 100,
                Top = 80,
            };

            Button confirmation = new Button() {
                Text = "Ok",
                Left = 400,
                Width = 50,
                Top = 80,
                DialogResult = DialogResult.OK
            };

            randomize.Click += (sender, e) => { textBox.Text = WebHelper.commonUseragents[Helper.RandomDicionaryValue(WebHelper.commonUseragents)]; };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            cancel.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(randomize);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(cancel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldFileName"></param>
        /// <param name="windowTitle"></param>
        /// <returns></returns>
        public static string RenameFileDialog(string oldFileName, string windowTitle)
        {
            //oldFileName = "Current File Name: " + oldFileName;
            Form prompt = new Form() {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = windowTitle,
                StartPosition = FormStartPosition.CenterScreen,
                MinimumSize = new System.Drawing.Size(500, 150)
            };

            Label textLabel = new Label() {
                Left = 50,
                Top = 20,
                Text = oldFileName,
                Width = 400
            };

            TextBox textBox = new TextBox() {
                Left = 50,
                Top = 50,
                Width = 400
            };

            Button cancel = new Button() {
                Text = "Cancel",
                Left = 300,
                Width = 100,
                Top = 70,
                DialogResult = DialogResult.Cancel
            };

            Button confirmation = new Button() {
                Text = "Ok",
                Left = 400,
                Width = 50,
                Top = 70,
                DialogResult = DialogResult.OK
            };

            confirmation.Click += (sender, e) => { prompt.Close(); };
            cancel.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(cancel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <param name="text"></param>
        public static void RichTextBoxDialog(string windowTitle, string text)
        {
            //oldFileName = "Current File Name: " + oldFileName;
            Form prompt = new Form() {
                Width = 500,
                Height = 500,
                FormBorderStyle = FormBorderStyle.SizableToolWindow,
                Text = windowTitle,
                StartPosition = FormStartPosition.CenterScreen,
                MinimumSize = new System.Drawing.Size(180, 300)
            };

            RichTextBox richTextBox = new RichTextBox() {
                Left = 10,
                Top = 10,
                Width = 470,
                Height = 440,
                Text = text,
                ReadOnly = true
            };

            richTextBox.WordWrap = false;
            richTextBox.ScrollBars = RichTextBoxScrollBars.Vertical | RichTextBoxScrollBars.Horizontal;
            richTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;

            prompt.Controls.Add(richTextBox);
            prompt.ShowDialog();
        }

        /// <summary>
        /// Returns a string of PHP code
        /// </summary>
        /// 
        /// <param name="windowTitle"></param>
        /// <param name="text"></param>
        public static string RichTextBoxEvalEditor(string windowTitle, string text, ref bool showResponse)
        {
            //oldFileName = "Current File Name: " + oldFileName;
            Form prompt = new Form() {
                Width = 500,
                Height = 520,
                FormBorderStyle = FormBorderStyle.SizableToolWindow,
                Text = windowTitle,
                StartPosition = FormStartPosition.CenterScreen,
                MinimumSize = new System.Drawing.Size(300, 300)
            };

            RichTextBox richTextBox = new RichTextBox() {
                Left = 10,
                Top = 10,
                Width = 470,
                Height = 440,
                Text = text
            };

            Button cancel = new Button() {
                Text = "Cancel",
                Left = 330,
                Width = 50,
                Top = 455,
                DialogResult = DialogResult.Cancel,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom
            };

            Button confirmation = new Button() {
                Text = "Ok",
                Left = 380,
                Width = 100,
                Top = 455,
                DialogResult = DialogResult.OK,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom
            };

            CheckBox chkbxShowResponse = new CheckBox() {
                Text = "Show Response",
                Left = 25,
                Top = 455,
                Checked = showResponse,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom
            };

            confirmation.Click += (sender, e) => { prompt.Close(); };
            cancel.Click += (sender, e) => { prompt.Close(); };

            //C# doesn't allow lambda functions to play with out/ref vars.... :(
            //set the tmp var equal to that of the checkbox status to return through the out parameter
            bool chkboxResult = showResponse;
            chkbxShowResponse.CheckedChanged += (sender, e) => { chkboxResult = chkbxShowResponse.Checked; };

            richTextBox.WordWrap = false;
            richTextBox.ScrollBars = RichTextBoxScrollBars.Vertical | RichTextBoxScrollBars.Horizontal;
            richTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;

            prompt.Controls.Add(richTextBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.Controls.Add(chkbxShowResponse);

            //show dialog and wait for result
            string result = prompt.ShowDialog() == DialogResult.OK ? richTextBox.Text : "";

            //set the ref var to the chkboxResult result value
            showResponse = chkboxResult;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static ListViewItem GetFirstSelectedListview(ListView lv)
        {
            if (lv.SelectedItems.Count > 0) {
                foreach (ListViewItem lvi in lv.SelectedItems) {
                    return lvi;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public static void CopyNodesFromTreeView(TreeView source, TreeView dest)
        {
            foreach (TreeNode tn in source.Nodes) {
                TreeNode newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.ImageIndex) {
                    ForeColor = tn.ForeColor
                };
                CopyChildrenFromTreeViewNode(newTn, tn);
                dest.Nodes.Add(newTn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="willCopied"></param>
        public static void CopyChildrenFromTreeViewNode(TreeNode parent, TreeNode willCopied)
        {
            foreach (TreeNode tn in willCopied.Nodes) {
                TreeNode newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.ImageIndex) {
                    ForeColor = tn.ForeColor
                };
                CopyChildrenFromTreeViewNode(newTn, tn);
                parent.Nodes.Add(newTn);
            }
        }
    }
}
