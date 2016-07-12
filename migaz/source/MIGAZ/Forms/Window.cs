using System;
using System.Windows.Forms;
using System.Collections;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.Reflection;
using MIGAZ.Classes;
using MIGAZ.Classes.ARM;

namespace MIGAZ
{
    public partial class Window : Form
    {
        private string subscriptionid;
        List<Resource> resources;
        Dictionary<string, Parameter> parameters;
        public List<CopyBlobDetail> copyblobdetails;
        Dictionary<string, string> processeditems;
        Dictionary<string, string> subscriptionsAndTenants;

        public Window()
        {
            InitializeComponent();
        }

        private void Window_Load(object sender, EventArgs e)
        {
            writeLog("Window_Load", "Program start");

            this.Text = "migAz (" + Assembly.GetEntryAssembly().GetName().Version.ToString() + ")";
            NewVersionAvailable(); // check if there a new version of the app
        }

        private void btnGetToken_Click(object sender, EventArgs e)
        {
            writeLog("GetToken_Click", "Start");


            cmbSubscriptions.Enabled = false;
            cmbSubscriptions.Items.Clear();
            lvwVirtualNetworks.Items.Clear();
            lvwStorageAccounts.Items.Clear();
            lvwVirtualMachines.Items.Clear();

            string token = GetToken("common", PromptBehavior.Always, true);

            subscriptionsAndTenants = new Dictionary<string, string>();
            foreach (XmlNode subscription in GetAzureASMResources("Subscriptions", null, token))
            {
                cmbSubscriptions.Items.Add(subscription.SelectSingleNode("SubscriptionID").InnerText + " | " + subscription.SelectSingleNode("SubscriptionName").InnerText);
                subscriptionsAndTenants.Add(subscription.SelectSingleNode("SubscriptionID").InnerText, subscription.SelectSingleNode("AADTenantID").InnerText);
            }

            cmbSubscriptions.Enabled = true;
            txtDestinationFolder.Enabled = true;
            btnChoosePath.Enabled = true;

            lblStatus.Text = "Ready";
            writeLog("GetToken_Click", "End");
        }

        private string GetToken(string tenantId, PromptBehavior promptBehavior, bool updateUI = false)
        {
  
            lblStatus.Text = "BUSY: Authenticating...";
            AuthenticationContext context = new AuthenticationContext("https://login.windows.net/" + tenantId);

            AuthenticationResult result = null;
            result = context.AcquireToken("https://management.core.windows.net/", app.Default.ClientId, new Uri(app.Default.ReturnURL), promptBehavior);
            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the token");
            }
            if (updateUI)
            {
                lblSignInText.Text = $"Signed in as {result.UserInfo.DisplayableId}";
            }

            return result.AccessToken;
             
          
        }

        private XmlNodeList GetAzureASMResources(string resourceType, Hashtable info, string token)
        {
            writeLog("GetAzureASMResources", "Start");

            string url = null;
            string node = null;
            switch (resourceType)
            {
                case "Subscriptions":
                    url = "https://management.core.windows.net/subscriptions";
                    lblStatus.Text = "BUSY: Getting Subscriptions...";
                    node = "Subscriptions/Subscription";
                    break;
                case "VirtualNetworks":
                    url = "https://management.core.windows.net/" + subscriptionid + "/services/networking/virtualnetwork";
                    node = "VirtualNetworkSites/VirtualNetworkSite";
                    lblStatus.Text = "BUSY: Getting Virtual Networks for Subscription ID : " + subscriptionid + "...";
                    break;
                case "ClientRootCertificates":
                    url = "https://management.core.windows.net/" + subscriptionid + "/services/networking/" + info["virtualnetworkname"] + "/gateway/clientrootcertificates";
                    node = "";
                    lblStatus.Text = "BUSY: Getting Client Root Certificates for Virtual Network : " + info["virtualnetworkname"] + "...";
                    break;
                case "ClientRootCertificate":
                    url = "https://management.core.windows.net/" + subscriptionid + "/services/networking/" + info["virtualnetworkname"] + "/gateway/clientrootcertificates/" + info["thumbprint"];
                    node = "";
                    lblStatus.Text = "BUSY: Getting certificate data for certificate : " + info["thumbprint"] + "...";
                    break;
                case "NetworkSecurityGroup":
                    url = "https://management.core.windows.net/" + subscriptionid + "/services/networking/networksecuritygroups/" + info["name"] + "?detaillevel=Full";
                    node = "";
                    lblStatus.Text = "BUSY: Getting Network Security Group : " + info["name"] + "...";
                    break;
                case "RouteTable":
                    url = "https://management.core.windows.net/" + subscriptionid + "/services/networking/routetables/" + info["name"] + "?detailLevel=full";
                    node = "";
                    lblStatus.Text = "BUSY: Getting Route Table : " + info["routetablename"] + "...";
                    break;
                case "NSGSubnet":
                    url = "https://management.core.windows.net/" + subscriptionid + "/services/networking/virtualnetwork/" + info["virtualnetworkname"] + "/subnets/" + info["subnetname"] + "/networksecuritygroups";
                    node = "";
                    lblStatus.Text = "BUSY: Getting NSG for subnet " + info["subnetname"] + "...";
                    break;
                case "VirtualNetworkGateway":
                    url = "https://management.core.windows.net/" + subscriptionid + "/services/networking/" + info["virtualnetworkname"] + "/gateway";
                    node = "Gateway";
                    lblStatus.Text = "BUSY: Getting Virtual Network Gateway : " + info["virtualnetworkname"] + "...";
                    break;
                case "VirtualNetworkGatewaySharedKey":
                    url = "https://management.core.windows.net/" + subscriptionid + "/services/networking/" + info["virtualnetworkname"] + "/gateway/connection/" + info["localnetworksitename"] + "/sharedkey";
                    node = "SharedKey";
                    lblStatus.Text = "BUSY: Getting Virtual Network Gateway Shared Key: " + info["localnetworksitename"] + "...";
                    break;
                case "StorageAccounts":
                    url = "https://management.core.windows.net/" + subscriptionid + "/services/storageservices";
                    node = "StorageServices/StorageService";
                    lblStatus.Text = "BUSY: Getting Storage Accounts for Subscription ID : " + subscriptionid + "...";
                    break;
                case "StorageAccount":
                    url = "https://management.core.windows.net/" + subscriptionid + "/services/storageservices/" + info["name"];
                    node = "StorageService";
                    lblStatus.Text = "BUSY: Getting Storage Accounts for Subscription ID : " + subscriptionid + "...";
                    break;
                case "StorageAccountKeys":
                    url = "https://management.core.windows.net/" + subscriptionid + "/services/storageservices/" + info["name"] + "/keys";
                    node = "StorageService";
                    lblStatus.Text = "BUSY: Getting Storage Accounts for Subscription ID : " + subscriptionid + "...";
                    break;
                case "CloudServices":
                    url = "https://management.core.windows.net/" + subscriptionid + "/services/hostedservices";
                    node = "HostedServices/HostedService";
                    lblStatus.Text = "BUSY: Getting Cloud Services for Subscription ID : " + subscriptionid + "...";
                    break;
                case "CloudService":
                    url = "https://management.core.windows.net/" + subscriptionid + "/services/hostedservices/" + info["name"] + "?embed-detail=true";
                    node = "HostedService";
                    lblStatus.Text = "BUSY: Getting Virtual Machines for Cloud Service : " + info["name"] + "...";
                    break;
                case "VirtualMachine":
                    url = "https://management.core.windows.net/" + subscriptionid + "/services/hostedservices/" + info["cloudservicename"] + "/deployments/" + info["deploymentname"] + "/roles/" + info["virtualmachinename"];
                    node = "";
                    lblStatus.Text = "BUSY: Getting Virtual Machines for Cloud Service : " + info["virtualmachinename"] + "...";
                    break;
                case "VMImages":
                    url = "https://management.core.windows.net/" + subscriptionid + "/services/images";
                    node = "Images/OSImage";
                    break;
            }

            Application.DoEvents();

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
            request.Headers.Add("x-ms-version", "2015-04-01");
            request.Method = "GET";

            writeLog("GetAzureASMResources", "GET " + url);

            string xml = "";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                xml = new StreamReader(response.GetResponseStream()).ReadToEnd();
                writeLog("GetAzureASMResources", "RESPONSE " + response.StatusCode);
            }
            catch (Exception exception)
            {
                writeLog("GetAzureASMResources", "EXCEPTION " + exception.Message);
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

                writeLog("GetAzureASMResources", "End");
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

                writeLog("GetAzureASMResources", "End");
                writeXMLtoFile(url, "");
                return xmlDoc.SelectNodes("Empty");
            }

        }

        private void cmbSubscriptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSubscriptions.Enabled == true)
            {
                writeLog("Subscriptions_SelectionChanged", "Start");

                lvwVirtualNetworks.Items.Clear();
                lvwStorageAccounts.Items.Clear();
                lvwVirtualMachines.Items.Clear();

                // Get Subscription from ComboBox
                subscriptionid = cmbSubscriptions.SelectedItem.ToString().Split(new char[] {'|'})[0].ToString().Trim();
                var token = GetToken(subscriptionsAndTenants[subscriptionid], PromptBehavior.Auto);

                foreach (XmlNode virtualnetworksite in GetAzureASMResources("VirtualNetworks", null, token))
                {
                    lvwVirtualNetworks.Items.Add(virtualnetworksite.SelectSingleNode("Name").InnerText);
                    Application.DoEvents();
                }

                foreach (XmlNode storageaccount in GetAzureASMResources("StorageAccounts", null, token))
                {
                    lvwStorageAccounts.Items.Add(storageaccount.SelectSingleNode("ServiceName").InnerText);
                    Application.DoEvents();
                }

                foreach (XmlNode cloudservice in GetAzureASMResources("CloudServices", null, token))
                {
                    string cloudservicename = cloudservice.SelectSingleNode("ServiceName").InnerText;

                    Hashtable cloudserviceinfo = new Hashtable();
                    cloudserviceinfo.Add("name", cloudservicename);

                    XmlNodeList hostedservice = GetAzureASMResources("CloudService", cloudserviceinfo, token);
                    if (hostedservice[0].SelectNodes("Deployments/Deployment").Count > 0)
                    {
                        if (hostedservice[0].SelectNodes("Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectNodes("RoleType").Count > 0)
                        {
                            if (hostedservice[0].SelectNodes("Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectSingleNode("RoleType").InnerText == "PersistentVMRole")
                            {
                                string virtualnetworkname = "empty";
                                if (hostedservice[0].SelectNodes("Deployments/Deployment")[0].SelectSingleNode("VirtualNetworkName") != null)
                                {
                                    virtualnetworkname = hostedservice[0].SelectNodes("Deployments/Deployment")[0].SelectSingleNode("VirtualNetworkName").InnerText;
                                }
                                string deploymentname = hostedservice[0].SelectNodes("Deployments/Deployment")[0].SelectSingleNode("Name").InnerText;
                                XmlNodeList roles = hostedservice[0].SelectNodes("Deployments/Deployment")[0].SelectNodes("RoleList/Role");
                                // GetVMLBMapping is necessary because a Cloud Service can have multiple availability sets
                                // On ARM, a load balancer can only be attached to 1 availability set
                                // Because of this, if multiple availability sets exist, we are breaking the cloud service in multiple load balancers
                                //     to respect all availability sets
                                Dictionary<string, string> vmlbmapping = GetVMLBMapping(cloudservicename, roles);
                                foreach (XmlNode role in roles)
                                {
                                    string virtualmachinename = role.SelectSingleNode("RoleName").InnerText;
                                    string loadbalancername = vmlbmapping[virtualmachinename];
                                    var listItem = new ListViewItem(cloudservicename);
                                    listItem.SubItems.AddRange(new[] { virtualmachinename, deploymentname, virtualnetworkname, loadbalancername });
                                    lvwVirtualMachines.Items.Add(listItem);
                                    Application.DoEvents();
                                }
                            }
                        }
                    }
                }

                lblStatus.Text = "Ready";

                writeLog("Subscriptions_SelectionChanged", "End");
            }
        }


        private void lvwVirtualNetworks_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateExportItemsCount();
        }

        private void lvwStorageAccounts_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateExportItemsCount();
        }

        private void lvwVirtualMachines_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateExportItemsCount();
        }

        private void UpdateExportItemsCount()
        {
            int numofobjects = lvwVirtualNetworks.CheckedItems.Count + lvwStorageAccounts.CheckedItems.Count + lvwVirtualMachines.CheckedItems.Count;
            btnExport.Text = "Export " + numofobjects.ToString() + " objects";
        }

        private void btnChoosePath_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
                txtDestinationFolder.Text = folderBrowserDialog.SelectedPath;
        }

        private void txtDestinationFolder_TextChanged(object sender, EventArgs e)
        {
            if (txtDestinationFolder.Text == "")
                btnExport.Enabled = false;
            else
                btnExport.Enabled = true;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            writeLog("Export_Click", "Start");

            app.Default.ExecutionId = Guid.NewGuid().ToString();
            resources = new List<Resource>();
            parameters = new Dictionary<string, Parameter>();

            processeditems = new Dictionary<string, string>();
            copyblobdetails = new List<CopyBlobDetail>();

            var token = GetToken(subscriptionsAndTenants[subscriptionid], PromptBehavior.Auto);

            writeLog("Export_Click", "Start processing selected virtual networks");
            // process selected virtual networks
            foreach (var selectedrow in lvwVirtualNetworks.CheckedItems)
            {
                var item = (ListViewItem)selectedrow;
                string virtualnetworkname = item.Text;
                lblStatus.Text = "BUSY: Exporting Virtual Network : " + virtualnetworkname;

                foreach (XmlNode virtualnetworksite in GetAzureASMResources("VirtualNetworks", null, token))
                {
                    if (virtualnetworksite.SelectSingleNode("Name").InnerText == virtualnetworkname)
                    {
                        BuildVirtualNetworkObject(virtualnetworksite, token);
                    }
                }
            }
            writeLog("Export_Click", "End processing selected virtual networks");

            writeLog("Export_Click", "Start processing selected storage accounts");
            // process selected storage accounts
            foreach (var selectedrow in lvwStorageAccounts.CheckedItems)
            {
                var item = (ListViewItem)selectedrow;
                string storageaccountname = item.Text;
                lblStatus.Text = "BUSY: Exporting Storage Account : " + storageaccountname;

                Hashtable storageaccountinfo = new Hashtable();
                storageaccountinfo.Add("name", storageaccountname);

                XmlNode storageaccount = GetAzureASMResources("StorageAccount", storageaccountinfo, token)[0];
                BuildStorageAccountObject(storageaccount);
            }
            writeLog("Export_Click", "End processing selected storage accounts");

            writeLog("Export_Click", "Start processing selected cloud services and virtual machines");
            // process selected cloud services and virtual machines
            string cloudservicecontrol = "";
            foreach (var selectedrow in lvwVirtualMachines.CheckedItems)
            {
                var listViewItem = (ListViewItem)selectedrow;
                string cloudservicename = listViewItem.Text;
                string virtualmachinename = listViewItem.SubItems[1].Text;
                string deploymentname = listViewItem.SubItems[2].Text;
                string virtualnetworkname = listViewItem.SubItems[3].Text.Replace(" ", "");
                string loadbalancername = listViewItem.SubItems[4].Text;
                string location = "";

                Hashtable cloudserviceinfo = new Hashtable();
                cloudserviceinfo.Add("name", cloudservicename);
                XmlNode cloudservice = GetAzureASMResources("CloudService", cloudserviceinfo, token)[0];
                location = cloudservice.SelectSingleNode("HostedServiceProperties/Location").InnerText;

                //if (cloudservicename != cloudservicecontrol)
                //{
                    cloudservicecontrol = cloudservicename;
                    lblStatus.Text = "BUSY: Exporting Cloud Service : " + cloudservicename;

                    BuildPublicIPAddressObject(cloudservice, loadbalancername);

                    BuildLoadBalancerObject(cloudservice, loadbalancername, token);
                //}

                Hashtable virtualmachineinfo = new Hashtable();
                virtualmachineinfo.Add("cloudservicename", cloudservicename);
                virtualmachineinfo.Add("deploymentname", deploymentname);
                virtualmachineinfo.Add("virtualmachinename", virtualmachinename);
                virtualmachineinfo.Add("virtualnetworkname", virtualnetworkname);
                virtualmachineinfo.Add("loadbalancername", loadbalancername);
                virtualmachineinfo.Add("location", location);

                XmlNode virtualmachine = GetAzureASMResources("VirtualMachine", virtualmachineinfo, token)[0];

                // create new virtual network if virtualnetworkname is "empty"
                if (virtualnetworkname == "empty")
                {
                    BuildNewVirtualNetworkObject(cloudservice, virtualmachineinfo);
                }

                // process availability set
                BuildAvailabilitySetObject(virtualmachine, virtualmachineinfo);

                // process network interface
                List<NetworkProfile_NetworkInterface> networkinterfaces = new List<NetworkProfile_NetworkInterface>();
                BuildNetworkInterfaceObject(virtualmachine, virtualmachineinfo, ref networkinterfaces, token);

                // process virtual machine
                BuildVirtualMachineObject(virtualmachine, virtualmachineinfo, networkinterfaces, token);
            }
            writeLog("Export_Click", "End processing selected cloud services and virtual machines");

            Template template = new Template();
            template.resources = resources;
            template.parameters = parameters;

            // save JSON template
            string jsontext = JsonConvert.SerializeObject(template, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });
            jsontext = jsontext.Replace("schemalink", "$schema");
            writeFile("export.json", jsontext);
            writeLog("Export_Click", "Write file export.json");

            // save blob copy details file
            jsontext = JsonConvert.SerializeObject(copyblobdetails, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });
            writeFile("copyblobdetails.json", jsontext);
            writeLog("Export_Click", "Write file copyblobdetails.json");

            // post Telemetry Record to ASMtoARMToolAPI
            if (app.Default.AllowTelemetry)
            {
                postTelemetryRecord();
            }

            lblStatus.Text = "Ready";

            writeLog("Export_Click", "End");
        }

        private void BuildPublicIPAddressObject(ref NetworkInterface networkinterface)
        {
            writeLog("BuildPublicIPAddressObject", "Start");

            PublicIPAddress publicipaddress = new PublicIPAddress();
            publicipaddress.name = networkinterface.name;
            publicipaddress.location = networkinterface.location;
            publicipaddress.properties = new PublicIPAddress_Properties();

            try // it fails if this public ip address was already processed. safe to continue.
            {
                processeditems.Add("Microsoft.Network/publicIPAddresses/" + publicipaddress.name, publicipaddress.location);
                resources.Add(publicipaddress);
                writeLog("BuildPublicIPAddressObject", "Microsoft.Network/publicIPAddresses/" + publicipaddress.name);
            }
            catch { }

            NetworkInterface_Properties networkinterface_properties = (NetworkInterface_Properties)networkinterface.properties;
            networkinterface_properties.ipConfigurations[0].properties.publicIPAddress = new Reference();
            networkinterface_properties.ipConfigurations[0].properties.publicIPAddress.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/publicIPAddresses/" + publicipaddress.name + "')]";
            networkinterface.properties = networkinterface_properties;

            networkinterface.dependsOn.Add(networkinterface_properties.ipConfigurations[0].properties.publicIPAddress.id);
            writeLog("BuildPublicIPAddressObject", "End");
        }

        private void BuildPublicIPAddressObject(XmlNode resource, string loadbalancername)
        {
            writeLog("BuildPublicIPAddressObject", "Start");

            string publicipaddress_name = loadbalancername;

            Hashtable dnssettings = new Hashtable();
            dnssettings.Add("domainNameLabel", (publicipaddress_name + app.Default.UniquenessSuffix).ToLower());

            PublicIPAddress_Properties publicipaddress_properties = new PublicIPAddress_Properties();
            publicipaddress_properties.dnsSettings = dnssettings;

            PublicIPAddress publicipaddress = new PublicIPAddress();
            publicipaddress.name = publicipaddress_name + "-PIP";
            publicipaddress.location = resource.SelectSingleNode("HostedServiceProperties/Location").InnerText;
            publicipaddress.properties = publicipaddress_properties;

            try // it fails if this public ip address was already processed. safe to continue.
            {
                processeditems.Add("Microsoft.Network/publicIPAddresses/" + publicipaddress.name, publicipaddress.location);
                resources.Add(publicipaddress);
                writeLog("BuildPublicIPAddressObject", "Microsoft.Network/publicIPAddresses/" + publicipaddress.name);
            }
            catch { }

            writeLog("BuildPublicIPAddressObject", "End");
        }

        private void BuildAvailabilitySetObject(XmlNode virtualmachine, Hashtable virtualmachineinfo)
        {
            writeLog("BuildAvailabilitySetObject", "Start");

            string virtualmachinename = virtualmachineinfo["virtualmachinename"].ToString();
            string cloudservicename = virtualmachineinfo["cloudservicename"].ToString();

            AvailabilitySet availabilityset = new AvailabilitySet();

            availabilityset.name = "defaultAS";
            if (virtualmachine.SelectSingleNode("AvailabilitySetName") != null)
            {
                availabilityset.name = virtualmachine.SelectSingleNode("AvailabilitySetName").InnerText;
            }
            availabilityset.location = virtualmachineinfo["location"].ToString();
            try // it fails if this availability set was already processed. safe to continue.
            {
                processeditems.Add("Microsoft.Compute/availabilitySets/" + availabilityset.name, availabilityset.location);
                resources.Add(availabilityset);
                writeLog("BuildAvailabilitySetObject", "Microsoft.Compute/availabilitySets/" + availabilityset.name);
            }
            catch { }

            writeLog("BuildAvailabilitySetObject", "End");
        }

        private void BuildLoadBalancerObject(XmlNode cloudservice, string loadbalancername, string token)
        {
            writeLog("BuildLoadBalancerObject", "Start");

            LoadBalancer loadbalancer = new LoadBalancer();
            loadbalancer.name = loadbalancername;
            loadbalancer.location = cloudservice.SelectSingleNode("HostedServiceProperties/Location").InnerText;

            FrontendIPConfiguration_Properties frontendipconfiguration_properties = new FrontendIPConfiguration_Properties();

            // if internal load balancer
            if (cloudservice.SelectNodes("Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/Type").Count > 0)
            {
                string virtualnetworkname = cloudservice.SelectSingleNode("Deployments/Deployment/VirtualNetworkName").InnerText;
                string subnetname = cloudservice.SelectSingleNode("Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/SubnetName").InnerText.Replace(" ", "");

                frontendipconfiguration_properties.privateIPAllocationMethod = "Dynamic";
                if (cloudservice.SelectNodes("Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/StaticVirtualNetworkIPAddress").Count > 0)
                {
                    frontendipconfiguration_properties.privateIPAllocationMethod = "Static";
                    frontendipconfiguration_properties.privateIPAddress = cloudservice.SelectSingleNode("Deployments/Deployment/LoadBalancers/LoadBalancer/FrontendIpConfiguration/StaticVirtualNetworkIPAddress").InnerText;
                }

                List<string> dependson = new List<string>();
                if (GetProcessedItem("Microsoft.Network/virtualNetworks/" + virtualnetworkname))
                {
                    dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Network/virtualNetworks/" + virtualnetworkname + "')]");
                }
                loadbalancer.dependsOn = dependson;

                Reference subnet_ref = new Reference();
                subnet_ref.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/virtualNetworks/" + virtualnetworkname + "/subnets/" + subnetname + "')]";
                frontendipconfiguration_properties.subnet = subnet_ref;
            }
            // if external load balancer
            else
            {
                List<string> dependson = new List<string>();
                dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Network/publicIPAddresses/" + loadbalancer.name + "-PIP')]");
                loadbalancer.dependsOn = dependson;

                Reference publicipaddress_ref = new Reference();
                publicipaddress_ref.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/publicIPAddresses/" + loadbalancer.name + "-PIP')]";
                frontendipconfiguration_properties.publicIPAddress = publicipaddress_ref;
            }


            LoadBalancer_Properties loadbalancer_properties = new LoadBalancer_Properties();

            FrontendIPConfiguration frontendipconfiguration = new FrontendIPConfiguration();
            frontendipconfiguration.properties = frontendipconfiguration_properties;

            List<FrontendIPConfiguration> frontendipconfigurations = new List<FrontendIPConfiguration>();
            frontendipconfigurations.Add(frontendipconfiguration);
            loadbalancer_properties.frontendIPConfigurations = frontendipconfigurations;

            Hashtable backendaddresspool = new Hashtable();
            backendaddresspool.Add("name", "default");
            List<Hashtable> backendaddresspools = new List<Hashtable>();
            backendaddresspools.Add(backendaddresspool);
            loadbalancer_properties.backendAddressPools = backendaddresspools;

            List<InboundNatRule> inboundnatrules = new List<InboundNatRule>();
            List<LoadBalancingRule> loadbalancingrules = new List<LoadBalancingRule>();
            List<Probe> probes = new List<Probe>();

            foreach (var selectedrow in lvwVirtualMachines.CheckedItems)
            {
                var listViewItem = (ListViewItem)selectedrow;
                if (listViewItem.SubItems[4].Text == loadbalancer.name)
                {
                    //process VM
                    string cloudservicename = listViewItem.Text;
                    string virtualmachinename = listViewItem.SubItems[1].Text;
                    string deploymentname = listViewItem.SubItems[2].Text;
                    string virtualnetworkname = listViewItem.SubItems[3].Text.Replace(" ", "");
                    string loadba = listViewItem.SubItems[4].Text;

                    Hashtable virtualmachineinfo = new Hashtable();
                    virtualmachineinfo.Add("cloudservicename", cloudservicename);
                    virtualmachineinfo.Add("deploymentname", deploymentname);
                    virtualmachineinfo.Add("virtualmachinename", virtualmachinename);
                    XmlNode virtualmachine = GetAzureASMResources("VirtualMachine", virtualmachineinfo, token)[0];

                    BuildLoadBalancerRules(virtualmachine, loadbalancer.name, ref inboundnatrules, ref loadbalancingrules, ref probes);
                }
            }

            loadbalancer_properties.inboundNatRules = inboundnatrules;
            loadbalancer_properties.loadBalancingRules = loadbalancingrules;
            loadbalancer_properties.probes = probes;
            loadbalancer.properties = loadbalancer_properties;

            try // it fails if this load balancer was already processed. safe to continue.
            {
                processeditems.Add("Microsoft.Network/loadBalancers/" + loadbalancer.name, loadbalancer.location);
                resources.Add(loadbalancer);
                writeLog("BuildLoadBalancerObject", "Microsoft.Network/loadBalancers/" + loadbalancer.name);
            }
            catch { }

            writeLog("BuildLoadBalancerObject", "End");
        }

        private void BuildLoadBalancerRules(XmlNode resource, string loadbalancername, ref List<InboundNatRule> inboundnatrules, ref List<LoadBalancingRule> loadbalancingrules, ref List<Probe> probes)
        {
            writeLog("BuildLoadBalancerRules", "Start");

            string virtualmachinename = resource.SelectSingleNode("RoleName").InnerText;

            foreach (XmlNode inputendpoint in resource.SelectNodes("ConfigurationSets/ConfigurationSet/InputEndpoints/InputEndpoint"))
            {
                if (inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName") == null) // if it's a inbound nat rule
                {
                    InboundNatRule_Properties inboundnatrule_properties = new InboundNatRule_Properties();
                    inboundnatrule_properties.frontendPort = Int64.Parse(inputendpoint.SelectSingleNode("Port").InnerText);
                    inboundnatrule_properties.backendPort = Int64.Parse(inputendpoint.SelectSingleNode("LocalPort").InnerText);
                    inboundnatrule_properties.protocol = inputendpoint.SelectSingleNode("Protocol").InnerText;

                    Reference frontendIPConfiguration = new Reference();
                    frontendIPConfiguration.id = "[concat(resourceGroup().id,'/providers/Microsoft.Network/loadBalancers/" + loadbalancername + "/frontendIPConfigurations/default')]";
                    inboundnatrule_properties.frontendIPConfiguration = frontendIPConfiguration;

                    InboundNatRule inboundnatrule = new InboundNatRule();
                    inboundnatrule.name = virtualmachinename + "-" + inputendpoint.SelectSingleNode("Name").InnerText;
                    inboundnatrule.name = inboundnatrule.name.Replace(" ", "");
                    inboundnatrule.properties = inboundnatrule_properties;

                    inboundnatrules.Add(inboundnatrule);
                }
                else // if it's a load balancing rule
                {
                    string name = inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName").InnerText;
                    XmlNode probenode = inputendpoint.SelectSingleNode("LoadBalancerProbe");

                    // build probe
                    Probe_Properties probe_properties = new Probe_Properties();
                    probe_properties.port = Int64.Parse(probenode.SelectSingleNode("Port").InnerText);
                    probe_properties.protocol = probenode.SelectSingleNode("Protocol").InnerText;

                    Probe probe = new Probe();
                    probe.name = name;
                    probe.properties = probe_properties;

                    try // fails if this probe already exists. safe to continue
                    {
                        processeditems.Add("Microsoft.Network/loadBalancers/" + loadbalancername + "/probes/" + probe.name, "");
                        probes.Add(probe);
                    }
                    catch { }

                    // build load balancing rule
                    Reference frontendipconfiguration_ref = new Reference();
                    frontendipconfiguration_ref.id = "[concat(resourceGroup().id,'/providers/Microsoft.Network/loadBalancers/" + loadbalancername + "/frontendIPConfigurations/default')]";

                    Reference backendaddresspool_ref = new Reference();
                    backendaddresspool_ref.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/loadBalancers/" + loadbalancername + "/backendAddressPools/default')]";

                    Reference probe_ref = new Reference();
                    probe_ref.id = "[concat(resourceGroup().id,'/providers/Microsoft.Network/loadBalancers/" + loadbalancername + "/probes/" + probe.name + "')]";

                    LoadBalancingRule_Properties loadbalancingrule_properties = new LoadBalancingRule_Properties();
                    loadbalancingrule_properties.frontendIPConfiguration = frontendipconfiguration_ref;
                    loadbalancingrule_properties.backendAddressPool = backendaddresspool_ref;
                    loadbalancingrule_properties.probe = probe_ref;
                    loadbalancingrule_properties.frontendPort = Int64.Parse(inputendpoint.SelectSingleNode("Port").InnerText);
                    loadbalancingrule_properties.backendPort = Int64.Parse(inputendpoint.SelectSingleNode("LocalPort").InnerText);
                    loadbalancingrule_properties.protocol = inputendpoint.SelectSingleNode("Protocol").InnerText;

                    LoadBalancingRule loadbalancingrule = new LoadBalancingRule();
                    loadbalancingrule.name = name;
                    loadbalancingrule.properties = loadbalancingrule_properties;

                    try // fails if this load balancing rule already exists. safe to continue
                    {
                        processeditems.Add("Microsoft.Network/loadBalancers/" + loadbalancername + "/loadBalancingRules/" + loadbalancingrule.name, "");
                        loadbalancingrules.Add(loadbalancingrule);
                        writeLog("BuildLoadBalancerRules", "Microsoft.Network/loadBalancers/" + loadbalancername + "/loadBalancingRules/" + loadbalancingrule.name);
                    }
                    catch { continue; }
                }
            }

            writeLog("BuildLoadBalancerRules", "End");
        }

        private void BuildVirtualNetworkObject(XmlNode resource, string token)
        {
            writeLog("BuildVirtualNetworkObject", "Start");

            List<string> dependson = new List<string>();

            List<string> addressprefixes = new List<string>();
            foreach (XmlNode addressprefix in resource.SelectNodes("AddressSpace/AddressPrefixes"))
            {
                addressprefixes.Add(addressprefix.SelectSingleNode("AddressPrefix").InnerText);
            }

            AddressSpace addressspace = new AddressSpace();
            addressspace.addressPrefixes = addressprefixes;

            List<string> dnsservers = new List<string>();
            foreach (XmlNode dnsserver in resource.SelectNodes("Dns/DnsServers/DnsServer"))
            {
                dnsservers.Add(dnsserver.SelectSingleNode("Address").InnerText);
            }

            VirtualNetwork_dhcpOptions dhcpoptions = new VirtualNetwork_dhcpOptions();
            dhcpoptions.dnsServers = dnsservers;

            VirtualNetwork virtualnetwork = new VirtualNetwork();
            virtualnetwork.name = resource.SelectSingleNode("Name").InnerText.Replace(" ", "");
            virtualnetwork.location = resource.SelectSingleNode("Location").InnerText;
            virtualnetwork.dependsOn = dependson;

            List<Subnet> subnets = new List<Subnet>();
            foreach (XmlNode subnetnode in resource.SelectNodes("Subnets/Subnet"))
            {
                Subnet_Properties properties = new Subnet_Properties();
                properties.addressPrefix = subnetnode.SelectSingleNode("AddressPrefix").InnerText;

                Subnet subnet = new Subnet();
                subnet.name = subnetnode.SelectSingleNode("Name").InnerText.Replace(" ", "");
                subnet.properties = properties;

                subnets.Add(subnet);

                // add Network Security Group if exists
                if (subnetnode.SelectNodes("NetworkSecurityGroup").Count > 0)
                {
                    NetworkSecurityGroup networksecuritygroup = BuildNetworkSecurityGroup(subnetnode.SelectSingleNode("NetworkSecurityGroup").InnerText, token);

                    // Add NSG reference to the subnet
                    Reference networksecuritygroup_ref = new Reference();
                    networksecuritygroup_ref.id = "[concat(resourceGroup().id,'/providers/Microsoft.Network/networkSecurityGroups/" + networksecuritygroup.name + "')]";

                    properties.networkSecurityGroup = networksecuritygroup_ref;

                    // Add NSG dependsOn to the Virtual Network object
                    if (!virtualnetwork.dependsOn.Contains(networksecuritygroup_ref.id))
                    {
                        virtualnetwork.dependsOn.Add(networksecuritygroup_ref.id);
                    }
                }

                // add Route Table if exists
                if (subnetnode.SelectNodes("RouteTableName").Count > 0)
                {
                    RouteTable routetable = BuildRouteTable(subnetnode.SelectSingleNode("RouteTableName").InnerText, token);

                    // Add Route Table reference to the subnet
                    Reference routetable_ref = new Reference();
                    routetable_ref.id = "[concat(resourceGroup().id,'/providers/Microsoft.Network/routeTables/" + routetable.name + "')]";

                    properties.routeTable = routetable_ref;

                    // Add Route Table dependsOn to the Virtual Network object
                    if (!virtualnetwork.dependsOn.Contains(routetable_ref.id))
                    {
                        virtualnetwork.dependsOn.Add(routetable_ref.id);
                    }
                }
            }

            VirtualNetwork_Properties virtualnetwork_properties = new VirtualNetwork_Properties();
            virtualnetwork_properties.addressSpace = addressspace;
            virtualnetwork_properties.subnets = subnets;
            virtualnetwork_properties.dhcpOptions= dhcpoptions;

            virtualnetwork.properties = virtualnetwork_properties;

            processeditems.Add("Microsoft.Network/virtualNetworks/" + virtualnetwork.name, virtualnetwork.location);
            resources.Add(virtualnetwork);
            writeLog("BuildVirtualNetworkObject", "Microsoft.Network/virtualNetworks/" + virtualnetwork.name);


            // Process Virtual Network Gateway if one exists
            if (resource.SelectNodes("Gateway").Count > 0)
            {
                // Gateway Public IP Address
                PublicIPAddress_Properties publicipaddress_properties = new PublicIPAddress_Properties();
                publicipaddress_properties.publicIPAllocationMethod = "Dynamic";

                PublicIPAddress publicipaddress = new PublicIPAddress();
                publicipaddress.name = virtualnetwork.name + "-VPNGateway-PIP";
                publicipaddress.location = virtualnetwork.location;
                publicipaddress.properties = publicipaddress_properties;

                processeditems.Add("Microsoft.Network/publicIPAddresses/" + publicipaddress.name, publicipaddress.location);
                resources.Add(publicipaddress);

                // Virtual Network Gateway
                Reference subnet_ref = new Reference();
                subnet_ref.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/virtualNetworks/" + virtualnetwork.name + "/subnets/GatewaySubnet')]";

                Reference publicipaddress_ref = new Reference();
                publicipaddress_ref.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/publicIPAddresses/" + publicipaddress.name + "')]";

                dependson = new List<string>();
                dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Network/virtualNetworks/" + virtualnetwork.name + "')]");
                dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Network/publicIPAddresses/" + publicipaddress.name + "')]");

                IpConfiguration_Properties ipconfiguration_properties = new IpConfiguration_Properties();
                ipconfiguration_properties.privateIPAllocationMethod = "Dynamic";
                ipconfiguration_properties.subnet = subnet_ref;
                ipconfiguration_properties.publicIPAddress = publicipaddress_ref;

                IpConfiguration virtualnetworkgateway_ipconfiguration = new IpConfiguration();
                virtualnetworkgateway_ipconfiguration.name = "VPNGatewayIPConfig";
                virtualnetworkgateway_ipconfiguration.properties = ipconfiguration_properties;

                VirtualNetworkGateway_Sku virtualnetworkgateway_sku = new VirtualNetworkGateway_Sku();
                virtualnetworkgateway_sku.name = "Basic";
                virtualnetworkgateway_sku.tier = "Basic";

                List<IpConfiguration> virtualnetworkgateway_ipconfigurations = new List<IpConfiguration>();
                virtualnetworkgateway_ipconfigurations.Add(virtualnetworkgateway_ipconfiguration);

                VirtualNetworkGateway_Properties virtualnetworkgateway_properties = new VirtualNetworkGateway_Properties();
                virtualnetworkgateway_properties.ipConfigurations = virtualnetworkgateway_ipconfigurations;
                virtualnetworkgateway_properties.sku = virtualnetworkgateway_sku;
                virtualnetworkgateway_properties.gatewayType = "Vpn";

                // If there is VPN Client configuration
                if (resource.SelectNodes("Gateway/VPNClientAddressPool/AddressPrefixes/AddressPrefix").Count > 0)
                {
                    addressprefixes = new List<string>();
                    addressprefixes.Add(resource.SelectNodes("Gateway/VPNClientAddressPool/AddressPrefixes/AddressPrefix")[0].InnerText);

                    AddressSpace vpnclientaddresspool = new AddressSpace();
                    vpnclientaddresspool.addressPrefixes = addressprefixes;

                    VPNClientConfiguration vpnclientconfiguration = new VPNClientConfiguration();
                    vpnclientconfiguration.vpnClientAddressPool = vpnclientaddresspool;

                    //Process vpnClientRootCertificates
                    Hashtable infocrc = new Hashtable();
                    infocrc.Add("virtualnetworkname", resource.SelectSingleNode("Name").InnerText);
                    XmlNode clientrootcertificates = GetAzureASMResources("ClientRootCertificates", infocrc, token)[0];

                    List<VPNClientCertificate> vpnclientrootcertificates = new List<VPNClientCertificate>();
                    foreach (XmlNode certificate in clientrootcertificates.SelectNodes("ClientRootCertificate"))
                    {
                        Hashtable infocert = new Hashtable();
                        infocert.Add("virtualnetworkname", resource.SelectSingleNode("Name").InnerText);
                        infocert.Add("thumbprint", certificate.SelectSingleNode("Thumbprint").InnerText);
                        XmlNode clientrootcertificate = GetAzureASMResources("ClientRootCertificate", infocert, token)[0];

                        VPNClientCertificate_Properties vpnclientcertificate_properties = new VPNClientCertificate_Properties();
                        vpnclientcertificate_properties.PublicCertData = Convert.ToBase64String(StrToByteArray(clientrootcertificate.InnerText));

                        VPNClientCertificate vpnclientcertificate = new VPNClientCertificate();
                        vpnclientcertificate.name = certificate.SelectSingleNode("Subject").InnerText.Replace("CN=", "");
                        vpnclientcertificate.properties = vpnclientcertificate_properties;

                        vpnclientrootcertificates.Add(vpnclientcertificate);
                    }

                    vpnclientconfiguration.vpnClientRootCertificates = vpnclientrootcertificates;

                    virtualnetworkgateway_properties.vpnClientConfiguration = vpnclientconfiguration;
                }

                Hashtable virtualnetworkgatewayinfo = new Hashtable();
                virtualnetworkgatewayinfo.Add("virtualnetworkname", resource.SelectSingleNode("Name").InnerText);
                virtualnetworkgatewayinfo.Add("localnetworksitename", "");
                XmlNode gateway = GetAzureASMResources("VirtualNetworkGateway", virtualnetworkgatewayinfo, token)[0];

                string vpnType = gateway.SelectSingleNode("GatewayType").InnerText;
                if (vpnType == "StaticRouting")
                {
                    vpnType = "PolicyBased";
                }
                else if (vpnType == "DynamicRouting")
                {
                    vpnType = "RouteBased";
                }
                virtualnetworkgateway_properties.vpnType = vpnType;

                VirtualNetworkGateway virtualnetworkgateway = new VirtualNetworkGateway();
                virtualnetworkgateway.location = virtualnetwork.location;
                virtualnetworkgateway.name = virtualnetwork.name + "-VPNGateway";
                virtualnetworkgateway.properties = virtualnetworkgateway_properties;
                virtualnetworkgateway.dependsOn = dependson;

                processeditems.Add("Microsoft.Network/virtualNetworkGateways/" + virtualnetworkgateway.name, virtualnetworkgateway.location);
                resources.Add(virtualnetworkgateway);
                writeLog("BuildVirtualNetworkObject", "Microsoft.Network/virtualNetworkGateways/" + virtualnetworkgateway.name);

                // Local Network Gateways & Connections
                foreach (XmlNode LocalNetworkSite in resource.SelectNodes("Gateway/Sites/LocalNetworkSite"))
                {
                    // Local Network Gateway
                    addressprefixes = new List<string>();
                    foreach (XmlNode addressprefix in LocalNetworkSite.SelectNodes("AddressSpace/AddressPrefixes"))
                    {
                        addressprefixes.Add(addressprefix.SelectSingleNode("AddressPrefix").InnerText);
                    }

                    AddressSpace localnetworkaddressspace = new AddressSpace();
                    localnetworkaddressspace.addressPrefixes = addressprefixes;

                    LocalNetworkGateway_Properties localnetworkgateway_properties = new LocalNetworkGateway_Properties();
                    localnetworkgateway_properties.localNetworkAddressSpace = localnetworkaddressspace;
                    localnetworkgateway_properties.gatewayIpAddress = LocalNetworkSite.SelectSingleNode("VpnGatewayAddress").InnerText;

                    LocalNetworkGateway localnetworkgateway = new LocalNetworkGateway();
                    localnetworkgateway.name = LocalNetworkSite.SelectSingleNode("Name").InnerText + "-LocalGateway";
                    localnetworkgateway.name = localnetworkgateway.name.Replace(" ", "");

                    localnetworkgateway.location = virtualnetwork.location;
                    localnetworkgateway.properties = localnetworkgateway_properties;

                    processeditems.Add("Microsoft.Network/localNetworkGateways/" + localnetworkgateway.name, localnetworkgateway.location);
                    resources.Add(localnetworkgateway);
                    writeLog("BuildVirtualNetworkObject", "Microsoft.Network/localNetworkGateways/" + localnetworkgateway.name);

                    // Connections
                    Reference virtualnetworkgateway_ref = new Reference();
                    virtualnetworkgateway_ref.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/virtualNetworkGateways/" + virtualnetworkgateway.name + "')]";

                    Reference localnetworkgateway_ref = new Reference();
                    localnetworkgateway_ref.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/localNetworkGateways/" + localnetworkgateway.name + "')]";

                    dependson = new List<string>();
                    dependson.Add(virtualnetworkgateway_ref.id);
                    dependson.Add(localnetworkgateway_ref.id);

                    GatewayConnection_Properties gatewayconnection_properties = new GatewayConnection_Properties();
                    gatewayconnection_properties.connectionType = LocalNetworkSite.SelectSingleNode("Connections/Connection/Type").InnerText;
                    gatewayconnection_properties.virtualNetworkGateway1 = virtualnetworkgateway_ref;
                    gatewayconnection_properties.localNetworkGateway2 = localnetworkgateway_ref;

                    virtualnetworkgatewayinfo["localnetworksitename"] = LocalNetworkSite.SelectSingleNode("Name").InnerText;
                    XmlNode connectionsharekey = GetAzureASMResources("VirtualNetworkGatewaySharedKey", virtualnetworkgatewayinfo, token)[0];
                    gatewayconnection_properties.sharedKey = connectionsharekey.SelectSingleNode("Value").InnerText;

                    GatewayConnection gatewayconnection = new GatewayConnection();
                    gatewayconnection.name = virtualnetworkgateway.name + "-" + localnetworkgateway.name + "-connection";
                    gatewayconnection.location = virtualnetwork.location;
                    gatewayconnection.properties = gatewayconnection_properties;
                    gatewayconnection.dependsOn = dependson;

                    processeditems.Add("Microsoft.Network/connections/" + gatewayconnection.name, gatewayconnection.location);
                    resources.Add(gatewayconnection);
                    writeLog("BuildVirtualNetworkObject", "Microsoft.Network/connections/" + gatewayconnection.name);
                }
            }

            writeLog("BuildVirtualNetworkObject", "End");
        }

        private void BuildNewVirtualNetworkObject(XmlNode resource, Hashtable info)
        {
            //string ipaddress = resource.SelectSingleNode("Deployments/Deployment/RoleInstanceList/RoleInstance/IpAddress").InnerText;
            //IPAddress ipaddressIP = IPAddress.Parse(ipaddress);
            //byte[] ipaddressBYTE = ipaddressIP.GetAddressBytes();

            //IPAddress subnetmaskIP = IPAddress.Parse("255.255.254.0");
            //byte[] subnetmaskBYTE = subnetmaskIP.GetAddressBytes();

            //byte[] addressprefixBYTE = new byte[4];
            //addressprefixBYTE[0] = (byte)((byte)ipaddressBYTE[0] & (byte)subnetmaskBYTE[0]);
            //addressprefixBYTE[1] = (byte)((byte)ipaddressBYTE[1] & (byte)subnetmaskBYTE[1]);
            //addressprefixBYTE[2] = (byte)((byte)ipaddressBYTE[2] & (byte)subnetmaskBYTE[2]);
            //addressprefixBYTE[3] = (byte)((byte)ipaddressBYTE[3] & (byte)subnetmaskBYTE[3]);

            //string addressprefix = "";
            //addressprefix += addressprefixBYTE[0].ToString() + ".";
            //addressprefix += addressprefixBYTE[1].ToString() + ".";
            //addressprefix += addressprefixBYTE[2].ToString() + ".";
            //addressprefix += addressprefixBYTE[3].ToString();

            writeLog("BuildNewVirtualNetworkObject", "Start");

            List<string> addressprefixes = new List<string>();
            addressprefixes.Add("192.168.0.0/23");

            AddressSpace addressspace = new AddressSpace();
            addressspace.addressPrefixes = addressprefixes;

            VirtualNetwork virtualnetwork = new VirtualNetwork();
            virtualnetwork.name = info["cloudservicename"].ToString() + "-VNET";
            virtualnetwork.location = resource.SelectSingleNode("HostedServiceProperties/Location").InnerText;

            List<Subnet> subnets = new List<Subnet>();
            Subnet_Properties properties = new Subnet_Properties();
            properties.addressPrefix = "192.168.0.0/23";

            Subnet subnet = new Subnet();
            subnet.name = "Subnet1";
            subnet.properties = properties;

            subnets.Add(subnet);

            VirtualNetwork_Properties virtualnetwork_properties = new VirtualNetwork_Properties();
            virtualnetwork_properties.addressSpace = addressspace;
            virtualnetwork_properties.subnets = subnets;

            virtualnetwork.properties = virtualnetwork_properties;

            try
            {
                processeditems.Add("Microsoft.Network/virtualNetworks/" + virtualnetwork.name, virtualnetwork.location);
                resources.Add(virtualnetwork);
                writeLog("BuildNewVirtualNetworkObject", "Microsoft.Network/virtualNetworks/" + virtualnetwork.name);
            }
            catch { }

            writeLog("BuildNewVirtualNetworkObject", "End");
        }

        private NetworkSecurityGroup BuildNetworkSecurityGroup(string networksecuritygroupname, string token)
        {
            writeLog("BuildNetworkSecurityGroup", "Start");

            Hashtable nsginfo = new Hashtable();
            nsginfo.Add("name", networksecuritygroupname);
            XmlNode resource = GetAzureASMResources("NetworkSecurityGroup", nsginfo, token)[0];

            NetworkSecurityGroup networksecuritygroup = new NetworkSecurityGroup();
            networksecuritygroup.name = resource.SelectSingleNode("Name").InnerText;
            networksecuritygroup.location = resource.SelectSingleNode("Location").InnerText;

            NetworkSecurityGroup_Properties networksecuritygroup_properties = new NetworkSecurityGroup_Properties();
            networksecuritygroup_properties.securityRules = new List<SecurityRule>();

            // for each rule
            foreach (XmlNode rule in resource.SelectNodes("Rules/Rule"))
            {
                // if not system rule
                if (rule.SelectNodes("IsDefault").Count == 0)
                {
                    SecurityRule_Properties securityrule_properties = new SecurityRule_Properties();
                    securityrule_properties.description = rule.SelectSingleNode("Name").InnerText;
                    securityrule_properties.direction = rule.SelectSingleNode("Type").InnerText;
                    securityrule_properties.priority = long.Parse(rule.SelectSingleNode("Priority").InnerText);
                    securityrule_properties.access = rule.SelectSingleNode("Action").InnerText;
                    securityrule_properties.sourceAddressPrefix = rule.SelectSingleNode("SourceAddressPrefix").InnerText;
                    securityrule_properties.sourceAddressPrefix.Replace("_", "");
                    securityrule_properties.destinationAddressPrefix = rule.SelectSingleNode("DestinationAddressPrefix").InnerText;
                    securityrule_properties.destinationAddressPrefix.Replace("_", "");
                    securityrule_properties.sourcePortRange = rule.SelectSingleNode("SourcePortRange").InnerText;
                    securityrule_properties.destinationPortRange = rule.SelectSingleNode("DestinationPortRange").InnerText;
                    securityrule_properties.protocol = rule.SelectSingleNode("Protocol").InnerText;

                    SecurityRule securityrule = new SecurityRule();
                    securityrule.name = rule.SelectSingleNode("Name").InnerText;
                    securityrule.properties = securityrule_properties;

                    networksecuritygroup_properties.securityRules.Add(securityrule);
                }
            }

            networksecuritygroup.properties = networksecuritygroup_properties;

            try // it fails if this network security group was already processed. safe to continue.
            {
                processeditems.Add("Microsoft.Network/networkSecurityGroups/" + networksecuritygroup.name, networksecuritygroup.location);
                resources.Add(networksecuritygroup);
                writeLog("BuildNetworkSecurityGroup", "Microsoft.Network/networkSecurityGroups/" + networksecuritygroup.name);
            }
            catch { }

            writeLog("BuildNetworkSecurityGroup", "End");

            return networksecuritygroup;
        }

        private RouteTable BuildRouteTable(string routetablename, string token)
        {
            writeLog("BuildRouteTable", "Start");

            Hashtable info = new Hashtable();
            info.Add("name", routetablename);
            XmlNode resource = GetAzureASMResources("RouteTable", info, token)[0];

            RouteTable routetable = new RouteTable();
            routetable.name = resource.SelectSingleNode("Name").InnerText;
            routetable.location = resource.SelectSingleNode("Location").InnerText;

            RouteTable_Properties routetable_properties = new RouteTable_Properties();
            routetable_properties.routes = new List<Route>();

            // for each route
            foreach (XmlNode routenode in resource.SelectNodes("RouteList/Route"))
            {
                //securityrule_properties.protocol = rule.SelectSingleNode("Protocol").InnerText;
                Route_Properties route_properties = new Route_Properties();
                route_properties.addressPrefix = routenode.SelectSingleNode("AddressPrefix").InnerText;

                // convert next hop type string
                switch (routenode.SelectSingleNode("NextHopType/Type").InnerText)
                {
                    case "VirtualAppliance":
                        route_properties.nextHopType = "VirtualAppliance";
                        break;
                    case "VPNGateway":
                        route_properties.nextHopType = "VirtualNetworkGateway";
                        break;
                    case "Internet":
                        route_properties.nextHopType = "Internet";
                        break;
                    case "VNETLocal":
                        route_properties.nextHopType = "VnetLocal";
                        break;
                    case "Null":
                        route_properties.nextHopType = "None";
                        break;
                }
                if (route_properties.nextHopType == "VirtualAppliance")
                    route_properties.nextHopIpAddress = routenode.SelectSingleNode("NextHopType/IpAddress").InnerText;

                Route route = new Route();
                route.name = routenode.SelectSingleNode("Name").InnerText;
                route.properties = route_properties;

                routetable_properties.routes.Add(route);
            }

            routetable.properties = routetable_properties;

            if(!resources.Contains(routetable))
            {
                processeditems.Add("Microsoft.Network/routeTables/" + routetable.name, routetable.location);
                resources.Add(routetable);
                writeLog("BuildRouteTable", "Microsoft.Network/routeTables/" + routetable.name);
            }

            writeLog("BuildRouteTable", "End");

            return routetable;
        }

        private void BuildNetworkInterfaceObject(XmlNode resource, Hashtable virtualmachineinfo, ref List<NetworkProfile_NetworkInterface> networkinterfaces, string token)
        {
            writeLog("BuildNetworkInterfaceObject", "Start");

            string virtualmachinename = virtualmachineinfo["virtualmachinename"].ToString();
            string cloudservicename = virtualmachineinfo["cloudservicename"].ToString();
            string deploymentname = virtualmachineinfo["deploymentname"].ToString();
            string virtualnetworkname = virtualmachineinfo["virtualnetworkname"].ToString();
            string loadbalancername = virtualmachineinfo["loadbalancername"].ToString();
            string subnet_name = "";

            if (virtualnetworkname != "empty")
            {
                virtualnetworkname = virtualmachineinfo["virtualnetworkname"].ToString();
                subnet_name = resource.SelectNodes("ConfigurationSets/ConfigurationSet/SubnetNames")[0].SelectSingleNode("SubnetName").InnerText.Replace(" ", "");
            }
            else
            {
                virtualnetworkname = cloudservicename + "-VNET";
                subnet_name = "Subnet1";
            }

            Reference subnet_ref = new Reference();
            subnet_ref.id = "[concat(resourceGroup().id,'/providers/Microsoft.Network/virtualNetworks/" + virtualnetworkname + "/subnets/" + subnet_name + "')]";

            string privateIPAllocationMethod = "Dynamic";
            string privateIPAddress = null;
            if (resource.SelectNodes("ConfigurationSets/ConfigurationSet")[0].SelectSingleNode("StaticVirtualNetworkIPAddress") != null)
            {
                privateIPAllocationMethod = "Static";
                privateIPAddress = resource.SelectNodes("ConfigurationSets/ConfigurationSet")[0].SelectSingleNode("StaticVirtualNetworkIPAddress").InnerText;
            }
            // if its a VM not connected to a virtual network
            //else if (virtualmachineinfo["virtualnetworkname"].ToString() == "empty")
            //{
            //    privateIPAllocationMethod = "Static";
            //    privateIPAddress = virtualmachineinfo["ipaddress"].ToString();
            //}

            // Get the list of endpoints
            XmlNodeList inputendpoints = resource.SelectNodes("ConfigurationSets/ConfigurationSet/InputEndpoints/InputEndpoint");

            // If there is at least one endpoint add the reference to the LB backend pool
            List<Reference> loadBalancerBackendAddressPools = new List<Reference>();
            if (inputendpoints.Count > 0)
            {
                Reference loadBalancerBackendAddressPool = new Reference();
                loadBalancerBackendAddressPool.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/loadBalancers/" + loadbalancername + "/backendAddressPools/default')]";

                loadBalancerBackendAddressPools.Add(loadBalancerBackendAddressPool);
            }

            // Adds the references to the inboud nat rules
            List<Reference> loadBalancerInboundNatRules = new List<Reference>();
            foreach (XmlNode inputendpoint in inputendpoints)
            {
                if (inputendpoint.SelectSingleNode("LoadBalancedEndpointSetName") == null) // don't want to add a load balance endpoint as an inbound nat rule
                {
                    string inboundnatrulename = virtualmachinename + "-" + inputendpoint.SelectSingleNode("Name").InnerText;
                    inboundnatrulename = inboundnatrulename.Replace(" ", "");

                    Reference loadBalancerInboundNatRule = new Reference();
                    loadBalancerInboundNatRule.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/loadBalancers/" + loadbalancername + "/inboundNatRules/" + inboundnatrulename + "')]";

                    loadBalancerInboundNatRules.Add(loadBalancerInboundNatRule);
                }
            }

            IpConfiguration_Properties ipconfiguration_properties = new IpConfiguration_Properties();
            ipconfiguration_properties.privateIPAllocationMethod = privateIPAllocationMethod;
            ipconfiguration_properties.privateIPAddress = privateIPAddress;
            ipconfiguration_properties.subnet = subnet_ref;
            ipconfiguration_properties.loadBalancerInboundNatRules = loadBalancerInboundNatRules;

            // basic size VMs cannot have load balancer rules
            if (!resource.SelectSingleNode("RoleSize").InnerText.Contains("Basic"))
            {
                ipconfiguration_properties.loadBalancerBackendAddressPools = loadBalancerBackendAddressPools;
            }

            string ipconfiguration_name = "ipconfig1";
            IpConfiguration ipconfiguration = new IpConfiguration();
            ipconfiguration.name = ipconfiguration_name;
            ipconfiguration.properties = ipconfiguration_properties;

            List<IpConfiguration> ipConfigurations = new List<IpConfiguration>();
            ipConfigurations.Add(ipconfiguration);

            NetworkInterface_Properties networkinterface_properties = new NetworkInterface_Properties();
            networkinterface_properties.ipConfigurations = ipConfigurations;
            if (resource.SelectNodes("ConfigurationSets/ConfigurationSet/IPForwarding").Count > 0)
            {
                networkinterface_properties.enableIPForwarding = true;
            }

            List<string> dependson = new List<string>();
            if (GetProcessedItem("Microsoft.Network/virtualNetworks/" + virtualnetworkname))
            {
                dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Network/virtualNetworks/" + virtualnetworkname + "')]");
            }
            dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Network/loadBalancers/" + loadbalancername + "')]");

            string networkinterface_name = virtualmachinename;
            NetworkInterface networkinterface = new NetworkInterface();
            networkinterface.name = networkinterface_name;
            networkinterface.location = virtualmachineinfo["location"].ToString();
            networkinterface.properties = networkinterface_properties;
            networkinterface.dependsOn = dependson;

            NetworkProfile_NetworkInterface_Properties networkinterface_ref_properties = new NetworkProfile_NetworkInterface_Properties();
            networkinterface_ref_properties.primary = true;

            NetworkProfile_NetworkInterface networkinterface_ref = new NetworkProfile_NetworkInterface();
            networkinterface_ref.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/networkInterfaces/" + networkinterface.name + "')]";
            networkinterface_ref.properties = networkinterface_ref_properties;

            if (resource.SelectNodes("ConfigurationSets/ConfigurationSet/NetworkSecurityGroup").Count > 0)
            {
                NetworkSecurityGroup networksecuritygroup = BuildNetworkSecurityGroup(resource.SelectSingleNode("ConfigurationSets/ConfigurationSet/NetworkSecurityGroup").InnerText, token);

                // Add NSG reference to the network interface
                Reference networksecuritygroup_ref = new Reference();
                networksecuritygroup_ref.id = "[concat(resourceGroup().id,'/providers/Microsoft.Network/networkSecurityGroups/" + networksecuritygroup.name + "')]";

                networkinterface_properties.NetworkSecurityGroup = networksecuritygroup_ref;
                networkinterface.properties = networkinterface_properties;

                // Add NSG dependsOn to the Network Interface object
                if (!networkinterface.dependsOn.Contains(networksecuritygroup_ref.id))
                {
                    networkinterface.dependsOn.Add(networksecuritygroup_ref.id);
                }

            }

            if (resource.SelectNodes("ConfigurationSets/ConfigurationSet/PublicIPs").Count > 0)
            {
                BuildPublicIPAddressObject(ref networkinterface);
            }

            networkinterfaces.Add(networkinterface_ref);
            processeditems.Add("Microsoft.Network/networkInterfaces/" + networkinterface.name, networkinterface.location);
            resources.Add(networkinterface);
            writeLog("BuildNetworkInterfaceObject", "Microsoft.Network/networkInterfaces/" + networkinterface.name);

            foreach (XmlNode additionalnetworkinterface in resource.SelectNodes("ConfigurationSets/ConfigurationSet/NetworkInterfaces/NetworkInterface"))
            {
                subnet_name = additionalnetworkinterface.SelectSingleNode("IPConfigurations/IPConfiguration/SubnetName").InnerText.Replace(" ", "");
                subnet_ref = new Reference();
                subnet_ref.id = "[concat(resourceGroup().id,'/providers/Microsoft.Network/virtualNetworks/" + virtualnetworkname + "/subnets/" + subnet_name + "')]";

                privateIPAllocationMethod = "Dynamic";
                privateIPAddress = null;
                if (additionalnetworkinterface.SelectSingleNode("IPConfigurations/IPConfiguration/StaticVirtualNetworkIPAddress") != null)
                {
                    privateIPAllocationMethod = "Static";
                    privateIPAddress = additionalnetworkinterface.SelectSingleNode("IPConfigurations/IPConfiguration/StaticVirtualNetworkIPAddress").InnerText;
                }

                ipconfiguration_properties = new IpConfiguration_Properties();
                ipconfiguration_properties.privateIPAllocationMethod = privateIPAllocationMethod;
                ipconfiguration_properties.privateIPAddress = privateIPAddress;
                ipconfiguration_properties.subnet = subnet_ref;

                ipconfiguration_name = "ipconfig1";
                ipconfiguration = new IpConfiguration();
                ipconfiguration.name = ipconfiguration_name;
                ipconfiguration.properties = ipconfiguration_properties;

                ipConfigurations = new List<IpConfiguration>();
                ipConfigurations.Add(ipconfiguration);

                networkinterface_properties = new NetworkInterface_Properties();
                networkinterface_properties.ipConfigurations = ipConfigurations;
                if (additionalnetworkinterface.SelectNodes("IPForwarding").Count > 0)
                {
                    networkinterface_properties.enableIPForwarding = true;
                }

                dependson = new List<string>();
                if (GetProcessedItem("Microsoft.Network/virtualNetworks/" + virtualnetworkname))
                {
                    dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Network/virtualNetworks/" + virtualnetworkname + "')]");
                }

                networkinterface_name = virtualmachinename + "-" + additionalnetworkinterface.SelectSingleNode("Name").InnerText;
                networkinterface = new NetworkInterface();
                networkinterface.name = networkinterface_name;
                networkinterface.location = virtualmachineinfo["location"].ToString();
                networkinterface.properties = networkinterface_properties;
                networkinterface.dependsOn = dependson;

                networkinterface_ref_properties = new NetworkProfile_NetworkInterface_Properties();
                networkinterface_ref_properties.primary = false;

                networkinterface_ref = new NetworkProfile_NetworkInterface();
                networkinterface_ref.id = "[concat(resourceGroup().id, '/providers/Microsoft.Network/networkInterfaces/" + networkinterface.name + "')]";
                networkinterface_ref.properties = networkinterface_ref_properties;

                networkinterfaces.Add(networkinterface_ref);
                processeditems.Add("Microsoft.Network/networkInterfaces/" + networkinterface.name, networkinterface.location);
                resources.Add(networkinterface);
                writeLog("BuildNetworkInterfaceObject", "Microsoft.Network/networkInterfaces/" + networkinterface.name);
            }

            writeLog("BuildNetworkInterfaceObject", "End");
        }

        private void BuildVirtualMachineObject(XmlNode resource, Hashtable virtualmachineinfo, List<NetworkProfile_NetworkInterface> networkinterfaces, string token)
        {
            writeLog("BuildVirtualMachineObject", "Start");

            string virtualmachinename = virtualmachineinfo["virtualmachinename"].ToString();
            string networkinterfacename = virtualmachinename;
            string ostype = resource.SelectSingleNode("OSVirtualHardDisk/OS").InnerText;

            XmlNode osvirtualharddisk = resource.SelectSingleNode("OSVirtualHardDisk");
            string olddiskurl = osvirtualharddisk.SelectSingleNode("MediaLink").InnerText;
            string[] splitarray = olddiskurl.Split(new char[] { '/', '.' });
            string oldstorageaccountname = splitarray[2];
            string newstorageaccountname = oldstorageaccountname + app.Default.UniquenessSuffix;
            string newdiskurl = olddiskurl.Replace(oldstorageaccountname, newstorageaccountname);

            Hashtable storageaccountdependencies = new Hashtable();
            storageaccountdependencies.Add(newstorageaccountname, "");

            HardwareProfile hardwareprofile = new HardwareProfile();
            hardwareprofile.vmSize = GetVMSize(resource.SelectSingleNode("RoleSize").InnerText);

            NetworkProfile networkprofile = new NetworkProfile();
            networkprofile.networkInterfaces = networkinterfaces;

            Vhd vhd = new Vhd();
            vhd.uri = newdiskurl;

            OsDisk osdisk = new OsDisk();
            osdisk.name = resource.SelectSingleNode("OSVirtualHardDisk/DiskName").InnerText;
            osdisk.vhd = vhd;
            osdisk.caching = resource.SelectSingleNode("OSVirtualHardDisk/HostCaching").InnerText;

            ImageReference imagereference = new ImageReference();
            OsProfile osprofile = new OsProfile();

            // if the tool is configured to create new VMs with empty data disks
            if (app.Default.BuildEmpty)
            {
                osdisk.createOption = "FromImage";

                osprofile.computerName = virtualmachinename;
                osprofile.adminUsername = "[parameters('adminUsername')]";
                osprofile.adminPassword = "[parameters('adminPassword')]";

                if (!parameters.ContainsKey("adminUsername"))
                {
                    Parameter parameter = new Parameter();
                    parameter.type = "string";
                    parameters.Add("adminUsername", parameter);
                }

                if (!parameters.ContainsKey("adminPassword"))
                {
                    Parameter parameter = new Parameter();
                    parameter.type = "securestring";
                    parameters.Add("adminPassword", parameter);
                }

                if (ostype == "Windows")
                {
                    imagereference.publisher = "MicrosoftWindowsServer";
                    imagereference.offer = "WindowsServer";
                    imagereference.sku = "2012-R2-Datacenter";
                    imagereference.version = "latest";
                }
                else if (ostype == "Linux")
                {
                    imagereference.publisher = "Canonical";
                    imagereference.offer = "UbuntuServer";
                    imagereference.sku = "16.04.0-LTS";
                    imagereference.version = "latest";
                }
                else
                {
                    imagereference.publisher = "<publisher>";
                    imagereference.offer = "<offer>";
                    imagereference.sku = "<sku>";
                    imagereference.version = "<version>";
                }
            }
            // if the tool is configured to attach copied disks
            else
            {
                osdisk.createOption = "Attach";
                osdisk.osType = ostype;

                // Block of code to help copying the blobs to the new storage accounts
                Hashtable storageaccountinfo = new Hashtable();
                storageaccountinfo.Add("name", oldstorageaccountname);

                XmlNode storageaccountkeys = GetAzureASMResources("StorageAccountKeys", storageaccountinfo, token)[0];
                string key = storageaccountkeys.SelectSingleNode("StorageServiceKeys/Primary").InnerText;

                CopyBlobDetail copyblobdetail = new CopyBlobDetail();
                copyblobdetail.SourceSA = oldstorageaccountname;
                copyblobdetail.SourceContainer = splitarray[7];
                copyblobdetail.SourceBlob = splitarray[8] + "." + splitarray[9];
                copyblobdetail.SourceKey = key;
                copyblobdetail.DestinationSA = newstorageaccountname;
                copyblobdetail.DestinationContainer = splitarray[7];
                copyblobdetail.DestinationBlob = splitarray[8] + "." + splitarray[9];
                copyblobdetails.Add(copyblobdetail);
                // end of block of code to help copying the blobs to the new storage accounts
            }

            // process data disks
            List<DataDisk> datadisks = new List<DataDisk>();
            XmlNodeList datadisknodes = resource.SelectNodes("DataVirtualHardDisks/DataVirtualHardDisk");
            foreach (XmlNode datadisknode in datadisknodes)
            {
                DataDisk datadisk = new DataDisk();
                datadisk.name = datadisknode.SelectSingleNode("DiskName").InnerText;
                datadisk.caching = datadisknode.SelectSingleNode("HostCaching").InnerText;
                datadisk.diskSizeGB = Int64.Parse(datadisknode.SelectSingleNode("LogicalDiskSizeInGB").InnerText);

                datadisk.lun = 0;
                if (datadisknode.SelectSingleNode("Lun") != null)
                {
                    datadisk.lun = Int64.Parse(datadisknode.SelectSingleNode("Lun").InnerText);
                }

                olddiskurl = datadisknode.SelectSingleNode("MediaLink").InnerText;
                splitarray = olddiskurl.Split(new char[] { '/', '.' });
                oldstorageaccountname = splitarray[2];
                newstorageaccountname = oldstorageaccountname + app.Default.UniquenessSuffix;
                newdiskurl = olddiskurl.Replace(oldstorageaccountname, newstorageaccountname);

                // if the tool is configured to create new VMs with empty data disks
                if (app.Default.BuildEmpty)
                {
                    datadisk.createOption = "Empty";
                }
                // if the tool is configured to attach copied disks
                else
                {
                    datadisk.createOption = "Attach";

                    // Block of code to help copying the blobs to the new storage accounts
                    Hashtable storageaccountinfo = new Hashtable();
                    storageaccountinfo.Add("name", oldstorageaccountname);

                    XmlNode storageaccountkeys = GetAzureASMResources("StorageAccountKeys", storageaccountinfo, token)[0];
                    string key = storageaccountkeys.SelectSingleNode("StorageServiceKeys/Primary").InnerText;

                    CopyBlobDetail copyblobdetail = new CopyBlobDetail();
                    copyblobdetail.SourceSA = oldstorageaccountname;
                    copyblobdetail.SourceContainer = splitarray[7];
                    copyblobdetail.SourceBlob = splitarray[8] + "." + splitarray[9];
                    copyblobdetail.SourceKey = key;
                    copyblobdetail.DestinationSA = newstorageaccountname;
                    copyblobdetail.DestinationContainer = splitarray[7];
                    copyblobdetail.DestinationBlob = splitarray[8] + "." + splitarray[9];
                    copyblobdetails.Add(copyblobdetail);
                    // end of block of code to help copying the blobs to the new storage accounts
                }

                vhd = new Vhd();
                vhd.uri = newdiskurl;
                datadisk.vhd = vhd;

                try { storageaccountdependencies.Add(newstorageaccountname, ""); }
                catch { }

                datadisks.Add(datadisk);
            }

            StorageProfile storageprofile = new StorageProfile();
            if (app.Default.BuildEmpty) { storageprofile.imageReference = imagereference; }
            storageprofile.osDisk = osdisk;
            storageprofile.dataDisks = datadisks;

            VirtualMachine_Properties virtualmachine_properties = new VirtualMachine_Properties();
            virtualmachine_properties.hardwareProfile = hardwareprofile;
            if (app.Default.BuildEmpty) { virtualmachine_properties.osProfile = osprofile; }
            virtualmachine_properties.networkProfile = networkprofile;
            virtualmachine_properties.storageProfile = storageprofile;

            List<string> dependson = new List<string>();
            dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Network/networkInterfaces/" + networkinterfacename + "')]");

            string availabilitysetname = "defaultAS";
            if (resource.SelectSingleNode("AvailabilitySetName") != null)
            {
                availabilitysetname = resource.SelectSingleNode("AvailabilitySetName").InnerText;
            }

            Reference availabilityset = new Reference();
            availabilityset.id = "[concat(resourceGroup().id, '/providers/Microsoft.Compute/availabilitySets/" + availabilitysetname + "')]";
            virtualmachine_properties.availabilitySet = availabilityset;

            dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Compute/availabilitySets/" + availabilitysetname + "')]");

            foreach (DictionaryEntry storageaccountdependency in storageaccountdependencies)
            {
                if (GetProcessedItem("Microsoft.Storage/storageAccounts/" + storageaccountdependency.Key))
                {
                    dependson.Add("[concat(resourceGroup().id, '/providers/Microsoft.Storage/storageAccounts/" + storageaccountdependency.Key + "')]");
                }
            }

            VirtualMachine virtualmachine = new VirtualMachine();
            virtualmachine.name = virtualmachinename;
            virtualmachine.location = virtualmachineinfo["location"].ToString();
            virtualmachine.properties = virtualmachine_properties;
            virtualmachine.dependsOn = dependson;

            processeditems.Add("Microsoft.Compute/virtualMachines/" + virtualmachine.name, virtualmachine.location);
            resources.Add(virtualmachine);
            writeLog("BuildVirtualMachineObject", "Microsoft.Compute/virtualMachines/" + virtualmachine.name);

            writeLog("BuildVirtualMachineObject", "End");
        }

        private void BuildStorageAccountObject(XmlNode resource)
        {
            writeLog("BuildStorageAccountObject", "Start");

            StorageAccount_Properties storageaccount_properties = new StorageAccount_Properties();
            storageaccount_properties.accountType = resource.SelectSingleNode("StorageServiceProperties/AccountType").InnerText;

            StorageAccount storageaccount = new StorageAccount();
            storageaccount.name = resource.SelectSingleNode("ServiceName").InnerText + app.Default.UniquenessSuffix;
            storageaccount.location = resource.SelectSingleNode("StorageServiceProperties/Location").InnerText;
            storageaccount.properties = storageaccount_properties;
            
            processeditems.Add("Microsoft.Storage/storageAccounts/" + storageaccount.name, storageaccount.location);
            resources.Add(storageaccount);
            writeLog("BuildStorageAccountObject", "Microsoft.Storage/storageAccounts/" + storageaccount.name);

            writeLog("BuildStorageAccountObject", "End");
        }

        private void writeFile(string filename, string text)
        {
            File.WriteAllText((txtDestinationFolder.Text + "\\" + filename), text);
        }

        private void writeLog(string function, string message)
        {
            ;
            string logfilepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MIGAZ-" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
            string text = DateTime.Now.ToString() + "   " + function + "  " + message + Environment.NewLine;
            File.AppendAllText(logfilepath, text);
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

        // convert an hex string into byte array
        public static byte[] StrToByteArray(string str)
        {
            Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
            for (int i = 0; i <= 255; i++)
                hexindex.Add(i.ToString("X2"), (byte)i);

            List<byte> hexres = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
                hexres.Add(hexindex[str.Substring(i, 2)]);

            return hexres.ToArray();
        }

        private void postTelemetryRecord()
        {
            TelemetryRecord telemetryrecord = new TelemetryRecord();
            telemetryrecord.ExecutionId = Guid.Parse(app.Default.ExecutionId);
            telemetryrecord.SubscriptionId = new Guid(subscriptionid);
            telemetryrecord.TenantId = String.Empty;
            telemetryrecord.ProcessedResources = processeditems;

            string jsontext = JsonConvert.SerializeObject(telemetryrecord, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(jsontext);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://asmtoarmtoolapi.azurewebsites.net/api/telemetry");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //TelemetryRecord mytelemetry = (TelemetryRecord)JsonConvert.DeserializeObject(jsontext, typeof(TelemetryRecord));
        }

        private string GetVMSize(string vmsize)
        {
            Dictionary<string,string> VMSizeTable = new Dictionary<string, string>();
            VMSizeTable.Add("ExtraSmall", "Standard_A0");
            VMSizeTable.Add("Small", "Standard_A1");
            VMSizeTable.Add("Medium", "Standard_A2");
            VMSizeTable.Add("Large", "Standard_A3");
            VMSizeTable.Add("ExtraLarge", "Standard_A4");
            VMSizeTable.Add("A5", "Standard_A5");
            VMSizeTable.Add("A6", "Standard_A6");
            VMSizeTable.Add("A7", "Standard_A7");
            VMSizeTable.Add("A8", "Standard_A8");
            VMSizeTable.Add("A9", "Standard_A9");
            VMSizeTable.Add("A10", "Standard_A10");
            VMSizeTable.Add("A11", "Standard_A11");

            if (VMSizeTable.ContainsKey(vmsize))
            {
                return VMSizeTable[vmsize];
            }
            else
            {
                return vmsize;
            }
        }

        private bool GetProcessedItem(string processeditem)
        {
            if (processeditems.ContainsKey(processeditem))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void NewVersionAvailable()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://asmtoarmtoolapi.azurewebsites.net/api/version");
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

            string version = "\"" + Assembly.GetEntryAssembly().GetName().Version.ToString() + "\"";
            string availableversion = result.ToString();

            if (version != availableversion)
            {
                DialogResult dialogresult = MessageBox.Show("New version " + availableversion + " is available at http://aka.ms/MIGAZ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private Dictionary<string, string> GetVMLBMapping(string cloudservicename, XmlNodeList roles)
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

        private void btnOptions_Click(object sender, EventArgs e)
        {
            Forms.formOptions formoptions = new Forms.formOptions();
            formoptions.ShowDialog(this);
        }

  
    }
}
