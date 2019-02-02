namespace bantam.Forms
{
    partial class Options
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
            this.components = new System.ComponentModel.Container();
            this.textBoxMaxCommentLength = new System.Windows.Forms.TextBox();
            this.labelStaticMaxLength = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarLoggingLevel = new System.Windows.Forms.TrackBar();
            this.checkBoxEnableLogging = new System.Windows.Forms.CheckBox();
            this.checkBoxRandomComments = new System.Windows.Forms.CheckBox();
            this.labelStaticCommentFrequency = new System.Windows.Forms.Label();
            this.trackBarCommentFrequency = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxMaxCookieSize = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.labelStaticLogLevel = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.toolTipCommentTracker = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLoggingLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCommentFrequency)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxMaxCommentLength
            // 
            this.textBoxMaxCommentLength.Location = new System.Drawing.Point(116, 121);
            this.textBoxMaxCommentLength.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxMaxCommentLength.MaxLength = 3;
            this.textBoxMaxCommentLength.Name = "textBoxMaxCommentLength";
            this.textBoxMaxCommentLength.Size = new System.Drawing.Size(35, 24);
            this.textBoxMaxCommentLength.TabIndex = 16;
            this.textBoxMaxCommentLength.Text = "32";
            // 
            // labelStaticMaxLength
            // 
            this.labelStaticMaxLength.AutoSize = true;
            this.labelStaticMaxLength.Location = new System.Drawing.Point(31, 124);
            this.labelStaticMaxLength.Name = "labelStaticMaxLength";
            this.labelStaticMaxLength.Size = new System.Drawing.Size(79, 16);
            this.labelStaticMaxLength.TabIndex = 15;
            this.labelStaticMaxLength.Text = "Max Length:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(338, 21);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(74, 24);
            this.textBox1.TabIndex = 14;
            this.textBox1.Text = "8192";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(191, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "Max Post Size (KiB):";
            // 
            // trackBarLoggingLevel
            // 
            this.trackBarLoggingLevel.Location = new System.Drawing.Point(240, 38);
            this.trackBarLoggingLevel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.trackBarLoggingLevel.Maximum = 3;
            this.trackBarLoggingLevel.Minimum = 1;
            this.trackBarLoggingLevel.Name = "trackBarLoggingLevel";
            this.trackBarLoggingLevel.Size = new System.Drawing.Size(154, 45);
            this.trackBarLoggingLevel.TabIndex = 12;
            this.trackBarLoggingLevel.Value = 1;
            // 
            // checkBoxEnableLogging
            // 
            this.checkBoxEnableLogging.AutoSize = true;
            this.checkBoxEnableLogging.Location = new System.Drawing.Point(15, 24);
            this.checkBoxEnableLogging.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBoxEnableLogging.Name = "checkBoxEnableLogging";
            this.checkBoxEnableLogging.Size = new System.Drawing.Size(118, 20);
            this.checkBoxEnableLogging.TabIndex = 11;
            this.checkBoxEnableLogging.Text = "Enable Logging";
            this.checkBoxEnableLogging.UseVisualStyleBackColor = true;
            // 
            // checkBoxRandomComments
            // 
            this.checkBoxRandomComments.AutoSize = true;
            this.checkBoxRandomComments.Checked = true;
            this.checkBoxRandomComments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRandomComments.Location = new System.Drawing.Point(12, 0);
            this.checkBoxRandomComments.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBoxRandomComments.Name = "checkBoxRandomComments";
            this.checkBoxRandomComments.Size = new System.Drawing.Size(176, 20);
            this.checkBoxRandomComments.TabIndex = 9;
            this.checkBoxRandomComments.Text = "Inject Random Comments";
            this.checkBoxRandomComments.UseVisualStyleBackColor = true;
            // 
            // labelStaticCommentFrequency
            // 
            this.labelStaticCommentFrequency.AutoSize = true;
            this.labelStaticCommentFrequency.Location = new System.Drawing.Point(47, 44);
            this.labelStaticCommentFrequency.Name = "labelStaticCommentFrequency";
            this.labelStaticCommentFrequency.Size = new System.Drawing.Size(127, 16);
            this.labelStaticCommentFrequency.TabIndex = 10;
            this.labelStaticCommentFrequency.Text = "Comment Frequency";
            // 
            // trackBarCommentFrequency
            // 
            this.trackBarCommentFrequency.Location = new System.Drawing.Point(29, 64);
            this.trackBarCommentFrequency.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.trackBarCommentFrequency.Maximum = 4;
            this.trackBarCommentFrequency.Minimum = 1;
            this.trackBarCommentFrequency.Name = "trackBarCommentFrequency";
            this.trackBarCommentFrequency.Size = new System.Drawing.Size(162, 45);
            this.trackBarCommentFrequency.TabIndex = 8;
            this.trackBarCommentFrequency.Value = 1;
            this.trackBarCommentFrequency.Scroll += new System.EventHandler(this.trackBarCommentFrequency_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(191, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 16);
            this.label4.TabIndex = 17;
            this.label4.Text = "Max Cookie Size (B):";
            // 
            // textBoxMaxCookieSize
            // 
            this.textBoxMaxCookieSize.Enabled = false;
            this.textBoxMaxCookieSize.Location = new System.Drawing.Point(338, 59);
            this.textBoxMaxCookieSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxMaxCookieSize.Name = "textBoxMaxCookieSize";
            this.textBoxMaxCookieSize.Size = new System.Drawing.Size(74, 24);
            this.textBoxMaxCookieSize.TabIndex = 18;
            this.textBoxMaxCookieSize.Text = "8192";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Location = new System.Drawing.Point(12, 265);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(435, 220);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Request Obfuscation";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelStaticLogLevel);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.trackBarLoggingLevel);
            this.groupBox2.Controls.Add(this.checkBoxEnableLogging);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(435, 126);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Logging";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(15, 74);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(140, 20);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Enable Global Logs";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // labelStaticLogLevel
            // 
            this.labelStaticLogLevel.AutoSize = true;
            this.labelStaticLogLevel.Location = new System.Drawing.Point(287, 75);
            this.labelStaticLogLevel.Name = "labelStaticLogLevel";
            this.labelStaticLogLevel.Size = new System.Drawing.Size(63, 16);
            this.labelStaticLogLevel.TabIndex = 14;
            this.labelStaticLogLevel.Text = "Log Level";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox3);
            this.groupBox3.Controls.Add(this.checkBox2);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.textBoxMaxCookieSize);
            this.groupBox3.Location = new System.Drawing.Point(12, 153);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(435, 106);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Request Settings";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxRandomComments);
            this.groupBox4.Controls.Add(this.labelStaticMaxLength);
            this.groupBox4.Controls.Add(this.trackBarCommentFrequency);
            this.groupBox4.Controls.Add(this.textBoxMaxCommentLength);
            this.groupBox4.Controls.Add(this.labelStaticCommentFrequency);
            this.groupBox4.Location = new System.Drawing.Point(20, 26);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 178);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(15, 23);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(143, 20);
            this.checkBox2.TabIndex = 19;
            this.checkBox2.Text = "Max Execution Time";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(15, 61);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(136, 20);
            this.checkBox3.TabIndex = 20;
            this.checkBox3.Text = "Disable Error Logs";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 498);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Options";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLoggingLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCommentFrequency)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxMaxCommentLength;
        private System.Windows.Forms.Label labelStaticMaxLength;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBarLoggingLevel;
        private System.Windows.Forms.CheckBox checkBoxEnableLogging;
        private System.Windows.Forms.CheckBox checkBoxRandomComments;
        private System.Windows.Forms.Label labelStaticCommentFrequency;
        private System.Windows.Forms.TrackBar trackBarCommentFrequency;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxMaxCookieSize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelStaticLogLevel;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ToolTip toolTipCommentTracker;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}