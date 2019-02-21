using System.Collections;
using System.Windows.Forms;

namespace bantam.Classes
{
    public class FileBrowserTreeNodeSorter : IComparer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;

            if ((string)tx.Name == "dir"
             && (string)ty.Name == "file") {
                return -1;
            }

            if ((string)tx.Name == "file"
             && (string)ty.Name == "dir") {
                return 1;
            }

            return string.Compare(tx.Text, ty.Text);
        }
    }
}
