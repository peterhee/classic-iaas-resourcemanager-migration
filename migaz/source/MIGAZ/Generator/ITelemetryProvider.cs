using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Generator
{
    public interface ITelemetryProvider
    {
        void PostTelemetryRecord(string tenantId, string subscriptionId, Dictionary<string, string> processedItems, string offercategories);
    }
}
