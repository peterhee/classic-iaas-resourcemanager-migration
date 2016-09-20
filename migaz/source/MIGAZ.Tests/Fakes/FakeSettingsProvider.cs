using MIGAZ.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Tests.Fakes
{
    class FakeSettingsProvider : ISettingsProvider
    {
        public FakeSettingsProvider()
        {
            UniquenessSuffix = "v2";
        }

        public bool AllowTelemetry
        {
            get; set;
        }

        public bool BuildEmpty
        {
            get; set;
        }

        public string ExecutionId
        {
            get; set;
        }

        public string UniquenessSuffix
        {
            get; set;
        }
    }
}
