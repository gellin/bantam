using System;
using System.Drawing;
using System.Windows.Forms;

namespace bantam.Classes
{
    /// <summary>
    /// 
    /// </summary>
    public class TextBoxButton : TextBox
    {
        Button btnBack = new Button();

        public TextBoxButton()
        {
            //InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mouseClickFunction"></param>
        /// <param name="textboxHeight"></param>
        public void Initialize(MouseEventHandler mouseClickFunction, int textboxHeight)
        {
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
