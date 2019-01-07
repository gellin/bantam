namespace bantam_php
{
    partial class PortScanner
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
            this.textBoxHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPorts = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxCommonPorts = new System.Windows.Forms.ComboBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxHost
            // 
            this.textBoxHost.Location = new System.Drawing.Point(65, 12);
            this.textBoxHost.Name = "textBoxHost";
            this.textBoxHost.Size = new System.Drawing.Size(334, 24);
            this.textBoxHost.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Host:";
            // 
            // textBoxPorts
            // 
            this.textBoxPorts.Enabled = false;
            this.textBoxPorts.Location = new System.Drawing.Point(65, 61);
            this.textBoxPorts.Name = "textBoxPorts";
            this.textBoxPorts.Size = new System.Drawing.Size(58, 24);
            this.textBoxPorts.TabIndex = 2;
            this.textBoxPorts.TextChanged += new System.EventHandler(this.textBoxPorts_TextChanged);
            this.textBoxPorts.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPorts_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Port:";
            // 
            // comboBoxCommonPorts
            // 
            this.comboBoxCommonPorts.FormattingEnabled = true;
            this.comboBoxCommonPorts.Items.AddRange(new object[] {
            "",
            "1-1024",
            "common ports",
            "all ports"});
            this.comboBoxCommonPorts.Location = new System.Drawing.Point(198, 62);
            this.comboBoxCommonPorts.Name = "comboBoxCommonPorts";
            this.comboBoxCommonPorts.Size = new System.Drawing.Size(201, 24);
            this.comboBoxCommonPorts.TabIndex = 4;
            this.comboBoxCommonPorts.SelectedIndexChanged += new System.EventHandler(this.comboBoxCommonPorts_SelectedIndexChanged);
            // 
            // btnScan
            // 
            this.btnScan.Enabled = false;
            this.btnScan.Location = new System.Drawing.Point(288, 103);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(111, 36);
            this.btnScan.TabIndex = 6;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(150, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "or";
            // 
            // PortScanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 151);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.comboBoxCommonPorts);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxPorts);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxHost);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PortScanner";
            this.Text = "PortScanner";
            this.Load += new System.EventHandler(this.PortScanner_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPorts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxCommonPorts;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Label label3;
    }
}