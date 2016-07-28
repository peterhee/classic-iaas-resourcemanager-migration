using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Generator
{
    static class ServiceUrls
    {
        private static Dictionary<string, string[]> _serviceUrls = new Dictionary<string, string[]>
        {
            {  "Azure Cloud", new[] { "https://management.core.windows.net/", "https://login.windows.net/" } },
            {  "Azure China Cloud", new[] { "https://management.core.chinacloudapi.cn/", "https://login.chinacloudapi.cn/" } },
            {  "Azure Germany Cloud", new[] { "https://management.core.cloudapi.de/", "https://login.microsoftonline.de/" } },
            {  "Azure US Government Cloud", new[] { "https://management.core.usgovcloudapi.net/", "https://login.windows.net/" } },
        };

        public static string GetServiceManagementUrl(string cloudName)
        {
            return _serviceUrls[cloudName][0];
        }

        public static string GetLoginUrl(string cloudName)
        {
            return _serviceUrls[cloudName][1];
        }
    }
}
