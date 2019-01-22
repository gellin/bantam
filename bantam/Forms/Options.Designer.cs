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
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLoggingLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCommentFrequency)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxMaxCommentLength
            // 
            this.textBoxMaxCommentLength.Location = new System.Drawing.Point(497, 392);
            this.textBoxMaxCommentLength.Name = "textBoxMaxCommentLength";
            this.textBoxMaxCommentLength.Size = new System.Drawing.Size(154, 20);
            this.textBoxMaxCommentLength.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(493, 370);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Max Comment Length:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(300, 162);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(149, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Max Post Size (KB):";
            // 
            // trackBarLoggingLevel
            // 
            this.trackBarLoggingLevel.Location = new System.Drawing.Point(153, 52);
            this.trackBarLoggingLevel.Maximum = 3;
            this.trackBarLoggingLevel.Minimum = 1;
            this.trackBarLoggingLevel.Name = "trackBarLoggingLevel";
            this.trackBarLoggingLevel.Size = new System.Drawing.Size(132, 45);
            this.trackBarLoggingLevel.TabIndex = 12;
            this.trackBarLoggingLevel.Value = 1;
            // 
            // checkBoxEnableLogging
            // 
            this.checkBoxEnableLogging.AutoSize = true;
            this.checkBoxEnableLogging.Location = new System.Drawing.Point(153, 23);
            this.checkBoxEnableLogging.Name = "checkBoxEnableLogging";
            this.checkBoxEnableLogging.Size = new System.Drawing.Size(100, 17);
            this.checkBoxEnableLogging.TabIndex = 11;
            this.checkBoxEnableLogging.Text = "Enable Logging";
            this.checkBoxEnableLogging.UseVisualStyleBackColor = true;
            // 
            // checkBoxRandomComments
            // 
            this.checkBoxRandomComments.AutoSize = true;
            this.checkBoxRandomComments.Location = new System.Drawing.Point(153, 366);
            this.checkBoxRandomComments.Name = "checkBoxRandomComments";
            this.checkBoxRandomComments.Size = new System.Drawing.Size(118, 17);
            this.checkBoxRandomComments.TabIndex = 9;
            this.checkBoxRandomComments.Text = "Random Comments";
            this.checkBoxRandomComments.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(149, 423);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Comment Frequency";
            // 
            // trackBarCommentFrequency
            // 
            this.trackBarCommentFrequency.Location = new System.Drawing.Point(300, 410);
            this.trackBarCommentFrequency.Maximum = 4;
            this.trackBarCommentFrequency.Minimum = 1;
            this.trackBarCommentFrequency.Name = "trackBarCommentFrequency";
            this.trackBarCommentFrequency.Size = new System.Drawing.Size(139, 45);
            this.trackBarCommentFrequency.TabIndex = 8;
            this.trackBarCommentFrequency.Value = 1;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 462);
            this.Controls.Add(this.textBoxMaxCommentLength);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackBarLoggingLevel);
            this.Controls.Add(this.checkBoxEnableLogging);
            this.Controls.Add(this.checkBoxRandomComments);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBarCommentFrequency);
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
    }
}