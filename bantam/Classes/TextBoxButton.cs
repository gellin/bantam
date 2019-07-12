using System;
using System.Drawing;
using System.Windows.Forms;

namespace bantam.Classes
{
    /// <summary>
    /// This is a custom Textbox control that contains a button in the left side that can trigger an action via (MouseEventHandler) and a custom KeyEventHandler
    /// </summary>
    public class TextBoxButton : TextBox
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Button btnBack = new Button();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mouseClickFunction"></param>
        /// <param name="textboxHeight"></param>
        public void Initialize(int x, int y, int w, int h, string name, KeyEventHandler keyEventHandler, MouseEventHandler mouseClickFunction, int textboxHeight)
        {
            this.Name = name;
            this.TabIndex = 1;
            this.Size = new System.Drawing.Size(w, h);
            this.Location = new System.Drawing.Point(x, y);
            this.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.KeyDown += keyEventHandler;

            btnBack.Size = new Size(25, textboxHeight + 2);
            btnBack.Location = new Point(1, -1);
            btnBack.Image = global::bantam.Properties.Resources.undo;
            btnBack.MouseClick += mouseClickFunction;
            btnBack.TabStop = false;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Cursor = System.Windows.Forms.Cursors.Hand;

            this.Controls.Add(btnBack);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        protected override void WndProc(ref Message msg)
        {
            base.WndProc(ref msg);

            if (msg.Msg == 0x30) {
                SendMessage(this.Handle, 0xd3, (IntPtr)1, (IntPtr)btnBack.Width);
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    }
}
