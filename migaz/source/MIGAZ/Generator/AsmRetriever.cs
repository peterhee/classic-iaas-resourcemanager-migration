using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MIGAZ.Generator
{
    public class AsmRetriever
    {
        private ILogProvider _logProvider;
        private IStatusProvider _statusProvider;

        public AsmRetriever(ILogProvider logProvider, IStatusProvider statusProvider)
        {
            _logProvider = logProvider;
            _statusProvider = statusProvider;
        }
        public virtual XmlNodeList GetAzureASMResources(string resourceType, string subscriptionId, Hashtable info, string token)
        {
            _logProvider.WriteLog("GetAzureASMResources", "Start");

            string url = null;
            string node = null;
            switch (resourceType)
            {
                case "Subscriptions":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + "subscriptions";
                    _statusProvider.UpdateStatus("BUSY: Getting Subscriptions...");
                    node = "Subscriptions/Subscription";
                    break; 
                case "VirtualNetworks":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/virtualnetwork";
                    node = "VirtualNetworkSites/VirtualNetworkSite";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Networks for Subscription ID : " + subscriptionId + "...");
                    break;
                case "ClientRootCertificates":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/clientrootcertificates";
                    node = "";
                    _statusProvider.UpdateStatus("BUSY: Getting Client Root Certificates for Virtual Network : " + info["virtualnetworkname"] + "...");
                    break;
                case "ClientRootCertificate":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/clientrootcertificates/" + info["thumbprint"];
                    node = "";
                    _statusProvider.UpdateStatus("BUSY: Getting certificate data for certificate : " + info["thumbprint"] + "...");
                    break;
                case "NetworkSecurityGroup":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/networksecuritygroups/" + info["name"] + "?detaillevel=Full";
                    node = "";
                    _statusProvider.UpdateStatus("BUSY: Getting Network Security Group : " + info["name"] + "...");
                    break;
                case "RouteTable":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/routetables/" + info["name"] + "?detailLevel=full";
                    node = "";
                    _statusProvider.UpdateStatus("BUSY: Getting Route Table : " + info["routetablename"] + "...");
                    break;
                case "NSGSubnet":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/virtualnetwork/" + info["virtualnetworkname"] + "/subnets/" + info["subnetname"] + "/networksecuritygroups";
                    node = "";
                    _statusProvider.UpdateStatus("BUSY: Getting NSG for subnet " + info["subnetname"] + "...");
                    break;
                case "VirtualNetworkGateway":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway";
                    node = "Gateway";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Network Gateway : " + info["virtualnetworkname"] + "...");
                    break;
                case "VirtualNetworkGatewaySharedKey":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/networking/" + info["virtualnetworkname"] + "/gateway/connection/" + info["localnetworksitename"] + "/sharedkey";
                    node = "SharedKey";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Network Gateway Shared Key: " + info["localnetworksitename"] + "...");
                    break;
                case "StorageAccounts":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/storageservices";
                    node = "StorageServices/StorageService";
                    _statusProvider.UpdateStatus("BUSY: Getting Storage Accounts for Subscription ID : " + subscriptionId + "...");
                    break;
                case "StorageAccount":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/storageservices/" + info["name"];
                    node = "StorageService";
                    _statusProvider.UpdateStatus("BUSY: Getting Storage Accounts for Subscription ID : " + subscriptionId + "...");
                    break;
                case "StorageAccountKeys":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/storageservices/" + info["name"] + "/keys";
                    node = "StorageService";
                    _statusProvider.UpdateStatus("BUSY: Getting Storage Accounts for Subscription ID : " + subscriptionId + "...");
                    break;
                case "CloudServices":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/hostedservices";
                    node = "HostedServices/HostedService";
                    _statusProvider.UpdateStatus("BUSY: Getting Cloud Services for Subscription ID : " + subscriptionId + "...");
                    break;
                case "CloudService":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/hostedservices/" + info["name"] + "?embed-detail=true";
                    node = "HostedService";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Machines for Cloud Service : " + info["name"] + "...");
                    break;
                case "VirtualMachine":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/hostedservices/" + info["cloudservicename"] + "/deployments/" + info["deploymentname"] + "/roles/" + info["virtualmachinename"];
                    node = "";
                    _statusProvider.UpdateStatus("BUSY: Getting Virtual Machines for Cloud Service : " + info["virtualmachinename"] + "...");
                    break;
                case "VMImages":
                    url = ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment) + subscriptionId + "/services/images";
                    node = "Images/OSImage";
                    break;
            }

            Application.DoEvents();

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
            request.Headers.Add("x-ms-version", "2015-04-01");
            request.Method = "GET";

            _logProvider.WriteLog("GetAzureASMResources", "GET " + url);

            string xml = "";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                xml = new StreamReader(response.GetResponseStream()).ReadToEnd();
                _logProvider.WriteLog("GetAzureASMResources", "RESPONSE " + response.StatusCode);
            }
            catch (Exception exception)
            {
                _logProvider.WriteLog("GetAzureASMResources", "EXCEPTION " + exception.Message);
                xml = "";
            }

            if (xml != "")
            {
                xml = xml.Replace(@"xmlns=""http://schemas.microsoft.com/windowsazure""", "");
                xml = xml.Replace(@"xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""", "");
                xml = xml.Replace(@"i:nil=""true""", "");
                xml = xml.Replace(@"i:type=", "type=");
                XmlDocument xmlDoc = new XmlDocument();

                if (xml[0].ToString() != "<")
                {
                    xml = "<data>" + xml + "</data>";
                }

                xmlDoc.LoadXml(xml);

                _logProvider.WriteLog("GetAzureASMResources", "End");
                writeXMLtoFile(url, xml);

                if (node == "")
                { return xmlDoc.ChildNodes; }
                else
                { return xmlDoc.SelectNodes(node); }
            }
            else
            {
                //XmlNodeList xmlnode = null;
                //return xmlnode;
                XmlDocument xmlDoc = new XmlDocument();

                _logProvider.WriteLog("GetAzureASMResources", "End");
                writeXMLtoFile(url, "");
                return xmlDoc.SelectNodes("Empty");
            }

        }

        private void writeXMLtoFile(string url, string xml)
        {
            string logfilepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MIGAZ-XML-" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
            string text = DateTime.Now.ToString() + "   " + url + Environment.NewLine;
            File.AppendAllText(logfilepath, text);
            text = xml + Environment.NewLine;
            File.AppendAllText(logfilepath, text);
            text = Environment.NewLine;
            File.AppendAllText(logfilepath, text);
        }

        public virtual void GetVMDetails(string subscriptionId, string cloudServiceName, string virtualMachineName, string token, out string deploymentName, out string virtualNetworkName, out string loadBalancerName)
        {
            Hashtable cloudserviceinfo = new Hashtable();
            cloudserviceinfo.Add("name", cloudServiceName);

            XmlNodeList hostedservice = GetAzureASMResources("CloudService", subscriptionId, cloudserviceinfo, token);
            if (hostedservice[0].SelectNodes("Deployments/Deployment").Count > 0)
            {
                if (hostedservice[0].SelectNodes("Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectNodes("RoleType").Count > 0)
                {
                    if (hostedservice[0].SelectNodes("Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectSingleNode("RoleType").InnerText == "PersistentVMRole")
                    {
                        virtualNetworkName = "empty";
                        if (hostedservice[0].SelectNodes("Deployments/Deployment")[0].SelectSingleNode("VirtualNetworkName") != null)
                        {
                            virtualNetworkName = hostedservice[0].SelectNodes("Deployments/Deployment")[0].SelectSingleNode("VirtualNetworkName").InnerText;
                        }
                        deploymentName = hostedservice[0].SelectNodes("Deployments/Deployment")[0].SelectSingleNode("Name").InnerText;
                        XmlNodeList roles = hostedservice[0].SelectNodes("Deployments/Deployment")[0].SelectNodes("RoleList/Role");
                        // GetVMLBMapping is necessary because a Cloud Service can have multiple availability sets
                        // On ARM, a load balancer can only be attached to 1 availability set
                        // Because of this, if multiple availability sets exist, we are breaking the cloud service in multiple load balancers
                        //     to respect all availability sets
                        Dictionary<string, string> vmlbmapping = GetVMLBMapping(cloudServiceName, roles);
                        foreach (XmlNode role in roles)
                        {
                            string currentVM = role.SelectSingleNode("RoleName").InnerText;
                            if (currentVM == virtualMachineName)
                            {
                                loadBalancerName = vmlbmapping[virtualMachineName];
                                return;
                            }
                        }
                    }
                }
            }
            throw new InvalidOperationException("Requested VM could not be found");
        }

        public Dictionary<string, string> GetVMLBMapping(string cloudservicename, XmlNodeList roles)
        {
            Dictionary<string, string> aslbnamemapping = new Dictionary<string, string>();
            Dictionary<string, string> aslbnamemapping2 = new Dictionary<string, string>();

            foreach (XmlNode role in roles)
            {
                string availabilitysetname = "empty";
                if (role.SelectNodes("AvailabilitySetName").Count > 0)
                {
                    availabilitysetname = role.SelectSingleNode("AvailabilitySetName").InnerText;
                }

                if (!aslbnamemapping.ContainsKey(availabilitysetname))
                {
                    aslbnamemapping.Add(availabilitysetname, "");
                }
            }

            if (aslbnamemapping.Count == 1)
            {
                foreach (KeyValuePair<string, string> keyvaluepair in aslbnamemapping)
                {
                    aslbnamemapping2.Add(keyvaluepair.Key, cloudservicename);
                }
            }
            else
            {
                int increment = 1;
                foreach (KeyValuePair<string, string> keyvaluepair in aslbnamemapping)
                {
                    aslbnamemapping2.Add(keyvaluepair.Key, cloudservicename + "-LB" + increment.ToString());
                    increment += 1;
                }
            }

            Dictionary<string, string> vmlbmapping = new Dictionary<string, string>();

            foreach (XmlNode role in roles)
            {
                string virtualmachinename = role.SelectSingleNode("RoleName").InnerText;
                string availabilitysetname = "empty";
                if (role.SelectNodes("AvailabilitySetName").Count > 0)
                {
                    availabilitysetname = role.SelectSingleNode("AvailabilitySetName").InnerText;
                }
                string loadbalancername = aslbnamemapping2[availabilitysetname];

                vmlbmapping.Add(virtualmachinename, loadbalancername);
            }

            return vmlbmapping;
        }
    }
}
