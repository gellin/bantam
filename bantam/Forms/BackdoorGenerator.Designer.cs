namespace bantam.Forms
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
            if (disposing && (components != null)) {
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
            this.chckbxGzipDecodeRequest = new System.Windows.Forms.CheckBox();
            this.labelOr = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxIVVarName = new System.Windows.Forms.TextBox();
            this.checkBoxSendIVInRequest = new System.Windows.Forms.CheckBox();
            this.buttonRandomIV = new System.Windows.Forms.Button();
            this.buttonRandomKey = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxEncrpytionIV = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxEncrpytionKey = new System.Windows.Forms.TextBox();
            this.checkBoxEncryptRequest = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkbxDisableLogging
            // 
            this.chkbxDisableLogging.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkbxDisableLogging.AutoSize = true;
            this.chkbxDisableLogging.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.chkbxDisableLogging.Location = new System.Drawing.Point(451, 417);
            this.chkbxDisableLogging.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkbxDisableLogging.Name = "chkbxDisableLogging";
            this.chkbxDisableLogging.Size = new System.Drawing.Size(156, 20);
            this.chkbxDisableLogging.TabIndex = 0;
            this.chkbxDisableLogging.Text = "Disable Error Logging";
            this.chkbxDisableLogging.UseVisualStyleBackColor = true;
            this.chkbxDisableLogging.CheckStateChanged += new System.EventHandler(this.chkbxDisableLogging_CheckStateChanged);
            // 
            // richTextBoxBackdoor
            // 
            this.richTextBoxBackdoor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxBackdoor.Location = new System.Drawing.Point(14, 34);
            this.richTextBoxBackdoor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.richTextBoxBackdoor.Name = "richTextBoxBackdoor";
            this.richTextBoxBackdoor.Size = new System.Drawing.Size(627, 369);
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
            this.comboBoxMethod.Location = new System.Drawing.Point(86, 413);
            this.comboBoxMethod.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxMethod.Name = "comboBoxMethod";
            this.comboBoxMethod.Size = new System.Drawing.Size(143, 24);
            this.comboBoxMethod.TabIndex = 3;
            this.comboBoxMethod.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lblMethod
            // 
            this.lblMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMethod.AutoSize = true;
            this.lblMethod.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMethod.Location = new System.Drawing.Point(10, 417);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(57, 16);
            this.lblMethod.TabIndex = 4;
            this.lblMethod.Text = "Method:";
            // 
            // lblRequestVarName
            // 
            this.lblRequestVarName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRequestVarName.AutoSize = true;
            this.lblRequestVarName.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequestVarName.Location = new System.Drawing.Point(10, 487);
            this.lblRequestVarName.Name = "lblRequestVarName";
            this.lblRequestVarName.Size = new System.Drawing.Size(70, 16);
            this.lblRequestVarName.TabIndex = 5;
            this.lblRequestVarName.Text = "Var Name:";
            // 
            // txtBoxVarName
            // 
            this.txtBoxVarName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtBoxVarName.Location = new System.Drawing.Point(86, 482);
            this.txtBoxVarName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBoxVarName.Name = "txtBoxVarName";
            this.txtBoxVarName.Size = new System.Drawing.Size(143, 24);
            this.txtBoxVarName.TabIndex = 6;
            this.txtBoxVarName.Text = "command";
            this.txtBoxVarName.TextChanged += new System.EventHandler(this.txtBoxVarName_TextChanged);
            // 
            // chkbxMinifyCode
            // 
            this.chkbxMinifyCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkbxMinifyCode.AutoSize = true;
            this.chkbxMinifyCode.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.chkbxMinifyCode.Location = new System.Drawing.Point(451, 450);
            this.chkbxMinifyCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkbxMinifyCode.Name = "chkbxMinifyCode";
            this.chkbxMinifyCode.Size = new System.Drawing.Size(97, 20);
            this.chkbxMinifyCode.TabIndex = 7;
            this.chkbxMinifyCode.Text = "Minify Code";
            this.chkbxMinifyCode.UseVisualStyleBackColor = true;
            this.chkbxMinifyCode.CheckedChanged += new System.EventHandler(this.chkbxMinifyCode_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 450);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 16);
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
            this.comboBoxVarType.Location = new System.Drawing.Point(86, 448);
            this.comboBoxVarType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxVarType.Name = "comboBoxVarType";
            this.comboBoxVarType.Size = new System.Drawing.Size(143, 24);
            this.comboBoxVarType.TabIndex = 9;
            this.comboBoxVarType.SelectedIndexChanged += new System.EventHandler(this.comboBoxVarType_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(656, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsToolStripMenuItem1});
            this.saveAsToolStripMenuItem.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.saveAsToolStripMenuItem.Text = "File";
            // 
            // saveAsToolStripMenuItem1
            // 
            this.saveAsToolStripMenuItem1.Name = "saveAsToolStripMenuItem1";
            this.saveAsToolStripMenuItem1.Size = new System.Drawing.Size(121, 22);
            this.saveAsToolStripMenuItem1.Text = "Save As";
            this.saveAsToolStripMenuItem1.Click += new System.EventHandler(this.saveAsToolStripMenuItem1_Click);
            // 
            // chckbxGzipDecodeRequest
            // 
            this.chckbxGzipDecodeRequest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chckbxGzipDecodeRequest.AutoSize = true;
            this.chckbxGzipDecodeRequest.Location = new System.Drawing.Point(451, 483);
            this.chckbxGzipDecodeRequest.Name = "chckbxGzipDecodeRequest";
            this.chckbxGzipDecodeRequest.Size = new System.Drawing.Size(159, 20);
            this.chckbxGzipDecodeRequest.TabIndex = 11;
            this.chckbxGzipDecodeRequest.Text = "Gzip Decode Requests";
            this.chckbxGzipDecodeRequest.UseVisualStyleBackColor = true;
            this.chckbxGzipDecodeRequest.CheckedChanged += new System.EventHandler(this.chckbxGzipDecodeRequest_CheckedChanged);
            // 
            // labelOr
            // 
            this.labelOr.AutoSize = true;
            this.labelOr.Location = new System.Drawing.Point(189, 645);
            this.labelOr.Name = "labelOr";
            this.labelOr.Size = new System.Drawing.Size(21, 16);
            this.labelOr.TabIndex = 37;
            this.labelOr.Text = "or";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 668);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 16);
            this.label8.TabIndex = 36;
            this.label8.Text = "IV Var Name:";
            // 
            // textBoxIVVarName
            // 
            this.textBoxIVVarName.Enabled = false;
            this.textBoxIVVarName.Location = new System.Drawing.Point(118, 665);
            this.textBoxIVVarName.MaxLength = 16;
            this.textBoxIVVarName.Name = "textBoxIVVarName";
            this.textBoxIVVarName.Size = new System.Drawing.Size(163, 24);
            this.textBoxIVVarName.TabIndex = 35;
            // 
            // checkBoxSendIVInRequest
            // 
            this.checkBoxSendIVInRequest.AutoSize = true;
            this.checkBoxSendIVInRequest.Enabled = false;
            this.checkBoxSendIVInRequest.Location = new System.Drawing.Point(145, 541);
            this.checkBoxSendIVInRequest.Name = "checkBoxSendIVInRequest";
            this.checkBoxSendIVInRequest.Size = new System.Drawing.Size(136, 20);
            this.checkBoxSendIVInRequest.TabIndex = 34;
            this.checkBoxSendIVInRequest.Text = "Send IV In Request";
            this.checkBoxSendIVInRequest.UseVisualStyleBackColor = true;
            this.checkBoxSendIVInRequest.CheckedChanged += new System.EventHandler(this.checkBoxSendIVInRequest_CheckedChanged);
            // 
            // buttonRandomIV
            // 
            this.buttonRandomIV.Enabled = false;
            this.buttonRandomIV.Image = global::bantam.Properties.Resources.generate_16x16;
            this.buttonRandomIV.Location = new System.Drawing.Point(287, 617);
            this.buttonRandomIV.Name = "buttonRandomIV";
            this.buttonRandomIV.Size = new System.Drawing.Size(24, 24);
            this.buttonRandomIV.TabIndex = 33;
            this.buttonRandomIV.UseVisualStyleBackColor = true;
            this.buttonRandomIV.Click += new System.EventHandler(this.buttonRandomIV_Click);
            // 
            // buttonRandomKey
            // 
            this.buttonRandomKey.Enabled = false;
            this.buttonRandomKey.Image = global::bantam.Properties.Resources.generate_16x16;
            this.buttonRandomKey.Location = new System.Drawing.Point(402, 578);
            this.buttonRandomKey.Name = "buttonRandomKey";
            this.buttonRandomKey.Size = new System.Drawing.Size(24, 24);
            this.buttonRandomKey.TabIndex = 32;
            this.buttonRandomKey.UseVisualStyleBackColor = true;
            this.buttonRandomKey.Click += new System.EventHandler(this.buttonRandomKey_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 578);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 16);
            this.label6.TabIndex = 31;
            this.label6.Text = "Encryption Key:";
            // 
            // textBoxEncrpytionIV
            // 
            this.textBoxEncrpytionIV.Enabled = false;
            this.textBoxEncrpytionIV.Location = new System.Drawing.Point(118, 617);
            this.textBoxEncrpytionIV.MaxLength = 16;
            this.textBoxEncrpytionIV.Name = "textBoxEncrpytionIV";
            this.textBoxEncrpytionIV.Size = new System.Drawing.Size(163, 24);
            this.textBoxEncrpytionIV.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 625);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 16);
            this.label2.TabIndex = 29;
            this.label2.Text = "Encryption IV:";
            // 
            // textBoxEncrpytionKey
            // 
            this.textBoxEncrpytionKey.Enabled = false;
            this.textBoxEncrpytionKey.Location = new System.Drawing.Point(118, 578);
            this.textBoxEncrpytionKey.MaxLength = 32;
            this.textBoxEncrpytionKey.Name = "textBoxEncrpytionKey";
            this.textBoxEncrpytionKey.Size = new System.Drawing.Size(278, 24);
            this.textBoxEncrpytionKey.TabIndex = 28;
            // 
            // checkBoxEncryptRequest
            // 
            this.checkBoxEncryptRequest.AutoSize = true;
            this.checkBoxEncryptRequest.Location = new System.Drawing.Point(14, 541);
            this.checkBoxEncryptRequest.Name = "checkBoxEncryptRequest";
            this.checkBoxEncryptRequest.Size = new System.Drawing.Size(121, 20);
            this.checkBoxEncryptRequest.TabIndex = 27;
            this.checkBoxEncryptRequest.Text = "Encrypt Request";
            this.checkBoxEncryptRequest.UseVisualStyleBackColor = true;
            this.checkBoxEncryptRequest.CheckedChanged += new System.EventHandler(this.checkBoxEncryptRequest_CheckedChanged);
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(13, 528);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(626, 1);
            this.label7.TabIndex = 38;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(287, 539);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 39;
            // 
            // BackdoorGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 711);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.labelOr);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxIVVarName);
            this.Controls.Add(this.checkBoxSendIVInRequest);
            this.Controls.Add(this.buttonRandomIV);
            this.Controls.Add(this.buttonRandomKey);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxEncrpytionIV);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxEncrpytionKey);
            this.Controls.Add(this.checkBoxEncryptRequest);
            this.Controls.Add(this.chckbxGzipDecodeRequest);
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
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BackdoorGenerator";
            this.ShowIcon = false;
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
        private System.Windows.Forms.CheckBox chckbxGzipDecodeRequest;
        private System.Windows.Forms.Label labelOr;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxIVVarName;
        private System.Windows.Forms.CheckBox checkBoxSendIVInRequest;
        private System.Windows.Forms.Button buttonRandomIV;
        private System.Windows.Forms.Button buttonRandomKey;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxEncrpytionIV;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxEncrpytionKey;
        private System.Windows.Forms.CheckBox checkBoxEncryptRequest;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}