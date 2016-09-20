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

        public ExportResults(AsmRetriever asmRetriever, string token, List<string> messages, string sourceSubscriptionId, string instructionsPath, string templatePath, string blobDetailsPath)
        {
            InitializeComponent();
            _migazPath = AppDomain.CurrentDomain.BaseDirectory;
            _templatePath = templatePath;
            _blobDetailsPath = blobDetailsPath;
            _instructionsPath = instructionsPath;
            _asmRetriever = asmRetriever;
            _token = token;

            // Initialise messages
            foreach (var message in messages)
            {
                txtMessages.Text += message + "\r\n";
            }

            // Initialise subscriptions
            Subscription currentSubscription = null;
            List<Subscription> subscriptions = new List<Subscription>();
            foreach (XmlNode subscription in asmRetriever.GetAzureASMResources("Subscriptions", null, null, token).SelectNodes("//Subscription"))
            {
                var sub = new Subscription { SubscriptionName = subscription.SelectSingleNode("SubscriptionName").InnerText, SubscriptionId = subscription.SelectSingleNode("SubscriptionID").InnerText };
                subscriptions.Add(sub);
                if (sub.SubscriptionId == sourceSubscriptionId)
                {
                    currentSubscription = sub;
                }
            }
            cboSubscription.DataSource = subscriptions;
            cboRGLocation.DisplayMember = "SubscriptionName";
            cboSubscription.SelectedItem = currentSubscription;
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
            string instructionsTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DeployDocTemplate.html");
            var reader = new StreamReader(instructionsTemplatePath);
            var content = reader.ReadToEnd();
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

        private void ExportResults_Load(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Asterisk.Play();
        }


    }
}
