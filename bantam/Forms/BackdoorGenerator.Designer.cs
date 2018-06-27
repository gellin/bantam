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
            this.richTextBoxBackdoor = new System.Windows.Forms.RichTextBox();
            this.comboBoxMethod = new System.Windows.Forms.ComboBox();
            this.lblMethod = new System.Windows.Forms.Label();
            this.lblRequestVarName = new System.Windows.Forms.Label();
            this.txtBoxVarName = new System.Windows.Forms.TextBox();
            this.chkbxMinifyCode = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxVarType = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkbxDisableLogging
            // 
            this.chkbxDisableLogging.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkbxDisableLogging.AutoSize = true;
            this.chkbxDisableLogging.Location = new System.Drawing.Point(402, 384);
            this.chkbxDisableLogging.Name = "chkbxDisableLogging";
            this.chkbxDisableLogging.Size = new System.Drawing.Size(127, 17);
            this.chkbxDisableLogging.TabIndex = 0;
            this.chkbxDisableLogging.Text = "Disable Error Logging";
            this.chkbxDisableLogging.UseVisualStyleBackColor = true;
            this.chkbxDisableLogging.CheckStateChanged += new System.EventHandler(this.chkbxDisableLogging_CheckStateChanged);
            // 
            // richTextBox1
            // 
            this.richTextBoxBackdoor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxBackdoor.Location = new System.Drawing.Point(12, 28);
            this.richTextBoxBackdoor.Name = "richTextBox1";
            this.richTextBoxBackdoor.Size = new System.Drawing.Size(527, 347);
            this.richTextBoxBackdoor.TabIndex = 2;
            this.richTextBoxBackdoor.Text = "";
            // 
            // comboBoxMethod
            // 
            this.comboBoxMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxMethod.FormattingEnabled = true;
            this.comboBoxMethod.Items.AddRange(new object[] {
            "eval",
            "assert",
            "create_function",
            "native anonymous",
            "tmp include",
            "preg_replace"});
            this.comboBoxMethod.Location = new System.Drawing.Point(72, 381);
            this.comboBoxMethod.Name = "comboBoxMethod";
            this.comboBoxMethod.Size = new System.Drawing.Size(123, 21);
            this.comboBoxMethod.TabIndex = 3;
            this.comboBoxMethod.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lblMethod
            // 
            this.lblMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMethod.AutoSize = true;
            this.lblMethod.Location = new System.Drawing.Point(9, 385);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(46, 13);
            this.lblMethod.TabIndex = 4;
            this.lblMethod.Text = "Method:";
            // 
            // lblRequestVarName
            // 
            this.lblRequestVarName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRequestVarName.AutoSize = true;
            this.lblRequestVarName.Location = new System.Drawing.Point(9, 442);
            this.lblRequestVarName.Name = "lblRequestVarName";
            this.lblRequestVarName.Size = new System.Drawing.Size(57, 13);
            this.lblRequestVarName.TabIndex = 5;
            this.lblRequestVarName.Text = "Var Name:";
            // 
            // txtBoxVarName
            // 
            this.txtBoxVarName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtBoxVarName.Location = new System.Drawing.Point(72, 439);
            this.txtBoxVarName.Name = "txtBoxVarName";
            this.txtBoxVarName.Size = new System.Drawing.Size(123, 20);
            this.txtBoxVarName.TabIndex = 6;
            this.txtBoxVarName.Text = "command";
            this.txtBoxVarName.TextChanged += new System.EventHandler(this.txtBoxVarName_TextChanged);
            // 
            // chkbxMinifyCode
            // 
            this.chkbxMinifyCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkbxMinifyCode.AutoSize = true;
            this.chkbxMinifyCode.Location = new System.Drawing.Point(402, 414);
            this.chkbxMinifyCode.Name = "chkbxMinifyCode";
            this.chkbxMinifyCode.Size = new System.Drawing.Size(81, 17);
            this.chkbxMinifyCode.TabIndex = 7;
            this.chkbxMinifyCode.Text = "Minify Code";
            this.chkbxMinifyCode.UseVisualStyleBackColor = true;
            this.chkbxMinifyCode.CheckedChanged += new System.EventHandler(this.chkbxMinifyCode_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 414);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Var Type:";
            // 
            // comboBoxVarType
            // 
            this.comboBoxVarType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxVarType.FormattingEnabled = true;
            this.comboBoxVarType.Items.AddRange(new object[] {
            "cookie",
            "post",
            "request"});
            this.comboBoxVarType.Location = new System.Drawing.Point(72, 411);
            this.comboBoxVarType.Name = "comboBoxVarType";
            this.comboBoxVarType.Size = new System.Drawing.Size(123, 21);
            this.comboBoxVarType.TabIndex = 9;
            this.comboBoxVarType.SelectedIndexChanged += new System.EventHandler(this.comboBoxVarType_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(551, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsToolStripMenuItem1});
            this.saveAsToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.saveAsToolStripMenuItem.Text = "File";
            // 
            // saveAsToolStripMenuItem1
            // 
            this.saveAsToolStripMenuItem1.Name = "saveAsToolStripMenuItem1";
            this.saveAsToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.saveAsToolStripMenuItem1.Text = "Save As";
            this.saveAsToolStripMenuItem1.Click += new System.EventHandler(this.saveAsToolStripMenuItem1_Click);
            // 
            // BackdoorGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 493);
            this.Controls.Add(this.comboBoxVarType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkbxMinifyCode);
            this.Controls.Add(this.txtBoxVarName);
            this.Controls.Add(this.lblRequestVarName);
            this.Controls.Add(this.lblMethod);
            this.Controls.Add(this.comboBoxMethod);
            this.Controls.Add(this.richTextBoxBackdoor);
            this.Controls.Add(this.chkbxDisableLogging);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "BackdoorGenerator";
            this.Text = "PHP Backdoor Generator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkbxDisableLogging;
        private System.Windows.Forms.RichTextBox richTextBoxBackdoor;
        private System.Windows.Forms.ComboBox comboBoxMethod;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.Label lblRequestVarName;
        private System.Windows.Forms.TextBox txtBoxVarName;
        private System.Windows.Forms.CheckBox chkbxMinifyCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxVarType;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem1;
    }
}