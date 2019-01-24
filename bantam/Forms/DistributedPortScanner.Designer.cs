namespace bantam.Forms
{
    partial class DistributedPortScanner
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
            this.checkedListBoxShells = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStripOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deSelectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnScan = new System.Windows.Forms.Button();
            this.textBoxTarget = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxStartPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxEndPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStripOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBoxShells
            // 
            this.checkedListBoxShells.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxShells.ContextMenuStrip = this.contextMenuStripOptions;
            this.checkedListBoxShells.FormattingEnabled = true;
            this.checkedListBoxShells.Location = new System.Drawing.Point(14, 38);
            this.checkedListBoxShells.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkedListBoxShells.Name = "checkedListBoxShells";
            this.checkedListBoxShells.Size = new System.Drawing.Size(501, 422);
            this.checkedListBoxShells.TabIndex = 0;
            // 
            // contextMenuStripOptions
            // 
            this.contextMenuStripOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.deSelectAllToolStripMenuItem});
            this.contextMenuStripOptions.Name = "contextMenuStripOptions";
            this.contextMenuStripOptions.Size = new System.Drawing.Size(142, 48);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // deSelectAllToolStripMenuItem
            // 
            this.deSelectAllToolStripMenuItem.Name = "deSelectAllToolStripMenuItem";
            this.deSelectAllToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.deSelectAllToolStripMenuItem.Text = "De-Select All";
            this.deSelectAllToolStripMenuItem.Click += new System.EventHandler(this.deSelectAllToolStripMenuItem_Click);
            // 
            // btnScan
            // 
            this.btnScan.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnScan.Enabled = false;
            this.btnScan.Location = new System.Drawing.Point(385, 515);
            this.btnScan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(129, 39);
            this.btnScan.TabIndex = 2;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // textBoxTarget
            // 
            this.textBoxTarget.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textBoxTarget.Location = new System.Drawing.Point(83, 476);
            this.textBoxTarget.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxTarget.MaxLength = 512;
            this.textBoxTarget.Name = "textBoxTarget";
            this.textBoxTarget.Size = new System.Drawing.Size(432, 24);
            this.textBoxTarget.TabIndex = 3;
            this.textBoxTarget.TextChanged += new System.EventHandler(this.textBoxTarget_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 482);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Target:";
            // 
            // textBoxStartPort
            // 
            this.textBoxStartPort.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textBoxStartPort.Location = new System.Drawing.Point(82, 523);
            this.textBoxStartPort.MaxLength = 5;
            this.textBoxStartPort.Name = "textBoxStartPort";
            this.textBoxStartPort.Size = new System.Drawing.Size(77, 24);
            this.textBoxStartPort.TabIndex = 5;
            this.textBoxStartPort.Text = "1";
            this.textBoxStartPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxStartPort_KeyPress);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 526);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "Port:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(176, 526);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "-";
            // 
            // textBoxEndPort
            // 
            this.textBoxEndPort.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textBoxEndPort.Location = new System.Drawing.Point(209, 523);
            this.textBoxEndPort.MaxLength = 5;
            this.textBoxEndPort.Name = "textBoxEndPort";
            this.textBoxEndPort.Size = new System.Drawing.Size(77, 24);
            this.textBoxEndPort.TabIndex = 8;
            this.textBoxEndPort.Text = "1000";
            this.textBoxEndPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxEndPort_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Hosts";
            // 
            // DistributedPortScanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 578);
            this.Controls.Add(this.textBoxEndPort);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxStartPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxTarget);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedListBoxShells);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 9.85F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DistributedPortScanner";
            this.Text = "Distributed Port Scanner";
            this.Load += new System.EventHandler(this.DistributedScanner_Load);
            this.contextMenuStripOptions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxShells;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.TextBox textBoxTarget;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxStartPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxEndPort;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOptions;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deSelectAllToolStripMenuItem;
        private System.Windows.Forms.Label label1;
    }
}