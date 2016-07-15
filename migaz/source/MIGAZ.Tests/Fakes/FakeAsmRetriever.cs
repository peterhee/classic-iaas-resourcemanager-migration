using MIGAZ.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;

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
            { "VirtualMachine", new string[] { "cloudservicename", "deploymentname", "virtualmachinename" } },
            { "VMImages", new string[] { } },
        };
        private Dictionary<string, XmlNodeList> _responses = new Dictionary<string, XmlNodeList>();

        public FakeAsmRetriever(ILogProvider logProvider, IStatusProvider statusProvider) : base(logProvider, statusProvider)
        {
        }

        public void SetResponse(string resourceType, Hashtable info, XmlNodeList nodes)
        {
            string key = resourceType + ":" + SerialiseHashTable(resourceType, info);
            _responses[key] = nodes;
        }

        public override XmlNodeList GetAzureASMResources(string resourceType, string subscriptionId, Hashtable info, string token)
        {
            string key = resourceType + ":" + SerialiseHashTable(resourceType, info);
            return _responses[key];
        }

        private string SerialiseHashTable(string resourceType, Hashtable ht)
        {
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
