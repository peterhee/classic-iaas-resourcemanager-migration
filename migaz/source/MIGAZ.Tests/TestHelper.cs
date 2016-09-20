using MIGAZ.Generator;
using MIGAZ.Tests.Fakes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Tests
{
    static class TestHelper
    {
        public const string TenantId = "11111111-1111-1111-1111-111111111111";
        public const string SubscriptionId = "22222222-2222-2222-2222-222222222222";

        public static void SetupObjects(out FakeAsmRetriever asmRetreiver, out TemplateGenerator templateGenerator)
        {
            ILogProvider logProvider = new FakeLogProvider();
            IStatusProvider statusProvider = new FakeStatusProvider();
            ITelemetryProvider telemetryProvider = new FakeTelemetryProvider();
            ITokenProvider tokenProvider = new FakeTokenProvider();
            ISettingsProvider settingsProvider = new FakeSettingsProvider();
            asmRetreiver = new FakeAsmRetriever(logProvider, statusProvider);
            templateGenerator = new TemplateGenerator(logProvider, statusProvider, telemetryProvider, tokenProvider, asmRetreiver, settingsProvider);
        }

        public static JObject GetJsonData(MemoryStream closedStream)
        {
            var newStream = new MemoryStream(closedStream.ToArray());
            var reader = new StreamReader(newStream);
            var templateText = reader.ReadToEnd();
            return JObject.Parse(templateText);
        }
    }
}
