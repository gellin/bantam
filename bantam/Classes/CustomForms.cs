using System;
using System.Windows.Forms;

namespace bantam_php
{
    /// <summary>
    /// 
    /// </summary>
    public static class CustomForms
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldFileName"></param>
        /// <param name="windowTitle"></param>
        /// <returns></returns>
        public static string RenameFileDialog(string oldFileName, string windowTitle)
        {
            //oldFileName = "Current File Name: " + oldFileName;
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = windowTitle,
                StartPosition = FormStartPosition.CenterScreen,
                MinimumSize = new System.Drawing.Size(500, 150)
            };

            Label textLabel = new Label()
            {
                Left = 50,
                Top = 20,
                Text = oldFileName,
                Width = 400
            };

            TextBox textBox = new TextBox()
            {
                Left = 50,
                Top = 50,
                Width = 400
            };

            Button cancel = new Button()
            {
                Text = "Cancel",
                Left = 300,
                Width = 100,
                Top = 70,
                DialogResult = DialogResult.Cancel
            };

            Button confirmation = new Button()
            {
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
            Form prompt = new Form()
            {
                Width = 500,
                Height = 500,
                FormBorderStyle = FormBorderStyle.SizableToolWindow,
                Text = windowTitle,
                StartPosition = FormStartPosition.CenterScreen,
                MinimumSize = new System.Drawing.Size(180, 300)
            };

            RichTextBox richTextBox = new RichTextBox()
            {
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
            Form prompt = new Form()
            {
                Width = 500,
                Height = 520,
                FormBorderStyle = FormBorderStyle.SizableToolWindow,
                Text = windowTitle,
                StartPosition = FormStartPosition.CenterScreen,
                MinimumSize = new System.Drawing.Size(300, 300)
            };

            RichTextBox richTextBox = new RichTextBox()
            {
                Left = 10,
                Top = 10,
                Width = 470,
                Height = 440,
                Text = text
            };

            Button cancel = new Button()
            {
                Text = "Cancel",
                Left = 330,
                Width = 50,
                Top = 455,
                DialogResult = DialogResult.Cancel,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom
            };

            Button confirmation = new Button()
            {
                Text = "Ok",
                Left = 380,
                Width = 100,
                Top = 455,
                DialogResult = DialogResult.OK,
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom
            };

            CheckBox chkbxShowResponse = new CheckBox()
            {
                Text = "Show Response",
                Left = 25,
                Top = 455,
                Checked = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom
            };

            confirmation.Click += (sender, e) => { prompt.Close(); };
            cancel.Click += (sender, e) => { prompt.Close(); };

            //C# doesn't allow lambda functions to play with out/ref vars.... :(
            //set the tmp var equal to that of the checkbox status to return through the out parameter
            bool chkboxResult = false;
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
    }
}
