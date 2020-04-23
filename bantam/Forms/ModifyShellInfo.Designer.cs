namespace bantam.Forms
{
    partial class ModifyShell
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxShellUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBoxArgName = new System.Windows.Forms.TextBox();
            this.btnUpdateShell = new System.Windows.Forms.Button();
            this.comboBoxVarType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAddShell = new System.Windows.Forms.Button();
            this.checkBoxResponseEncryption = new System.Windows.Forms.CheckBox();
            this.comboBoxEncryptionMode = new System.Windows.Forms.ComboBox();
            this.checkBoxGZipRequest = new System.Windows.Forms.CheckBox();
            this.labelDynAddHostsStatus = new System.Windows.Forms.Label();
            this.checkBoxEncryptRequest = new System.Windows.Forms.CheckBox();
            this.textBoxEncrpytionKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxEncrpytionIV = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonRandomKey = new System.Windows.Forms.Button();
            this.buttonRandomIV = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBoxSendIVInRequest = new System.Windows.Forms.CheckBox();
            this.textBoxIVVarName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.labelOr = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Shell URL";
            // 
            // txtBoxShellUrl
            // 
            this.txtBoxShellUrl.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.txtBoxShellUrl.Location = new System.Drawing.Point(22, 34);
            this.txtBoxShellUrl.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtBoxShellUrl.Name = "txtBoxShellUrl";
            this.txtBoxShellUrl.Size = new System.Drawing.Size(461, 24);
            this.txtBoxShellUrl.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(19, 138);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Code Var Name:";
            // 
            // txtBoxArgName
            // 
            this.txtBoxArgName.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.txtBoxArgName.Location = new System.Drawing.Point(134, 135);
            this.txtBoxArgName.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtBoxArgName.Name = "txtBoxArgName";
            this.txtBoxArgName.Size = new System.Drawing.Size(129, 24);
            this.txtBoxArgName.TabIndex = 4;
            this.txtBoxArgName.Text = "command";
            // 
            // btnUpdateShell
            // 
            this.btnUpdateShell.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateShell.Location = new System.Drawing.Point(339, 376);
            this.btnUpdateShell.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnUpdateShell.Name = "btnUpdateShell";
            this.btnUpdateShell.Size = new System.Drawing.Size(145, 44);
            this.btnUpdateShell.TabIndex = 6;
            this.btnUpdateShell.Text = "Update Shell";
            this.btnUpdateShell.UseVisualStyleBackColor = true;
            this.btnUpdateShell.Visible = false;
            this.btnUpdateShell.Click += new System.EventHandler(this.btnUpdateShell_Click);
            // 
            // comboBoxVarType
            // 
            this.comboBoxVarType.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.comboBoxVarType.FormattingEnabled = true;
            this.comboBoxVarType.Location = new System.Drawing.Point(134, 95);
            this.comboBoxVarType.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.comboBoxVarType.Name = "comboBoxVarType";
            this.comboBoxVarType.Size = new System.Drawing.Size(129, 24);
            this.comboBoxVarType.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(19, 94);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Request Method:";
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label5.Location = new System.Drawing.Point(22, 71);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(462, 2);
            this.label5.TabIndex = 9;
            // 
            // btnAddShell
            // 
            this.btnAddShell.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.btnAddShell.Location = new System.Drawing.Point(338, 376);
            this.btnAddShell.Name = "btnAddShell";
            this.btnAddShell.Size = new System.Drawing.Size(145, 45);
            this.btnAddShell.TabIndex = 10;
            this.btnAddShell.Text = "Add Shell";
            this.btnAddShell.UseVisualStyleBackColor = true;
            this.btnAddShell.Click += new System.EventHandler(this.btnAddShell_Click);
            // 
            // checkBoxResponseEncryption
            // 
            this.checkBoxResponseEncryption.AutoSize = true;
            this.checkBoxResponseEncryption.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxResponseEncryption.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.checkBoxResponseEncryption.Location = new System.Drawing.Point(22, 176);
            this.checkBoxResponseEncryption.Name = "checkBoxResponseEncryption";
            this.checkBoxResponseEncryption.Size = new System.Drawing.Size(131, 20);
            this.checkBoxResponseEncryption.TabIndex = 11;
            this.checkBoxResponseEncryption.Text = "Encrypt Response";
            this.checkBoxResponseEncryption.UseVisualStyleBackColor = true;
            this.checkBoxResponseEncryption.CheckedChanged += new System.EventHandler(this.checkBoxResponseEncryption_CheckedChanged);
            // 
            // comboBoxEncryptionMode
            // 
            this.comboBoxEncryptionMode.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.comboBoxEncryptionMode.FormattingEnabled = true;
            this.comboBoxEncryptionMode.Location = new System.Drawing.Point(163, 172);
            this.comboBoxEncryptionMode.Name = "comboBoxEncryptionMode";
            this.comboBoxEncryptionMode.Size = new System.Drawing.Size(100, 24);
            this.comboBoxEncryptionMode.TabIndex = 12;
            // 
            // checkBoxGZipRequest
            // 
            this.checkBoxGZipRequest.AutoSize = true;
            this.checkBoxGZipRequest.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.checkBoxGZipRequest.Location = new System.Drawing.Point(338, 94);
            this.checkBoxGZipRequest.Name = "checkBoxGZipRequest";
            this.checkBoxGZipRequest.Size = new System.Drawing.Size(136, 20);
            this.checkBoxGZipRequest.TabIndex = 13;
            this.checkBoxGZipRequest.Text = "GZip Request Data";
            this.checkBoxGZipRequest.UseVisualStyleBackColor = true;
            // 
            // labelDynAddHostsStatus
            // 
            this.labelDynAddHostsStatus.AutoSize = true;
            this.labelDynAddHostsStatus.ForeColor = System.Drawing.Color.Red;
            this.labelDynAddHostsStatus.Location = new System.Drawing.Point(19, 404);
            this.labelDynAddHostsStatus.Name = "labelDynAddHostsStatus";
            this.labelDynAddHostsStatus.Size = new System.Drawing.Size(0, 16);
            this.labelDynAddHostsStatus.TabIndex = 14;
            // 
            // checkBoxEncryptRequest
            // 
            this.checkBoxEncryptRequest.AutoSize = true;
            this.checkBoxEncryptRequest.Location = new System.Drawing.Point(22, 231);
            this.checkBoxEncryptRequest.Name = "checkBoxEncryptRequest";
            this.checkBoxEncryptRequest.Size = new System.Drawing.Size(121, 20);
            this.checkBoxEncryptRequest.TabIndex = 15;
            this.checkBoxEncryptRequest.Text = "Encrypt Request";
            this.checkBoxEncryptRequest.UseVisualStyleBackColor = true;
            this.checkBoxEncryptRequest.CheckedChanged += new System.EventHandler(this.checkBoxEncryptRequest_CheckedChanged);
            // 
            // textBoxEncrpytionKey
            // 
            this.textBoxEncrpytionKey.Enabled = false;
            this.textBoxEncrpytionKey.Location = new System.Drawing.Point(122, 267);
            this.textBoxEncrpytionKey.MaxLength = 32;
            this.textBoxEncrpytionKey.Name = "textBoxEncrpytionKey";
            this.textBoxEncrpytionKey.Size = new System.Drawing.Size(278, 24);
            this.textBoxEncrpytionKey.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 314);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "Encryption IV:";
            // 
            // textBoxEncrpytionIV
            // 
            this.textBoxEncrpytionIV.Enabled = false;
            this.textBoxEncrpytionIV.Location = new System.Drawing.Point(122, 306);
            this.textBoxEncrpytionIV.MaxLength = 16;
            this.textBoxEncrpytionIV.Name = "textBoxEncrpytionIV";
            this.textBoxEncrpytionIV.Size = new System.Drawing.Size(163, 24);
            this.textBoxEncrpytionIV.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 270);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 16);
            this.label6.TabIndex = 19;
            this.label6.Text = "Encryption Key:";
            // 
            // buttonRandomKey
            // 
            this.buttonRandomKey.Enabled = false;
            this.buttonRandomKey.Image = global::bantam.Properties.Resources.generate_16x16;
            this.buttonRandomKey.Location = new System.Drawing.Point(406, 267);
            this.buttonRandomKey.Name = "buttonRandomKey";
            this.buttonRandomKey.Size = new System.Drawing.Size(24, 24);
            this.buttonRandomKey.TabIndex = 20;
            this.buttonRandomKey.UseVisualStyleBackColor = true;
            this.buttonRandomKey.Click += new System.EventHandler(this.buttonRandomKey_Click);
            // 
            // buttonRandomIV
            // 
            this.buttonRandomIV.Enabled = false;
            this.buttonRandomIV.Image = global::bantam.Properties.Resources.generate_16x16;
            this.buttonRandomIV.Location = new System.Drawing.Point(291, 306);
            this.buttonRandomIV.Name = "buttonRandomIV";
            this.buttonRandomIV.Size = new System.Drawing.Size(24, 24);
            this.buttonRandomIV.TabIndex = 21;
            this.buttonRandomIV.UseVisualStyleBackColor = true;
            this.buttonRandomIV.Click += new System.EventHandler(this.buttonRandomIV_Click);
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(18, 219);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(462, 2);
            this.label7.TabIndex = 22;
            // 
            // checkBoxSendIVInRequest
            // 
            this.checkBoxSendIVInRequest.AutoSize = true;
            this.checkBoxSendIVInRequest.Enabled = false;
            this.checkBoxSendIVInRequest.Location = new System.Drawing.Point(149, 231);
            this.checkBoxSendIVInRequest.Name = "checkBoxSendIVInRequest";
            this.checkBoxSendIVInRequest.Size = new System.Drawing.Size(136, 20);
            this.checkBoxSendIVInRequest.TabIndex = 23;
            this.checkBoxSendIVInRequest.Text = "Send IV In Request";
            this.checkBoxSendIVInRequest.UseVisualStyleBackColor = true;
            this.checkBoxSendIVInRequest.CheckedChanged += new System.EventHandler(this.checkBoxSendIVInRequest_CheckedChanged);
            // 
            // textBoxIVVarName
            // 
            this.textBoxIVVarName.Enabled = false;
            this.textBoxIVVarName.Location = new System.Drawing.Point(122, 354);
            this.textBoxIVVarName.MaxLength = 16;
            this.textBoxIVVarName.Name = "textBoxIVVarName";
            this.textBoxIVVarName.Size = new System.Drawing.Size(163, 24);
            this.textBoxIVVarName.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 362);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 16);
            this.label8.TabIndex = 25;
            this.label8.Text = "IV Var Name:";
            // 
            // labelOr
            // 
            this.labelOr.AutoSize = true;
            this.labelOr.Location = new System.Drawing.Point(193, 334);
            this.labelOr.Name = "labelOr";
            this.labelOr.Size = new System.Drawing.Size(21, 16);
            this.labelOr.TabIndex = 26;
            this.labelOr.Text = "or";
            // 
            // ModifyShell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 433);
            this.Controls.Add(this.labelOr);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxIVVarName);
            this.Controls.Add(this.checkBoxSendIVInRequest);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.buttonRandomIV);
            this.Controls.Add(this.buttonRandomKey);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxEncrpytionIV);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxEncrpytionKey);
            this.Controls.Add(this.checkBoxEncryptRequest);
            this.Controls.Add(this.labelDynAddHostsStatus);
            this.Controls.Add(this.checkBoxGZipRequest);
            this.Controls.Add(this.comboBoxEncryptionMode);
            this.Controls.Add(this.checkBoxResponseEncryption);
            this.Controls.Add(this.btnAddShell);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxVarType);
            this.Controls.Add(this.btnUpdateShell);
            this.Controls.Add(this.txtBoxArgName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBoxShellUrl);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModifyShell";
            this.ShowIcon = false;
            this.Text = "Add Shell";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxShellUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBoxArgName;
        private System.Windows.Forms.Button btnUpdateShell;
        private System.Windows.Forms.ComboBox comboBoxVarType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAddShell;
        private System.Windows.Forms.CheckBox checkBoxResponseEncryption;
        private System.Windows.Forms.ComboBox comboBoxEncryptionMode;
        private System.Windows.Forms.CheckBox checkBoxGZipRequest;
        private System.Windows.Forms.Label labelDynAddHostsStatus;
        private System.Windows.Forms.CheckBox checkBoxEncryptRequest;
        private System.Windows.Forms.TextBox textBoxEncrpytionKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxEncrpytionIV;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonRandomKey;
        private System.Windows.Forms.Button buttonRandomIV;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBoxSendIVInRequest;
        private System.Windows.Forms.TextBox textBoxIVVarName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelOr;
    }
}