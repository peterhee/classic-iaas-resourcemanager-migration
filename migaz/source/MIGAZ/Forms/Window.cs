using System;
using System.Windows.Forms;
using System.Collections;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Reflection;
using MIGAZ.Models;
using MIGAZ.Generator;
using MIGAZ.Forms;
using System.Threading.Tasks;
using System.Linq;

namespace MIGAZ
{
    public partial class Window : Form
    {
        private string subscriptionid;
        private Dictionary<string, string> subscriptionsAndTenants;
        private AsmRetriever _asmRetriever;
        private TemplateGenerator _templateGenerator;
        private ISaveSelectionProvider _saveSelectionProvider;
        private ILogProvider _logProvider;
        private IStatusProvider _statusProvider;

        public Window()
        {
            InitializeComponent();
            _logProvider = new FileLogProvider();
            _statusProvider = new UIStatusProvider(lblStatus);
            _asmRetriever = new AsmRetriever(_logProvider, _statusProvider);
            _saveSelectionProvider = new UISaveSelectionProvider();
            var tokenProvider = new InteractiveTokenProvider();
            var telemetryProvider = new CloudTelemetryProvider();
            var settingsProvider = new AppSettingsProvider();
            _templateGenerator = new TemplateGenerator(_logProvider, _statusProvider, telemetryProvider, tokenProvider, _asmRetriever, settingsProvider);
        }

        private void Window_Load(object sender, EventArgs e)
        {
            writeLog("Window_Load", "Program start");

            this.Text = "migAz (" + Assembly.GetEntryAssembly().GetName().Version.ToString() + ")";
            NewVersionAvailable(); // check if there a new version of the app
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If save selection option is enabled
            if (app.Default.SaveSelection)
            {
                _saveSelectionProvider.Save(Guid.Parse(subscriptionid), lvwVirtualNetworks, lvwStorageAccounts, lvwVirtualMachines);
            }
        }

        private async void btnGetToken_Click(object sender, EventArgs e)
        {
            writeLog("GetToken_Click", "Start");


            cmbSubscriptions.Enabled = false;
            cmbSubscriptions.Items.Clear();
            lvwVirtualNetworks.Items.Clear();
            lvwStorageAccounts.Items.Clear();
            lvwVirtualMachines.Items.Clear();
            _asmRetriever._documentCache.Clear(); // need to clear cache to allow relogin without returning previous cached list of subscriptions

            string token = GetToken("common", PromptBehavior.Always, true);

            if (token != null)
            {
                subscriptionsAndTenants = new Dictionary<string, string>();
                foreach (XmlNode subscription in (await _asmRetriever.GetAzureASMResources("Subscriptions", null, null, token)).SelectNodes("//Subscription"))
                {
                    cmbSubscriptions.Items.Add(subscription.SelectSingleNode("SubscriptionID").InnerText + " | " + subscription.SelectSingleNode("SubscriptionName").InnerText);
                    subscriptionsAndTenants.Add(subscription.SelectSingleNode("SubscriptionID").InnerText, subscription.SelectSingleNode("AADTenantID").InnerText);
                }

                cmbSubscriptions.Enabled = true;
                txtDestinationFolder.Enabled = true;
                btnChoosePath.Enabled = true;
            }
            else
            {
                writeLog("GetToken_Click", "Failed to get token");
            }

            lblStatus.Text = "Ready";
            writeLog("GetToken_Click", "End");
        }

        private string GetToken(string tenantId, PromptBehavior promptBehavior, bool updateUI = false)
        {
  
            lblStatus.Text = "BUSY: Authenticating...";
            AuthenticationContext context = new AuthenticationContext(ServiceUrls.GetLoginUrl(app.Default.AzureEnvironment) + tenantId);

            AuthenticationResult result = null;
            try
            {
                result = context.AcquireToken(ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment), app.Default.ClientId, new Uri(app.Default.ReturnURL), promptBehavior);
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
            catch (Exception exception)
            {
                DialogResult dialogresult = MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

       

        private async void cmbSubscriptions_SelectedIndexChanged(object sender, EventArgs e)
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

                var retrieveTasks = new List<Task<XmlDocument>>();
                Task<XmlDocument> vnetRetrieveTask = _asmRetriever.GetAzureASMResources("VirtualNetworks", subscriptionid, null, token);
                Task<XmlDocument> storageRetrieveTask = _asmRetriever.GetAzureASMResources("StorageAccounts", subscriptionid, null, token);
                Task<XmlDocument> cloudServiceRetrieveTask = _asmRetriever.GetAzureASMResources("CloudServices", subscriptionid, null, token);
                retrieveTasks.AddRange(new[] { vnetRetrieveTask, storageRetrieveTask, cloudServiceRetrieveTask });

                await Task.WhenAll(retrieveTasks);

                foreach (XmlNode virtualnetworksite in vnetRetrieveTask.Result.SelectNodes("//VirtualNetworkSite"))
                {
                    lvwVirtualNetworks.Items.Add(virtualnetworksite.SelectSingleNode("Name").InnerText);
                }

                foreach (XmlNode storageaccount in storageRetrieveTask.Result.SelectNodes("//StorageService"))
                {
                    lvwStorageAccounts.Items.Add(storageaccount.SelectSingleNode("ServiceName").InnerText);
                }

                var cloudServiceVMTasks = new List<Task>();
                foreach (XmlNode cloudservice in cloudServiceRetrieveTask.Result.SelectNodes("//HostedService"))
                {
                    cloudServiceVMTasks.Add(RetrieveVMsFromCloudService(token, cloudservice));
                }

                await Task.WhenAll(cloudServiceVMTasks);

                lblStatus.Text = "BUSY: Getting Reserved IPs";
                XmlDocument reservedips = await _asmRetriever.GetAzureASMResources("ReservedIPs", subscriptionid, null, token);

                // If save selection option is enabled
                if (app.Default.SaveSelection)
                {
                    lblStatus.Text = "BUSY: Reading saved selection";
                    _saveSelectionProvider.Read(Guid.Parse(subscriptionid), ref lvwVirtualNetworks, ref lvwStorageAccounts, ref lvwVirtualMachines);
                }

                lblStatus.Text = "Ready";

                writeLog("Subscriptions_SelectionChanged", "End");
            }
        }

        private async Task RetrieveVMsFromCloudService(string token, XmlNode cloudservice)
        {
            string cloudservicename = cloudservice.SelectSingleNode("ServiceName").InnerText;
            Hashtable cloudserviceinfo = new Hashtable();
            cloudserviceinfo.Add("name", cloudservicename);

            XmlDocument hostedservice = await _asmRetriever.GetAzureASMResources("CloudService", subscriptionid, cloudserviceinfo, token);
            if (hostedservice.SelectNodes("//Deployments/Deployment").Count > 0)
            {
                if (hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectNodes("RoleType").Count > 0)
                {
                    if (hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role")[0].SelectSingleNode("RoleType").InnerText == "PersistentVMRole")
                    {

                        XmlNodeList roles = hostedservice.SelectNodes("//Deployments/Deployment")[0].SelectNodes("RoleList/Role");

                        foreach (XmlNode role in roles)
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                string virtualmachinename = role.SelectSingleNode("RoleName").InnerText;
                                lblStatus.Text = "BUSY: Retrieved VM: " + virtualmachinename;
                                var listItem = new ListViewItem(cloudservicename);
                                listItem.SubItems.AddRange(new[] { virtualmachinename });
                                lvwVirtualMachines.Items.Add(listItem);
                            }));
                        }
                    }
                }
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

        private async void lvwVirtualMachines_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateExportItemsCount();

            if (app.Default.AutoSelectDependencies)
            {
                if (e.Item.Checked)
                {
                    await AutoSelectDependencies(e);
                }
            }
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

        private async void btnExport_Click(object sender, EventArgs e)
        {
            btnExport.Enabled = false;

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

            if (!Directory.Exists(txtDestinationFolder.Text))
            {
                MessageBox.Show("The chosen output folder does not exist.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // If save selection option is enabled
                if (app.Default.SaveSelection)
                {
                    lblStatus.Text = "BUSY: Reading saved selection";
                    _saveSelectionProvider.Save(Guid.Parse(subscriptionid), lvwVirtualNetworks, lvwStorageAccounts, lvwVirtualMachines);
                }

                var instructionsPath = Path.Combine(txtDestinationFolder.Text, "DeployInstructions.html");
                var templatePath = Path.Combine(txtDestinationFolder.Text, "export.json");
                var blobDetailsPath = Path.Combine(txtDestinationFolder.Text, "copyblobdetails.json");
                var templateWriter = new StreamWriter(templatePath);
                var blobDetailWriter = new StreamWriter(blobDetailsPath);
                try
                {
                    var messages = await _templateGenerator.GenerateTemplate(subscriptionsAndTenants[subscriptionid], subscriptionid, artefacts, templateWriter, blobDetailWriter);
                    var token = GetToken(subscriptionsAndTenants[subscriptionid], PromptBehavior.Auto);
                    var exportResults = new ExportResults(_asmRetriever, token, messages, subscriptionid, instructionsPath, templatePath, blobDetailsPath);
                    exportResults.ShowDialog(this);
                }
                catch (Exception ex)
                {
                    writeLog("btnExport_Click", "Error generating template : " + ex.ToString());
                    MessageBox.Show("Something went wrong when generating the template. Check the log file for details.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            btnExport.Enabled = true;
        }

        

        private void writeLog(string function, string message)
        {
            string logfilepath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MigAz\\MIGAZ-" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
            string text = DateTime.Now.ToString() + "   " + function + "  " + message + Environment.NewLine;
            File.AppendAllText(logfilepath, text);
        }



        private async Task NewVersionAvailable()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://asmtoarmtoolapi.azurewebsites.net/api/version");
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                string version = "\"" + Assembly.GetEntryAssembly().GetName().Version.ToString() + "\"";
                string availableversion = result.ToString();

                if (version != availableversion)
                {
                    DialogResult dialogresult = MessageBox.Show("New version " + availableversion + " is available at http://aka.ms/MIGAZ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception exception)
            {
                DialogResult dialogresult = MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       

        private void btnOptions_Click(object sender, EventArgs e)
        {
            Forms.formOptions formoptions = new Forms.formOptions();
            formoptions.ShowDialog(this);
        }


        private async Task AutoSelectDependencies (ItemCheckedEventArgs listViewRow)
        {
            string cloudServiceName = listViewRow.Item.ListView.Items[listViewRow.Item.Index].SubItems[0].Text;
            string virtualMachineName = listViewRow.Item.ListView.Items[listViewRow.Item.Index].SubItems[1].Text;

            // Get Subscription from ComboBox
            var token = GetToken(subscriptionsAndTenants[subscriptionid], PromptBehavior.Auto);
            
            // Get VM details
            var vmDetails = await _asmRetriever.GetVMDetails(subscriptionid, cloudServiceName, virtualMachineName, token);

            Hashtable virtualmachineinfo = new Hashtable();
            virtualmachineinfo.Add("cloudservicename", cloudServiceName);
            virtualmachineinfo.Add("deploymentname", vmDetails.DeploymentName);
            virtualmachineinfo.Add("virtualmachinename", virtualMachineName);
            virtualmachineinfo.Add("virtualnetworkname", vmDetails.VirtualNetworkName);
            virtualmachineinfo.Add("loadbalancername", vmDetails.LoadBalancerName);

            XmlDocument virtualmachine = await _asmRetriever.GetAzureASMResources("VirtualMachine", subscriptionid, virtualmachineinfo, token);

            // process virtual network
            foreach (ListViewItem virtualNetwork in lvwVirtualNetworks.Items)
            {
                if (virtualNetwork.Text == vmDetails.VirtualNetworkName)
                {
                    lvwVirtualNetworks.Items[virtualNetwork.Index].Checked = true;
                    lvwVirtualNetworks.Items[virtualNetwork.Index].Selected = true;
                }
            }

            // process OS disk
            XmlNode osvirtualharddisk = virtualmachine.SelectSingleNode("//OSVirtualHardDisk");
            string olddiskurl = osvirtualharddisk.SelectSingleNode("MediaLink").InnerText;
            string[] splitarray = olddiskurl.Split(new char[] { '/' });
            string storageaccountname = splitarray[2].Split(new char[] { '.' })[0];

            foreach (ListViewItem storageAccount in lvwStorageAccounts.Items)
            {
                if (storageAccount.Text == storageaccountname)
                {
                    lvwStorageAccounts.Items[storageAccount.Index].Checked = true;
                    lvwStorageAccounts.Items[storageAccount.Index].Selected = true;
                }
            }

            // process data disks
            XmlNodeList datadisknodes = virtualmachine.SelectNodes("//DataVirtualHardDisks/DataVirtualHardDisk");
            foreach (XmlNode datadisknode in datadisknodes)
            {
                olddiskurl = datadisknode.SelectSingleNode("MediaLink").InnerText;
                splitarray = olddiskurl.Split(new char[] { '/' });
                storageaccountname = splitarray[2].Split(new char[] { '.' })[0];

                foreach (ListViewItem storageAccount in lvwStorageAccounts.Items)
                {
                    if (storageAccount.Text == storageaccountname)
                    {
                        lvwStorageAccounts.Items[storageAccount.Index].Checked = true;
                        lvwStorageAccounts.Items[storageAccount.Index].Selected = true;
                    }
                }
            }

            lblStatus.Text = "Ready";
        }
    }
}
