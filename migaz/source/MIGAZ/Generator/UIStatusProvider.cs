using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIGAZ.Generator
{
    class UIStatusProvider : IStatusProvider
    {
        private ToolStripItem _label;
        public UIStatusProvider(ToolStripItem label)
        {
            _label = label;
        }

        public void UpdateStatus(string message)
        {
            _label.Text = message;
        }
    }
}
