using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Classes
{
    public class CopyBlobDetail
    {
        public string SourceSA;
        public string SourceContainer;
        public string SourceBlob;
        public string SourceKey;
        public string DestinationSA;
        public string DestinationContainer;
        public string DestinationBlob;
        public string DestinationKey = "";
        public string Status = "";
        public long TotalBytes = 0;
        public long BytesCopied = 0;
        public string StartTime = "";
        public string EndTime = "";
        public string SnapshotTime = "";
    }

    public class TelemetryRecord
    {
        public string TenantId;
        public System.Guid SubscriptionId;
        public Dictionary<string, string> ProcessedResources;
    }
}
