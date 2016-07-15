using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Models
{
    public class TelemetryRecord
    {
        public Guid ExecutionId;
        public string TenantId;
        public System.Guid SubscriptionId;
        public Dictionary<string, string> ProcessedResources;
    }
}
