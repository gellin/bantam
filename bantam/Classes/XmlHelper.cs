using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace bantam.Classes
{
    static class XmlHelper
    {
        public async static Task LoadShells(string configFile)
        {
            if (File.Exists(configFile)) {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configFile);

                XmlNodeList itemNodes = xmlDoc.SelectNodes("//servers/server");

                if (itemNodes.Count > 0) {
                    foreach (XmlNode itemNode in itemNodes) {
                        string hostTarget = (itemNode.Attributes?["host"] != null) ? itemNode.Attributes?["host"].Value : string.Empty;
                        string requestArg = (itemNode.Attributes?["request_arg"] != null) ? itemNode.Attributes?["request_arg"].Value : string.Empty;
                        string requestMethod = (itemNode.Attributes?["request_method"] != null) ? itemNode.Attributes?["request_method"].Value : string.Empty;
                        string responseEncryption = (itemNode.Attributes?["response_encryption"] != null) ? itemNode.Attributes?["response_encryption"].Value : string.Empty;
                        string responseEncryptionMode = (itemNode.Attributes?["response_encryption_mode"] != null) ? itemNode.Attributes?["response_encryption_mode"].Value : string.Empty;
                        string gzipRequest = (itemNode.Attributes?["gzip_request"] != null) ? itemNode.Attributes?["gzip_request"].Value : string.Empty;
                        string requestEncryption = (itemNode.Attributes?["request_encryption"] != null) ? itemNode.Attributes?["request_encryption"].Value : string.Empty;
                        string requestEncryptionKey = (itemNode.Attributes?["request_encryption_key"] != null) ? itemNode.Attributes?["request_encryption_key"].Value : string.Empty;
                        string requestEncryptionIV = (itemNode.Attributes?["request_encryption_iv"] != null) ? itemNode.Attributes?["request_encryption_iv"].Value : string.Empty;
                        string requestEncryptionIVVarName = (itemNode.Attributes?["request_encryption_iv_var_name"] != null) ? itemNode.Attributes?["request_encryption_iv_var_name"].Value : string.Empty;
                        
                        if (string.IsNullOrEmpty(hostTarget)) {
                            continue;
                        }

                        if (BantamMain.Shells.ContainsKey(hostTarget)) {
                            continue;
                        } else {
                            BantamMain.Shells.TryAdd(hostTarget, new ShellInfo());
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
                            if (responseEncryption == "1") {
                                BantamMain.Shells[hostTarget].responseEncryption = true;
                            } else {
                                BantamMain.Shells[hostTarget].responseEncryption = false;
                            }
                        }

                        if (string.IsNullOrEmpty(requestEncryption) == false 
                        && requestEncryption == "1") {
                            BantamMain.Shells[hostTarget].requestEncryption = true;

                            if (string.IsNullOrEmpty(requestEncryptionKey) == false) {
                                BantamMain.Shells[hostTarget].requestEncryptionKey = requestEncryptionKey;
                            } else {
                                //todo global logging
                            }

                            if (string.IsNullOrEmpty(requestEncryptionIV) == false) {
                                BantamMain.Shells[hostTarget].requestEncryptionIV = requestEncryptionIV;
                                BantamMain.Shells[hostTarget].requestEncryptionIVRequestVarName = string.Empty;
                                BantamMain.Shells[hostTarget].sendRequestEncryptionIV = false;
                            } else {
                                if (string.IsNullOrEmpty(requestEncryptionIVVarName) == false) {
                                    BantamMain.Shells[hostTarget].sendRequestEncryptionIV = true;
                                    BantamMain.Shells[hostTarget].requestEncryptionIV = string.Empty;
                                    BantamMain.Shells[hostTarget].requestEncryptionIVRequestVarName = requestEncryptionIVVarName;
                                } else {
                                    //todo global logging
                                }
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
                                //todo level 3 log failed check
                            }
                        }

                        try {
                            Program.g_BantamMain.InitializeShellData(hostTarget);
                        } catch (Exception) {
                            //todo logging
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
                //if (shellInfo.down) {
                // continue;
                //}

                XmlNode serverNode = xmlDoc.CreateElement("server");

                XmlAttribute hostAttribute = xmlDoc.CreateAttribute("host");
                hostAttribute.Value = host.Key;

                serverNode.Attributes.Append(hostAttribute);

                XmlAttribute requestArgAttribute = xmlDoc.CreateAttribute("request_arg");
                requestArgAttribute.Value = shellInfo.requestArgName;

                serverNode.Attributes.Append(requestArgAttribute);

                XmlAttribute requestMethod = xmlDoc.CreateAttribute("request_method");
                requestMethod.Value = (shellInfo.sendDataViaCookie ? "cookie" : "post");
                serverNode.Attributes.Append(requestMethod);

                XmlAttribute responseEncryption = xmlDoc.CreateAttribute("response_encryption");
                responseEncryption.Value = (shellInfo.responseEncryption ? "1" : "0");
                serverNode.Attributes.Append(responseEncryption);

                XmlAttribute responseEncrpytionMode = xmlDoc.CreateAttribute("response_encryption_mode");
                responseEncrpytionMode.Value = shellInfo.responseEncryptionMode.ToString();
                serverNode.Attributes.Append(responseEncrpytionMode);

                XmlAttribute gzipRequest = xmlDoc.CreateAttribute("gzip_request");
                gzipRequest.Value = (shellInfo.gzipRequestData ? "1" : "0");
                serverNode.Attributes.Append(gzipRequest);

                XmlAttribute requestEncryption = xmlDoc.CreateAttribute("request_encryption");
                requestEncryption.Value = (shellInfo.requestEncryption ? "1" : "0");
                serverNode.Attributes.Append(requestEncryption);

                XmlAttribute requestEncryptionKey = xmlDoc.CreateAttribute("request_encryption_key");
                requestEncryptionKey.Value = shellInfo.requestEncryptionKey;
                serverNode.Attributes.Append(requestEncryptionKey);

                XmlAttribute requestEncryptionIV = xmlDoc.CreateAttribute("request_encryption_iv");
                requestEncryptionIV.Value = shellInfo.requestEncryptionIV;
                serverNode.Attributes.Append(requestEncryptionIV);

                XmlAttribute requestEncryptionIVVarName = xmlDoc.CreateAttribute("request_encryption_iv_var_name");
                requestEncryptionIVVarName.Value = shellInfo.requestEncryptionIVRequestVarName;
                serverNode.Attributes.Append(requestEncryptionIVVarName);

                rootNode.AppendChild(serverNode);
            }
            xmlDoc.Save(configFile);
        }
    }
}
