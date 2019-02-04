﻿namespace bantam.Forms
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBoxPhpVarNameMaxLen = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxRandomPhpVarNames = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelStaticLogLevel = new System.Windows.Forms.Label();
            this.checkBoxGlobalLogs = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxDisableErrorLogs = new System.Windows.Forms.CheckBox();
            this.checkBoxMaxExecutionTime = new System.Windows.Forms.CheckBox();
            this.toolTipCommentTracker = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLoggingLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCommentFrequency)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxMaxCommentLength
            // 
            this.textBoxMaxCommentLength.Location = new System.Drawing.Point(108, 119);
            this.textBoxMaxCommentLength.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxMaxCommentLength.MaxLength = 3;
            this.textBoxMaxCommentLength.Name = "textBoxMaxCommentLength";
            this.textBoxMaxCommentLength.Size = new System.Drawing.Size(35, 24);
            this.textBoxMaxCommentLength.TabIndex = 16;
            this.textBoxMaxCommentLength.Text = "24";
            this.toolTip1.SetToolTip(this.textBoxMaxCommentLength, "Sets the max length for each comment, a random length less than this value will b" +
        "e selected.");
            this.textBoxMaxCommentLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxMaxCommentLength_KeyPress);
            // 
            // labelStaticMaxLength
            // 
            this.labelStaticMaxLength.AutoSize = true;
            this.labelStaticMaxLength.Location = new System.Drawing.Point(23, 122);
            this.labelStaticMaxLength.Name = "labelStaticMaxLength";
            this.labelStaticMaxLength.Size = new System.Drawing.Size(79, 16);
            this.labelStaticMaxLength.TabIndex = 15;
            this.labelStaticMaxLength.Text = "Max Length:";
            this.toolTip1.SetToolTip(this.labelStaticMaxLength, "Sets the max length for each comment, a random length less than this value will b" +
        "e selected.");
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
            this.trackBarLoggingLevel.Location = new System.Drawing.Point(236, 47);
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
            this.checkBoxEnableLogging.Checked = true;
            this.checkBoxEnableLogging.CheckState = System.Windows.Forms.CheckState.Checked;
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
            this.toolTip1.SetToolTip(this.checkBoxRandomComments, "Injects comments with random text, throughout the request.");
            this.checkBoxRandomComments.UseVisualStyleBackColor = true;
            this.checkBoxRandomComments.CheckedChanged += new System.EventHandler(this.checkBoxRandomComments_CheckedChanged);
            // 
            // labelStaticCommentFrequency
            // 
            this.labelStaticCommentFrequency.AutoSize = true;
            this.labelStaticCommentFrequency.Location = new System.Drawing.Point(39, 44);
            this.labelStaticCommentFrequency.Name = "labelStaticCommentFrequency";
            this.labelStaticCommentFrequency.Size = new System.Drawing.Size(127, 16);
            this.labelStaticCommentFrequency.TabIndex = 10;
            this.labelStaticCommentFrequency.Text = "Comment Frequency";
            // 
            // trackBarCommentFrequency
            // 
            this.trackBarCommentFrequency.Location = new System.Drawing.Point(21, 64);
            this.trackBarCommentFrequency.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.trackBarCommentFrequency.Maximum = 4;
            this.trackBarCommentFrequency.Minimum = 1;
            this.trackBarCommentFrequency.Name = "trackBarCommentFrequency";
            this.trackBarCommentFrequency.Size = new System.Drawing.Size(162, 45);
            this.trackBarCommentFrequency.TabIndex = 8;
            this.toolTip1.SetToolTip(this.trackBarCommentFrequency, "Determines the rate of comment injection (1 = 25%, 4 = 100%) of locations utilize" +
        "d.");
            this.trackBarCommentFrequency.Value = 2;
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
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Location = new System.Drawing.Point(12, 265);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(435, 201);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Request Obfuscation";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBoxPhpVarNameMaxLen);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.checkBoxRandomPhpVarNames);
            this.groupBox5.Location = new System.Drawing.Point(225, 26);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(195, 73);
            this.groupBox5.TabIndex = 18;
            this.groupBox5.TabStop = false;
            this.toolTipCommentTracker.SetToolTip(this.groupBox5, "asd");
            this.toolTip1.SetToolTip(this.groupBox5, "asd");
            // 
            // textBoxPhpVarNameMaxLen
            // 
            this.textBoxPhpVarNameMaxLen.Location = new System.Drawing.Point(120, 31);
            this.textBoxPhpVarNameMaxLen.MaxLength = 2;
            this.textBoxPhpVarNameMaxLen.Name = "textBoxPhpVarNameMaxLen";
            this.textBoxPhpVarNameMaxLen.Size = new System.Drawing.Size(35, 24);
            this.textBoxPhpVarNameMaxLen.TabIndex = 20;
            this.textBoxPhpVarNameMaxLen.Text = "16";
            this.toolTip1.SetToolTip(this.textBoxPhpVarNameMaxLen, "Sets the max length for each PHP variable, a random length less than this value w" +
        "ill be selected.");
            this.textBoxPhpVarNameMaxLen.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPhpVarNameMaxLen_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 16);
            this.label1.TabIndex = 19;
            this.label1.Text = "Max Length:";
            this.toolTip1.SetToolTip(this.label1, "Sets the max length for each PHP variable, a random length less than this value w" +
        "ill be selected.");
            // 
            // checkBoxRandomPhpVarNames
            // 
            this.checkBoxRandomPhpVarNames.AutoSize = true;
            this.checkBoxRandomPhpVarNames.Checked = true;
            this.checkBoxRandomPhpVarNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRandomPhpVarNames.Location = new System.Drawing.Point(11, 0);
            this.checkBoxRandomPhpVarNames.Name = "checkBoxRandomPhpVarNames";
            this.checkBoxRandomPhpVarNames.Size = new System.Drawing.Size(172, 20);
            this.checkBoxRandomPhpVarNames.TabIndex = 18;
            this.checkBoxRandomPhpVarNames.Text = "Random PHP Var Names";
            this.toolTip1.SetToolTip(this.checkBoxRandomPhpVarNames, "Creates random variable names for send PHP code (always on).");
            this.checkBoxRandomPhpVarNames.UseVisualStyleBackColor = true;
            this.checkBoxRandomPhpVarNames.CheckedChanged += new System.EventHandler(this.checkBoxRandomPhpVarNames_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxRandomComments);
            this.groupBox4.Controls.Add(this.labelStaticMaxLength);
            this.groupBox4.Controls.Add(this.trackBarCommentFrequency);
            this.groupBox4.Controls.Add(this.textBoxMaxCommentLength);
            this.groupBox4.Controls.Add(this.labelStaticCommentFrequency);
            this.groupBox4.Location = new System.Drawing.Point(15, 26);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(194, 164);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelStaticLogLevel);
            this.groupBox2.Controls.Add(this.checkBoxGlobalLogs);
            this.groupBox2.Controls.Add(this.trackBarLoggingLevel);
            this.groupBox2.Controls.Add(this.checkBoxEnableLogging);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(435, 126);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Logging";
            // 
            // labelStaticLogLevel
            // 
            this.labelStaticLogLevel.AutoSize = true;
            this.labelStaticLogLevel.Location = new System.Drawing.Point(286, 31);
            this.labelStaticLogLevel.Name = "labelStaticLogLevel";
            this.labelStaticLogLevel.Size = new System.Drawing.Size(63, 16);
            this.labelStaticLogLevel.TabIndex = 14;
            this.labelStaticLogLevel.Text = "Log Level";
            // 
            // checkBoxGlobalLogs
            // 
            this.checkBoxGlobalLogs.AutoSize = true;
            this.checkBoxGlobalLogs.Location = new System.Drawing.Point(15, 74);
            this.checkBoxGlobalLogs.Name = "checkBoxGlobalLogs";
            this.checkBoxGlobalLogs.Size = new System.Drawing.Size(140, 20);
            this.checkBoxGlobalLogs.TabIndex = 13;
            this.checkBoxGlobalLogs.Text = "Enable Global Logs";
            this.checkBoxGlobalLogs.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxDisableErrorLogs);
            this.groupBox3.Controls.Add(this.checkBoxMaxExecutionTime);
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
            // checkBoxDisableErrorLogs
            // 
            this.checkBoxDisableErrorLogs.AutoSize = true;
            this.checkBoxDisableErrorLogs.Checked = true;
            this.checkBoxDisableErrorLogs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDisableErrorLogs.Location = new System.Drawing.Point(15, 61);
            this.checkBoxDisableErrorLogs.Name = "checkBoxDisableErrorLogs";
            this.checkBoxDisableErrorLogs.Size = new System.Drawing.Size(136, 20);
            this.checkBoxDisableErrorLogs.TabIndex = 20;
            this.checkBoxDisableErrorLogs.Text = "Disable Error Logs";
            this.toolTip1.SetToolTip(this.checkBoxDisableErrorLogs, "Sends php code with request to disable error logging.");
            this.checkBoxDisableErrorLogs.UseVisualStyleBackColor = true;
            this.checkBoxDisableErrorLogs.CheckedChanged += new System.EventHandler(this.checkBoxDisableErrorLogs_CheckedChanged);
            // 
            // checkBoxMaxExecutionTime
            // 
            this.checkBoxMaxExecutionTime.AutoSize = true;
            this.checkBoxMaxExecutionTime.Location = new System.Drawing.Point(15, 23);
            this.checkBoxMaxExecutionTime.Name = "checkBoxMaxExecutionTime";
            this.checkBoxMaxExecutionTime.Size = new System.Drawing.Size(143, 20);
            this.checkBoxMaxExecutionTime.TabIndex = 19;
            this.checkBoxMaxExecutionTime.Text = "Max Execution Time";
            this.toolTip1.SetToolTip(this.checkBoxMaxExecutionTime, "Sends php code with request to extend execution time.");
            this.checkBoxMaxExecutionTime.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 475);
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
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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
        private System.Windows.Forms.CheckBox checkBoxGlobalLogs;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ToolTip toolTipCommentTracker;
        private System.Windows.Forms.CheckBox checkBoxDisableErrorLogs;
        private System.Windows.Forms.CheckBox checkBoxMaxExecutionTime;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBoxRandomPhpVarNames;
        private System.Windows.Forms.TextBox textBoxPhpVarNameMaxLen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}