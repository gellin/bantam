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
            this.SuspendLayout();
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(24, 22);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(21, 16);
            this.labelIP.TabIndex = 0;
            this.labelIP.Text = "IP:";
            // 
            // buttonGetIpv4
            // 
            this.buttonGetIpv4.Location = new System.Drawing.Point(334, 16);
            this.buttonGetIpv4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonGetIpv4.Name = "buttonGetIpv4";
            this.buttonGetIpv4.Size = new System.Drawing.Size(87, 28);
            this.buttonGetIpv4.TabIndex = 1;
            this.buttonGetIpv4.Text = "Get My IPv4";
            this.buttonGetIpv4.UseVisualStyleBackColor = true;
            this.buttonGetIpv4.Click += new System.EventHandler(this.buttonGetIpv4_Click);
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(96, 18);
            this.textBoxIP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(222, 24);
            this.textBoxIP.TabIndex = 2;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(96, 50);
            this.textBoxPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(58, 24);
            this.textBoxPort.TabIndex = 3;
            this.textBoxPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPort_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Port:";
            // 
            // labelMethod
            // 
            this.labelMethod.AutoSize = true;
            this.labelMethod.Location = new System.Drawing.Point(24, 103);
            this.labelMethod.Name = "labelMethod";
            this.labelMethod.Size = new System.Drawing.Size(57, 16);
            this.labelMethod.TabIndex = 5;
            this.labelMethod.Text = "Method:";
            // 
            // comboBoxMethod
            // 
            this.comboBoxMethod.FormattingEnabled = true;
            this.comboBoxMethod.Location = new System.Drawing.Point(96, 100);
            this.comboBoxMethod.Name = "comboBoxMethod";
            this.comboBoxMethod.Size = new System.Drawing.Size(133, 24);
            this.comboBoxMethod.TabIndex = 6;
            // 
            // btnPopShell
            // 
            this.btnPopShell.Location = new System.Drawing.Point(334, 103);
            this.btnPopShell.Name = "btnPopShell";
            this.btnPopShell.Size = new System.Drawing.Size(87, 28);
            this.btnPopShell.TabIndex = 7;
            this.btnPopShell.Text = "Pop Shell";
            this.btnPopShell.UseVisualStyleBackColor = true;
            this.btnPopShell.Click += new System.EventHandler(this.btnPopShell_Click);
            // 
            // ReverseShell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 145);
            this.Controls.Add(this.btnPopShell);
            this.Controls.Add(this.comboBoxMethod);
            this.Controls.Add(this.labelMethod);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.buttonGetIpv4);
            this.Controls.Add(this.labelIP);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
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
    }
}