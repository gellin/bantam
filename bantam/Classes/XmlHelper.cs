using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
                        string hostTarget = (itemNode.Attributes?["host"] != null)  ? itemNode.Attributes?["host"].Value : "";
                        string requestArg = (itemNode.Attributes?["request_arg"] != null) ? itemNode.Attributes?["request_arg"].Value : "";
                        string requestMethod = (itemNode.Attributes?["request_method"] != null) ? itemNode.Attributes?["request_method"].Value : "";

                        if (string.IsNullOrEmpty(hostTarget)) {
                            continue;
                        }

                        if (BantamMain.Shells.ContainsKey(hostTarget)) {
                            continue;
                        } else {
                            BantamMain.Shells.Add(hostTarget, new ShellInfo());
                        }

                        if (string.IsNullOrEmpty(requestArg) == false
                        && requestArg != "command") {
                            BantamMain.Shells[hostTarget].requestArgName = requestArg;
                        }

                        if (string.IsNullOrEmpty(requestMethod) == false
                         && requestMethod == "cookie") {
                            BantamMain.Shells[hostTarget].sendDataViaCookie = true;
                        }

                        try {
                            Program.g_BantamMain.InitializeShellData(hostTarget);
                        } catch(Exception) {

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
                ShellInfo hostInfo = (ShellInfo)host.Value;

                if (hostInfo.down) {
                    continue;
                }

                XmlNode serverNode = xmlDoc.CreateElement("server");

                XmlAttribute hostAttribute = xmlDoc.CreateAttribute("host");
                hostAttribute.Value = host.Key;
                serverNode.Attributes.Append(hostAttribute);

                XmlAttribute requestArgAttribute = xmlDoc.CreateAttribute("request_arg");
                requestArgAttribute.Value = hostInfo.requestArgName;
                serverNode.Attributes.Append(requestArgAttribute);

                XmlAttribute requestMethod = xmlDoc.CreateAttribute("request_method");
                requestMethod.Value = (hostInfo.sendDataViaCookie ? "cookie" : "post"); //todo post is not the most proper thing it could differ
                serverNode.Attributes.Append(requestMethod);

                rootNode.AppendChild(serverNode);
            }
            xmlDoc.Save(configFile);
        }
    }
}
