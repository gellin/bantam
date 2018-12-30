namespace bantam_php
{
    partial class ProxyOptions
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
            this.buttonOk = new System.Windows.Forms.Button();
            this.txtBoxProxyUrl = new System.Windows.Forms.TextBox();
            this.txtBoxProxyPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxProxyType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(263, 74);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 27);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtBoxProxyUrl
            // 
            this.txtBoxProxyUrl.Location = new System.Drawing.Point(12, 30);
            this.txtBoxProxyUrl.Name = "txtBoxProxyUrl";
            this.txtBoxProxyUrl.Size = new System.Drawing.Size(210, 20);
            this.txtBoxProxyUrl.TabIndex = 2;
            // 
            // txtBoxProxyPort
            // 
            this.txtBoxProxyPort.Location = new System.Drawing.Point(257, 30);
            this.txtBoxProxyPort.Name = "txtBoxProxyPort";
            this.txtBoxProxyPort.Size = new System.Drawing.Size(84, 20);
            this.txtBoxProxyPort.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Proxy Url";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(254, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Proxy Port";
            // 
            // comboBoxProxyType
            // 
            this.comboBoxProxyType.FormattingEnabled = true;
            this.comboBoxProxyType.Items.AddRange(new object[] {
            "socks",
            "http"});
            this.comboBoxProxyType.Location = new System.Drawing.Point(12, 74);
            this.comboBoxProxyType.Name = "comboBoxProxyType";
            this.comboBoxProxyType.Size = new System.Drawing.Size(109, 21);
            this.comboBoxProxyType.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Proxy Type";
            // 
            // ProxyOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 108);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxProxyType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxProxyPort);
            this.Controls.Add(this.txtBoxProxyUrl);
            this.Controls.Add(this.buttonOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ProxyOptions";
            this.Text = "ProxySettings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProxyOptions_FormClosing);
            this.Load += new System.EventHandler(this.ProxyOptions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.TextBox txtBoxProxyUrl;
        private System.Windows.Forms.TextBox txtBoxProxyPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxProxyType;
        private System.Windows.Forms.Label label3;
    }
}