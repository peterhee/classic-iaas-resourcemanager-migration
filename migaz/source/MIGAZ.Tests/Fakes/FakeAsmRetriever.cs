using MIGAZ.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;
using System.IO;

namespace MIGAZ.Tests.Fakes
{
    class FakeAsmRetriever : AsmRetriever
    {
        private Dictionary<string, string[]> _keyProperties = new Dictionary<string, string[]>
        {
            { "Subscriptions", new string[] { } },
            { "VirtualNetworks", new string[] { } },
            { "ClientRootCertificates", new string[] { "virtualnetworkname"} },
            { "ClientRootCertificate", new string[] { "virtualnetworkname", "thumbprint" } },
            { "NetworkSecurityGroup", new string[] { "name"} },
            { "RouteTable", new string[] { "name"} },
            { "NSGSubnet", new string[] { "virtualnetworkname", "subnetname" } },
            { "VirtualNetworkGateway", new string[] { "virtualnetworkname" } },
            { "VirtualNetworkGatewaySharedKey", new string[] { "virtualnetworkname", "localnetworksitename" } },
            { "StorageAccounts", new string[] { } },
            { "StorageAccount", new string[] { "name" } },
            { "StorageAccountKeys", new string[] { "name" } },
            { "CloudServices", new string[] { } },
            { "CloudService", new string[] { "name" } },
            { "VirtualMachine", new string[] { "cloudservicename", "virtualmachinename", "deploymentname"} },
            { "VMImages", new string[] { } },
        };
        private Dictionary<string, XmlDocument> _responses = new Dictionary<string, XmlDocument>();

        public FakeAsmRetriever(ILogProvider logProvider, IStatusProvider statusProvider) : base(logProvider, statusProvider)
        {
        }

        public void LoadDocuments(string path)
        {
            foreach (var filename in Directory.GetFiles(path, "*.xml"))
            {
                var title = Path.GetFileNameWithoutExtension(filename);
                var parts = title.Split('-');
                string resourceType;
                var info = new Hashtable();

                switch (parts[0].ToLower())
                {
                    case "cloudservice":
                        resourceType = "CloudService";
                        info.Add("name", parts[1]);
                        break;
                    case "virtualmachine":
                        resourceType = "VirtualMachine";
                        info.Add("cloudservicename", parts[1]);
                        info.Add("virtualmachinename", parts[2]);
                        info.Add("deploymentname", parts[3]);
                        break;
                    case "storageaccountkeys":
                        resourceType = "StorageAccountKeys";
                        info.Add("name", parts[1]);
                        break;
                    case "storageaccount":
                        resourceType = "StorageAccount";
                        info.Add("name", parts[1]);
                        break;
                    case "virtualnetworks":
                        resourceType = "VirtualNetworks";
                        break;
                    case "clientrootcertificates":
                        resourceType = "ClientRootCertificates";
                        info.Add("virtualnetworkname", parts[1]);
                        break;
                    case "clientrootcertificate":
                        resourceType = "ClientRootCertificate";
                        info.Add("virtualnetworkname", parts[1]);
                        info.Add("thumbprint", parts[2]);
                        break;
                    case "virtualnetworkgateway":
                        resourceType = "VirtualNetworkGateway";
                        info.Add("virtualnetworkname", parts[1]);
                        break;
                    case "virtualnetworkgatewaysharedkey":
                        resourceType = "VirtualNetworkGatewaySharedKey";
                        info.Add("virtualnetworkname", parts[1]);
                        info.Add("localnetworksitename", parts[2]);
                        break;
                    case "networksecuritygroup":
                        resourceType = "NetworkSecurityGroup";
                        info.Add("name", parts[1]);
                        break;
                    case "routetable":
                        resourceType = "RouteTable";
                        info.Add("name", parts[1]);
                        break;
                    case "reservedips":
                        resourceType = "ReservedIPs";
                        break;
                    default:
                        throw new Exception();
                }

                var doc = new XmlDocument();
                doc.Load(filename);
                SetResponse(resourceType, info, doc);
            }
        }

        public void SetResponse(string resourceType, Hashtable info, XmlDocument doc)
        {
            string key = resourceType + ":" + SerialiseHashTable(resourceType, info);
            _responses[key] = doc;
        }

        public override async Task<XmlDocument> GetAzureASMResources(string resourceType, string subscriptionId, Hashtable info, string token)
        {
            string key = resourceType + ":" + SerialiseHashTable(resourceType, info);
            var xmlDoc = _responses[key];
            return RemoveXmlns(xmlDoc.OuterXml);
        }

        private string SerialiseHashTable(string resourceType, Hashtable ht)
        {
            if (ht == null)
            {
                return String.Empty;
            }
            var sb = new StringBuilder();

            // Sort keys
            var keyList = new List<string>();
            foreach(var key in ht.Keys)
            {
                keyList.Add((string)key);
            }
            keyList.Sort();

            foreach (var key in keyList)
            {
                if (_keyProperties[resourceType].Contains(key)) // Don't include properties from the hashtable that aren't needed
                {
                    sb.Append(key);
                    sb.Append("=");
                    sb.Append(ht[key]);
                    sb.Append(";");
                }
            }
            return sb.ToString();
        }
    }
}
