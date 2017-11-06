namespace bantam_php
{
    partial class BackdoorGenerator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkbxDisableLogging = new System.Windows.Forms.CheckBox();
            this.chkbxUseCookie = new System.Windows.Forms.CheckBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // chkbxDisableLogging
            // 
            this.chkbxDisableLogging.AutoSize = true;
            this.chkbxDisableLogging.Location = new System.Drawing.Point(307, 299);
            this.chkbxDisableLogging.Name = "chkbxDisableLogging";
            this.chkbxDisableLogging.Size = new System.Drawing.Size(127, 17);
            this.chkbxDisableLogging.TabIndex = 0;
            this.chkbxDisableLogging.Text = "Disable Error Logging";
            this.chkbxDisableLogging.UseVisualStyleBackColor = true;
            // 
            // chkbxUseCookie
            // 
            this.chkbxUseCookie.AutoSize = true;
            this.chkbxUseCookie.Location = new System.Drawing.Point(307, 276);
            this.chkbxUseCookie.Name = "chkbxUseCookie";
            this.chkbxUseCookie.Size = new System.Drawing.Size(128, 17);
            this.chkbxUseCookie.TabIndex = 1;
            this.chkbxUseCookie.Text = "Cookie Request Data";
            this.chkbxUseCookie.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(443, 242);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "eval",
            "assert",
            "create_function",
            "native anonymous",
            "tmp include"});
            this.comboBox1.Location = new System.Drawing.Point(12, 276);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(142, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // BackdoorGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 365);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.chkbxUseCookie);
            this.Controls.Add(this.chkbxDisableLogging);
            this.Name = "BackdoorGenerator";
            this.Text = "PayloadGenerator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkbxDisableLogging;
        private System.Windows.Forms.CheckBox chkbxUseCookie;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}