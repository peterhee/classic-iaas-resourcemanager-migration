using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Generator
{
    class AppSettingsProvider : ISettingsProvider
    {
        public bool AllowTelemetry
        {
            get
            {
                return app.Default.AllowTelemetry;
            }

            set
            {
                app.Default.AllowTelemetry = value;
                app.Default.Save();
            }
        }

        public bool BuildEmpty
        {
            get
            {
                return app.Default.BuildEmpty;
            }

            set
            {
                app.Default.BuildEmpty = value;
                app.Default.Save();
            }
        }

        public string ExecutionId
        {
            get
            {
                return app.Default.ExecutionId;
            }

            set
            {
                app.Default.ExecutionId = value;
                app.Default.Save();
            }
        }

        public string UniquenessSuffix
        {
            get
            {
                return app.Default.UniquenessSuffix;
            }

            set
            {
                app.Default.UniquenessSuffix = value;
                app.Default.Save();
            }
        }
    }
}
