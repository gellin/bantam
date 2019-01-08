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
            this.buttonOk.Location = new System.Drawing.Point(402, 90);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(115, 33);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtBoxProxyUrl
            // 
            this.txtBoxProxyUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtBoxProxyUrl.Location = new System.Drawing.Point(16, 37);
            this.txtBoxProxyUrl.Margin = new System.Windows.Forms.Padding(4);
            this.txtBoxProxyUrl.Name = "txtBoxProxyUrl";
            this.txtBoxProxyUrl.Size = new System.Drawing.Size(350, 21);
            this.txtBoxProxyUrl.TabIndex = 2;
            // 
            // txtBoxProxyPort
            // 
            this.txtBoxProxyPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtBoxProxyPort.Location = new System.Drawing.Point(402, 37);
            this.txtBoxProxyPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtBoxProxyPort.Name = "txtBoxProxyPort";
            this.txtBoxProxyPort.Size = new System.Drawing.Size(115, 21);
            this.txtBoxProxyPort.TabIndex = 3;
            this.txtBoxProxyPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxProxyPort_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Proxy Url";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(398, 17);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Proxy Port";
            // 
            // comboBoxProxyType
            // 
            this.comboBoxProxyType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.comboBoxProxyType.FormattingEnabled = true;
            this.comboBoxProxyType.Items.AddRange(new object[] {
            "socks",
            "http"});
            this.comboBoxProxyType.Location = new System.Drawing.Point(16, 100);
            this.comboBoxProxyType.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxProxyType.Name = "comboBoxProxyType";
            this.comboBoxProxyType.Size = new System.Drawing.Size(144, 23);
            this.comboBoxProxyType.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 80);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Proxy Type";
            // 
            // ProxyOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 135);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxProxyType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxProxyPort);
            this.Controls.Add(this.txtBoxProxyUrl);
            this.Controls.Add(this.buttonOk);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ProxyOptions";
            this.Text = "Proxy Options";
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