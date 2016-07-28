using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Generator
{
    class InteractiveTokenProvider : ITokenProvider
    {
        public string GetToken(string tenantId)
        {
            AuthenticationContext context = new AuthenticationContext(ServiceUrls.GetLoginUrl(app.Default.AzureEnvironment) + tenantId);

            AuthenticationResult result = null;
            result = context.AcquireToken(ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment), app.Default.ClientId, new Uri(app.Default.ReturnURL), PromptBehavior.Auto);
            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the token");
            }


            return result.AccessToken;

        }
    }
}
