using System;
using System.Windows.Forms;

namespace bantam_php
{
    class GUI_Helper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytes(double bytes)
        {
            string[] suffix = { "B", "KB", "MB", "GB", "TB" };
            int i = 0;

            for (; i < suffix.Length && bytes >= 1024; i++, bytes /= 1024) { }

            return String.Format("{0:0.##} {1}", bytes, suffix[i]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public static ListViewItem GetFirstSelectedListview(ListView lv)
        {
            if (lv.SelectedItems.Count > 0)
            {
                foreach (ListViewItem lvi in lv.SelectedItems)
                {
                    return lvi;
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public static void CopyNodes(TreeView source, TreeView dest)
        {
            TreeNode newTn;
            foreach (TreeNode tn in source.Nodes)
            {
                newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.ImageIndex)
                {
                    ForeColor = tn.ForeColor
                };
                CopyChilds(newTn, tn);
                dest.Nodes.Add(newTn);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="willCopied"></param>
        public static void CopyChilds(TreeNode parent, TreeNode willCopied)
        {
            TreeNode newTn;
            foreach (TreeNode tn in willCopied.Nodes)
            {
                newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.ImageIndex)
                {
                    ForeColor = tn.ForeColor
                };
                CopyChilds(newTn, tn);
                parent.Nodes.Add(newTn);
            }
        }
    }
}
