using Microsoft.Rest;
using SignalRDashboard.Data.Milliman.DataSources.Models;

namespace SignalRDashboard.Data.Milliman.DataSources
{
    public class AzureClient
    {
        public string AzureSubscriptionId { get; set; }
        public string AzureApplicationId { get; set; }
        public string AzureServicePrincipalPassword { get; set; }
        public string AzureTenantId { get; set; }

        private static string AuthToken { get; set; }
        private static TokenCredentials TokenCredentials { get; set; }

        public void Authenticate()
        {
            //AuthToken = GetAuthorizationToken();
            //TokenCredentials = new TokenCredentials(AuthToken);
        }

        public AzureStatusData GetAzureMetrics()
        {
            //var res = GetAzureResourceGroup();

            //var restCredentials = Config.RmAccount.GetAzureServiceCredentials();
            //var cloudCredentials = Config.StorageAccount.GetAzureCredentials(azureSubscriptionId);
            //var res = new ResourceManagementClient(restCredentials) { SubscriptionId = azureSubscriptionId };

            //var hdis = HdInsights(cloudCredentials);
            //var allResources = res.Resources.List();
            //var databases = new List<DatabaseResource>();

            var data = new AzureStatusData {Id = "test", Name = "Test"};
            return data;
        }

        //private string GetAuthorizationToken()
        //{
        //    try
        //    {
        //        var cc = new ClientCredential(AzureApplicationId, AzureServicePrincipalPassword);
        //        var context = new AuthenticationContext("https://login.windows.net/" + AzureTenantId);
        //        var result = context.AcquireTokenAsync("https://management.azure.com/", cc);
        //        if (result == null)
        //        {
        //            throw new InvalidOperationException("Failed to obtain the JWT token");
        //        }
        //        return result.Result.AccessToken;
        //    }
        //    catch (Exception ex)
        //    {
        //        var sdf = ex.Message;
        //        return sdf;
        //    }
        //}

        //private ResourceGroup GetAzureResourceGroup()
        //{
        //    var resourceManagementClient = new ResourceManagementClient(TokenCredentials)
        //    {
        //        SubscriptionId = AzureSubscriptionId
        //    };

        //    var resourceGroup = resourceManagementClient.ResourceGroups.Get("idm-develop");

        //    return resourceGroup;
        //}
    }
}