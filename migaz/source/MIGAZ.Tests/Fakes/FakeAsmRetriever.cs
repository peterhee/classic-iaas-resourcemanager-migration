using MIGAZ.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Xml;

namespace MIGAZ.Tests.Fakes
{
    class FakeAsmRetriever : AsmRetriever
    {
        private Dictionary<string, XmlNodeList> _responses = new Dictionary<string, XmlNodeList>();

        public FakeAsmRetriever(ILogProvider logProvider, IStatusProvider statusProvider) : base(logProvider, statusProvider)
        {
        }

        public void SetResponse(string resourceType, Hashtable info, XmlNodeList nodes)
        {
            string key = resourceType + ":" + SerialiseHashTable(info);
            _responses[key] = nodes;
        }

        public override XmlNodeList GetAzureASMResources(string resourceType, string subscriptionId, Hashtable info, string token)
        {
            string key = resourceType + ":" + SerialiseHashTable(info);
            return _responses[key];
        }

        private string SerialiseHashTable(Hashtable ht)
        {
            var sb = new StringBuilder();
            foreach(var key in ht.Keys)
            {
                sb.Append(key);
                sb.Append(ht[key]);
            }
            return sb.ToString();
        }
    }
}
