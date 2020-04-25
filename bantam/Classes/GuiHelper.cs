using System.Windows.Forms;

namespace bantam.Classes
{
    static class GuiHelper
    {
        /// <summary>
        /// Gets the first selected item from the specified ListView
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
        /// Recursively copies all the Treeview Nodes from the source to the dest
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public static void CopyNodesFromTreeView(TreeView source, TreeView dest)
        {
            if (source != null && dest != null) {
                foreach (TreeNode tn in source.Nodes) {
                    TreeNode newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.ImageIndex) {
                        ForeColor = tn.ForeColor,
                        Name = tn.Name
                    };
                    CopyChildrenFromTreeViewNode(newTn, tn);
                    dest.Nodes.Add(newTn);
                }
            }
        }

        /// <summary>
        /// Recursively copies Treeview Child Nodes
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="willCopied"></param>
        public static void CopyChildrenFromTreeViewNode(TreeNode parent, TreeNode willCopied)
        {
            foreach (TreeNode tn in willCopied.Nodes) {
                TreeNode newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.ImageIndex) {
                    ForeColor = tn.ForeColor,
                    Name = tn.Name
                };
                CopyChildrenFromTreeViewNode(newTn, tn);
                parent.Nodes.Add(newTn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Form BuildForm(string title, int width, int height)
        {
            Form prompt = new Form {
                Width = width,
                Height = height,
                FormBorderStyle = FormBorderStyle.FixedSingle,
                Text = title,
                StartPosition = FormStartPosition.CenterScreen,
                MinimumSize = new System.Drawing.Size(width, height)
            };
            return prompt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUserAgent"></param>
        /// <param name="windowTitle"></param>
        /// <returns></returns>
        public static string UserAgentSwitcher(string currentUserAgent, string windowTitle)
        {
            Form prompt = BuildForm(windowTitle, 500, 150);

            Label textLabel = new Label {
                Left = 17,
                Top = 20,
                Text = currentUserAgent,
                Width = 450
            };

            TextBox textBox = new TextBox {
                Left = 17,
                Top = 50,
                Width = 450
            };

            Button cancel = new Button {
                Text = "Cancel",
                Left = 200,
                Width = 100,
                Top = 80,
                DialogResult = DialogResult.Cancel
            };

            Button randomize = new Button {
                Text = "Random",
                Left = 300,
                Width = 100,
                Top = 80,
            };

            Button confirmation = new Button {
                Text = "Ok",
                Left = 400,
                Width = 50,
                Top = 80,
                DialogResult = DialogResult.OK
            };

            randomize.Click += (sender, e) =>
            {
                textBox.Text = WebRequestHelper.commonUseragents[Helper.RandomDictionaryValue(WebRequestHelper.commonUseragents)];
            };

            cancel.Click += (sender, e) => { prompt.Close(); };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(randomize);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(cancel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldFileName"></param>
        /// <param name="windowTitle"></param>
        /// <returns></returns>
        public static string RenameFileDialog(string oldFileName, string windowTitle)
        {
            Form prompt = BuildForm(windowTitle, 500, 150);

            Label textLabel = new Label {
                Left = 50,
                Top = 20,
                Text = oldFileName,
                Width = 400
            };

            TextBox textBox = new TextBox {
                Left = 50,
                Top = 50,
                Width = 400
            };

            Button cancel = new Button {
                Text = "Cancel",
                Left = 300,
                Width = 100,
                Top = 70,
                DialogResult = DialogResult.Cancel
            };

            Button confirmation = new Button {
                Text = "Ok",
                Left = 400,
                Width = 50,
                Top = 70,
                DialogResult = DialogResult.OK
            };

            confirmation.Click += (sender, e) => { prompt.Close(); };
            cancel.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(cancel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <param name="text"></param>
        public static RichTextBox RichTextBoxDialog(string windowTitle, string text)
        {
            Form prompt = BuildForm(windowTitle, 500, 500);

            RichTextBox richTextBox = new RichTextBox {
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
            prompt.Show();

            return richTextBox;
        }

        /// <summary>
        /// Returns a string of PHP code
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <param name="text"></param>
        /// <param name="showResponse"></param>
        /// <returns></returns>
        public static string RichTextBoxEvalEditor(string windowTitle, string text, ref bool showResponse)
        {
            Form prompt = BuildForm(windowTitle, 500, 520);

            RichTextBox richTextBox = new RichTextBox {
                Left = 10,
                Top = 10,
                Width = 470,
                Height = 440,
                Text = text
            };

            Button confirmation = new Button {
                Text = "Ok",
                Left = 380,
                Width = 100,
                Top = 455,
                DialogResult = DialogResult.OK,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom
            };

            CheckBox chkbxShowResponse = new CheckBox {
                Text = "Show Response",
                Left = 25,
                Top = 455,
                Checked = showResponse,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom
            };

            bool chkboxResult = showResponse;
            chkbxShowResponse.CheckedChanged += (sender, e) => { chkboxResult = chkbxShowResponse.Checked; };

            richTextBox.WordWrap = false;
            richTextBox.ScrollBars = RichTextBoxScrollBars.Vertical | RichTextBoxScrollBars.Horizontal;
            richTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;

            prompt.Controls.Add(richTextBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(chkbxShowResponse);

            string result = prompt.ShowDialog() == DialogResult.OK ? richTextBox.Text : string.Empty;

            confirmation.Click += (sender, e) => { prompt.Close(); };

            showResponse = chkboxResult;

            return result;
        }
    }
}
