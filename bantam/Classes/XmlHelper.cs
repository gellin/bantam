using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace bantam_php
{
    class XmlHelper
    {
        public static void LoadShells(string configFile)
        {
            if (File.Exists(configFile)) {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configFile);

                XmlNodeList itemNodes = xmlDoc.SelectNodes("//servers/server");

                if (itemNodes.Count > 0) {
                    foreach (XmlNode itemNode in itemNodes) {
                        string hostTarget = (itemNode.Attributes?["host"] != null)  ? itemNode.Attributes?["host"].Value : string.Empty;
                        string requestArg = (itemNode.Attributes?["request_arg"] != null) ? itemNode.Attributes?["request_arg"].Value : string.Empty;
                        string requestMethod = (itemNode.Attributes?["request_method"] != null) ? itemNode.Attributes?["request_method"].Value : string.Empty;
                        string responseEncryption = (itemNode.Attributes?["response_encryption"] != null) ? itemNode.Attributes?["response_encryption"].Value : string.Empty;
                        string responseEncryptionMode = (itemNode.Attributes?["response_encryption_mode"] != null) ? itemNode.Attributes?["response_encryption_mode"].Value : string.Empty;
                        string gzipRequest = (itemNode.Attributes?["gzip_request"] != null) ? itemNode.Attributes?["gzip_request"].Value : string.Empty;

                        if (string.IsNullOrEmpty(hostTarget)) {
                            continue;
                        }

                        if (BantamMain.Shells.ContainsKey(hostTarget)) {
                            continue;
                        } else {
                            BantamMain.Shells.Add(hostTarget, new ShellInfo());
                        }

                        //todo default this 
                        if (string.IsNullOrEmpty(requestArg) == false
                        && requestArg != "command") {
                            BantamMain.Shells[hostTarget].requestArgName = requestArg;
                        }
                        
                        //todo don't default this
                        if (string.IsNullOrEmpty(requestMethod) == false
                         && requestMethod == "cookie") {
                            BantamMain.Shells[hostTarget].sendDataViaCookie = true;
                        }

                        if (string.IsNullOrEmpty(responseEncryption) == false) {
                            if(responseEncryption == "1") {
                                BantamMain.Shells[hostTarget].responseEncryption = true;
                            } else {
                                BantamMain.Shells[hostTarget].responseEncryption = false;
                            }
                        }

                        if (string.IsNullOrEmpty(gzipRequest) == false) {
                            if (gzipRequest == "1") {
                                BantamMain.Shells[hostTarget].gzipRequestData = true;
                            } else {
                                BantamMain.Shells[hostTarget].gzipRequestData = false;
                            }
                        }

                        if (string.IsNullOrEmpty(responseEncryptionMode) == false) {
                            if (responseEncryptionMode == (EncryptionHelper.RESPONSE_ENCRYPTION_TYPES.OPENSSL).ToString("D")) {
                                BantamMain.Shells[hostTarget].responseEncryptionMode = (int)EncryptionHelper.RESPONSE_ENCRYPTION_TYPES.OPENSSL;
                            } else if (responseEncryptionMode == EncryptionHelper.RESPONSE_ENCRYPTION_TYPES.MCRYPT.ToString("D")) {
                                BantamMain.Shells[hostTarget].responseEncryptionMode = (int)EncryptionHelper.RESPONSE_ENCRYPTION_TYPES.MCRYPT;
                            } else {
                                BantamMain.Shells[hostTarget].responseEncryptionMode = (int)EncryptionHelper.RESPONSE_ENCRYPTION_TYPES.OPENSSL;
                                //todo level 3 log failed check
                            }
                        }

                        try {
                            Program.g_BantamMain.InitializeShellData(hostTarget);
                        } catch(Exception) {
                            //todo loging
                        }
                            
                    }
                }
            } else {
                MessageBox.Show("Config file (" + configFile + ") is missing.", "Oh... Shied..");
            }
        }

        public static void SaveShells(string configFile)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlNode rootNode = xmlDoc.CreateElement("servers");
            xmlDoc.AppendChild(rootNode);

            foreach (KeyValuePair<String, ShellInfo> host in BantamMain.Shells) {
                ShellInfo shellInfo = (ShellInfo)host.Value;

                //saves shells that are down, possibly make this an option
                if (shellInfo.down) {
                   // continue;
                }

                XmlNode serverNode = xmlDoc.CreateElement("server");

                XmlAttribute hostAttribute = xmlDoc.CreateAttribute("host");
                hostAttribute.Value = host.Key;

                serverNode.Attributes.Append(hostAttribute);

                XmlAttribute requestArgAttribute = xmlDoc.CreateAttribute("request_arg");
                requestArgAttribute.Value = shellInfo.requestArgName;

                serverNode.Attributes.Append(requestArgAttribute);

                XmlAttribute requestMethod = xmlDoc.CreateAttribute("request_method");
                requestMethod.Value = (shellInfo.sendDataViaCookie ? "cookie" : "post"); //todo
                serverNode.Attributes.Append(requestMethod);

                XmlAttribute responseEncryption = xmlDoc.CreateAttribute("response_encryption");
                responseEncryption.Value = (shellInfo.responseEncryption ? "1" : "0" ); //todo
                serverNode.Attributes.Append(responseEncryption);

                XmlAttribute responseEncrpytionMode = xmlDoc.CreateAttribute("response_encryption_mode");
                responseEncrpytionMode.Value = shellInfo.responseEncryptionMode.ToString(); //todo
                serverNode.Attributes.Append(responseEncrpytionMode);

                XmlAttribute gzipRequest = xmlDoc.CreateAttribute("gzip_request");
                gzipRequest.Value = (shellInfo.gzipRequestData ? "1" : "0"); //todo
                serverNode.Attributes.Append(gzipRequest);

                rootNode.AppendChild(serverNode);
            }
            xmlDoc.Save(configFile);
        }
    }
}
