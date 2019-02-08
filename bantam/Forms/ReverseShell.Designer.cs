namespace bantam.Forms
{
    partial class ReverseShell
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
            this.labelIP = new System.Windows.Forms.Label();
            this.buttonGetIpv4 = new System.Windows.Forms.Button();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelMethod = new System.Windows.Forms.Label();
            this.comboBoxMethod = new System.Windows.Forms.ComboBox();
            this.btnPopShell = new System.Windows.Forms.Button();
            this.checkBoxDisabledFunctionsBypass = new System.Windows.Forms.CheckBox();
            this.comboBoxArch = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.checkBoxLogShellCode = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(12, 22);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(21, 16);
            this.labelIP.TabIndex = 0;
            this.labelIP.Text = "IP:";
            // 
            // buttonGetIpv4
            // 
            this.buttonGetIpv4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetIpv4.Location = new System.Drawing.Point(294, 16);
            this.buttonGetIpv4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonGetIpv4.Name = "buttonGetIpv4";
            this.buttonGetIpv4.Size = new System.Drawing.Size(105, 28);
            this.buttonGetIpv4.TabIndex = 1;
            this.buttonGetIpv4.Text = "Get My IPv4";
            this.buttonGetIpv4.UseVisualStyleBackColor = true;
            this.buttonGetIpv4.Click += new System.EventHandler(this.buttonGetIpv4_Click);
            // 
            // textBoxIP
            // 
            this.textBoxIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxIP.Location = new System.Drawing.Point(84, 18);
            this.textBoxIP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(200, 24);
            this.textBoxIP.TabIndex = 2;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(84, 50);
            this.textBoxPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(58, 24);
            this.textBoxPort.TabIndex = 3;
            this.textBoxPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPort_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Port:";
            // 
            // labelMethod
            // 
            this.labelMethod.AutoSize = true;
            this.labelMethod.Location = new System.Drawing.Point(12, 90);
            this.labelMethod.Name = "labelMethod";
            this.labelMethod.Size = new System.Drawing.Size(57, 16);
            this.labelMethod.TabIndex = 5;
            this.labelMethod.Text = "Method:";
            // 
            // comboBoxMethod
            // 
            this.comboBoxMethod.FormattingEnabled = true;
            this.comboBoxMethod.Location = new System.Drawing.Point(84, 87);
            this.comboBoxMethod.Name = "comboBoxMethod";
            this.comboBoxMethod.Size = new System.Drawing.Size(133, 24);
            this.comboBoxMethod.TabIndex = 6;
            // 
            // btnPopShell
            // 
            this.btnPopShell.Location = new System.Drawing.Point(296, 199);
            this.btnPopShell.Name = "btnPopShell";
            this.btnPopShell.Size = new System.Drawing.Size(109, 40);
            this.btnPopShell.TabIndex = 7;
            this.btnPopShell.Text = "Pop Shell";
            this.btnPopShell.UseVisualStyleBackColor = true;
            this.btnPopShell.Click += new System.EventHandler(this.btnPopShell_Click);
            // 
            // checkBoxDisabledFunctionsBypass
            // 
            this.checkBoxDisabledFunctionsBypass.AutoSize = true;
            this.checkBoxDisabledFunctionsBypass.Location = new System.Drawing.Point(15, 144);
            this.checkBoxDisabledFunctionsBypass.Name = "checkBoxDisabledFunctionsBypass";
            this.checkBoxDisabledFunctionsBypass.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBoxDisabledFunctionsBypass.Size = new System.Drawing.Size(263, 20);
            this.checkBoxDisabledFunctionsBypass.TabIndex = 8;
            this.checkBoxDisabledFunctionsBypass.Text = "disable_functions / open_basedir bypass";
            this.checkBoxDisabledFunctionsBypass.UseVisualStyleBackColor = true;
            this.checkBoxDisabledFunctionsBypass.CheckedChanged += new System.EventHandler(this.checkBoxDisabledFunctionsBypass_CheckedChanged);
            // 
            // comboBoxArch
            // 
            this.comboBoxArch.Enabled = false;
            this.comboBoxArch.FormattingEnabled = true;
            this.comboBoxArch.Items.AddRange(new object[] {
            "x64",
            "x86"});
            this.comboBoxArch.Location = new System.Drawing.Point(14, 170);
            this.comboBoxArch.Name = "comboBoxArch";
            this.comboBoxArch.Size = new System.Drawing.Size(101, 24);
            this.comboBoxArch.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(14, 127);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(391, 1);
            this.label7.TabIndex = 23;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 245);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 16);
            this.lblStatus.TabIndex = 24;
            // 
            // checkBoxLogShellCode
            // 
            this.checkBoxLogShellCode.AutoSize = true;
            this.checkBoxLogShellCode.Location = new System.Drawing.Point(14, 219);
            this.checkBoxLogShellCode.Name = "checkBoxLogShellCode";
            this.checkBoxLogShellCode.Size = new System.Drawing.Size(115, 20);
            this.checkBoxLogShellCode.TabIndex = 25;
            this.checkBoxLogShellCode.Text = "Log Shell Code";
            this.checkBoxLogShellCode.UseVisualStyleBackColor = true;
            // 
            // ReverseShell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 266);
            this.Controls.Add(this.checkBoxLogShellCode);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBoxArch);
            this.Controls.Add(this.checkBoxDisabledFunctionsBypass);
            this.Controls.Add(this.btnPopShell);
            this.Controls.Add(this.comboBoxMethod);
            this.Controls.Add(this.labelMethod);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.buttonGetIpv4);
            this.Controls.Add(this.labelIP);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReverseShell";
            this.Text = "Reverse Shell Spawner";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Button buttonGetIpv4;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelMethod;
        private System.Windows.Forms.ComboBox comboBoxMethod;
        private System.Windows.Forms.Button btnPopShell;
        private System.Windows.Forms.CheckBox checkBoxDisabledFunctionsBypass;
        private System.Windows.Forms.ComboBox comboBoxArch;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox checkBoxLogShellCode;
    }
}