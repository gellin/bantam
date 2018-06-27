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
            int i = 0;
            string[] suffixes = { "B", "KB", "MB", "GB", "TB" };

            for (; i < suffixes.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                
            }

            return String.Format("{0:0.##} {1}", bytes, suffixes[i]);
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
            foreach (TreeNode tn in source.Nodes)
            {
                TreeNode newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.ImageIndex)
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
            foreach (TreeNode tn in willCopied.Nodes)
            {
                TreeNode newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.ImageIndex)
                {
                    ForeColor = tn.ForeColor
                };
                CopyChilds(newTn, tn);
                parent.Nodes.Add(newTn);
            }
        }
    }
}
