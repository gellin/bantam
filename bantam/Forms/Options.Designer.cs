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
            this.textBoxMaxCommentLength = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarLoggingLevel = new System.Windows.Forms.TrackBar();
            this.checkBoxEnableLogging = new System.Windows.Forms.CheckBox();
            this.checkBoxRandomComments = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarCommentFrequency = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxMaxCommentSize = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLoggingLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCommentFrequency)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxMaxCommentLength
            // 
            this.textBoxMaxCommentLength.Location = new System.Drawing.Point(580, 482);
            this.textBoxMaxCommentLength.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxMaxCommentLength.Name = "textBoxMaxCommentLength";
            this.textBoxMaxCommentLength.Size = new System.Drawing.Size(86, 24);
            this.textBoxMaxCommentLength.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(575, 455);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "Max Comment Length:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(753, 315);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(116, 24);
            this.textBox1.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(577, 319);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "Max Post Size (KB):";
            // 
            // trackBarLoggingLevel
            // 
            this.trackBarLoggingLevel.Location = new System.Drawing.Point(178, 69);
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
            this.checkBoxEnableLogging.Location = new System.Drawing.Point(178, 28);
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
            this.checkBoxRandomComments.Location = new System.Drawing.Point(178, 450);
            this.checkBoxRandomComments.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBoxRandomComments.Name = "checkBoxRandomComments";
            this.checkBoxRandomComments.Size = new System.Drawing.Size(142, 20);
            this.checkBoxRandomComments.TabIndex = 9;
            this.checkBoxRandomComments.Text = "Random Comments";
            this.checkBoxRandomComments.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(174, 521);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "Comment Frequency";
            // 
            // trackBarCommentFrequency
            // 
            this.trackBarCommentFrequency.Location = new System.Drawing.Point(350, 505);
            this.trackBarCommentFrequency.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.trackBarCommentFrequency.Maximum = 4;
            this.trackBarCommentFrequency.Minimum = 1;
            this.trackBarCommentFrequency.Name = "trackBarCommentFrequency";
            this.trackBarCommentFrequency.Size = new System.Drawing.Size(162, 45);
            this.trackBarCommentFrequency.TabIndex = 8;
            this.trackBarCommentFrequency.Value = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(577, 378);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 16);
            this.label4.TabIndex = 17;
            this.label4.Text = "Max Comment Size (b):";
            // 
            // textBoxMaxCommentSize
            // 
            this.textBoxMaxCommentSize.Enabled = false;
            this.textBoxMaxCommentSize.Location = new System.Drawing.Point(752, 378);
            this.textBoxMaxCommentSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxMaxCommentSize.Name = "textBoxMaxCommentSize";
            this.textBoxMaxCommentSize.Size = new System.Drawing.Size(116, 24);
            this.textBoxMaxCommentSize.TabIndex = 18;
            this.textBoxMaxCommentSize.Text = "8192";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(500, 97);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(307, 157);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 569);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxMaxCommentSize);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxMaxCommentLength);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackBarLoggingLevel);
            this.Controls.Add(this.checkBoxEnableLogging);
            this.Controls.Add(this.checkBoxRandomComments);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBarCommentFrequency);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Options";
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLoggingLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCommentFrequency)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxMaxCommentLength;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBarLoggingLevel;
        private System.Windows.Forms.CheckBox checkBoxEnableLogging;
        private System.Windows.Forms.CheckBox checkBoxRandomComments;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarCommentFrequency;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxMaxCommentSize;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}