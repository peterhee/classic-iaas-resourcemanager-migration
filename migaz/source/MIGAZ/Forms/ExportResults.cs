using MIGAZ.Generator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MIGAZ.Forms
{
    public partial class ExportResults : Form
    {
        private string _migazPath;
        private string _templatePath;
        private string _blobDetailsPath;
        private string _instructionsPath;
        private AsmRetriever _asmRetriever;
        private string _token;
        private List<string> _messages;
        private string _sourceSubscriptionId;

        public ExportResults(AsmRetriever asmRetriever, string token, List<string> messages, string sourceSubscriptionId, string instructionsPath, string templatePath, string blobDetailsPath)
        {
            InitializeComponent();
            _migazPath = AppDomain.CurrentDomain.BaseDirectory;
            _templatePath = templatePath;
            _blobDetailsPath = blobDetailsPath;
            _instructionsPath = instructionsPath;
            _asmRetriever = asmRetriever;
            _token = token;
            _messages = messages;
            _sourceSubscriptionId = sourceSubscriptionId;
        }

        private class Subscription
        {
            public string SubscriptionName { get; set; }
            public string SubscriptionId { get; set; }
        }



        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void btnViewTemplate_Click(object sender, EventArgs e)
        {
            ProcessStartInfo pInfo = new ProcessStartInfo();
            pInfo.FileName = _templatePath;
            pInfo.UseShellExecute = true;
            Process p = Process.Start(pInfo);
        }

        private void btnGenerateInstructions_Click(object sender, EventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MIGAZ.DeployDocTemplate.html";
            string content;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                content = reader.ReadToEnd();
            }

            content = content.Replace("{subscriptionId}", ((Subscription)cboSubscription.SelectedItem).SubscriptionId);
            content = content.Replace("{templatePath}", _templatePath);
            content = content.Replace("{blobDetailsPath}", _blobDetailsPath);
            content = content.Replace("{resourceGroupName}", txtRGName.Text);
            content = content.Replace("{location}", cboRGLocation.Text);
            content = content.Replace("{migAzPath}", _migazPath);

            var writer = new StreamWriter(_instructionsPath);
            writer.Write(content);
            writer.Close();

            ProcessStartInfo pInfo = new ProcessStartInfo();
            pInfo.FileName = _instructionsPath;
            pInfo.UseShellExecute = true;
            Process p = Process.Start(pInfo);
        }

        private async void ExportResults_Load(object sender, EventArgs e)
        {
            // Initialise messages
            foreach (var message in _messages)
            {
                txtMessages.Text += message + "\r\n";
            }

            // Initialise subscriptions
            Subscription currentSubscription = null;
            List<Subscription> subscriptions = new List<Subscription>();
            foreach (XmlNode subscription in (await _asmRetriever.GetAzureASMResources("Subscriptions", null, null, _token)).SelectNodes("//Subscription"))
            {
                var sub = new Subscription { SubscriptionName = subscription.SelectSingleNode("SubscriptionName").InnerText, SubscriptionId = subscription.SelectSingleNode("SubscriptionID").InnerText };
                subscriptions.Add(sub);
                if (sub.SubscriptionId == _sourceSubscriptionId)
                {
                    currentSubscription = sub;
                }
            }
            cboSubscription.DataSource = subscriptions;
            cboRGLocation.DisplayMember = "SubscriptionName";
            cboSubscription.SelectedItem = currentSubscription;

            System.Media.SystemSounds.Asterisk.Play();
        }


    }
}
