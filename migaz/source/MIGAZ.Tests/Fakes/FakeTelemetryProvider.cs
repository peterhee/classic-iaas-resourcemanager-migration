using MIGAZ.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Tests.Fakes
{
    class FakeTelemetryProvider : ITelemetryProvider
    {
        public void PostTelemetryRecord(string tenantId, string subscriptionId, Dictionary<string, string> processedItems)
        {
            
        }
    }
}
