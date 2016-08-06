using MIGAZ.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIGAZ.Generator
{
    public class CloudTelemetryProvider : ITelemetryProvider
    {
        public void PostTelemetryRecord(string tenantId, string subscriptionId, Dictionary<string, string> processedItems)
        {
            TelemetryRecord telemetryrecord = new TelemetryRecord();
            telemetryrecord.ExecutionId = Guid.Parse(app.Default.ExecutionId);
            telemetryrecord.SubscriptionId = new Guid(subscriptionId);
            telemetryrecord.TenantId = tenantId;
            telemetryrecord.ProcessedResources = processedItems;

            string jsontext = JsonConvert.SerializeObject(telemetryrecord, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore });

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(jsontext);

            try
            {
                string sourceversion = Assembly.GetEntryAssembly().GetName().Version.ToString();

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://asmtoarmtoolapi.azurewebsites.net/api/telemetry?sourceVersion=" + sourceversion);
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
            catch (Exception exception)
            {
                DialogResult dialogresult = MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
