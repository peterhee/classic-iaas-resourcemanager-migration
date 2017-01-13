using System;
using System.Collections.Generic;

namespace MIGAZ.Models
{
    public class TelemetryRecord
    {
        public Guid ExecutionId;
        public string TenantId;
        public System.Guid SubscriptionId;
        public Dictionary<string, string> ProcessedResources;
        public string OfferCategories;
        public string SourceVersion;
    }
}
