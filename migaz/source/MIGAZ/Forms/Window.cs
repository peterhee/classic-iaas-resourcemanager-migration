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
using MIGAZ.Generator;

namespace MIGAZ
{
    public partial class Window : Form
    {
        private string subscriptionid;
        private Dictionary<string, string> subscriptionsAndTenants;
        private AsmRetriever _asmRetriever;
        private ILogProvider _logProvider;
        private IStatusProvider _statusProvider;

        public Window()
        {
            InitializeComponent();
            _logProvider = new FileLogProvider();
            _statusProvider = new UIStatusProvider(lblStatus);
            _asmRetriever = new AsmRetriever(_logProvider, _statusProvider);
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
            foreach (XmlNode subscription in _asmRetriever.GetAzureASMResources("Subscriptions", null, null, token))
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

                foreach (XmlNode virtualnetworksite in _asmRetriever.GetAzureASMResources("VirtualNetworks", subscriptionid, null, token))
                {
                    lvwVirtualNetworks.Items.Add(virtualnetworksite.SelectSingleNode("Name").InnerText);
                    Application.DoEvents();
                }

                foreach (XmlNode storageaccount in _asmRetriever.GetAzureASMResources("StorageAccounts", subscriptionid, null, token))
                {
                    lvwStorageAccounts.Items.Add(storageaccount.SelectSingleNode("ServiceName").InnerText);
                    Application.DoEvents();
                }

                foreach (XmlNode cloudservice in _asmRetriever.GetAzureASMResources("CloudServices", subscriptionid, null, token))
                {
                    string cloudservicename = cloudservice.SelectSingleNode("ServiceName").InnerText;

                    Hashtable cloudserviceinfo = new Hashtable();
                    cloudserviceinfo.Add("name", cloudservicename);

                    XmlNodeList hostedservice = _asmRetriever.GetAzureASMResources("CloudService", subscriptionid, cloudserviceinfo, token);
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
                                Dictionary<string, string> vmlbmapping = _asmRetriever.GetVMLBMapping(cloudservicename, roles);
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
            var artefacts = new AsmArtefacts();
            foreach (var selectedItem in lvwStorageAccounts.CheckedItems)
            {
                var listItem = (ListViewItem)selectedItem;
                artefacts.StorageAccounts.Add(listItem.Text);
            }

            foreach (var selectedItem in lvwVirtualNetworks.CheckedItems)
            {
                var listItem = (ListViewItem)selectedItem;
                artefacts.VirtualNetworks.Add(listItem.Text);
            }

            foreach (var selectedItem in lvwVirtualMachines.CheckedItems)
            {
                var listItem = (ListViewItem)selectedItem;
                artefacts.VirtualMachines.Add(
                    new CloudServiceVM
                    {
                        CloudService = listItem.Text,
                        VirtualMachine = listItem.SubItems[1].Text,
                    });
            }
            var generator = new TemplateGenerator(_logProvider, _statusProvider);
            generator.GenerateTemplate(subscriptionsAndTenants[subscriptionid], subscriptionid, artefacts, txtDestinationFolder.Text);

        }

        

        private void writeLog(string function, string message)
        {
            string logfilepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MIGAZ-" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
            string text = DateTime.Now.ToString() + "   " + function + "  " + message + Environment.NewLine;
            File.AppendAllText(logfilepath, text);
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

       

        private void btnOptions_Click(object sender, EventArgs e)
        {
            Forms.formOptions formoptions = new Forms.formOptions();
            formoptions.ShowDialog(this);
        }

  
    }
}
