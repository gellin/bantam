namespace bantam.Forms
{
    partial class UploadFile
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.txtBoxFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblStaticPath = new System.Windows.Forms.Label();
            this.lblDynPath = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.vectorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linEnumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.linuxPrivCheckerpyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.richTextBox1.Location = new System.Drawing.Point(14, 28);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(518, 366);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBrowse.Location = new System.Drawing.Point(11, 474);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(102, 35);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpload.Enabled = false;
            this.btnUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.btnUpload.Location = new System.Drawing.Point(420, 469);
            this.btnUpload.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(111, 40);
            this.btnUpload.TabIndex = 2;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // txtBoxFileName
            // 
            this.txtBoxFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtBoxFileName.Location = new System.Drawing.Point(82, 408);
            this.txtBoxFileName.Margin = new System.Windows.Forms.Padding(4);
            this.txtBoxFileName.Name = "txtBoxFileName";
            this.txtBoxFileName.Size = new System.Drawing.Size(449, 22);
            this.txtBoxFileName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 413);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "File Name:";
            // 
            // lblStaticPath
            // 
            this.lblStaticPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStaticPath.AutoSize = true;
            this.lblStaticPath.Location = new System.Drawing.Point(11, 446);
            this.lblStaticPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStaticPath.Name = "lblStaticPath";
            this.lblStaticPath.Size = new System.Drawing.Size(36, 16);
            this.lblStaticPath.TabIndex = 5;
            this.lblStaticPath.Text = "Path:";
            // 
            // lblDynPath
            // 
            this.lblDynPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDynPath.AutoSize = true;
            this.lblDynPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblDynPath.Location = new System.Drawing.Point(52, 447);
            this.lblDynPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDynPath.Name = "lblDynPath";
            this.lblDynPath.Size = new System.Drawing.Size(0, 15);
            this.lblDynPath.TabIndex = 6;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.vectorsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(542, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // vectorsToolStripMenuItem
            // 
            this.vectorsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.linEnumToolStripMenuItem,
            this.linuxPrivCheckerpyToolStripMenuItem});
            this.vectorsToolStripMenuItem.Name = "vectorsToolStripMenuItem";
            this.vectorsToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.vectorsToolStripMenuItem.Text = "Vectors";
            // 
            // linEnumToolStripMenuItem
            // 
            this.linEnumToolStripMenuItem.Name = "linEnumToolStripMenuItem";
            this.linEnumToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.linEnumToolStripMenuItem.Text = "LinEnum.sh";
            this.linEnumToolStripMenuItem.Click += new System.EventHandler(this.linEnumToolStripMenuItem_Click);
            // 
            // linuxPrivCheckerpyToolStripMenuItem
            // 
            this.linuxPrivCheckerpyToolStripMenuItem.Name = "linuxPrivCheckerpyToolStripMenuItem";
            this.linuxPrivCheckerpyToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.linuxPrivCheckerpyToolStripMenuItem.Text = "LinuxPrivChecker.py";
            this.linuxPrivCheckerpyToolStripMenuItem.Click += new System.EventHandler(this.linuxPrivCheckerpyToolStripMenuItem_Click);
            // 
            // UploadFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 518);
            this.Controls.Add(this.lblDynPath);
            this.Controls.Add(this.lblStaticPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxFileName);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UploadFile";
            this.ShowIcon = false;
            this.Text = "Upload File";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.TextBox txtBoxFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStaticPath;
        private System.Windows.Forms.Label lblDynPath;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem vectorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem linEnumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem linuxPrivCheckerpyToolStripMenuItem;
    }
}